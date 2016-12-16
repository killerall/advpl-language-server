using advpl_parser.expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.symbolTable
{
    class VariableSymbol : Symbol
    {
        private ArrayList m_expressions { get; set; } = new ArrayList();
        public String AssumeType {get;set;}
        private VariableSymbol viculado;

        public VariableSymbol(String name, Type type) : base(name, type)
        {
            AssumeType = "";
        }
        public bool hasExpression()
        {
            return m_expressions.Count > 0;
        }

        public void AddExpression(Expression exp)
        {
            m_expressions.Add(exp);
        }
	
    }
}
