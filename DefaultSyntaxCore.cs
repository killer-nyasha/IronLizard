using System;

namespace IronLizard
{
    public class DefaultSyntaxCore : SyntaxCore
    {
        static void Plus(Runtime r)
        {
            var b = r.stack.Pop();
            var a = r.stack.Pop();
            r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        static void Print(Runtime r)
        {
            Console.WriteLine(r.stack.Pop());
        }

        public DefaultSyntaxCore()
        {
            TextChars.Add('_');
            BreakChars.Add('(');
            BreakChars.Add(')');
            BreakChars.Add('.');
            BreakChars.Add(',');
            Keywords.Add(new Operator(KeywordType.Binary, "+", Plus));
            Keywords.Add(new Operator(KeywordType.Prefix, "+", Plus));
            Keywords.Add(new Operator(KeywordType.Postfix, "+", Plus));
            Keywords.Add(new Operator(KeywordType.Postfix, "++", Plus));
            Keywords.Add(new Operator(KeywordType.Prefix, "print", Print, -100));
            Keywords.Add(new Operator(KeywordType.Prefix, "if", Print, -500));
            Keywords.Add(new Operator(KeywordType.Prefix, "else", Print, -500));

            Keywords.Add(new Keyword(KeywordType.LeftBracket, "("));
            Keywords.Add(new Keyword(KeywordType.RightBracket, ")"));
            Keywords.Add(new Keyword(KeywordType.EndLine, ";"));
            Keywords.Add(new Keyword(KeywordType.Comma, ","));
        }
    }
}
