using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class UnaryExpression : Expression
    {
        public bool prefix = true;
        protected Expression expr;
        protected int kind;
        public UnaryExpression():base()
        {
         
        }
        public UnaryExpression(ParserRuleContext ctx, int k, Expression e):base(ctx)
        {   
            kind = k;
            expr = e;

        }
        public String getOperator()
        {
            String op = null;
            switch (kind)
            {
                case ExpressionConstants.E_INC:
                    op = "++";
                    break;
                case ExpressionConstants.E_DEC:
                    op = "--";
                    break;
                case ExpressionConstants.E_LNOT:
                    op = "!";
                    break;

            }
            return op;
        }
    }
}
