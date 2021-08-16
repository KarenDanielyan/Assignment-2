using System.Collections.Generic;
namespace A2.CodeAnalysis
{
    //  Lexer
    class Tokenizer
    {
        private readonly string _text;
        private int _location;
        private List<string> _errormessages = new List<string>();

        public Tokenizer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> ErrorMessages => _errormessages;

        private char Current
        {
            get
            {
                if (_location >= _text.Length)
                    return '\0';

                return _text[_location];
            }
        }

        private void Next()
        {
            _location++;
        }

        public SyntaxToken NextToken()
        {
            //I want native numbers, basic ariphmetic, pow and sqrt//
            // whitespace

            if(_location >= _text.Length)
            {
                return new SyntaxToken(SyntaxType.EndofLineToken, _location, "\0", null);
            }

            if(char.IsDigit(Current))
            {
                var start = _location;

                while (char.IsDigit(Current))
                    Next();

                var length = _location - start;
                var text = _text.Substring(start, length);
                if(!int.TryParse(text, out var value))
                {
                    _errormessages.Add($"The number {_text} is out of boundaries(Int32).");
                }

                return new SyntaxToken(SyntaxType.NumberToken, start, text, value);
            }

            if(char.IsWhiteSpace(Current))
            {
                var start = _location;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _location - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxType.SpaceToken, start, text, null);
            }

            if(Current == '+')
            {
                return new SyntaxToken(SyntaxType.AdditionToken, _location++, "+", null);
            }
            else if (Current == '-')
            {
                return new SyntaxToken(SyntaxType.SubstractToken, _location++, "-", null);
            }
            else if (Current == '*')
            {
                return new SyntaxToken(SyntaxType.MultiplyToken, _location++, "*", null);
            }
            else if (Current == '/')
            {
                return new SyntaxToken(SyntaxType.DivisionToken, _location++, "/", null);
            }
            else if (Current == '^')
            {
                return new SyntaxToken(SyntaxType.PowerToken, _location++, "^", null);
            }
            else if (Current == '(')
            {
                return new SyntaxToken(SyntaxType.OpenParenToken, _location++, "(", null);
            }
            else if (Current == ')')
            {
                return new SyntaxToken(SyntaxType.CloseParenToken, _location++, ")", null);
            }

            _errormessages.Add($"ERROR: Unindentified character input: '{Current}'");
            return new SyntaxToken(SyntaxType.BadToken, _location++, _text.Substring(_location - 1, 1), null);

        }
    }
}