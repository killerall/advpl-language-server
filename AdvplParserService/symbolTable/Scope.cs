using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    /**
     * Interface para um escopo, temos um escopo global, quando criamos uma função/method, criamos
     * uma novo escopo.
     * 
     * @author rodrigo.antonio
     *
     */
    public interface Scope
    {
        String getScopeName();

        /** Define o escopo pai, para o escopo global é nulo*/
        Scope getEnclosingScope();

        /** Define um simbulo no escopo atual */
        void define(Symbol sym);

        /** Procura um nome no proprio escopo, ou no escopo do pai, se não estiver aqui*/
        Symbol resolve(String name);

        bool existLocalScope(String name);
    }
}
