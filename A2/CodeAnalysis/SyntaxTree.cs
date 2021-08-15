using System.Collections.Generic;
using System.Linq;
namespace A2.CodeAnalysis
{
    sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> errorMessages, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            ErrorMessages = errorMessages.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> ErrorMessages { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}