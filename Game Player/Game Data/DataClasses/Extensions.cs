using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    public static class Extensions
    {
        public static ICloneable[] DeepClone(this ICloneable[] array)
        {
            ICloneable[] newArray = (ICloneable[])array.Clone();
            for (int i = 0; i < array.Length; i++)
                newArray[i] = (ICloneable)array[i].Clone();
            return newArray;
        }
    }
}
