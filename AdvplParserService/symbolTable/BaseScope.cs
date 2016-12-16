using System;
using System.Collections.Generic;

namespace advpl_parser.symbolTable
{
    public abstract class BaseScope : Scope
    {
        Scope enclosingScope; // null if global (outermost) scope
        Dictionary<String, Symbol> symbols = new Dictionary<String, Symbol>();

        public BaseScope(Scope parent) {
            this.enclosingScope = parent;
        }
        public void define(Symbol sym)
        {
            symbols.Add(sym.name, sym);
            sym.scope = this; // track the scope in each symbol
        }

        public bool existLocalScope(String name)
        {
            Symbol s;
            symbols.TryGetValue(name, out s);
            return (s != null);
        }
        
        public virtual String getScopeName()
        {
            throw new NotImplementedException();
        }

        public Symbol resolve(String name)
        {
            Symbol s;
            symbols.TryGetValue(name, out s);
            if (s != null) return s;
            // if not here, check any enclosing scope
            if (getParentScope() != null) return getParentScope().resolve(name);
            return null; // not found
        }
        public Scope getParentScope()
        {
            return getEnclosingScope();
        }
        public Scope getEnclosingScope()
        {
            return enclosingScope;
        }
    }
}
