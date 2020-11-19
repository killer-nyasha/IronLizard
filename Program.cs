using System;

namespace IronLizard
{
    class Program
    {
        static void Main(string[] args)
        {
            //+1+ + 2++
            //1 + 2 + 3
            //if 1 + 2 3 + 4 + 5 else 6 + 7
            //1 + f (1 + 2 3 4) + 5

            //100 + (if 1 + 2, if 3 + 4 + 5, 8, ilse 6 + 7) + 200
            //100 + (if 1 + 2, if 3 + 4 + 5, 8, ilse, if 6 + 7, ilse 9) + 200

            //"100 + (if 1 + 2, (if 3 + 4 + 5, 8), else 6 + 7) + 200"
            //"100 + (if 1 + 2, (if 3 + 4 + 5, 8), elif 10 11, else 6 + 7) + 200"

            SolverSyntaxCore core = new SolverSyntaxCore();

            Lexer lexer = new Lexer("(x + y) * (x + y)", core);
            //12+ 34+5+ if 67+ else

            Parser parser = new Parser(lexer);
            Runtime runtime = new Runtime(parser);
            //runtime.Print();
            runtime.RunLazy();

            SolverSyntaxCore.keywords = runtime.keywords;


            int iter = (int)1e6;

            DateTime dt = DateTime.Now;
            for (int i = 1; i < iter; i++)
            {
                SolverSyntaxCore.Run();
            }
            TimeSpan ts = DateTime.Now - dt;

            Console.WriteLine(ts.TotalMilliseconds);
            Console.WriteLine(iter / ts.TotalMilliseconds * 1000);

            //runtime.Run(false);
        }
    }
}
