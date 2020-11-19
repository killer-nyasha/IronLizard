using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace IronLizard
{
    //https://stackoverflow.com/questions/21758074/c-sharp-compare-two-dictionaries-for-equality/31590664
    public class DictionaryComparer<TKey, TValue> :
    IEqualityComparer<Dictionary<TKey, TValue>>
    {
        private IEqualityComparer<TValue> valueComparer;
        public DictionaryComparer(IEqualityComparer<TValue> valueComparer = null)
        {
            this.valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
        }
        public bool Equals(Dictionary<TKey, TValue> x, Dictionary<TKey, TValue> y)
        {
            if (x.Count != y.Count)
                return false;
            if (x.Keys.Except(y.Keys).Any())
                return false;
            if (y.Keys.Except(x.Keys).Any())
                return false;
            foreach (var pair in x)
                if (!valueComparer.Equals(pair.Value, y[pair.Key]))
                    return false;
            return true;
        }

        public int GetHashCode(Dictionary<TKey, TValue> obj)
        {
            throw new NotImplementedException();
        }
    }

    public struct Element : IComparable<Element>
    {
        public int Variable;
        public int Pow;

        public int CompareTo(Element el)
        {
            return (Variable - el.Variable) /** 65536 + (Pow - el.Pow)*/;
        }

        public override bool Equals(object obj)
        {
            if (obj is Element el)
                return Variable == el.Variable && Pow == el.Pow;
            else return false;
        }

        public override int GetHashCode()
        {
            return Variable /** 65536 + Pow*/;
        }

        public override string ToString()
        {
            return $"{(Pow == 0 ? "" : SolverSyntaxCore.variableNames[Variable]) + (Pow != 1 ? Pow.ToString() : "")}";
        }
    }

    public struct ElementsMul : IComparable<ElementsMul>
    {
        public List<Element> Elements;
        //public double Coef;

        public override bool Equals(object obj)
        {
            if (obj is ElementsMul el)
                return CompareTo(el) == 0;
            else return false;
        }

        public int CompareTo(ElementsMul el)
        {
            //if (Elements.Count != el.Elements.Count)
            //    return false;

            Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
            el.Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

            for (int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].GetHashCode() != el.Elements[i].GetHashCode())
                    return Elements[i].GetHashCode() - el.Elements[i].GetHashCode();
            }
            return 0;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            for (int i = 0; i < Elements.Count; i++)
                hash += Elements[i].GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return Elements.Aggregate($"", (x,y) => x + y.ToString() );
        }
    }

    public struct Monomial : IComparable<Monomial>
    {
        //public Dictionary<ElementsMul, double> 
        ElementsMul Variables;
        public double Coef;

        public Monomial(int id, double value)
        {
            Variables = new ElementsMul { Elements = new List<Element>() { new Element() { Variable = id, Pow = 1 } } };
            Coef = value;
        }

        public Monomial(double value)
        {
            Variables = new ElementsMul { Elements = new List<Element>() { new Element() { Variable = 0, Pow = 0 } } };
            Coef = value;
        }

        public int CompareTo(Monomial el)
        {
            //if (Elements.Count != el.Elements.Count)
            //    return false;

            return Variables.CompareTo(el.Variables);

            //Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
            //el.Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

            //for (int i = 0; i < Elements.Count; i++)
            //{
            //    if (Elements[i].GetHashCode() != el.Elements[i].GetHashCode())
            //        return Elements[i].GetHashCode() - el.Elements[i].GetHashCode();
            //}
            //return 0;
        }


        public static Monomial operator*(Monomial a, Monomial b)
        {
            Monomial c = new Monomial();
            c.Coef = a.Coef * b.Coef;

            if (b.Variables.Elements.Count > a.Variables.Elements.Count)
            {
                Monomial d = a;
                a = b;
                b = d;
            }

            a.Variables.Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
            b.Variables.Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

            c.Variables.Elements = new List<Element>(a.Variables.Elements);

            foreach (var ai in b.Variables.Elements)
            {
                int aii = a.Variables.Elements.BinarySearch(ai);
                if (aii >= 0)
                {
                    var v = c.Variables.Elements[aii];
                    v.Pow += ai.Pow;
                    c.Variables.Elements[aii] = v;
                }
                else c.Variables.Elements.Add(ai);
            }

            return c;
        }

        public override string ToString()
        {
            return (Coef != 1 ? Coef.ToString() : "") + Variables.ToString();
        }

        public string ToString(double coef)
        {
            return coef.ToString() + Variables.ToString();
        }
    }

    public struct Polynomial
    {
        //public Dictionary<ElementsMul, double> 
        List<Monomial> Terms;

        public Polynomial(int id, double value)
        {
            Terms = new List<Monomial>() { new Monomial(id, value) };// new ElementsMul { Elements = new List<Element>() { new Element() { Variable = id, Pow = 1 } } };
            //Coef = value;
        }

        public Polynomial(double value)
        {
            Terms = new List<Monomial>() { new Monomial(value) };
            //Coef = value;
        }

        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            Polynomial c = new Polynomial();

            if (b.Terms.Count > a.Terms.Count)
            {
                Polynomial d = a;
                a = b;
                b = d;
            }

            a.Terms.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
            b.Terms.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

            c.Terms = new List<Monomial>(a.Terms);

            foreach (var ai in b.Terms)
            {
                int aii = a.Terms.BinarySearch(ai);
                if (aii >= 0)
                {
                    var v = c.Terms[aii];
                    v.Coef += ai.Coef;
                    c.Terms[aii] = v;
                }
                else c.Terms.Add(ai);
            }

            return c;
        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            Polynomial c = new Polynomial();

            if (b.Terms.Count > a.Terms.Count)
            {
                Polynomial d = a;
                a = b;
                b = d;
            }

            a.Terms.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
            b.Terms.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

            c.Terms = new List<Monomial>();

            foreach (var ai in a.Terms)
                foreach (var bi in b.Terms)
            {
                c.Terms.Add(ai * bi);

                //int aii = a.Terms.BinarySearch(ai);
                //if (aii != -1)
                //{
                //    var v = c.Terms[aii];
                //    v.Coef += ai.Coef;
                //    c.Terms[aii] = v;
                //}
                //else c.Terms.Add(ai);
            }

            return c;
        }


        public override string ToString()
        {
            return Terms.Aggregate($"", (x, y) => x + (y.Coef > 0 ? " + " + y.ToString() : " - " + y.ToString(-y.Coef)));
        }
    }

}
