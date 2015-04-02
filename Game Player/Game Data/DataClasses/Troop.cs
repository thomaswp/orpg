using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Troop : ICloneable
    {
        public int id = 0;
        public string name = "";
        public Member[] members = { };
        public Page[] pages = { };

        public object Clone()
        {
            Troop t = (Troop)this.MemberwiseClone();
            t.members = (Member[])this.members.DeepClone();
            t.pages = (Page[])this.pages.DeepClone();
            return t;
        }

        [Serializable()]
        public class Member : ICloneable
        {
            public int enemyId = 1;
            public int x = 0;
            public int y = 0;
            public bool hidden = false;
            public bool immortal = false;

            public object Clone()
            {
                return this.MemberwiseClone();
            }
        }

        [Serializable()]
        public class Page : ICloneable
        {
            public Condition condition = new Condition();
            public int span = 0;
            public EventCommand[] list = { new EventCommand() };

            public object Clone()
            {
                Page p = (Page)this.MemberwiseClone();
                p.condition = (Condition)this.condition.Clone();
                p.list = (EventCommand[])this.list.DeepClone();
                return p;
            }

            [Serializable()]
            public class Condition : ICloneable
            {
                public bool turnValid = false;
                public bool enemyValid = false;
                public bool actorValid = false;
                public bool switchValid = false;
                public int turnA = 0;
                public int turnB = 0;
                public int enemyIndex = 0;
                public int enemyHp = 50;
                public int actorId = 1;
                public int actorHp = 50;
                public int switchId = 1;

                public object Clone()
                {
                    return this.MemberwiseClone();
                }
            }
        }
    }
}
