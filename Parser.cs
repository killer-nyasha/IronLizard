using System;
using System.Collections.Generic;

namespace IronLizard
{
    public class Parser
    {
        Stack<Keyword> ParserStack = new Stack<Keyword>();

        Lexer lexer;
        public Keyword endKeyword;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            this.endKeyword = lexer.endKeyword;
        }

        bool endLine = false;
        bool comma = false;
        bool pop = false;
        bool endBrackets = false;
        bool end = false;

        Keyword EndLine()
        {
            if (ParserStack.Count > 0)
                return ParserStack.Pop();
            else
                endLine = false;
                return null;
        }

        Keyword Comma()
        {
            if (ParserStack.Count > 0 && ParserStack.Peek().Type != KeywordType.LeftBracket)
                return ParserStack.Pop();
            else
                endLine = false;
                return null;
        }

        bool PopPredicate(Operator operatorToken)
        {
            if (ParserStack.Count == 0)
                return false;

            if (ParserStack.Peek() is Operator otherOperator)
            {
                if (operatorToken.Associativity == Associativity.Left)
                    return (operatorToken.Priority <= otherOperator.Priority);
                else
                    return operatorToken.Priority < otherOperator.Priority;
            }

            return false;
        }

        Keyword kwtoken; // нужно для Pop
        Operator OperatorToken;

        Keyword Pop()
        {
            if (ParserStack.Count > 0)
            {
                if (PopPredicate(OperatorToken))
                {
                    return ParserStack.Pop();
                }
            }

            pop = false;
            ParserStack.Push(OperatorToken);
            return null;
        }

        Keyword EndBrackets()
        {
            while (ParserStack.Count > 0 && ParserStack.Peek().Type != KeywordType.LeftBracket)
                return ParserStack.Pop();

            endBrackets = false;
            if (ParserStack.Count > 0 && ParserStack.Peek().Type == KeywordType.LeftBracket)
                ParserStack.Pop();
            else
                throw new Exception("Bracket wasn't closed");

            return kwtoken;
        }

        KeywordType lastKeywordType = KeywordType.Prefix;

        public Keyword GetNext()
        {
            if (endLine)
                return EndLine();
            if (comma)
                return Comma();
            if (pop)
                return Pop();
            if (endBrackets)
                return EndBrackets();

            do kwtoken = lexer.GetNext(); while (kwtoken == null);

            /*Keyword*/

            if (end)
                return lexer.endKeyword;

            if (kwtoken == lexer.endKeyword)
            {
                end = endLine = true;
                return EndLine();
                //end
            }

            if (kwtoken is Operator operatorToken)
            {
                lastKeywordType = operatorToken.Type;

                if (kwtoken.Type == KeywordType.Simple)
                {
                    return kwtoken;
                }

                OperatorToken = operatorToken;
                pop = true;
                return Pop();
            }
            if (kwtoken.Type == KeywordType.Comma)
            {
                comma = true;
                return Comma();
            }
            else if (kwtoken.Type == KeywordType.EndLine)
            {
                endLine = true;
                return EndLine();
            }
            else if (kwtoken.Type == KeywordType.LeftBracket)
            {
                ParserStack.Push(kwtoken);
                return kwtoken;
            }
            else if (kwtoken.Type == KeywordType.RightBracket)
            {
                return EndBrackets();
            }
            else
            {
                return kwtoken;
                //decimal number = 0;
                //if (decimal.TryParse(kwtoken.Text, out number))
                //{

                //}
                //else
                //{

                //}
            }

            return EndLine();
        }
    }
}
