using System;
using Antlr4.Runtime;

namespace advpl_parser.expression
{
    class BinaryExpression : Expression
    {
        private Expression left { get; set; }
        private Expression right { get; set; }

        protected int kind;

        public BinaryExpression(ParserRuleContext ctx) : base(ctx)
        {
        }
        public BinaryExpression(Expression left, int k, Expression right) :base()
        {            
            this.left = left;
            this.right = right;
            this.kind = k;
        }
        public BinaryExpression() :base()
        {
            
        }
        public bool isLeftEmpty()
        {
            return left == null;
        }
        public bool isRightEmpty()
        {   
            return right == null;
        }
        public String getOperator()
        {
            String op = null;
            switch (kind)
            {
                case ExpressionConstants.E_ASSIGN:
                    op = ":=";
                    break;
                case ExpressionConstants.E_PLUS:
                    op = "+";
                    break;
                case ExpressionConstants.E_MINUS:
                    op = "-";
                    break;
                case ExpressionConstants.E_MULT:
                    op = "*";
                    break;
                case ExpressionConstants.E_DIV:
                    op = "/";
                    break;
                case ExpressionConstants.E_MOD:
                    op = "%";
                    break;
                case ExpressionConstants.E_LAND:
                    op = ".And.";
                    break;
                case ExpressionConstants.E_LOR:
                    op = ".Or.";
                    break;
                case 501:
                    op = "->";
                    break;

            }
            return op;
        }
        public String getContent()
        {
            String op = getOperator();
            return left.Content + op + right.Content;
        }
    }
}
