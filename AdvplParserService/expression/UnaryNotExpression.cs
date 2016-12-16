using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class UnaryNotExpression : UnaryExpression
    {
        public UnaryNotExpression(ParserRuleContext ctx, Expression e) : base(ctx, ExpressionConstants.E_LNOT, e)
        {
        }
    }
}
