﻿namespace IronLizard
{
    public enum KeywordType
    {
        Prefix,
        Postfix,
        Binary,

        Simple,
        LeftBracket,
        RightBracket,
        EndLine,
        Comma,
        IdentifierOrLiteral
    }

    public class Keyword
    {
        public KeywordType Type { get; set; }
        public string Text { get; set; }

        public Keyword(KeywordType type, string text)
        {
            Type = type;
            Text = text;
        }

        int TypeHashCode()
        {
            return (Type < KeywordType.Simple ? Type.GetHashCode() : KeywordType.Simple.GetHashCode());
        }

        public override int GetHashCode()
        {
            return TypeHashCode() + Text.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Keyword kw && (Text == kw.Text && TypeHashCode() == kw.TypeHashCode());
        }

        public override string ToString()
        {
            return Type.ToString()/*.Remove(1)*/ + "__" + Text;
        }
    }

    public enum Associativity
    {
        Left,
        Right
    }

    public delegate void OnMeetDelegate(Runtime runtime);

    public class Operator : Keyword
    {
        public int Priority;
        public Associativity Associativity;
        public OnMeetDelegate OnMeet;

        public Operator(KeywordType type, string text, OnMeetDelegate onMeet, int priority = 0, Associativity associativity = Associativity.Left) : base(type, text)
        {
            Priority = priority;
            Associativity = associativity;
            OnMeet = onMeet;
        }
    }

}
