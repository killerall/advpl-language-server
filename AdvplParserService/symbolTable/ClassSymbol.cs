using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    class ClassSymbol : ScopedSymbol, Scope, Type
    {
        /** This is the superclass not enclosingScope field. We still record
        *  the enclosing scope so we can push in and pop out of class defs.
        */
        ClassSymbol superClass;
        /** List of all fields and methods */
        public Dictionary<String, Symbol> members = new Dictionary<String, Symbol>();


        public ClassSymbol(String name, Scope enclosingScope, ClassSymbol superClass) :base (name, enclosingScope)
        {   
            this.superClass = superClass;
        }

        public override Scope getParentScope()
        {
            if (superClass == null)
                return base.getEnclosingScope(); // globals
            return superClass; // if not root object, return super
        }
        public void addMember(String name, Symbol syb)
        {
            this.members.Add(name, syb);
        }
        public string getName()
        {
            throw new NotImplementedException();
        }
        /** For a.b, only look in a's class hierarchy to resolve b, not globals */
        public Symbol resolveMember(String name)
        {
            Symbol s;
            members.TryGetValue(name, out s);
            if (s != null) return s;
            // if not here, check just the superclass chain
            if (superClass != null)
            {
                return superClass.resolveMember(name);
            }
            return null; // not found
        }

        public override Dictionary<string, Symbol> getMembers()
        {
            return members;
        }
    }
}
