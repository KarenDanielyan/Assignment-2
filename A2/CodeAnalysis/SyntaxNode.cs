using System.Collections.Generic;
namespace A2.CodeAnalysis
{
    abstract class SyntaxNode
    {
        public abstract SyntaxType Type { get; }

        public abstract IEnumerable<SyntaxNode> GetChildern();

    }
}