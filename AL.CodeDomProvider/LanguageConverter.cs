using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.AL;

namespace ALCodeDomProvider
{
    public class LanguageConverter
    {
        Dictionary<string, string> CONVTABLE;
        Dictionary<string, string> REVCONVTABLE;
        public LanguageConverter()
        {
            CONVTABLE = new Dictionary<string, string>();
            REVCONVTABLE = new Dictionary<string, string>();
            CONVTABLE.Add("base", "mybase");
            CONVTABLE.Add("bool", "boolean");
            CONVTABLE.Add("break", "exit");
            CONVTABLE.Add("case", "val");
            CONVTABLE.Add("const", "constant");
            CONVTABLE.Add("continue", "persist");
            CONVTABLE.Add("double", "real");
            CONVTABLE.Add("enum", "enumerator");
            CONVTABLE.Add("return", "backwith");
            CONVTABLE.Add("sealed", "final");
            CONVTABLE.Add("short", "shortint");
            CONVTABLE.Add("int", "integer");
            CONVTABLE.Add("long", "longint");
            CONVTABLE.Add("namespace", "program");
            CONVTABLE.Add("switch", "match");
            CONVTABLE.Add("null", "nothing");
            CONVTABLE.Add("lock", "secure");
            CONVTABLE.Add("this", "self");
            CONVTABLE.Add("using", "include");
            CONVTABLE.Add("uint", "uinteger");
            CONVTABLE.Add("ushort", "ushortint");
            CONVTABLE.Add("ulong", "ulongint");
            CONVTABLE.Add("set", "put");
            CONVTABLE.Add("void", "sub");
            CONVTABLE.Add("extern", "external");
            CONVTABLE.Add("partial", "shared");
            //CONVTABLE.Add("{", "begin");
            //CONVTABLE.Add("}", "end");

            foreach (KeyValuePair<string, string> P in CONVTABLE)
                REVCONVTABLE.Add(P.Value, P.Key);
        }
        string[] PP = new string[12] {
            
    "#if",
   "#else",
    "#elif",
    "#endif",
    "#define",
    "#undef",
    "#warning",
    "#error",
    "#line",
    "#pragma",
    "#region",
    "#endregion"


        };

        List<int> PPLines = new List<int>();
        List<string> PPCode = new List<string>();
        bool ContainsPP(string s)
        {
            bool c = false;
            int i = 0;
            while (i < 12 && !c)
            {
                c = s.Contains(PP[i]);
                i++;
            }
            return c;
        }
        string RemovePP(string code)
        {
            PPLines.Clear();
            PPCode.Clear();
            string c = "";
            string[] l = code.Split('\n');
            for (int i = 0; i < l.Length; i++)
            {
                if (!ContainsPP(l[i]))
                    c += l[i] + "\n";
                else
                {
                    PPLines.Add(i);
                    PPCode.Add(l[i]);
                }
            }
            return c;
        }
        string InsertPP(string code)
        {
            if (PPLines.Count > 0)
            {
                string[] l = code.Split('\n');
                string c = "";
                int cpp = 0;
                for (int i = 0; i < l.Length; i++)
                {
                    if (i == PPLines[cpp])
                    {
                        c += PPCode[cpp] + "\n" + l[i] + "\n";
                        if (cpp != PPCode.Count - 1)
                            cpp++;
                    }
                    else c += l[i] + "\n";
                }
                return c;
            }
            else
                return code;
  }

        List<ICSharpCode.NRefactory.CSharp.AstNode> k = new List<ICSharpCode.NRefactory.CSharp.AstNode>();
        List<AstNode> p;
        void ExtractTokens(AstNode nod)
        {
            if (nod.NodeType == NodeType.Token || nod.GetType().Name == "PrimitiveType" || nod.GetType().Name == "BaseReferenceExpression" || nod.GetType().Name == "ThisReferenceExpression" || nod.GetType().Name == "NullReferenceExpression")
                p.Add(nod);
            if(nod.HasChildren)
            foreach (AstNode no in nod.Children)
                ExtractTokens(no);
        }

        void ExtractTokensCS(ICSharpCode.NRefactory.CSharp.AstNode nod)
        {
            if (nod.NodeType == ICSharpCode.NRefactory.CSharp.NodeType.Token || nod.GetType().Name == "PrimitiveType" || nod.GetType().Name == "BaseReferenceExpression" || nod.GetType().Name == "ThisReferenceExpression" || nod.GetType().Name == "NullReferenceExpression")
                k.Add(nod);
            if (nod.HasChildren)
                foreach (ICSharpCode.NRefactory.CSharp.AstNode no in nod.Children)
                    ExtractTokensCS(no);
        }
        public string InterpretAlToCs(string al)
        {
            string cs = al;
          //  cs = RemovePP(cs);
          ICSharpCode.NRefactory.AL.ALParser  alp = new ICSharpCode.NRefactory.AL.ALParser();
          SyntaxTree tree =  alp.Parse(cs, "");
          p = new List<AstNode>();
          foreach (AstNode nd in tree.Children)
              ExtractTokens(nd);
            // Convert
    
             int minus =1;
          string[] l = cs.Split('\n');
          foreach (AstNode node in p)
          {
              if (REVCONVTABLE.ContainsKey(node.ToString()))
              {

                  //cs = cs.Remove(to.Location.Position, to.ValueString.Length);
                  //cs = cs.Insert(to.Location.Position, REVCONVTABLE[to.ValueString]);

                  string ln = l[node.StartLocation.Line - minus];
                  int location = ln.IndexOf(node.ToString());

                  ln = ln.Remove(location, node.ToString().Length);
                  ln = ln.Insert(location, REVCONVTABLE[node.ToString()]);
                  if (location != node.StartLocation.Column)
                      l[node.StartLocation.Line - minus] = ln;
                  else l[node.StartLocation.Line - minus] = ln;

              }
          }
          string c = "";
          for (int i = 0; i < l.Length; i++)
              c += l[i] + "\n";

          return InsertPP(c);

        }
        public string InterpretCsToAl(string al)
        {
            string cs = al;
            //  cs = RemovePP(cs);
            ICSharpCode.NRefactory.CSharp.CSharpParser alp = new ICSharpCode.NRefactory.CSharp.CSharpParser();
           ICSharpCode.NRefactory.CSharp.SyntaxTree tree = alp.Parse(cs, "");
           k = new List<ICSharpCode.NRefactory.CSharp.AstNode>();
            foreach (ICSharpCode.NRefactory.CSharp.AstNode nd in tree.Children)
                ExtractTokensCS(nd);
            // Convert

            int minus = 1;
            string[] l = cs.Split('\n');
            foreach (ICSharpCode.NRefactory.CSharp.AstNode node in k)
            {
                if (CONVTABLE.ContainsKey(node.ToString()))
                {

                    //cs = cs.Remove(to.Location.Position, to.ValueString.Length);
                    //cs = cs.Insert(to.Location.Position, REVCONVTABLE[to.ValueString]);

                    string ln = l[node.StartLocation.Line - minus];
                    int location = ln.IndexOf(node.ToString());

                    ln = ln.Remove(location, node.ToString().Length);
                    ln = ln.Insert(location, CONVTABLE[node.ToString()]);
                    if (location != node.StartLocation.Column)
                        l[node.StartLocation.Line - minus] = ln;
                    else l[node.StartLocation.Line - minus] = ln;

                }
            }
            string c = "";
            for (int i = 0; i < l.Length; i++)
                c += l[i] + "\n";

            return InsertPP(c);

        }
    }
}
