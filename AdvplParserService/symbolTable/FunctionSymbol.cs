using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    public class FunctionSymbol : Symbol, Scope
    {
        Dictionary<String, Symbol> arguments = new Dictionary<String, Symbol>();
        ArrayList paramNames;
        Scope enclosingScope;
        public int startFuncionPos { get; set; }
        public int endFuncionPos { get; set; }

        public FunctionSymbol(String name, Type retType, Scope enclosingScope) :base(name, retType)
        {   
            this.enclosingScope = enclosingScope;
            paramNames = new ArrayList();
        }

        public void define(Symbol sym)
        {
            arguments.Add(sym.name, sym);
            sym.scope = this; // track the scope in each symbol
        }

        public bool existLocalScope(string name)
        {
            Symbol s;
            arguments.TryGetValue(name,out s);
            return (s != null);
        }
        public void addParam(String name)
        {
            paramNames.Add(name);
        }
        /*public String[] getParam()
        {
            return paramNames.toArray(new String[paramNames.Size()]);
        }*/

        public Scope getEnclosingScope()
        {
            return enclosingScope;
        }

        public string getScopeName()
        {
            return name;
        }

        public Symbol resolve(string name)
        {
            Symbol s;
            arguments.TryGetValue(name, out s);
            if (s != null) return s;
            // if not here, check any enclosing scope
            if (getEnclosingScope() != null)
            {
                return getEnclosingScope().resolve(name);
            }
            return null; // not found
        }

        public ArrayList getVariables()
        {
            /*Map<Symbol> list = arguments.values();
            Iterator<Symbol> it = list.iterator();
            ArrayList<VariableSymbol> returnList = new ArrayList<VariableSymbol>();
            while (it.hasNext())
            {
                Symbol syb = it.next();
                if (syb instanceof VariableSymbol)
				returnList.add((VariableSymbol)syb);
            }
            */
            return null;//  returnList;

        }
    }
}
