using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace DataClasses
{
    [Serializable()]
    public class Animation : ICloneable
    {
        public int id = 0;
        public string name = "";
        public string animationName = "";
        public int animationHue = 0;
        public int position = 1;
        public int frameMax = 1;
        public Frame[] frames = { new Frame() };
        public Timing[] timings = {};

        public object Clone()
        {
            Animation a = (Animation)this.MemberwiseClone();
            a.frames = (Frame[])this.frames.DeepClone();
            a.timings = (Timing[])this.timings.DeepClone();
            return a;
        }

        [Serializable()]
        public class Frame : ICloneable
        {
            public int cellMax;
            public int[,] cellData = new int[0,0];

            public object Clone()
            {
                Frame f = (Frame)this.MemberwiseClone();
                f.cellData = (int[,])this.cellData.Clone();
                return f;
            }
        }

        [Serializable()]
        public class Timing : ICloneable
        {
            public int frame = 0;
            public AudioFile se = new AudioFile("", 80, 100);
            public int flashScope = 0;
            public Color flashColor = new Color(255, 255, 255, 255);
            public int flashDuration = 5;
            public int condition = 0;

            public object Clone()
            {
                Timing t =  (Timing)this.MemberwiseClone();
                t.se = (AudioFile)se.Clone();
                return t;
            }
        }
    }
}
