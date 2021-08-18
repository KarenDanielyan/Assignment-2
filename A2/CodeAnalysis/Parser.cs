using System.Collections.Generic;
namespace A2.CodeAnalysis
{
    //  Parser
    class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private int _position;
        private List<string> _errormessages = new List<string>();

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var tokenizer = new Tokenizer(text);
            SyntaxToken token;

            do
            {
                token = tokenizer.NextToken();

                if (token.Type != SyntaxType.SpaceToken && token.Type != SyntaxType.BadToken)
                    tokens.Add(token);
            }
            while (token.Type != SyntaxType.EndofLineToken);


            _tokens = tokens.ToArray();
            _errormessages.AddRange(tokenizer.ErrorMessages);
        }


        public IEnumerable<string> ErrorMessages => _errormessages;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _tokens.Length)
            {
                return _tokens[_tokens.Length - 1];
            }
            else
                return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;

            return current;
        }

        private SyntaxToken Match(SyntaxType type)
        {
            if (Current.Type == type)
                return NextToken();
            else
            {
                _errormessages.Add($"ERROR: Unexcpected token <{Current.Type}>, excpected <{type}>");
                return new SyntaxToken(type, Current.Position, null, null);
            }
        }

        public SyntaxTree Parse()
        {
            var expresion = ParseExpression();
            var end0fFile = Match(SyntaxType.EndofLineToken);

            return new SyntaxTree(_errormessages, expresion, end0fFile);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Type.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParsePrimeExpression();

                left = new UnaryExpressionSyntax(operand, operatorToken);
            }
            else
                left = ParsePrimeExpression();

            while (true)
            {
                var precedence = Current.Type.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                        break;

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, right, operatorToken);
            }
            return left;
        }

        private ExpressionSyntax ParsePrimeExpression()
        {

            if(Current.Type == SyntaxType.OpenParenToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxType.CloseParenToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);

            }

            var numberToken = Match(SyntaxType.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}