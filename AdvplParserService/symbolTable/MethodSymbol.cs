using System;
using System.Collections.Generic;

namespace advpl_parser.symbolTable
{
    class MethodSymbol : FunctionSymbol
    {
        Dictionary<String, Symbol> orderedArgs = new Dictionary<String, Symbol>();
        private String className;
        public MethodSymbol(String name, Type retType, Scope parent) : base(name, retType, parent)
        {
            
        }

        public Dictionary<String, Symbol> getMembers() { return orderedArgs; }

        public String getClassName()
        {

            return className;
        }

        public void setClassName(String s)
        {

            className = s;
        }

    }
}
