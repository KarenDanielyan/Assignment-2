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
                else if (b.OperatorToken.Type == SyntaxType.PowerToken)
                {
                    return Power(left, right);
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
        private int Power( int left, int right)
        {
            if (right == 0)
            {
                return 1;
            }

            // for an even number, the last bit is zero
            if ((right & 1) == 0)
            {
                // shifting one bit to the right is equivalent to dividing by two
                var p = Power(left, right >> 1);
                return p * p;
            }
            else
            {
                return left * Power(left, right - 1);
            }
        }
        #endregion
    }

}