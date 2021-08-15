using System.Collections.Generic;
namespace A2.CodeAnalysis
{
    sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public UnaryExpressionSyntax(ExpressionSyntax operand, SyntaxToken operatorToken)
        {
            Operand = operand;
            OperatorToken = operatorToken;
        }

        public override SyntaxType Type => SyntaxType.UnaryExpression;
        public ExpressionSyntax Operand { get; }
        public SyntaxToken OperatorToken { get; }

        public override IEnumerable<SyntaxNode> GetChildern()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}