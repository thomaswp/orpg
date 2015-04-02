using System;
using System.Collections.Generic;
using System.Text;
//using DataClasses;

namespace Game_Player.Game
{
    /// <summary>
    /// A class containing instances of each user-created map.
    /// </summary> 

    public class Map
    {
        public string TilesetName { get; set; }
        public string[] AutotileNames { get; set; }
        public string PanoramaName { get; set; }
        public int PanoramaHue { get; set; }
        public string FogName { get; set; }
        public int FogHue { get; set; }
        public int FogOpacity { get; set; }
        public int FogBlendType { get; set; }
        public int FogZoom { get; set; }
        public int FogSx { get; set; }
        public int FogSy { get; set; }
        public string BattlebackName { get; set; }
        public int DisplayX { get; set; }
        public int DisplayY { get; set; }
        public bool NeedRefresh { get; set; }



        private int[] passages;
        public int[] Passages { get { return passages; } }

        private int[] priorities;
        public int[] Priorities { get { return priorities; } }

        private int[] terrainTags;
        public int[] TerrainTags { get { return terrainTags; } }

        private Dictionary<int, Event> events;
        public Dictionary<int, Event> Events { get { return events; } }

        private int fogOX;
        public int FogOX { get { return fogOX; } }

        private int fogOY;
        public int FogOY { get { return fogOY; } }

        private Tone fogTone;
        public Tone FogTone { get { return fogTone; } }

        public int mapId;
        public int MapId { get { return mapId; } }
        public int Width { get { return map.width; } }
        public int Height { get { return map.height; } }
        public int[] EncounterList { get { return map.encounterList; } }
        public int EncouterStep { get { return map.encounterStep; } }
        public int[, ,] Data { get { return map.data; } }

        public bool IsScrolling { get { throw new Exception(); } }

        DataClasses.Map map;
        CommonEvent[] commonEvents;
        Tone fogToneTarget;
        int fogToneDuration, fogOpacityDuration, fogOpacityTarget, scrollDirection, scrollSpeed, scrollRest;

        public void Setup(int mapId)
        {
            this.mapId = mapId;
            map = Game_Player.Data.Maps[mapId];

            DataClasses.Tileset tileset = Game_Player.Data.Tilesets[map.tilesetId];
            TilesetName = tileset.tilesetName;
            AutotileNames = tileset.autotileNames;
            PanoramaName = tileset.panoramaName;
            PanoramaHue = tileset.panoramaHue;
            FogName = tileset.fogName;
            FogHue = tileset.fogHue;
            FogOpacity = tileset.fogOpacity;
            FogBlendType = tileset.fogBlendType;
            FogZoom = tileset.fogZoom;
            FogSx = tileset.fogSx;
            FogSy = tileset.fogSy;
            BattlebackName = tileset.battlebackName;
            passages = tileset.passages;
            priorities = tileset.priorities;
            terrainTags = tileset.terrainTags;
            DisplayX = 0;
            DisplayY = 0;
            NeedRefresh = false;
            events = new Dictionary<int, Event>();
            foreach (int key in map.events.Keys)
            {
                events.Add(key, new Event(key, map.events[key]));
            }
            commonEvents = new CommonEvent[Game_Player.Data.CommonEvents.Length + 1];
            for (int i = 1; i < Game_Player.Data.CommonEvents.Length; i++)
                commonEvents[i] = new CommonEvent(i);
            fogTone = new Tone(0, 0, 0, 0);
            fogToneTarget = new Tone(0, 0, 0, 0);
            scrollDirection = 2;
            scrollSpeed = 4;
        }

        public void Autoplay()
        {
            if (map.autoplayBgm)
                Audio.BGM.Play(map.bgm);
            if (map.autoplayBgs)
                Audio.BGS.Play(map.bgs);
        }

        public void Refresh()
        {
            if (mapId > 0)
            {
                foreach (Event e in events.Values)
                    if (e != null)
                        e.Refresh();
                foreach (CommonEvent e in commonEvents)
                    if (e != null)
                        e.Refresh();
            }

            NeedRefresh = false;
        }

        public void ScrollUp(int distance)
        {
            DisplayY = Math.Max(DisplayY - distance, 0);
        }

        public void ScrollDown(int distance)
        {
            DisplayY = Math.Min(DisplayY + distance, (Height - 15) * 128);
        }

        public void ScrollRight(int distance)
        {
            DisplayX = Math.Min(DisplayX + distance, (Width - 20) * 128);
        }

        public void ScrollLeft(int distance)
        {
            DisplayX = Math.Max(DisplayX - distance, 0);
        }

        public bool IsValid(int x, int y)
        {
            return (x >= 0 && x < Width && y >= 0 && y < Height);
        }

        public bool IsPassable(int x, int y, int d)
        {
            return IsPassable(x, y, d, null);
        }

        public bool IsPassable(int x, int y, int d, Character selfEvent)
        {
            if (!IsValid(x, y))
                return false;

            int bit = (1 << (d / 2 - 1)) & 0x0f;

            foreach (Event e in events.Values)
            {
                if (e != null)
                {
                    if (e.TileId >= 0 && e != selfEvent &&
                        e.X == x && e.Y == y && !e.Through)
                    {
                        if ((passages[e.TileId] & bit) != 0)
                            return false;
                        else if ((passages[e.TileId] & 0x0f) == 0x0f)
                            return false;
                        else if (priorities[e.TileId] == 0)
                            return true;
                    }
                }
            }

            foreach (int i in new int[] { 2, 1, 0 })
            {
                if (x != x.MinMax(0, Data.GetLength(0)) || y != y.MinMax(0, Data.GetLength(1)))
                    return false;

                int tileId = Data[x, y, i];
                if ((passages[tileId] & bit) != 0)
                    return false;
                else if ((passages[tileId] & 0x0f) == 0x0f)
                    return false;
                else if (priorities[tileId] == 0)
                    return true;
            }
            return true;
        }

        public bool IsBush(int x, int y)
        {
            if (x != x.MinMax(0, Data.GetLength(0)) || y != y.MinMax(0, Data.GetLength(1)))
                return false;

            if (mapId != 0)
            {
                foreach (int i in new int[] { 2, 1, 0 })
                {
                    int tileId = Data[x, y, i];
                    if ((passages[tileId] & 0x40) == 0x40)
                        return true;
                }
            }
            return false;
        }

        public bool IsCounter(int x, int y)
        {
            if (x != x.MinMax(0, Data.GetLength(0)) || y != y.MinMax(0, Data.GetLength(1)))
                return false;

            if (mapId != 0)
            {
                foreach (int i in new int[] { 2, 1, 0 })
                {
                    int tileId = Data[x, y, i];
                    if ((passages[tileId] & 0x80) == 0x80)
                        return true;
                }
            }
            return false;            
        }

        public int TerrainTag(int x, int y)
        {
            if (x != x.MinMax(0, terrainTags.GetLength(0)) || y != y.MinMax(0, terrainTags.GetLength(1)))
                return 0;

            if (mapId != 0)
            {
                foreach (int i in new int[] { 2, 1, 0 })
                {
                    int tileId = Data[x, y, i];
                    if (terrainTags[tileId] != 0)
                        return terrainTags[tileId];
                }
            }
            return 0;              
        }

        public int CheckEvent(int x, int y)
        {
            foreach (Event e in events.Values)
                if (e.X == x && e.Y == y)
                    return e.Id;
            return 0;
        }

        public void StartScroll(int direction, int distance, int speed)
        {
            scrollDirection = direction;
            scrollRest = distance * 128;
            scrollSpeed = speed;
        }

        public void StartFogToneChange(Tone tone, int duration)
        {
            fogToneTarget = tone;
            fogToneDuration = duration;
            if (duration == 0)
                fogTone = tone;
        }

        public void StartFogOpacityChange(int opacity, int duration)
        {
            fogOpacityTarget = opacity;
            fogOpacityDuration = duration;
            if (duration == 0)
                FogOpacity = opacity;
        }

        public void Update()
        {
            if (Globals.GameMap.NeedRefresh)
                Refresh();

            if (scrollRest > 0)
            {
                int distance = (int)Math.Pow(2, scrollSpeed);

                switch (scrollDirection)
                {
                    case 2: ScrollDown(distance); break;
                    case 4: ScrollLeft(distance); break;
                    case 6: ScrollRight(distance); break;
                    case 8: ScrollUp(distance); break;
                }

                scrollRest -= distance;
            }

            foreach (Event e in events.Values)
                if (e != null)
                    e.Update();

            foreach (CommonEvent e in commonEvents)
                if (e != null)
                    e.Update();

            fogOX -= FogSx / 8;
            fogOY -= FogSy / 8;

            if (fogToneDuration >= 1)
            {
                int d = fogToneDuration;
                Tone target = fogToneTarget;

                fogTone.Red = (fogTone.Red * (d - 1) + target.Red) / d;
                fogTone.Green = (fogTone.Green * (d - 1) + target.Green) / d;
                fogTone.Blue = (fogTone.Blue * (d - 1) + target.Blue) / d;
                fogTone.Gray = (fogTone.Gray * (d - 1) + target.Gray) / d;

                fogToneDuration--;
            }

            if (fogOpacityDuration >= 1)
            {
                int d = fogOpacityDuration;
                FogOpacity = (FogOpacity * (d - 1) + fogOpacityTarget) / d;
                fogOpacityDuration--;
            }
        }
    }
}
