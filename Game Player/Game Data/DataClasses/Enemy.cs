using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Enemy : ICloneable
    {
        public int id = 0;
        public string name = "";
        public string battlerName = "";
        public int battlerHue = 0;
        public int maxhp = 500;
        public int maxsp = 500;
        public int str = 50;
        public int dex = 50;
        public int agi = 50;
        public int intel = 50;
        public int atk = 100;
        public int pdef = 100;
        public int mdef = 100;
        public int eva = 0;
        public int animation1Id = 0;
        public int animation2Id = 0;
        public int[] elementRanks = { };
        public int[] stateRanks = { };
        public Action[] actions = { new Action() };
        public int exp = 0;
        public int gold = 0;
        public int itemId = 0;
        public int weaponId = 0;
        public int armorId = 0;
        public int treasureProb = 100;

        public object Clone()
        {
            Enemy e = (Enemy)this.MemberwiseClone();
            e.elementRanks = (int[])this.elementRanks.Clone();
            e.stateRanks = (int[])this.stateRanks.Clone();
            e.actions = (Action[])this.actions.DeepClone();
            return e;
        }

        [Serializable()]
        public class Action : ICloneable
        {
            public int kind = 0;
            public int basic = 0;
            public int skillId = 1;
            public int conditionTurn_a = 0;
            public int conditionTurn_b = 1;
            public int conditionHp = 100;
            public int conditionLevel = 1;
            public int conditionSwitch_id = 0;
            public int rating = 5;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }
    }
}
