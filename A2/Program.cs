using System;
using System.Collections.Generic;

namespace A2
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                CheckArguments(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            while(true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                var tokens = new Tokenizer(line);
                while(true)
                {
                    var token = tokens.NextToken();

                    if (token.Type == SyntaxType.EndofLineToken)
                        break;

                    if (token.Value != null)
                        Console.WriteLine($"{token.Type}: '{token.Text}' {token.Value}");
                    else
                        Console.WriteLine($"{token.Type}: '{token.Text}'");

                }

                Console.WriteLine();
            }

        }

//      BASIC FUNCTIONS
        static void CheckArguments(string[] args)
        {
            if (args.Length > arg.Count)
            {
                throw tooManyArguments;
            }
        }

//      INITIALIZATION ITEMS

        static List<String> arg = new List<String>
        {"-v", "-t" };

        static Exception tooManyArguments = new Exception("Too many arguments!!!(Max: 2)");


    }

    public enum SyntaxType
    {
        NumberToken,
        SpaceToken,
        AdditionToken,
        DivisionToken,
        MultiplyToken,
        SubstractToken,
        OpenParenToken,
        CloseParenToken,
        BadToken,
        EndofLineToken
    }

    public class SyntaxToken
    {
        public SyntaxToken(SyntaxType type, int position, string text, object value)
        {
            Type = type;
            Position = position;
            Text = text;
            Value = value;
        }

        public SyntaxType Type { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }

    class Tokenizer
    {
        private readonly string _text;
        private int _location;
        public Tokenizer(string text)
        {
            _text = text;
        }

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
                int.TryParse(text, out var value);

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
            else if (Current == '(')
            {
                return new SyntaxToken(SyntaxType.OpenParenToken, _location++, "(", null);
            }
            else if (Current == ')')
            {
                return new SyntaxToken(SyntaxType.CloseParenToken, _location++, ")", null);
            }

            return new SyntaxToken(SyntaxType.BadToken, _location++, _text.Substring(_location - 1, 1), null);

        }
    }

    class Parser
    {

    }
}
