using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVP
{
    /// <summary>
    ///  Code Visual Presentation Instruction
    /// </summary>
    public struct CVPInstruction
    {
        public CVPINS Instruction;
        public int Length;
        public byte[] Data;
        public int Line;
        public int Column;

        public CVPInstruction(byte ins, byte[] data,int ln,int col)
        {
            Instruction = (CVPINS)ins;
            Length = data.Length;
            Data = data;
            Line = ln;
            Column = col;
        }

    }
}
