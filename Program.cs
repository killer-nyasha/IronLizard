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

            Lexer lexer = new Lexer("100 + (if 1 + 2, (if 3 + 4 + 5, 8), else 6 + 7) + 200", new DefaultSyntaxCore());
            //12+ 34+5+ if 67+ else


            Parser parser = new Parser(lexer);
            Runtime runtime = new Runtime(parser);
            //runtime.Print();
            //runtime.Run();
            runtime.RunLazy();
        }
    }
}
