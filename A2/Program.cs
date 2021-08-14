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

                if (line == "1+1")
                {
                    Console.WriteLine("2");
                }
                else
                    Console.WriteLine("ERROR: Wrong Expression.");
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
}
