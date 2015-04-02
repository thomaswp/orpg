using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class MoveRoute : ICloneable
    {
        public bool repeat = true;
        public bool skippable = false;
        public MoveCommand[] list = { new MoveCommand() };

        public object Clone()
        {
            MoveRoute m = (MoveRoute)this.MemberwiseClone();
            m.list = (MoveCommand[])list.DeepClone();
            return m;
        }
    }
}
