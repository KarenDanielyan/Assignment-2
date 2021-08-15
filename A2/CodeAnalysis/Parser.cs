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
            var expresion = ParseTerm();
            var end0fFile = Match(SyntaxType.EndofLineToken);

            return new SyntaxTree(_errormessages, expresion, end0fFile);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }

        public ExpressionSyntax ParseTerm()
        {
            var l_prime = ParseFactor();

            while (Current.Type == SyntaxType.AdditionToken ||
                    Current.Type == SyntaxType.SubstractToken)
            {
                var operatorToken = NextToken();
                var r_prime = ParseFactor();

                l_prime = new BinaryExpressionSyntax(l_prime, r_prime, operatorToken);
            }

            return l_prime;
        }

        public ExpressionSyntax ParseFactor()
        {
            var l_prime = ParsePrimeExpression();

            while (Current.Type == SyntaxType.MultiplyToken ||
                    Current.Type == SyntaxType.DivisionToken)
            {
                var operatorToken = NextToken();
                var r_prime = ParsePrimeExpression();

                l_prime = new BinaryExpressionSyntax(l_prime, r_prime, operatorToken);
            }

            return l_prime;
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