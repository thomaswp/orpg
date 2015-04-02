using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace DataClasses
{
    [Serializable()]
    public class Map : ICloneable
    {
        public int tilesetId = 1;
        public int width;
        public int height;
        public bool autoplayBgm = false;
        public AudioFile bgm = new AudioFile();
        public bool autoplayBgs = false;
        public AudioFile bgs = new AudioFile("", 80, 100);
        public int[] encounterList = {};
        public int encounterStep = 30;
        public int[,,] data;
        //public Event[] events = {};
        public Dictionary<int, Event> events = new Dictionary<int, Event>();

        public int id;
        public string name = "";
        public int parentId = 0;
        public int order = 0;
        public bool expanded = false;
        public int scrollX = 0;
        public int scrollY = 0;



        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.data = new int[width, height, 3];
        }

        public object Clone()
        {
            Map m = (Map)this.MemberwiseClone();
            m.bgm = (AudioFile)bgm.Clone();
            m.bgs = (AudioFile)bgs.Clone();
            m.encounterList = (int[])encounterList.Clone();
            m.data = (int[,,])data.Clone();

            m.events = new Dictionary<int, Event>();
            foreach (int key in events.Keys)
                m.events.Add(key, (Event)events[key].Clone());

            return m;
        }
    }
}
