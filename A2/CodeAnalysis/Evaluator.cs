using System;
namespace A2.CodeAnalysis
{

    class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            if (node is NumberExpressionSyntax n)
            {
                return (int) n.NumberToken.Value;
            }

            if (node is UnaryExpressionSyntax u)
            {
                var operand = EvaluateExpression(u.Operand);

                if (u.OperatorToken.Type == SyntaxType.SubstractToken)
                {
                    return -operand;
                }
                else if (u.OperatorToken.Type == SyntaxType.AdditionToken)
                {
                    return operand;
                }
                else
                    throw new Exception($"Unrecognized unary operator `{u.OperatorToken.Type}`.");

            }

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.LeftToken);
                var right = EvaluateExpression(b.RightToken);

                if (b.OperatorToken.Type == SyntaxType.AdditionToken)
                {
                    return Sum(left, right);
                }
                else if (b.OperatorToken.Type == SyntaxType.SubstractToken)
                {
                    return Substract(left,right);
                }
                else if (b.OperatorToken.Type == SyntaxType.MultiplyToken)
                {
                    return Multiply(left,right);
                }
                else if (b.OperatorToken.Type == SyntaxType.DivisionToken)
                {
                    return Divide(left, right);
                }
                else
                    throw new Exception($"Unrecognized binary operetor {b.OperatorToken.Type}.");
            }

            if(node is ParenthesizedExpressionSyntax p)
            {
                return EvaluateExpression(p.Expression);
            }

            throw new Exception($"Unrecognized node {node.Type}.");
        }
        #region AriphmeticFunctions
        private int Sum( int left, int right)
        {
            return left + right;
        }
        private int Substract( int left, int right)
        {
            return left - right;
        }
        private int Multiply( int left, int right)
        {
            return left * right;
        }
        private int Divide( int left, int right)
        {
            try
            {
                return left / right;
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("Attempted to divide by zero.");
                return 0;
            }
        }
        #endregion
    }

}