using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    /// <summary>
    /// A list of all the user-manipulated variables.
    /// </summary>
    public class Variables
    {
        int[] data = new int[1000];

        public int this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
    }
}
