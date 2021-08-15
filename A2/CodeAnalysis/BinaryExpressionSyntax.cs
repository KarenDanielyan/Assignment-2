using System.Collections.Generic;
namespace A2.CodeAnalysis
{
    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax leftToken, ExpressionSyntax rightToken, SyntaxToken operatorToken)
        {
            LeftToken = leftToken;
            RightToken = rightToken;
            OperatorToken = operatorToken;
        }

        public override SyntaxType Type => SyntaxType.BinaryExpression;
        public ExpressionSyntax LeftToken { get; }
        public ExpressionSyntax RightToken { get; }
        public SyntaxToken OperatorToken { get; }

        public override IEnumerable<SyntaxNode> GetChildern()
        {
            yield return LeftToken;
            yield return OperatorToken;
            yield return RightToken;
        }
    }
}