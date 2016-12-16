using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    public class Symbol
    {
        public enum Type { tINVALID, tUSER, tSTATIC, tMAIN, tLOCAL, tARGUMENT, tFunction, tPRIVATE, tDEFINE, tALIAS, tDATA, tMETHOD }

        public ParserRuleContext context_create { get; set; }

        public String name { get; }      // All symbols at least have a name
        Type type { get; }
        public Scope scope { get; set; }              // All symbols know what scope contains them.
        String nameinsource { get; }      // All symbols at least have a name

        public Symbol(String name)
        {
            this.name = name.ToUpper();
            nameinsource = name;
        }
        public Symbol(String name, Type type) :this(name)
        {
            this.type = type;
        }
        public void setContext(ParserRuleContext ctx)
        {
            context_create = ctx;
        }
    }
}
