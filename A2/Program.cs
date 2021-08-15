using System;
using System.Collections.Generic;
using System.Linq;
using A2.CodeAnalysis;

namespace A2
{
    class Program
    {

        static void Main(string[] args)
        {
            bool printTree = default;
            try
            {
                CheckArguments(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach( var arg in args)
            {
                switch(arg)
                {
                    case "-t":
                        printTree = true;
                        break;
                    default:
                        throw new Exception($"Unknown argument '{arg}'.");
                }   
            }
           
            while(true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(line))
                    return;
                else if (line == "#end")
                    break;

                var syntaxTree = SyntaxTree.Parse(line);
                var color = Console.ForegroundColor;

                if (printTree)
                {
                    //Styling
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    TreePrint(syntaxTree.Root);
                    Console.ForegroundColor = color;
                }
                if(syntaxTree.ErrorMessages.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    foreach (var message in syntaxTree.ErrorMessages)
                        Console.WriteLine(message);
                    Console.ForegroundColor = color;
                }
                else
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                
            }

        }

//      BASIC FUNCTIONS

        static void TreePrint(SyntaxNode node, string indent = "", bool isLast = true)
        {

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
        static Exception tooManyArguments = new Exception($"Too many arguments!!!(Max: {arg.Count()})");
    }
}
