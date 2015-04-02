using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Weapon : ItemType, ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public string iconName { get; set; }
        public string description { get; set; }
        public int animation1Id = 0;
        public int animation2Id = 0;
        public int price = 0;
        public int atk = 0;
        public int pdef = 0;
        public int mdef = 0;
        public int strPlus = 0;
        public int dexPlus = 0;
        public int agiPlus = 0;
        public int intPlus = 0;
        public int[] elementSet = { };
        public int[] plusStateSet = { };
        public int[] minusStateSet = { };

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
