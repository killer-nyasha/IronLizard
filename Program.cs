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

            Lexer lexer = new Lexer("x*y", core);
            //12+ 34+5+ if 67+ else


            Parser parser = new Parser(lexer);
            Runtime runtime = new Runtime(parser);
            //runtime.Print();
            runtime.RunLazy();

            SolverSyntaxCore.keywords = runtime.keywords;
            SolverSyntaxCore.Run();

            //runtime.Run(false);
        }
    }
}
