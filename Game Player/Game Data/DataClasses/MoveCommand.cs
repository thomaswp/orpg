using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class MoveCommand : ICloneable
    {
        public int code = 0;
        public string[] parameters = { };

        public MoveCommand() { }

        public MoveCommand(int code, string[] parameters)
        {
            this.code = code;
            this.parameters = parameters;
        }

        public object Clone()
        {
            MoveCommand m = (MoveCommand)this.MemberwiseClone();
            m.parameters = (string[])parameters.Clone();
            return m;
        }
    }
}
