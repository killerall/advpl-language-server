using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace advpl_parser.util
{
    class NoCaseAntlrStringStream: AntlrInputStream
    {
        protected char[] lookaheadData;
        public NoCaseAntlrStringStream(string text) : base(text)
        {
            
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
