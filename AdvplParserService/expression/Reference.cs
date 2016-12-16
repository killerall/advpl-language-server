using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class Reference : Expression
    {
        string varName;
        ArrayAccess arrayAccess;
        public Reference(ParserRuleContext ctx) :base(ctx)        {
            
            varName = ctx.GetText();
        }
    }
}
