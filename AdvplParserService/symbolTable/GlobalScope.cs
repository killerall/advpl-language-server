using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static advpl_language_server.AdvplParserService.AdvplParser;

namespace advpl_parser.symbolTable
{
    class GlobalScope : BaseScope
    {
        ProgramContext m_programContext { get; set; }
        public GlobalScope(Scope enclosingScope, ProgramContext ctx) : base(enclosingScope)
        {   
            m_programContext = ctx;
        }
        public override String getScopeName()
        {
            return "globals";
        }
    }

   
}
