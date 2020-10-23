namespace IronLizard
{
    class Program
    {
        static void Main(string[] args)
        {
            //+1+ + 2++
            //1 + 2 + 3
            //if 1 + 2 3 + 4 + 5 else 6 + 7

            Lexer lexer = new Lexer("1 + f (1 + 2 3 4) + 5", new DefaultSyntaxCore());
            Parser parser = new Parser(lexer);
            Runtime runtime = new Runtime(parser);
            runtime.Print();
            //runtime.Run();
            //runtime.RunLazy();
        }
    }
}
