using System.Collections.Generic;

namespace IronLizard
{
    public class SyntaxCore
    {
        public HashSet<char> BreakChars = new HashSet<char>();
        public HashSet<char> TextChars = new HashSet<char>();
        public HashSet<Keyword> Keywords = new HashSet<Keyword>();

        public Keyword GetKeyword(Keyword key)
        {
            Keyword ret = null;
            Keywords.TryGetValue(key, out ret);
            return ret;
        }
    }
}
