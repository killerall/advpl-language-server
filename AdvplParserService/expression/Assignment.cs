using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class Assignment : BinaryExpression
    {
        public Assignment(Expression left, Expression right) :base(left, ExpressionConstants.E_ASSIGN, right)
        {

        }
    }
}
