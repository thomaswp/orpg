using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace DataClasses
{
    [Serializable()]
    public class Item : ItemType, ICloneable
    {
        public int id { get; set; }
        public string name { get; set; }
        public string iconName { get; set; }
        public string description { get; set; }
        public int scope = 0;
        public int occasion = 0;
        public int animation1Id = 0;
        public int animation2Id = 0;
        public AudioFile menuSe = new AudioFile("", 80, 100);
        public int commonEventId = 0;
        public int price = 0;
        public bool consumable = true;
        public int parameterType = 0;
        public int parameterPoints = 0;
        public int recoverHpRate = 0;
        public int recoverHp = 0;
        public int recoverSpRate = 0;
        public int recoverSp = 0;
        public int hit = 100;
        public int pdefF = 0;
        public int mdefF = 0;
        public int variance = 0;
        public int[] elementSet ={ };
        public int[] plusStateSet = { };
        public int[] minusStateSet = { };

        public object Clone()
        {
            Item i = (Item)this.MemberwiseClone();
            i.menuSe = (AudioFile)this.menuSe;
            i.elementSet = (int[])this.elementSet;
            i.plusStateSet = (int[])this.plusStateSet;
            i.minusStateSet = (int[])this.minusStateSet;
            return i;
        }
    }
}
