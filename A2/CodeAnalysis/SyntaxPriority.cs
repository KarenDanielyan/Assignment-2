namespace A2.CodeAnalysis
{
    internal static class SyntaxPriority
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.AdditionToken:
                case SyntaxType.SubstractToken:
                    return 1;
                case SyntaxType.MultiplyToken:
                case SyntaxType.DivisionToken:
                    return 2;

                default:
                    return 0;
            }
        }
    }
}