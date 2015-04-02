using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataClasses
{
    public interface ItemType
    {
        int id { get; set; }
        string name { get; set; }
        string iconName { get; set; }
        string description { get; set; }
    }
}
