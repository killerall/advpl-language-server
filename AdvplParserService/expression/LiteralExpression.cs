using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class LiteralExpression : Expression
    {
        public enum TYPES { StringDouble, StringSimple, Number, Logical, Nil }
        private TYPES type;
        public LiteralExpression(TYPES t, ParserRuleContext ctx):base(ctx)
        {            
            type = t;
        }
        public TYPES getType()
        {
            return type;
        }
    }
}
