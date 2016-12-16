using Antlr4.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class Identifier : Expression
    {
        public String name;
        public Identifier(ParserRuleContext ctx) :base(ctx)
        {
            
            name = ctx.GetText();
        }
        private ArrayList expres;
        public void set(ArrayList expres)
        {
            this.expres = expres;

        }
    }
}
