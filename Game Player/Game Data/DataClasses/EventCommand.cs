using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class EventCommand : ICloneable
    {
        public int code = 0;
        public int indent = 0;
        public Parameter parameters = new Parameter();

        public object Clone()
        {
            EventCommand e = (EventCommand)this.MemberwiseClone();
            e.parameters = (Parameter)this.parameters.Clone();
            return e;
        }
    }
}
