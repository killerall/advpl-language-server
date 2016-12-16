using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class Argument : ExpressionList
    {
        public Argument(ParserRuleContext ctx) :base(ctx)
        {

        }
    }
}
