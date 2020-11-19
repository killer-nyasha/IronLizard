using System;
using System.Text;
using System.Collections.Generic;

namespace IronLizard
{
    public class Lexer
    {
        string text;
        int index;
        SyntaxCore core;

        Keyword search = new Keyword(KeywordType.Simple, "");

        KeywordType lastKeywordType = KeywordType.Simple;

        Keyword callKeyword = new Operator(KeywordType.Postfix, "__call__", r => r, -200);
        public Keyword endKeyword = new Keyword(KeywordType.Simple, "__end__");

        bool hasSpacesPre = true;
        bool hasSpacesPost = true;

        StringBuilder builder = new StringBuilder();

        public Lexer(string text, SyntaxCore core)
        {
            this.text = text;
            this.core = core;
        }

        Queue<Keyword> toReturn = new Queue<Keyword>();

        Keyword NewToken()
        {
            if (builder.Length == 0)
            {
                hasSpacesPre = hasSpacesPost;
                return null;
            }

            if (toReturn.Count > 0)
            {
                Keyword ret = toReturn.Dequeue();
                return ret;
            }

            string keywordText = builder.ToString();
            builder.Clear();
            search.Text = keywordText;
            Keyword result;

            search.Type = KeywordType.Simple;
            if (core.Keywords.TryGetValue(search, out result))
            {
                if (keywordText[0] == '(')
                {
                    if (!hasSpacesPre)
                        toReturn.Enqueue(callKeyword);
                    else
                    { int j = 111; }

                    hasSpacesPost = true;
                    //if (lastKeywordType == KeywordType.Simple)
                }
                else if (keywordText[0] == ')')
                    hasSpacesPost = false;

                toReturn.Enqueue(result);

                lastKeywordType = result.Type;
                hasSpacesPre = hasSpacesPost;
                return null;
            }

            if (hasSpacesPost == hasSpacesPre)
            {
                search.Type = KeywordType.Binary;
                if (core.Keywords.TryGetValue(search, out result))
                {
                    toReturn.Enqueue(result);
                    lastKeywordType = result.Type;
                    hasSpacesPre = hasSpacesPost;
                    return null;
                }
            }

            if (/*!hasSpacesPost && */hasSpacesPre)
            {
                search.Type = KeywordType.Prefix;
                if (core.Keywords.TryGetValue(search, out result))
                {
                    toReturn.Enqueue(result);
                    lastKeywordType = result.Type;
                    hasSpacesPre = hasSpacesPost;
                    return null;
                }
            }

            if (/*hasSpacesPost && */!hasSpacesPre)
            {
                search.Type = KeywordType.Postfix;
                if (core.Keywords.TryGetValue(search, out result))
                {
                    toReturn.Enqueue(result);
                    lastKeywordType = result.Type;
                    hasSpacesPre = hasSpacesPost;
                    return null;
                }
            }

            Keyword ret1 = new Keyword(KeywordType.IdentifierOrLiteral, keywordText);
            lastKeywordType = ret1.Type/*KeywordType.Simple*/;
            hasSpacesPre = hasSpacesPost;
            return ret1;
        }

        public Keyword GetNext()
        {
            if (toReturn.Count > 0)
            {
                Keyword ret = toReturn.Dequeue();
                return ret;
            }

            if (!(index < text.Length))
                return endKeyword;

            while (index < text.Length)
            {
                char ti = text[index];
                index++;

                if (!char.IsWhiteSpace(ti))
                {
                    builder.Append(ti);
                    if (core.BreakChars.Contains(text[index - 1]) || 
                        index < text.Length &&
                        (core.BreakChars.Contains(text[index])
                        || (char.IsLetterOrDigit(text[index - 1]) != char.IsLetterOrDigit(text[index])))
                        )
                    {
                        hasSpacesPost = false;
                        return NewToken();
                    }
                }
                else
                {
                    hasSpacesPost = true;
                    return NewToken();
                }
            }
            return NewToken();
        }
    }
}
