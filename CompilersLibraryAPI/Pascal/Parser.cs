
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace CompilersLibraryAPI.GCC
{
    public class MListViewItem : ListViewItem
    {
        public CompileMessage msg;
    }
    public class Token
    {
        public int kind;    // token kind
        public int pos;     // token position in bytes in the source text (starting at 0)
        public int charPos;  // token position in characters in the source text (starting at 0)
        public int col;     // token column (starting at 1)
        public int line;    // token line (starting at 1)
        public string val;  // token value
        public Token next;  // ML 2005-03-11 Tokens are kept in linked list
    }

    //-----------------------------------------------------------------------------------
    // Buffer
    //-----------------------------------------------------------------------------------
    public class Buffer
    {
        // This Buffer supports the following cases:
        // 1) seekable stream (file)
        //    a) whole stream in buffer
        //    b) part of stream in buffer
        // 2) non seekable stream (network, console)

        public const int EOF = char.MaxValue + 1;
        const int MIN_BUFFER_LENGTH = 1024; // 1KB
        const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
        byte[] buf;         // input buffer
        int bufStart;       // position of first byte in buffer relative to input stream
        int bufLen;         // length of buffer
        int fileLen;        // length of input stream (may change if the stream is no file)
        int bufPos;         // current position in buffer
        Stream stream;      // input stream (seekable)
        bool isUserStream;  // was the stream opened by the user?

        public Buffer(Stream s, bool isUserStream)
        {
            stream = s; this.isUserStream = isUserStream;

            if (stream.CanSeek)
            {
                fileLen = (int)stream.Length;
                bufLen = Math.Min(fileLen, MAX_BUFFER_LENGTH);
                bufStart = Int32.MaxValue; // nothing in the buffer so far
            }
            else
            {
                fileLen = bufLen = bufStart = 0;
            }

            buf = new byte[(bufLen > 0) ? bufLen : MIN_BUFFER_LENGTH];
            if (fileLen > 0) Pos = 0; // setup buffer to position 0 (start)
            else bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
            if (bufLen == fileLen && stream.CanSeek) Close();
        }

        protected Buffer(Buffer b)
        { // called in UTF8Buffer constructor
            buf = b.buf;
            bufStart = b.bufStart;
            bufLen = b.bufLen;
            fileLen = b.fileLen;
            bufPos = b.bufPos;
            stream = b.stream;
            // keep destructor from closing the stream
            b.stream = null;
            isUserStream = b.isUserStream;
        }

        ~Buffer() { Close(); }

        protected void Close()
        {
            if (!isUserStream && stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        public virtual int Read()
        {
            if (bufPos < bufLen)
            {
                return buf[bufPos++];
            }
            else if (Pos < fileLen)
            {
                Pos = Pos; // shift buffer start to Pos
                return buf[bufPos++];
            }
            else if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0)
            {
                return buf[bufPos++];
            }
            else
            {
                return EOF;
            }
        }

        public int Peek()
        {
            int curPos = Pos;
            int ch = Read();
            Pos = curPos;
            return ch;
        }

        // beg .. begin, zero-based, inclusive, in byte
        // end .. end, zero-based, exclusive, in byte
        public string GetString(int beg, int end)
        {
            int len = 0;
            char[] buf = new char[end - beg];
            int oldPos = Pos;
            Pos = beg;
            while (Pos < end) buf[len++] = (char)Read();
            Pos = oldPos;
            return new String(buf, 0, len);
        }

        public int Pos
        {
            get { return bufPos + bufStart; }
            set
            {
                if (value >= fileLen && stream != null && !stream.CanSeek)
                {
                    // Wanted position is after buffer and the stream
                    // is not seek-able e.g. network or console,
                    // thus we have to read the stream manually till
                    // the wanted position is in sight.
                    while (value >= fileLen && ReadNextStreamChunk() > 0) ;
                }

                if (value < 0 || value > fileLen)
                {
                    throw new FatalError("buffer out of bounds access, position: " + value);
                }

                if (value >= bufStart && value < bufStart + bufLen)
                { // already in buffer
                    bufPos = value - bufStart;
                }
                else if (stream != null)
                { // must be swapped in
                    stream.Seek(value, SeekOrigin.Begin);
                    bufLen = stream.Read(buf, 0, buf.Length);
                    bufStart = value; bufPos = 0;
                }
                else
                {
                    // set the position to the end of the file, Pos will return fileLen.
                    bufPos = fileLen - bufStart;
                }
            }
        }

        // Read the next chunk of bytes from the stream, increases the buffer
        // if needed and updates the fields fileLen and bufLen.
        // Returns the number of bytes read.
        private int ReadNextStreamChunk()
        {
            int free = buf.Length - bufLen;
            if (free == 0)
            {
                // in the case of a growing input stream
                // we can neither seek in the stream, nor can we
                // foresee the maximum length, thus we must adapt
                // the buffer size on demand.
                byte[] newBuf = new byte[bufLen * 2];
                Array.Copy(buf, newBuf, bufLen);
                buf = newBuf;
                free = bufLen;
            }
            int read = stream.Read(buf, bufLen, free);
            if (read > 0)
            {
                fileLen = bufLen = (bufLen + read);
                return read;
            }
            // end of stream reached
            return 0;
        }
    }

    //-----------------------------------------------------------------------------------
    // UTF8Buffer
    //-----------------------------------------------------------------------------------
    public class UTF8Buffer : Buffer
    {
        public UTF8Buffer(Buffer b) : base(b) { }

        public override int Read()
        {
            int ch;
            do
            {
                ch = base.Read();
                // until we find a utf8 start (0xxxxxxx or 11xxxxxx)
            } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
            if (ch < 128 || ch == EOF)
            {
                // nothing to do, first 127 chars are the same in ascii and utf8
                // 0xxxxxxx or end of file character
            }
            else if ((ch & 0xF0) == 0xF0)
            {
                // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x07; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F; ch = base.Read();
                int c4 = ch & 0x3F;
                ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
            }
            else if ((ch & 0xE0) == 0xE0)
            {
                // 1110xxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x0F; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F;
                ch = (((c1 << 6) | c2) << 6) | c3;
            }
            else if ((ch & 0xC0) == 0xC0)
            {
                // 110xxxxx 10xxxxxx
                int c1 = ch & 0x1F; ch = base.Read();
                int c2 = ch & 0x3F;
                ch = (c1 << 6) | c2;
            }
            return ch;
        }
    }

    //-----------------------------------------------------------------------------------
    // Scanner
    //-----------------------------------------------------------------------------------
    public class Scanner
    {
        const char EOL = '\n';
        const int eofSym = 0; /* pdt */
        const int maxT = 61;
        const int noSym = 61;
        char valCh;       // current input character (for token.val)

        public Buffer buffer; // scanner buffer

        Token t;          // current token
        int ch;           // current input character
        int pos;          // byte position of current character
        int charPos;      // position by unicode characters starting with 0
        int col;          // column number of current character
        int line;         // line number of current character
        int oldEols;      // EOLs that appeared in a comment;
        static readonly Hashtable start; // maps first token character to start state

        Token tokens;     // list of tokens already peeked (first token is a dummy)
        Token pt;         // current peek token

        char[] tval = new char[128]; // text of current token
        int tlen;         // length of current token

        static Scanner()
        {
            start = new Hashtable(128);
            for (int i = 97; i <= 122; ++i) start[i] = 1;
            for (int i = 48; i <= 57; ++i) start[i] = 15;
            start[39] = 2;
            start[59] = 11;
            start[46] = 31;
            start[43] = 13;
            start[45] = 14;
            start[40] = 18;
            start[41] = 19;
            start[44] = 20;
            start[58] = 32;
            start[61] = 21;
            start[91] = 22;
            start[93] = 23;
            start[94] = 25;
            start[60] = 33;
            start[62] = 34;
            start[42] = 29;
            start[47] = 30;
            start[Buffer.EOF] = -1;

        }

        public Scanner(string fileName)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new Buffer(stream, false);
                Init();
            }
            catch (IOException)
            {
                throw new FatalError("Cannot open file " + fileName);
            }
        }

        public Scanner(Stream s)
        {
            buffer = new Buffer(s, true);
            Init();
        }

        void Init()
        {
            pos = -1; line = 1; col = 0; charPos = -1;
            oldEols = 0;
            NextCh();
            if (ch == 0xEF)
            { // check optional byte order mark for UTF-8
                NextCh(); int ch1 = ch;
                NextCh(); int ch2 = ch;
                if (ch1 != 0xBB || ch2 != 0xBF)
                {
                    throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
                }
                buffer = new UTF8Buffer(buffer); col = 0; charPos = -1;
                NextCh();
            }
            pt = tokens = new Token();  // first token is a dummy
        }

        void NextCh()
        {
            if (oldEols > 0) { ch = EOL; oldEols--; }
            else
            {
                pos = buffer.Pos;
                // buffer reads unicode chars, if UTF8 has been detected
                ch = buffer.Read(); col++; charPos++;
                // replace isolated '\r' by '\n' in order to make
                // eol handling uniform across Windows, Unix and Mac
                if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
                if (ch == EOL) { line++; col = 0; }
            }
            if (ch != Buffer.EOF)
            {
                valCh = (char)ch;
                ch = char.ToLower((char)ch);
            }

        }

        void AddCh()
        {
            if (tlen >= tval.Length)
            {
                char[] newBuf = new char[2 * tval.Length];
                Array.Copy(tval, 0, newBuf, 0, tval.Length);
                tval = newBuf;
            }
            if (ch != Buffer.EOF)
            {
                tval[tlen++] = valCh;
                NextCh();
            }
        }



        bool Comment0()
        {
            int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
            NextCh();
            if (ch == '*')
            {
                NextCh();
                for (; ; )
                {
                    if (ch == '*')
                    {
                        NextCh();
                        if (ch == ')')
                        {
                            level--;
                            if (level == 0) { oldEols = line - line0; NextCh(); return true; }
                            NextCh();
                        }
                    }
                    else if (ch == Buffer.EOF) return false;
                    else NextCh();
                }
            }
            else
            {
                buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
            }
            return false;
        }

        bool Comment1()
        {
            int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
            NextCh();
            for (; ; )
            {
                if (ch == '}')
                {
                    level--;
                    if (level == 0) { oldEols = line - line0; NextCh(); return true; }
                    NextCh();
                }
                else if (ch == Buffer.EOF) return false;
                else NextCh();
            }
        }


        void CheckLiteral()
        {
            switch (t.val.ToLower())
            {
                case "case": t.kind = 9; break;
                case "end": t.kind = 10; break;
                case "program": t.kind = 11; break;
                case "uses": t.kind = 15; break;
                case "const": t.kind = 17; break;
                case "type": t.kind = 18; break;
                case "var": t.kind = 19; break;
                case "procedure": t.kind = 20; break;
                case "function": t.kind = 21; break;
                case "begin": t.kind = 23; break;
                case "array": t.kind = 25; break;
                case "of": t.kind = 28; break;
                case "packed": t.kind = 29; break;
                case "goto": t.kind = 31; break;
                case "while": t.kind = 32; break;
                case "do": t.kind = 33; break;
                case "repeat": t.kind = 34; break;
                case "until": t.kind = 35; break;
                case "for": t.kind = 36; break;
                case "to": t.kind = 37; break;
                case "downto": t.kind = 38; break;
                case "if": t.kind = 39; break;
                case "then": t.kind = 40; break;
                case "else": t.kind = 41; break;
                case "with": t.kind = 42; break;
                case "nil": t.kind = 43; break;
                case "not": t.kind = 44; break;
                case "in": t.kind = 51; break;
                case "or": t.kind = 52; break;
                case "div": t.kind = 55; break;
                case "mod": t.kind = 56; break;
                case "and": t.kind = 57; break;
                case "record": t.kind = 58; break;
                case "set": t.kind = 59; break;
                case "file": t.kind = 60; break;
                default: break;
            }
        }

        Token NextToken()
        {
            while (ch == ' ' ||
                ch >= 9 && ch <= 10 || ch >= 12 && ch <= 13
            ) NextCh();
            if (ch == '(' && Comment0() || ch == '{' && Comment1()) return NextToken();
            int apx = 0;
            int recKind = noSym;
            int recEnd = pos;
            t = new Token();
            t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
            int state;
            if (start.ContainsKey(ch)) { state = (int)start[ch]; }
            else { state = 0; }
            tlen = 0; AddCh();

            switch (state)
            {
                case -1: { t.kind = eofSym; break; } // NextCh already done
                case 0:
                    {
                        if (recKind != noSym)
                        {
                            tlen = recEnd - t.pos;
                            SetScannerBehindT();
                        }
                        t.kind = recKind; break;
                    } // NextCh already done
                case 1:
                    recEnd = pos; recKind = 1;
                    if (ch >= '0' && ch <= '9' || ch >= 'a' && ch <= 'z') { AddCh(); goto case 1; }
                    else { t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }
                case 2:
                    if (ch <= '&' || ch >= '(' && ch <= 65535) { AddCh(); goto case 2; }
                    else if (ch == 39) { AddCh(); goto case 16; }
                    else { goto case 0; }
                case 3:
                    {
                        tlen -= apx;
                        SetScannerBehindT();
                        t.kind = 3; break;
                    }
                case 4:
                    recEnd = pos; recKind = 4;
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 4; }
                    else if (ch == 'e') { AddCh(); goto case 5; }
                    else { t.kind = 4; break; }
                case 5:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 7; }
                    else if (ch == '+' || ch == '-') { AddCh(); goto case 6; }
                    else { goto case 0; }
                case 6:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 7; }
                    else { goto case 0; }
                case 7:
                    recEnd = pos; recKind = 4;
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 7; }
                    else { t.kind = 4; break; }
                case 8:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 10; }
                    else if (ch == '+' || ch == '-') { AddCh(); goto case 9; }
                    else { goto case 0; }
                case 9:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 10; }
                    else { goto case 0; }
                case 10:
                    recEnd = pos; recKind = 4;
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 10; }
                    else { t.kind = 4; break; }
                case 11:
                    { t.kind = 5; break; }
                case 12:
                    { t.kind = 6; break; }
                case 13:
                    { t.kind = 7; break; }
                case 14:
                    { t.kind = 8; break; }
                case 15:
                    recEnd = pos; recKind = 3;
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 15; }
                    else if (ch == '.') { apx++; AddCh(); goto case 17; }
                    else if (ch == 'e') { AddCh(); goto case 8; }
                    else { t.kind = 3; break; }
                case 16:
                    recEnd = pos; recKind = 2;
                    if (ch == 39) { AddCh(); goto case 2; }
                    else { t.kind = 2; break; }
                case 17:
                    if (ch >= '0' && ch <= '9') { apx = 0; AddCh(); goto case 4; }
                    else if (ch == '.') { apx++; AddCh(); goto case 3; }
                    else { goto case 0; }
                case 18:
                    { t.kind = 12; break; }
                case 19:
                    { t.kind = 13; break; }
                case 20:
                    { t.kind = 16; break; }
                case 21:
                    { t.kind = 24; break; }
                case 22:
                    { t.kind = 26; break; }
                case 23:
                    { t.kind = 27; break; }
                case 24:
                    { t.kind = 30; break; }
                case 25:
                    { t.kind = 45; break; }
                case 26:
                    { t.kind = 46; break; }
                case 27:
                    { t.kind = 48; break; }
                case 28:
                    { t.kind = 50; break; }
                case 29:
                    { t.kind = 53; break; }
                case 30:
                    { t.kind = 54; break; }
                case 31:
                    recEnd = pos; recKind = 14;
                    if (ch == '.') { AddCh(); goto case 12; }
                    else { t.kind = 14; break; }
                case 32:
                    recEnd = pos; recKind = 22;
                    if (ch == '=') { AddCh(); goto case 24; }
                    else { t.kind = 22; break; }
                case 33:
                    recEnd = pos; recKind = 47;
                    if (ch == '>') { AddCh(); goto case 26; }
                    else if (ch == '=') { AddCh(); goto case 27; }
                    else { t.kind = 47; break; }
                case 34:
                    recEnd = pos; recKind = 49;
                    if (ch == '=') { AddCh(); goto case 28; }
                    else { t.kind = 49; break; }

            }
            t.val = new String(tval, 0, tlen);
            return t;
        }

        private void SetScannerBehindT()
        {
            buffer.Pos = t.pos;
            NextCh();
            line = t.line; col = t.col; charPos = t.charPos;
            for (int i = 0; i < tlen; i++) NextCh();
        }

        // get the next token (possibly a token already seen during peeking)
        public Token Scan()
        {
            if (tokens.next == null)
            {
                return NextToken();
            }
            else
            {
                pt = tokens = tokens.next;
                return tokens;
            }
        }

        // peek for the next token, ignore pragmas
        public Token Peek()
        {
            do
            {
                if (pt.next == null)
                {
                    pt.next = NextToken();
                }
                pt = pt.next;
            } while (pt.kind > maxT); // skip pragmas

            return pt;
        }

        // make sure that peeking starts at the current scan position
        public void ResetPeek() { pt = tokens; }

    } // end Scanner

public class DetectionResult
{
    public List<string> Vars;
    public List<string> Types;
    public List<string> Consts;
    public List<string> Fields;

    public DetectionResult()
    {
        Vars = new List<string>();
        Types = new List<string>();
        Consts = new List<string>();
        Fields = new List<string>();
    }
}
public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _string = 2;
	public const int _intcon = 3;
	public const int _realcon = 4;
	public const int _semicol = 5;
	public const int _dotdot = 6;
	public const int _plus = 7;
	public const int _minus = 8;
	public const int _case = 9;
	public const int _end = 10;
	public const int maxT = 61;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;

bool FirstOfVariant() {
		int next = Peek().kind;
		return next == _plus || next == _minus  || next == _ident || next == _string
		|| next == _intcon || next == _realcon;
	}

	Token Peek() {
		scanner.ResetPeek();
		return scanner.Peek();
	}



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void Pascal() {
		Expect(11);
		Expect(1);
		if (la.kind == 12) {
			Get();
			IdentList();
			Expect(13);
		}
		Expect(5);
		Block();
		Expect(14);
	}

	void IdentList() {
		Expect(1);
		while (la.kind == 16) {
			Get();
			Expect(1);
		}
	}

	void Block() {
		if (la.kind == 15) {
			Get();
			Label();
			while (la.kind == 16) {
				Get();
				Label();
			}
			Expect(5);
		}
		if (la.kind == 17) {
			Get();
			ConstDecl();
			while (la.kind == 1) {
				ConstDecl();
			}
		}
		if (la.kind == 18) {
			Get();
			TypeDecl();
			while (la.kind == 1) {
				TypeDecl();
			}
		}
		if (la.kind == 19) {
			Get();
			VarDecl();
			while (la.kind == 1) {
				VarDecl();
			}
		}
		while (la.kind == 20 || la.kind == 21) {
			if (la.kind == 20) {
				Get();
				Expect(1);
				if (la.kind == 12) {
					FormParList();
				}
				Expect(5);
				if (StartOf(1)) {
					Block();
				} else if (la.kind == 1) {
					Directive();
				} else SynErr(62);
				Expect(5);
			} else {
				Get();
				Expect(1);
				if (la.kind == 12) {
					FormParList();
				}
				if (la.kind == 22) {
					Get();
					Expect(1);
				}
				Expect(5);
				if (StartOf(1)) {
					Block();
				} else if (la.kind == 1) {
					Directive();
				} else SynErr(63);
				Expect(5);
			}
		}
		Expect(23);
		StatementSeq();
		Expect(10);
	}

	void Label() {
	Expect(3);
	}

	void ConstDecl() {
        detect.Fields.Add(la.val);
		Expect(1);
		Expect(24);
		Constant();
		Expect(5);
	}
    public DetectionResult detect;
	void TypeDecl() {
        detect.Types.Add(la.val);
		Expect(1);
		Expect(24);
		Type();
		Expect(5);
	}

	void VarDecl() {
        detect.Vars.Add(la.val);
		IdentList();
		Expect(22);
		Type();
		Expect(5);
	}

	void FormParList() {
		Expect(12);
		FormParSect();
		while (la.kind == 5) {
			Get();
			FormParSect();
		}
		Expect(13);
	}

	void Directive() {
		Expect(1);
	}

	void StatementSeq() {
		Statement();
		while (la.kind == 5) {
			Get();
			Statement();
		}
	}

	void Constant() {
		if (StartOf(2)) {
			if (la.kind == 7 || la.kind == 8) {
				if (la.kind == 7) {
					Get();
				} else {
					Get();
				}
			}
			if (la.kind == 1) {
				Get();
			} else if (la.kind == 3) {
				Get();
			} else if (la.kind == 4) {
				Get();
			} else SynErr(64);
		} else if (la.kind == 2) {
			Get();
		} else SynErr(65);
	}

	void Type() {
		if (StartOf(3)) {
			SimpleType();
		} else if (la.kind == 45) {
			Get();
			Expect(1);
		} else if (StartOf(4)) {
			if (la.kind == 29) {
				Get();
			}
			if (la.kind == 25) {
				Get();
				Expect(26);
				SimpleType();
				while (la.kind == 16) {
					Get();
					SimpleType();
				}
				Expect(27);
				Expect(28);
				Type();
			} else if (la.kind == 58) {
				Get();
				if (la.kind == 1 || la.kind == 9) {
					FieldList();
				}
				Expect(10);
			} else if (la.kind == 59) {
				Get();
				Expect(28);
				Type();
			} else if (la.kind == 60) {
				Get();
				Expect(28);
				Type();
			} else SynErr(66);
		} else SynErr(67);
	}

	void FormParSect() {
		if (la.kind == 1 || la.kind == 19) {
			if (la.kind == 19) {
				Get();
			}
			IdentList();
			Expect(22);
			if (la.kind == 1) {
				Get();
			} else if (la.kind == 25 || la.kind == 29) {
				ConformantArray();
			} else SynErr(68);
		} else if (la.kind == 20) {
			Get();
			Expect(1);
			if (la.kind == 12) {
				FormParList();
			}
		} else if (la.kind == 21) {
			Get();
			Expect(1);
			if (la.kind == 12) {
				FormParList();
			}
			Expect(22);
			Expect(1);
		} else SynErr(69);
	}

	void ConformantArray() {
		if (la.kind == 25) {
			Get();
			Expect(26);
			Bounds();
			while (la.kind == 5) {
				Get();
				Bounds();
			}
			Expect(27);
			Expect(28);
			if (la.kind == 1) {
				Get();
			} else if (la.kind == 25 || la.kind == 29) {
				ConformantArray();
			} else SynErr(70);
		} else if (la.kind == 29) {
			Get();
			Expect(25);
			Expect(26);
			Bounds();
			Expect(27);
			Expect(28);
			Expect(1);
		} else SynErr(71);
	}

	void Bounds() {
		Expect(1);
		Expect(6);
		Expect(1);
		Expect(22);
		Expect(1);
	}

	void Statement() {
		if (la.kind == 3) {
			Label();
			Expect(22);
		}
		if (StartOf(5)) {
			switch (la.kind) {
			case 1: {
				Designator();
				if (la.kind == 30) {
					Get();
					Expr();
				} else if (StartOf(6)) {
					if (la.kind == 12) {
						ActParList();
					}
				} else SynErr(72);
				break;
			}
			case 31: {
				Get();
				Label();
				break;
			}
			case 23: {
				Get();
				StatementSeq();
				Expect(10);
				break;
			}
			case 32: {
				Get();
				Expr();
				Expect(33);
				Statement();
				break;
			}
			case 34: {
				Get();
				StatementSeq();
				Expect(35);
				Expr();
				break;
			}
			case 36: {
				Get();
				Expect(1);
				Expect(30);
				Expr();
				if (la.kind == 37) {
					Get();
				} else if (la.kind == 38) {
					Get();
				} else SynErr(73);
				Expr();
				Expect(33);
				Statement();
				break;
			}
			case 39: {
				Get();
				Expr();
				Expect(40);
				Statement();
				if (la.kind == 41) {
					Get();
					Statement();
				}
				break;
			}
			case 9: {
				Get();
				Expr();
				Expect(28);
				Case();
				while (la.kind == _semicol && Peek().kind != _end) {
					Expect(5);
					Case();
				}
				if (la.kind == 5) {
					Get();
				}
				Expect(10);
				break;
			}
			case 42: {
				Get();
				Designator();
				while (la.kind == 16) {
					Get();
					Designator();
				}
				Expect(33);
				Statement();
				break;
			}
			}
		}
	}

	void Designator() {
		Expect(1);
		while (la.kind == 14 || la.kind == 26 || la.kind == 45) {
			if (la.kind == 26) {
				Get();
				Expr();
				while (la.kind == 16) {
					Get();
					Expr();
				}
				Expect(27);
			} else if (la.kind == 14) {
				Get();
				Expect(1);
			} else {
				Get();
			}
		}
	}

	void Expr() {
		SimExpr();
		if (StartOf(7)) {
			RelOp();
			SimExpr();
		}
	}

	void ActParList() {
		Expect(12);
		ActPar();
		while (la.kind == 16) {
			Get();
			ActPar();
		}
		Expect(13);
	}

	void Case() {
		Constant();
		while (la.kind == 16) {
			Get();
			Constant();
		}
		Expect(22);
		Statement();
	}

	void ActPar() {
		Expr();
		if (la.kind == 22) {
			Get();
			Expr();
			if (la.kind == 22) {
				Get();
				Expr();
			}
		}
	}

	void SimExpr() {
		if (la.kind == 7 || la.kind == 8) {
			if (la.kind == 7) {
				Get();
			} else {
				Get();
			}
		}
		Term();
		while (la.kind == 7 || la.kind == 8 || la.kind == 52) {
			AddOp();
			Term();
		}
	}

	void RelOp() {
		switch (la.kind) {
		case 24: {
			Get();
			break;
		}
		case 46: {
			Get();
			break;
		}
		case 47: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		case 49: {
			Get();
			break;
		}
		case 50: {
			Get();
			break;
		}
		case 51: {
			Get();
			break;
		}
		default: SynErr(74); break;
		}
	}

	void Term() {
		Factor();
		while (StartOf(8)) {
			MultOp();
			Factor();
		}
	}

	void AddOp() {
		if (la.kind == 7) {
			Get();
		} else if (la.kind == 8) {
			Get();
		} else if (la.kind == 52) {
			Get();
		} else SynErr(75);
	}

	void Factor() {
		switch (la.kind) {
		case 1: {
			Designator();
			if (la.kind == 12) {
				ActParList();
			}
			break;
		}
		case 3: {
			Get();
			break;
		}
		case 4: {
			Get();
			break;
		}
		case 2: {
			Get();
			break;
		}
		case 26: {
			Set();
			break;
		}
		case 43: {
			Get();
			break;
		}
		case 12: {
			Get();
			Expr();
			Expect(13);
			break;
		}
		case 44: {
			Get();
			Factor();
			break;
		}
		default: SynErr(76); break;
		}
	}

	void MultOp() {
		if (la.kind == 53) {
			Get();
		} else if (la.kind == 54) {
			Get();
		} else if (la.kind == 55) {
			Get();
		} else if (la.kind == 56) {
			Get();
		} else if (la.kind == 57) {
			Get();
		} else SynErr(77);
	}

	void Set() {
		Expect(26);
		if (StartOf(9)) {
			Elem();
			while (la.kind == 16) {
				Get();
				Elem();
			}
		}
		Expect(27);
	}

	void Elem() {
		Expr();
		if (la.kind == 6) {
			Get();
			Expr();
		}
	}

	void SimpleType() {
		if (la.kind == _ident && Peek().kind != _dotdot) {
			Expect(1);
		} else if (StartOf(10)) {
			Constant();
			Expect(6);
			Constant();
		} else if (la.kind == 12) {
			Get();
			IdentList();
			Expect(13);
		} else SynErr(78);
	}

	void FieldList() {
		if (la.kind == 1) {
			FixedPart();
			if (la.kind == _semicol && Peek().kind == _case) {
				Expect(5);
				VariantPart();
			}
		} else if (la.kind == 9) {
			VariantPart();
		} else SynErr(79);
		if (la.kind == 5) {
			Get();
		}
	}

	void FixedPart() {
		FieldDecl();
		while (la.kind == _semicol && Peek().kind == _ident) {
			Expect(5);
			FieldDecl();
		}
	}

	void VariantPart() {
		Expect(9);
		Expect(1);
		if (la.kind == 22) {
			Get();
			Expect(1);
		}
		Expect(28);
		Variant();
		while (la.kind == _semicol && FirstOfVariant()) {
			Expect(5);
			Variant();
		}
	}

	void FieldDecl() {
        detect.Fields.Add(la.val);
		IdentList();
		Expect(22);
		Type();
	}

	void Variant() {
		Constant();
		while (la.kind == 16) {
			Get();
			Constant();
		}
		Expect(22);
		Expect(12);
		if (la.kind == 1 || la.kind == 9) {
			FieldList();
		}
		Expect(13);
	}



	public void Parse() {
        detect = new DetectionResult();
		la = new Token();
		la.val = "";		
		Get();
		Pascal();
		Expect(0);

	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,T,T, T,T,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,x,x,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,x,x},
		{x,T,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,x,T, T,x,T,x, T,x,x,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,T,x,x, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,x,x, x,x,x},
		{x,T,T,T, T,x,x,T, T,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x}

	};
} // end Parser


public class Errors {
    public Errors()
    {
        Messages = new List<CompileMessage>();
    }
	public int count = 0;                                    // number of errors detected
    public List<CompileMessage> Messages;

	public virtual void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "string expected"; break;
			case 3: s = "intcon expected"; break;
			case 4: s = "realcon expected"; break;
			case 5: s = "semicol expected"; break;
			case 6: s = "dotdot expected"; break;
			case 7: s = "plus expected"; break;
			case 8: s = "minus expected"; break;
			case 9: s = "case expected"; break;
			case 10: s = "end expected"; break;
			case 11: s = "\"program\" expected"; break;
			case 12: s = "\"(\" expected"; break;
			case 13: s = "\")\" expected"; break;
			case 14: s = "\".\" expected"; break;
			case 15: s = "\"uses\" expected"; break;
			case 16: s = "\",\" expected"; break;
			case 17: s = "\"const\" expected"; break;
			case 18: s = "\"type\" expected"; break;
			case 19: s = "\"var\" expected"; break;
			case 20: s = "\"procedure\" expected"; break;
			case 21: s = "\"function\" expected"; break;
			case 22: s = "\":\" expected"; break;
			case 23: s = "\"begin\" expected"; break;
			case 24: s = "\"=\" expected"; break;
			case 25: s = "\"array\" expected"; break;
			case 26: s = "\"[\" expected"; break;
			case 27: s = "\"]\" expected"; break;
			case 28: s = "\"of\" expected"; break;
			case 29: s = "\"packed\" expected"; break;
			case 30: s = "\":=\" expected"; break;
			case 31: s = "\"goto\" expected"; break;
			case 32: s = "\"while\" expected"; break;
			case 33: s = "\"do\" expected"; break;
			case 34: s = "\"repeat\" expected"; break;
			case 35: s = "\"until\" expected"; break;
			case 36: s = "\"for\" expected"; break;
			case 37: s = "\"to\" expected"; break;
			case 38: s = "\"downto\" expected"; break;
			case 39: s = "\"if\" expected"; break;
			case 40: s = "\"then\" expected"; break;
			case 41: s = "\"else\" expected"; break;
			case 42: s = "\"with\" expected"; break;
			case 43: s = "\"nil\" expected"; break;
			case 44: s = "\"not\" expected"; break;
			case 45: s = "\"^\" expected"; break;
			case 46: s = "\"<>\" expected"; break;
			case 47: s = "\"<\" expected"; break;
			case 48: s = "\"<=\" expected"; break;
			case 49: s = "\">\" expected"; break;
			case 50: s = "\">=\" expected"; break;
			case 51: s = "\"in\" expected"; break;
			case 52: s = "\"or\" expected"; break;
			case 53: s = "\"*\" expected"; break;
			case 54: s = "\"/\" expected"; break;
			case 55: s = "\"div\" expected"; break;
			case 56: s = "\"mod\" expected"; break;
			case 57: s = "\"and\" expected"; break;
			case 58: s = "\"record\" expected"; break;
			case 59: s = "\"set\" expected"; break;
			case 60: s = "\"file\" expected"; break;
			case 61: s = "??? expected"; break;
			case 62: s = "invalid Block"; break;
			case 63: s = "invalid Block"; break;
			case 64: s = "invalid Constant"; break;
			case 65: s = "invalid Constant"; break;
			case 66: s = "invalid Type"; break;
			case 67: s = "invalid Type"; break;
			case 68: s = "invalid FormParSect"; break;
			case 69: s = "invalid FormParSect"; break;
			case 70: s = "invalid ConformantArray"; break;
			case 71: s = "invalid ConformantArray"; break;
			case 72: s = "invalid Statement"; break;
			case 73: s = "invalid Statement"; break;
			case 74: s = "invalid RelOp"; break;
			case 75: s = "invalid AddOp"; break;
			case 76: s = "invalid Factor"; break;
			case 77: s = "invalid MultOp"; break;
			case 78: s = "invalid SimpleType"; break;
			case 79: s = "invalid FieldList"; break;

			default: s = "error " + n; break;
		}
	Messages.Add(new CompileMessage(line, col, s, CompileMessage.MessageTypes.Info));
		count++;
	}

	public virtual void SemErr (int line, int col, string s) {
        Messages.Add(new CompileMessage(line, col, s, CompileMessage.MessageTypes.Info));
		count++;
	}
	
	public virtual void SemErr (string s) {
        Messages.Add(new CompileMessage(0, 0, s, CompileMessage.MessageTypes.Info));
		count++;
	}
	
	public virtual void Warning (int line, int col, string s) {
        Messages.Add(new CompileMessage(line, col, s, CompileMessage.MessageTypes.Note));
	}
	
	public virtual void Warning(string s) {
        Messages.Add(new CompileMessage(0, 0, s, CompileMessage.MessageTypes.Note));
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}
}