using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using System.Collections;

using Antlr4.Runtime.Misc;

namespace advpl_parser.util
{
   

    class AdvplErrorListener : BaseErrorListener
    {
        public ArrayList Errors;
        public AdvplErrorListener ()
        {
            Errors = new ArrayList();
        }
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            
            AdvplError info = new AdvplError();
            info.Line = line;
            
            info.Column = charPositionInLine;
            info.TokenSize = offendingSymbol.Text.Length;
            
            if (msg.Contains("missing {':', 'TO', 'NEXT', 'END', 'SELF', 'PROJECT', 'DEFAULT', 'ASSUME', 'DATA', 'WSMETHOD', 'DESCRIPTION', 'AS', 'OF', IDENTIFIER}"))
                info.Message = "missing IDENTIFIER";
            else
                info.Message = msg;
            Errors.Add(info);


        }

    }
}
