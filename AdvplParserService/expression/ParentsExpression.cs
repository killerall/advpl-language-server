using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static advpl_language_server.AdvplParserService.AdvplParser;

namespace advpl_parser.expression
{
    class ParentsExpression : Expression
    {
        public ExpressionList expression { get; set; }
        public ParentsExpression(ParensContext ctx) : base(ctx)
        {
        }
    }
}
