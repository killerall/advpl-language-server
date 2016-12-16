using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class FunctionCall : Expression
    {
        ExpressionList arg;

        public string name;
        public FunctionCall() : base()
        {

        }
        public FunctionCall(ParserRuleContext ctx, String name) : base(ctx)
        {
            this.name = name;
        }
        public String getName()
        {
            return name.ToUpper();
        }

        public void setArg(ExpressionList nodeArgs)
        {
            arg = nodeArgs;
        }
        
    }
}
