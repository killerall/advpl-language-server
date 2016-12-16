using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class CommaExpression : Expression
    {
        public CommaExpression(ParserRuleContext ctx):base (ctx)
        { }
    }
}
