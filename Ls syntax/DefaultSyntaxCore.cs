using System;
using System.Collections.Generic;

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

        //static Stack<int> elseStack = new Stack<int>();

        static void Else(Runtime r)
        {
            int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        static void Elif(Runtime r)
        {
            int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        static void If(Runtime r)
        {
            int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
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
            Keywords.Add(new NestedOperator(KeywordType.Prefix, "if", If, 0, -500));
            //Keywords.Add(new Operator(KeywordType.Prefix, "iff", If, -500));
            Keywords.Add(new NestedOperator(KeywordType.Prefix, "else", Else, 0, -500));
            Keywords.Add(new NestedOperator(KeywordType.Prefix, "elif", Elif, 0, -500));
            Keywords.Add(new Operator(KeywordType.Prefix, "?", Print, -500));
            Keywords.Add(new Operator(KeywordType.Prefix, ":", Print, -600));

            Keywords.Add(new Keyword(KeywordType.LeftBracket, "("));
            Keywords.Add(new Keyword(KeywordType.RightBracket, ")"));
            Keywords.Add(new Keyword(KeywordType.EndLine, ";"));
            Keywords.Add(new Keyword(KeywordType.Comma, ","));
        }
    }
}
