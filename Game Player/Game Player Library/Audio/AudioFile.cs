using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    [Serializable()]
    public class AudioFile : ICloneable
    {
        public string name = "";
        public int volume = 100;
        public int pitch = 100;

        public AudioFile() { }
        public AudioFile(string name, int volume, int pitch)
        {
            this.name = name;
            this.volume = volume;
            this.pitch = pitch;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
