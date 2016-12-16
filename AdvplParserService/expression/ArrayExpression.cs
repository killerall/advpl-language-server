using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advpl_parser.expression
{
    class ArrayExpression : ExpressionList
    {
        public ArrayExpression(ParserRuleContext ctx) :base(ctx)
        {

        }
    }
    /*	
	@Override
	public Object toJava(){
		ArrayList<Object> arrayList = new ArrayList<Object>();
		
		Iterator<Expression> iterator = expressions.iterator();		
		while (iterator.hasNext()){
			Expression exp = iterator.next();
			
			if (exp instanceof ArrayExpression){
				arrayList.add(toArrayList((ArrayExpression) exp));
			}else{
				arrayList.add(exp.toJava());
			}
		}
		
		return arrayList;
	}
	
	private ArrayList<Object> toArrayList(ArrayExpression expArray){		
		ArrayList<Object> arrayList = new ArrayList<Object>();
		
		Iterator<Expression> iterator = expArray.getExpressions().iterator();		
		while (iterator.hasNext()){
			Expression exp = iterator.next();
			
			if (exp instanceof ArrayExpression){
				arrayList.add(toArrayList((ArrayExpression) exp));
			}else{
				arrayList.add(exp.toJava());
			}
		}
		
		return arrayList;
		
	}*/
}
