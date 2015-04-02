using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class CommonEvent : ICloneable
    {
        public int id = 0;
        public string name = "";
        public int trigger = 0;
        public int switchId = 1;
        public EventCommand[] list = { new EventCommand() };

        public object Clone()
        {
            CommonEvent c = (CommonEvent)this.MemberwiseClone();
            c.list = (EventCommand[])this.list.DeepClone();
            return c;
        }
    }
}
