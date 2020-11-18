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

    public struct Element
    {
        public int Variable;
        public int Pow;

        public override bool Equals(object obj)
        {
            if (obj is Element el)
                return Variable == el.Variable && Pow == el.Pow;
            else return false;
        }

        public override int GetHashCode()
        {
            return Variable * 65536 + Pow;
        }
    }

    public struct ElementsMul
    {
        public List<Element> Elements;
        public double Coef;

        public override bool Equals(object obj)
        {
            if (obj is ElementsMul el)
            {
                if (Elements.Count != el.Elements.Count)
                    return false;

                Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);
                el.Elements.Sort((x, y) => x.GetHashCode() - y.GetHashCode() /*x.Variable + x.Pow) - y.Variable * y.Pow*/);

                for (int i = 0; i < Elements.Count; i++)
                {
                    if (Elements[i].GetHashCode() != el.Elements[i].GetHashCode())
                        return false;
                }
                return true;
            }
            else return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            for (int i = 0; i < Elements.Count; i++)
                hash += Elements[i].GetHashCode();
            return hash;
        }
    }

    public struct Monomial
    {
        public Dictionary<ElementsMul, double> Variables;

        public Monomial(int id, double value)
        {
            Variables = new Dictionary<ElementsMul, double>();
            Variables.Add(new ElementsMul { Elements = new List<Element>() { new Element() { Variable = id, Pow = 0 } }, Coef = value }, value);
        }

        public Monomial(double value)
        {
            Variables = new Dictionary<ElementsMul, double>();
            Variables.Add(new ElementsMul { Elements = new List<Element>() { new Element() { Variable = 0, Pow = 0 } }, Coef = value }, value);
        }

        public static Monomial operator*(Monomial a, Monomial b)
        {
            Monomial c = new Monomial();

            if (b.Variables.Count > a.Variables.Count)
            {
                Monomial d = a;
                a = b;
                b = d;
            }

            c.Variables = a.Variables;

            foreach (var ai in b.Variables)
            {
                if (c.Variables.ContainsKey(ai.Key))
                    c.Variables[ai.Key] *= ai.Value;
                else c.Variables.Add(ai.Key, ai.Value);
            }

            return c;
        }
    }
}
