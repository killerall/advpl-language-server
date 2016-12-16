using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using System.Collections;

namespace advpl_parser.expression
{
    class ExpressionList : Expression
    {
        protected ArrayList expressions { get; set; }

        public ExpressionList(ParserRuleContext ctx) : base(ctx)
        {
        }
        public void set(ArrayList exp)
        {
            expressions = exp;
        }
        public void add(Expression exp)
        {
            if (expressions == null)
                expressions = new ArrayList();
            expressions.Add(exp);
        }

    }
}
