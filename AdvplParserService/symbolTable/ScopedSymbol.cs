using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    public abstract class ScopedSymbol : Symbol, Scope
    {
        Scope enclosingScope;

        public ScopedSymbol(string name, Type type, Scope enclosingScope) : base(name, type)
        {
            this.enclosingScope = enclosingScope;
        }
        public ScopedSymbol(string name,Scope enclosingScope) : base(name)
        {
            this.enclosingScope = enclosingScope;
        }

        public void define(Symbol sym)
        {   
            getMembers().Add(name, sym);
            sym.scope = this; // track the scope in each symbol
        }

        public bool existLocalScope(string name)
        {
            Symbol s;
            getMembers().TryGetValue(name, out s);
            return (s != null);
        }
        
        public string getScopeName()
        {
            throw new NotImplementedException();
        }

        public Symbol resolve(string name)
        {
            Symbol s;
            getMembers().TryGetValue (name, out s);
            if (s != null)
                return s;
            if (getParentScope() != null)
            {
                return getParentScope().resolve(name);
            }

            return null; // not found
        }
        public virtual Scope getParentScope() { return getEnclosingScope(); }
        public virtual Scope getEnclosingScope() { return enclosingScope; }

        /** Indicate how subclasses store scope members. Allows us to
     *  factor out common code in this class.
     */
        public abstract Dictionary<String, Symbol> getMembers();
    }
}
