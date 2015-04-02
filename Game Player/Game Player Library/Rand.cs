using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public static class Rand
    {
        private static Random random = new Random(DateTime.Now.Second);

        public static int Next(int maxValue) { return random.Next(maxValue); }
        public static int Next(int minValue, int maxValue) { return random.Next(minValue, maxValue); }
        public static double NextDouble() { return random.NextDouble(); }
    }
}
