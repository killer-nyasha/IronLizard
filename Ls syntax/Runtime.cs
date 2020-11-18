using System;
using System.Collections.Generic;

namespace IronLizard
{
    public class Runtime
    {
        Parser parser;

        public Runtime(Parser parser)
        {
            this.parser = parser;
        }

        //public Stack<string> stack = new Stack<string>();
        //public Stack<int> args = new Stack<int>();

        public List<Keyword> keywords = new List<Keyword>();

        public void RunLazy()
        {
            while (true)
            {
                Keyword keyword;
                do keyword = parser.GetNext(); while (keyword == null);
                if (keyword == parser.endKeyword)
                {
                    keywords.Reverse();
                    //keywords.Add(keyword);
                    break;
                }

                //int index = keywords.Count;
                keywords.Add(keyword);

                //if (keyword.Type == KeywordType.Binary)
                //{
                //    args.Pop();
                //}
                //else if (keyword.Type == KeywordType.Prefix || keyword.Type == KeywordType.Postfix)
                //{

                //}
                //else if (keyword.Type == KeywordType.IdentifierOrLiteral)
                //{
                //    args.Push(index);
                //}
            }

        }

        //public void Run(bool oneExpr)
        //{
        //    for (int i = 0; i < keywords.Count; i++)
        //    //while (true)
        //    {
        //        Keyword keyword = keywords[i];
        //        //do keyword = parser.GetNext(); while (keyword == null);
        //        //if (keyword == parser.endKeyword)
        //        //    break;

        //        if (keyword.Type == KeywordType.Binary || keyword.Type == KeywordType.Prefix || keyword.Type == KeywordType.Postfix)
        //        {
        //            ((Operator)keyword).OnMeet(this);
        //        }
        //        else if (keyword.Type == KeywordType.IdentifierOrLiteral)
        //        {
        //            //throw new Exception();
        //            stack.Push(keyword.Text);
        //        }

        //        //if (oneExpr && stack.Count == 0)
        //        //{
        //        //    return;
        //        //}
        //    }
        }

        public void Print()
        {
            while (true)
            {
                var keyword = parser.GetNext();
                Console.WriteLine(keyword);
                if (keyword == parser.endKeyword)
                    break;
            }
        }
    }
}
