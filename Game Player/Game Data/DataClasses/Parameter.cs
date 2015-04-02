using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Parameter : ICloneable
    {
        private object[] children;
        public object[] Children { get { return children; } }
        public object Child { get { return children.Length > 0 ? children[0] : null; } }

        public int Size { get { return children.Length; } }

        public Parameter()
        {
            children = new object[] { };
        }

        public Parameter(object child)
        {
            if (child is Parameter)
            {
                //children = new object[] { child };
                children = ((Parameter)child).children;
            }
            else if (child is string)
            {
                string s = (string)child;

                int tryInt = 0;
                bool tryBool = false;

                if (int.TryParse(s, out tryInt))
                    children = new object[] { tryInt };
                else if (bool.TryParse(s, out tryBool))
                    children = new object[] { tryBool };
                else
                    children = new object[] { child };
            }
            else
            {
                children = new object[] { child };
            }
        }

        public Parameter this[int index]
        {
            get { return new Parameter(children[index]); }
            set { children[index] = value; }
        }

        void Resize() { Resize(Size + 1); }
        void Resize(int newSize)
        {
            Array.Resize<object>(ref children, newSize);
        }

        public void Add(Parameter param)
        {
            Resize();
            children[Size - 1] = param;
        }

        public void Add(object param)
        {
            Resize();
            children[Size - 1] = param;
        }

        public static implicit operator Parameter(bool p)
        {
            return new Parameter(p);
        }

        public static implicit operator bool(Parameter p)
        {
            if (p.Child is Parameter)
                return (Parameter)p.Child;

            return (bool)p.Child;
        }

        public static implicit operator Parameter(string p)
        {
            return new Parameter(p);
        }

        public static implicit operator string(Parameter p)
        {
            if (p.Child is Parameter)
                return (Parameter)p.Child;

            return (string)p.Child;
        }

        public static implicit operator Game_Player.AudioFile(Parameter p)
        {
            return (Game_Player.AudioFile)p.Child;
        }

        public override string ToString()
        {
            string s = "{";
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] is Parameter)
                {
                    Parameter p = (Parameter)children[i];
                    if (p.children.Length == 1)
                        s += p.Child.ToString();
                    else
                        s += "[+]";
                }
                else
                    s += children[i].ToString();

                if (i != children.Length - 1)
                    s += ", ";
            }
            s += "}";

            return s;
        }

        public static implicit operator Parameter(int p)
        {
            return new Parameter(p);
        }

        public static implicit operator int(Parameter p)
        {
            if (p.Child is Parameter)
                return (Parameter)p.Child;

            return (int)p.Child;
        }

        public object Clone()
        {
            Parameter ps = new Parameter();

            for (int i = 0; i < Size; i++)
            {
                if (children[i] is ICloneable)
                    ps.Add(new Parameter(((ICloneable)children[i]).Clone()));
                else
                    ps.Add(new Parameter(ps.Clone()));
            }

            return ps;
        }
    }
}
