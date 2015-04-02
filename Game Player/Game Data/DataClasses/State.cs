using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class State : ICloneable, IComparable
    {
        public int id = 0;
        public string name = "";
        public int animationId = 0;
        public int restriction = 0;
        public bool nonresistance = false;
        public bool zeroHp = false;
        public bool cantGetExp = false;
        public bool cantEvade = false;
        public bool slipDamage = false;
        public int rating = 5;
        public int hitRate = 100;
        public int maxhpRate = 100;
        public int maxspRate = 100;
        public int strRate = 100;
        public int dexRate = 100;
        public int agiRate = 100;
        public int intRate = 100;
        public int atkRate = 100;
        public int pdefRate = 100;
        public int mdefRate = 100;
        public int eva = 0;
        public bool battleOnly = true;
        public int holdTurn = 0;
        public int autoReleaseProb = 0;
        public int shockReleaseProb = 0;
        public int[] guardElementSet = { };
        public int[] plusStateSet = { };
        public int[] minusStateSet = { };

        public object Clone()
        {
            State s = (State)this.MemberwiseClone();
            s.guardElementSet = (int[])this.guardElementSet.Clone();
            s.plusStateSet = (int[])this.plusStateSet.Clone();
            s.minusStateSet = (int[])this.minusStateSet.Clone();
            return s;
        }

        public int CompareTo(object o)
        {
            State state = (State)o;
            if (this.rating > state.rating)
                return -1;
            else if (this.rating < state.rating)
                return 1;
            else if (this.restriction > state.restriction)
                return -1;
            else if (this.restriction < state.restriction)
                return 1;
            else
                return 0;
        }
    }
}
