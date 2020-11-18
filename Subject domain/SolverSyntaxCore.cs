using System;
using System.Collections.Generic;

namespace IronLizard
{
    public class SolverSyntaxCore : SyntaxCore
    {
        public static Stack<Monomial> stack = new Stack<Monomial>();
        public static List<Keyword> keywords;
        public static Dictionary<int, string> variableNames = new Dictionary<int, string>();
        public static Dictionary<string, int> variableIds = new Dictionary<string, int>();

        static int NextVarId = 1;

        public static int Run(int index = 0, bool oneExpr = false)
        {
            int i = index;
            for (; i < (oneExpr ? keywords.Count : index+1); i++)
            {
                Keyword keyword = keywords[i];

                if (keyword.Type == KeywordType.Binary || keyword.Type == KeywordType.Prefix || keyword.Type == KeywordType.Postfix)
                {
                    ((Operator)keyword).OnMeet();
                }
                else if (keyword.Type == KeywordType.IdentifierOrLiteral)
                {
                    double d;
                    if (double.TryParse(keyword.Text, out d))
                    {
                        stack.Push(new Monomial(d));
                    }
                    else
                    {
                        if (!variableIds.ContainsKey(keyword.Text))
                        {
                            int newVarId = NextVarId++;
                            variableNames.Add(newVarId, keyword.Text);
                            variableIds.Add(keyword.Text, newVarId);

                            stack.Push(new Monomial(newVarId, 1));
                        }
                        else
                        {
                            stack.Push(new Monomial(variableIds[keyword.Text], 1));
                        }

                    }

                    //stack.Push(keyword.Text);
                }
            }
            return i;
        }

        static void Mul(int i)
        {
            int k = Run(i);
            Run(k);

            var a = stack.Pop();
            var b = stack.Pop();

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            stack.Push(a*b);
        }

        //static void Print(Runtime r)
        //{
        //    Console.WriteLine(r.stack.Pop());
        //}

        static void Else()
        {
            //int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        static void Elif()
        {
            //int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        static void If()
        {
            //int k = r.args.Pop();

            //elseStack.Push(k);

            //var b = r.stack.Pop();
            //var a = r.stack.Pop();
            //r.stack.Push((int.Parse(b) + int.Parse(a)).ToString());
        }

        public SolverSyntaxCore()
        {
            TextChars.Add('_');
            BreakChars.Add('(');
            BreakChars.Add(')');
            BreakChars.Add('.');
            BreakChars.Add(',');

            Keywords.Add(new Keyword(KeywordType.LeftBracket, "("));
            Keywords.Add(new Keyword(KeywordType.RightBracket, ")"));
            Keywords.Add(new Keyword(KeywordType.EndLine, ";"));
            Keywords.Add(new Keyword(KeywordType.Comma, ","));

            Keywords.Add(new Operator(KeywordType.Binary, "*", Mul));
        }
    }
}
