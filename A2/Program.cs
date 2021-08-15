using System;
using System.Collections.Generic;
using System.Linq;

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
                if (String.IsNullOrWhiteSpace(line))
                    return;

                var parser = new Parser(line);
                var expression = parser.Parse();


                //Stying
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkCyan;

                    TreePrint(expression);

                Console.ForegroundColor = color;

                /*
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
                */
            }

        }

//      BASIC FUNCTIONS

        static void TreePrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            //├──
            //│
            //└──
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Type);
            if(node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "     " : "│    ";

            var lastChild = node.GetChildern().LastOrDefault();

            foreach (var c_node in node.GetChildern())
            {
                TreePrint(c_node, indent, c_node == lastChild);
            }    
        }

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

//  Initialization types
    enum SyntaxType
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
        EndofLineToken,
        NumberExpression,
        BinaryExpression
    }

    abstract class SyntaxNode
    {
        public abstract SyntaxType Type { get; }

        public abstract IEnumerable<SyntaxNode> GetChildern();

    }

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

    abstract class ExpressionSyntax : SyntaxNode
    {
        public ExpressionSyntax()
        {
                
        }
    }

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxType Type => SyntaxType.NumberExpression;

        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildern()
        {
            yield return NumberToken; 
        }
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax leftToken, ExpressionSyntax rightToken, SyntaxToken operatorToken)
        {
            LeftToken = leftToken;
            RightToken = rightToken;
            OperatorToken = operatorToken;
        }

        public override SyntaxType Type => SyntaxType.BinaryExpression;
        public ExpressionSyntax LeftToken { get; }
        public ExpressionSyntax RightToken { get; }
        public SyntaxToken OperatorToken { get; }

        public override IEnumerable<SyntaxNode> GetChildern()
        {
            yield return LeftToken;
            yield return OperatorToken;
            yield return RightToken;
        }
    }

//  Lexer
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

    //  Parser
    class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;
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

        }
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
                return new SyntaxToken(type, Current.Position, null, null);
        }

        public ExpressionSyntax Parse()
        {
            var l_prime = ParsePrimeExpression();

            while( Current.Type == SyntaxType.AdditionToken ||
                    Current.Type == SyntaxType.SubstractToken)
            {
                var operatorToken = NextToken();
                var r_prime = ParsePrimeExpression();

                l_prime = new BinaryExpressionSyntax(l_prime, r_prime, operatorToken);
            }

            return l_prime;
        }

        private ExpressionSyntax ParsePrimeExpression()
        {
            var numberToken = Match(SyntaxType.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}
