using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Armor : ItemType, ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public string iconName { get; set; }
        public string description { get; set; }
        public int kind = 0;
        public int autoStateId = 0;
        public int price = 0;
        public int pdef = 0;
        public int mdef = 0;
        public int eva = 0;
        public int strPlus = 0;
        public int dexPlus = 0;
        public int agiPlus = 0;
        public int intPlus = 0;
        public int[] guardElementSet = { };
        public int[] guardStateSet = { };

        public object Clone()
        {
            Armor a = (Armor)this.MemberwiseClone();
            a.guardElementSet = (int[])this.guardElementSet.Clone();
            a.guardStateSet = (int[])this.guardStateSet.Clone();
            return a;
        }
    }
}
