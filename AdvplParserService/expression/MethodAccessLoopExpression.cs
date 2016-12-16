using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class MethodAccessLoopExpression : FunctionCall
    {
        public MethodAccessLoopExpression(ParserRuleContext ctx, String name):base(ctx,name)
        {

        }
    }
}
