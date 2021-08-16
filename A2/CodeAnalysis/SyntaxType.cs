namespace A2.CodeAnalysis
{
    //  Initialization types
    enum SyntaxType
    {
        //Tokens
        BadToken,
        EndofLineToken,

        NumberToken,
        SpaceToken,
        AdditionToken,
        DivisionToken,
        MultiplyToken,
        SubstractToken,
        PowerToken,
        OpenParenToken,
        CloseParenToken,

        //Expressions
        ParenthesizedExpression,
        NumberExpression,
        UnaryExpression,
        BinaryExpression
    }
}