
using advpl_language_server.AdvplParserService;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    class CheckSymbols
    {
        public static Symbol.Type getType(int tokenType)
        {
            switch (tokenType)
            {
                case -1: return Symbol.Type.tFunction;
                case AdvplParser.USER: return Symbol.Type.tUSER;
                case AdvplParser.STATIC: return Symbol.Type.tSTATIC;
                case AdvplParser.MAIN: return Symbol.Type.tMAIN;
            }
            return Symbol.Type.tINVALID;
        }

        public static void error(IToken t, String msg)
        {
            /*System.err.printf("line %d:%d %s\n", t.getLine(), t.getCharPositionInLine(),
                              msg);*/
        }
    }
}
