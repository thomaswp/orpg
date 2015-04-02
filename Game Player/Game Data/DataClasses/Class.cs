using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Class : ICloneable
    {
        public int id = 0;
        public string name = "";
        public int position = 0;
        public int[] weaponSet = { };
        public int[] armorSet = { };
        public int[] elementRanks = { };
        public int[] stateRanks = { };
        public Learning[] learnings = { };

        public object Clone()
        {
            Class c = (Class)this.MemberwiseClone();
            c.weaponSet = (int[])this.weaponSet.Clone();
            c.armorSet = (int[])this.armorSet.Clone();
            c.elementRanks = (int[])this.elementRanks.Clone();
            c.stateRanks = (int[])this.stateRanks.Clone();
            c.learnings = (Learning[])this.learnings.DeepClone();
            return c;
        }

        [Serializable()]
        public class Learning : ICloneable
        {
            public int level = 1;
            public int skillId = 1;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
