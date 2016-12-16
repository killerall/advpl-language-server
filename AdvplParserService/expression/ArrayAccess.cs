using Antlr4.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class ArrayAccess : Expression
    {
        private ArrayList lists {get;set;}
        public ArrayAccess(ParserRuleContext ctx) : base(ctx)
        {
            
        }
    }
}
