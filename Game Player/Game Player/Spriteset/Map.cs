using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Spriteset
{
    public class Map
    {
        Viewport viewport1, viewport2, viewport3;
        Tilemap tilemap;
        Plane panorama, fog;
        Sprite[] characterSprites;
        Game.Weather weather;
        Sprites.Picture[] pictureSprites;
        Sprites.Timer timerSprite;
        string panoramaName, fogName;
        int panoramaHue, fogHue;

        public Map()
        {
            viewport1 = new Viewport(Graphics.ScreenRect);
            viewport2 = new Viewport(Graphics.ScreenRect);
            viewport3 = new Viewport(Graphics.ScreenRect);

            viewport2.Z = 200;
            viewport3.Z = 5000;

            //testing
            //for (int i = 48; i < 96; i++)
            //{
            //    int j = i - 48;
            //    Globals.GameMap.Data[j % 5, j / 5, 0] = i;
            //}

            tilemap = new Tilemap(viewport1);
            tilemap.Tileset = Cache.LoadTileset(Globals.GameMap.TilesetName);
            tilemap.AutoTiles = new Bitmap[7];
            for (int i = 0; i <= 6; i++)
            {
                string autotileName = Globals.GameMap.AutotileNames[i];
                tilemap.AutoTiles[i] = Cache.LoadAutotile(autotileName);
            }
            tilemap.MapData = Globals.GameMap.Data;
            tilemap.Priorities = Globals.GameMap.Priorities;
            tilemap.MakeMap(); //added

            panorama = new Plane(viewport1);
            panorama.Z = -1000;

            fog = new Plane(viewport1);
            fog.Z = 3000;

            characterSprites = new Sprites.Character[] { };
            foreach (Game.Event evnt in Globals.GameMap.Events.Values)
            {
                Sprites.Character sprite = new Sprites.Character(viewport1, evnt);
                characterSprites = characterSprites.Plus<Sprite>(sprite);
            }
            characterSprites = characterSprites.Plus<Sprite>(new Sprites.Character(viewport1, Globals.GamePlayer));

            weather = new Game.Weather(viewport1);

            pictureSprites = new Sprites.Picture[] { };
            for (int i = 1; i <= 50; i++)
                pictureSprites = pictureSprites.Plus<Sprites.Picture>(new Sprites.Picture(viewport2, Globals.GameScreen.Pictures[i]));

            timerSprite = new Sprites.Timer();

            
        }

        public void Dispose()
        {
            tilemap.Tileset.Dispose();
            for (int i = 0; i < 6; i++)
                tilemap.AutoTiles[i].Dispose();
            tilemap.Dispose();

            panorama.Dispose();

            fog.Dispose();

            foreach (Sprite sprite in characterSprites)
                sprite.Dispose();

            weather.Dispose();

            foreach (Sprite sprite in pictureSprites)
                sprite.Dispose();

            timerSprite.Dispose();

            viewport1.Dispose();
            viewport2.Dispose();
            viewport3.Dispose();
        }

        public void Update()
        {
            if (panoramaName != Globals.GameMap.PanoramaName ||
                panoramaHue != Globals.GameMap.PanoramaHue)
            {
                panoramaName = Globals.GameMap.PanoramaName;
                panoramaHue = Globals.GameMap.PanoramaHue;

                if (panorama.Bitmap != null)
                {
                    panorama.Bitmap.Dispose();
                    panorama.Bitmap = null;
                }
                if (panoramaName != "")
                    panorama.Bitmap = Cache.LoadPanorama(panoramaName, panoramaHue);

                //Graphics.FrameReset();  //Do we need this!?
            }

            if (fogName != Globals.GameMap.FogName ||
                fogHue != Globals.GameMap.FogHue)
            {
                fogName = Globals.GameMap.FogName;
                fogHue = Globals.GameMap.FogHue;

                if (fog.Bitmap != null)
                {
                    fog.Bitmap.Dispose();
                    fog.Bitmap = null;
                }
                if (fogName != "")
                    fog.Bitmap = Cache.LoadFog(fogName, fogHue);
                
                //Reset?
            }

            tilemap.OX = Globals.GameMap.DisplayX / 4;
            tilemap.OY = Globals.GameMap.DisplayY / 4;
            tilemap.Update();

            panorama.OX = Globals.GameMap.DisplayX / 8;
            panorama.OY = Globals.GameMap.DisplayY / 8;

            fog.ZoomX = Globals.GameMap.FogZoom / 100.0;
            fog.ZoomY = Globals.GameMap.FogZoom / 100.0;
            fog.Opactiy = Globals.GameMap.FogOpacity;
            //blend type?
            fog.OX = Globals.GameMap.DisplayX / 4 + Globals.GameMap.FogOX;
            fog.OY = Globals.GameMap.DisplayY / 4 + Globals.GameMap.FogOY;
            fog.Tone = Globals.GameMap.FogTone;

            foreach (Sprite sprite in characterSprites)
                sprite.Update();

            weather.Type = Globals.GameScreen.WeatherType;
            weather.Max = Globals.GameScreen.WeatherMax;
            weather.OX = Globals.GameMap.DisplayX / 4;
            weather.OY = Globals.GameMap.DisplayY / 4;
            weather.Update();

            foreach (Sprite sprite in pictureSprites)
                sprite.Update();

            timerSprite.Update();

            viewport1.Tone = Globals.GameScreen.Tone;
            viewport1.OX = Globals.GameScreen.Shake;

            viewport3.Color = Globals.GameScreen.FlashColor;

            viewport1.Update();
            viewport3.Update();
        }
    }
}
