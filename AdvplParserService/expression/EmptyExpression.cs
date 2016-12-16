using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class EmptyExpression : Expression
    {
        public EmptyExpression(ParserRuleContext ctx) : base(ctx)        {
            
            Content = "";
        }
    }
}
