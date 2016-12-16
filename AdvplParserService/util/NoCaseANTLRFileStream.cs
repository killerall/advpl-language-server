using System;
using Antlr4.Runtime;
using System.IO;

namespace advpl_parser.util
{
    class NoCaseANTLRFileStream : AntlrFileStream    
    {

        protected char[] lookaheadData;
        public NoCaseANTLRFileStream(String fileName) : base(fileName)
        {
            string text = File.ReadAllText(fileName);
            lookaheadData = text.ToUpper().ToCharArray();
        }
        public int getP()
        {
            return p;
        }
        
        public override int La(int i)
        {
            if (i == 0)
            {
                return 0; // undefined
            }
            if (i < 0)
            {
                i++; // e.g., translate LA(-1) to use offset 0
                if ((p + i - 1) < 0)
                {
                    return IntStreamConstants.Eof; // invalid; no char before first char
                }

            }

            if ((p + i - 1) >= n)
            {

                return IntStreamConstants.Eof;
            }
            return lookaheadData[p + i - 1];
        }


    }


}
