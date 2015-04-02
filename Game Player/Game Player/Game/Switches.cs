using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Switches
    {
        bool[] data = new bool[1000];

        public bool this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
    }
}
