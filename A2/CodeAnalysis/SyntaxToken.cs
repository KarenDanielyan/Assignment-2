using System.Collections.Generic;
using System.Linq;
namespace A2.CodeAnalysis
{
    class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxType type, int position, string text, object value)
        {
            Type = type;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxType Type { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildern()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}