using System;
using System.Collections.Generic;
using System.Text;
using Game_Player;

namespace Game_Player
{
    public static class Cache
    {
        static Dictionary<CacheData, Bitmap> dic = new Dictionary<CacheData, Bitmap>();

        public static Bitmap LoadBitmap(string path, string filename)
        {
            return LoadBitmap(path, filename, 0);
        }

        public static Bitmap LoadBitmap(string path, string filename, int hue)
        {
            List<CacheData> remove = new List<CacheData>();
            foreach (CacheData k in dic.Keys)
                if (dic[k].Disposed)
                    remove.Add(k);
            foreach (CacheData k in remove)
                dic.Remove(k);

            CacheData key = new CacheData(path + filename, hue);
            if (key.Name == "")
                return new Bitmap(32, 32);

            Bitmap bmp;
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                if (dic.ContainsKey(new CacheData(key.Name)))
                {
                    bmp = dic[new CacheData(key.Name)].Clone();
                    bmp.HueChange(hue);
                    dic.Add(key, bmp);
                    return bmp;
                }
                else
                {
                    bmp = new Bitmap(key.Name);
                    bmp.HueChange(hue);
                    dic.Add(key, bmp);
                    return bmp;
                }
            }
        }

        public static Bitmap LoadAnimation(string filename, int hue)
        {
            return LoadBitmap(@"Graphics\Animations\", filename, hue);
        }

        public static Bitmap LoadIcon(string filename)
        {
            return LoadBitmap(@"Graphics\Icons\", filename);
        }

        public static Bitmap LoadFog(string filename, int hue)
        {
            return LoadBitmap(@"Graphics\Fogs\", filename, hue);
        }

        public static Bitmap LoadPanorama(string filename, int hue)
        {
            return LoadBitmap(@"Graphics\Panoramas\", filename, hue);
        }

        public static Bitmap LoadTitle(string filename)
        {
            return LoadBitmap(@"Graphics\Titles\", filename);
        }

        public static Bitmap LoadWindowskin(string filename)
        {
            return LoadBitmap(@"Graphics\Windowskins\", filename);
        }

        public static Bitmap LoadTileset(string filename)
        {
            return LoadBitmap(@"Graphics\Tilesets\", filename);
        }

        public static Bitmap LoadAutotile(string filename)
        {
            return LoadBitmap(@"Graphics\Autotiles\", filename);
        }

        public static Bitmap LoadCharacter(string filename, int hue)
        {
            return LoadBitmap(@"Graphics\Characters\", filename, hue);
        }

        public static Bitmap LoadTile(string filename, int tileId, int hue)
        {
            CacheData key = new CacheData(filename, hue, tileId);

            Bitmap tileset = LoadTileset(filename);

            if (!dic.ContainsKey(key) || dic[key].Disposed)
            {
                if (dic.ContainsKey(key))
                    dic[key] = new Bitmap(32, 32);
                else
                    dic.Add(key, new Bitmap(32, 32));

                int x = (tileId - 384) % 8 * 32;
                int y = (tileId - 384) / 8 * 32;
                Rect rect = new Rect(x, y, 32, 32);
                dic[key].BlockTransfer(0, 0, tileset, rect);
                dic[key].HueChange(hue);
            }

            return dic[key];
        }

        public struct CacheData
        {
            private int tileId;
            public int TileId
            {
                get { return tileId; }
                set { tileId = value; }
            }

            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            private int hue;
            public int Hue
            {
                get { return hue; }
                set { hue = value; }
            }


            public CacheData(string name) : this(name, 0, 0) { }
            public CacheData(string name, int hue) : this(name, hue, 0) { }
            public CacheData(string name, int hue, int tileId)
            {
                this.name = name;
                this.hue = hue;
                this.tileId = tileId;
            }

        }
    }
}
