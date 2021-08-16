namespace A2.CodeAnalysis
{
    internal static class SyntaxPriority
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxType type)
        {
            switch (type)
            {
                case SyntaxType.AdditionToken:
                case SyntaxType.SubstractToken:
                    return 4;
                default:
                    return 0;
            }
        }

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
                case SyntaxType.PowerToken:
                    return 3;

                default:
                    return 0;
            }
        }
    }
}