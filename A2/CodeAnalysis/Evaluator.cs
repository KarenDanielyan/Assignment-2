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
                    return left + right;
                }
                else if (b.OperatorToken.Type == SyntaxType.SubstractToken)
                {
                    return left - right;
                }
                else if (b.OperatorToken.Type == SyntaxType.MultiplyToken)
                {
                    return left * right;
                }
                else if (b.OperatorToken.Type == SyntaxType.DivisionToken)
                {
                    return left / right;
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
    }
}