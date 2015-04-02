using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public struct Tone
    {
        int _red;
        public int Red
        {
            get { return _red; }
            set { _red = MakeToneValue(value); }
        }

        int _green;
        public int Green
        {
            get { return _green; }
            set { _green = MakeToneValue(value); }
        }

        int _blue;
        public int Blue
        {
            get { return _blue; }
            set { _blue = MakeToneValue(value); }
        }

        int _gray;
        public int Gray
        {
            get { return _gray; }
            set { _gray = Math.Max(Math.Min(value, 255), 0); }
        }

        public Tone(int red, int green, int blue) : this(red, green, blue, 0) { }

        public Tone(int red, int green, int blue, int gray)
        {
            _red = red;
            _green = green;
            _blue = blue;
            _gray = gray;
        }

        int MakeToneValue(int value)
        {
            return Math.Max(Math.Min(value, 255), -255);
        }

        public Color Modify(Color arg)
        {
            int red = arg.Red + Red;
            int green = arg.Green + Green;
            int blue = arg.Blue + Blue;

            Color c = new Color(red, green, blue);

            double perc = Gray / 255.0;
            int average = (c.Red + c.Green + c.Blue) / 3;

            c.Red += (int)((average - c.Red) * perc);
            c.Green += (int)((average - c.Green) * perc);
            c.Blue += (int)((average - c.Blue) * perc);

            return c;
        }
    }
}
