using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Event : ICloneable
    {
        public int id = 0;
        public string name = "";
        public int x;
        public int y;
        public Page[] pages = { new Page() };

        public Event(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public object Clone()
        {
            Event e = (Event)this.MemberwiseClone();
            e.pages = (Page[])this.pages.DeepClone();
            return e;
        }

        [Serializable()]
        public class Page : ICloneable
        {
            public Condition condition = new Condition();
            public Graphic graphic = new Graphic();
            public int moveType = 0;
            public int moveSpeed = 3;
            public int moveFrequency = 3;
            public MoveRoute moveRoute = new MoveRoute();
            public bool walkAnime = true;
            public bool stepAnime = false;
            public bool directionFix = false;
            public bool through = false;
            public bool alwaysOnTop = false;
            public int trigger = 0;
            public EventCommand[] list = new EventCommand[] { new EventCommand() };

            public object Clone()
            {
                Page p = (Page)this.MemberwiseClone();
                p.condition = (Condition)condition.Clone();
                p.graphic = (Graphic)graphic.Clone();
                p.moveRoute = (MoveRoute)moveRoute.Clone();
                p.list = (EventCommand[])list.DeepClone();
                return p;
            }

            [Serializable()]
            public class Condition : ICloneable
            {
                public bool switch1Valid = false;
                public bool switch2Valid = false;
                public bool variableValid = false;
                public bool selfSwitchValid = false;
                public int switch1Id = 1;
                public int switch2Id = 1;
                public int variableId = 1;
                public int variableValue = 0;
                public char selfSwitchCh = 'A';

                public object Clone()
                {
                    return this.MemberwiseClone();
                }
            }

            [Serializable()]
            public class Graphic : ICloneable
            {
                public int tileId = 0;
                public string characterName = "";
                public int characterHue = 0;
                public int direction = 2;
                public int pattern = 0;
                public int opacity = 255;
                public int blendType = 0;

                public object Clone()
                {
                    return this.MemberwiseClone();
                }
            }
        }
    }
}
