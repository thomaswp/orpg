using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace DataClasses
{
    [Serializable()]
    public class Skill : ICloneable
    {
        public int id = 0;
        public string name = "";
        public string iconName = "";
        public string description = "";
        public int scope = 0;
        public int occasion = 1;
        public int animation1Id = 0;
        public int animation2Id = 0;
        public AudioFile menuSe = new AudioFile("", 80, 100);
        public int commonEventId = 0;
        public int spCost = 0;
        public int power = 0;
        public int atkF = 0;
        public int evaF = 0;
        public int strF = 0;
        public int dexF = 0;
        public int agiF = 0;
        public int intF = 100;
        public int hit = 100;
        public int pdefF = 0;
        public int mdefF = 100;
        public int variance = 15;
        public int[] elementSet = { };
        public int[] plusStateSet = { };
        public int[] minusStateSet = { };

        public object Clone()
        {
            Skill s = (Skill)this.MemberwiseClone();
            s.menuSe = (AudioFile)this.menuSe.Clone();
            s.elementSet = (int[])this.elementSet.Clone();
            s.plusStateSet = (int[])this.plusStateSet.Clone();
            s.minusStateSet = (int[])this.minusStateSet.Clone();
            return s;
        }
    }
}
