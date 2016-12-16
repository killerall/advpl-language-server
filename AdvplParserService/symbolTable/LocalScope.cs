using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    class LocalScope : BaseScope
    {
        public LocalScope(Scope parent): base(parent)
        {
        }
        public override String getScopeName()
        {
            return "locals";
        }
    }
}
