using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    [Serializable()]
    public class Tileset : ICloneable
    {
        public int id = 0;
        public string name = "";
        public string tilesetName = "";
        public string[] autotileNames = new string[7];
        public string panoramaName = "";
        public int panoramaHue = 0;
        public string fogName = "";
        public int fogHue = 0;
        public int fogOpacity = 64;
        public int fogBlendType = 0;
        public int fogZoom = 200;
        public int fogSx = 0;
        public int fogSy = 0;
        public string battlebackName = "";
        public int[] passages = new int[384];
        public int[] priorities = new int[384];
        public int[] terrainTags = new int[384];

        public Tileset() { priorities[0] = 5; }

        public object Clone()
        {
            Tileset t = (Tileset)this.MemberwiseClone();
            t.autotileNames = (string[])this.autotileNames;
            t.passages = (int[])this.passages;
            t.priorities = (int[])this.priorities;
            t.terrainTags = (int[])this.terrainTags;
            return t;
        }
    }
}
