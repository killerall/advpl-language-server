using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class Expression
    {
        public String Content { get; set; }
        public ParserRuleContext Context { get; set; }

        public Expression(ParserRuleContext ctx)
        {
            Context = ctx;
            Content = ctx.GetText();
        }
        public Expression()
        {
            
        }

    }
}
