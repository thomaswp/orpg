using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Game
{
    public class Weather
    {
        private int type;
        public int Type
        {
            get { return type; }
            set 
            {
                if (type == value)
                    return;

                type = value;
                switch (type)
                {
                    case 1: bitmap = rainBitmap; break;
                    case 2: bitmap = stormBitmap; break;
                    case 3: bitmap = snowBitmap; break;
                    default: bitmap = null; break;
                }

                for (int i = 0; i < 40; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite != null)
                    {
                        sprite.Visible = i < max;
                        sprite.Bitmap = bitmap;
                    }
                }
            }
        }

        private int ox;
        public int OX
        {
            get { return ox; }
            set 
            {
                if (ox == value)
                    return;

                ox = value;
                foreach (Sprite sprite in sprites)
                    sprite.OX = ox;
            }
        }

        private int oy;
        public int OY
        {
            get { return oy; }
            set 
            {
                if (oy == value)
                    return;

                oy = value;
                foreach (Sprite sprite in sprites)
                    sprite.OY = oy;
            }
        }

        private double max;
        public double Max
        {
            get { return max; }
            set 
            {
                if (max == value)
                    return;

                max = value.MinMax(0, 40);
                for (int i = 0; i < 40; i++)
                {
                    Sprite sprite = sprites[i];
                    if (sprite != null)
                        sprite.Visible = i < max;
                }
            }
        }

        private Bitmap rainBitmap, stormBitmap, snowBitmap, bitmap;
        private Sprite[] sprites;

        public Weather(Viewport viewport)
        {
            type = 0;
            max = 0;
            ox = 0;
            oy = 0;

            Color color1 = new Color(255, 255, 255, 255);
            Color color2 = new Color(255, 255, 255, 128);

            rainBitmap = new Bitmap(7, 56);

            for (int i = 0; i <= 6; i++)
                rainBitmap.FillRect(6 - i, i * 8, 1, 8, color1);

            stormBitmap = new Bitmap(34, 64);
            for (int i = 0; i <= 31; i++)
            {
                stormBitmap.FillRect(33 - i, i * 2, 1, 2, color2);
                stormBitmap.FillRect(32 - i, i * 2, 1, 2, color1);
                stormBitmap.FillRect(31 - i, i * 2, 1, 2, color2);
            }

            snowBitmap = new Bitmap(6, 6);
            snowBitmap.FillRect(0, 1, 6, 4, color2);
            snowBitmap.FillRect(1, 0, 4, 6, color2);
            snowBitmap.FillRect(1, 2, 4, 2, color1);
            snowBitmap.FillRect(2, 1, 2, 4, color1);

            sprites = new Sprite[40];
            for (int i = 0; i < 40; i++)
            {
                Sprite sprite = new Sprite(viewport);
                sprite.Z = 1000;
                sprite.Visible = false;
                sprite.Opactiy = 0;
                sprites[i] = sprite;
            }
        }

        public void Dispose()
        {
            foreach (Sprite sprite in sprites)
                sprite.Dispose();
            rainBitmap.Dispose();
            stormBitmap.Dispose();
            snowBitmap.Dispose();
        }

        public void Update()
        {
            if (type == 0)
                return;

            for (int i = 0; i < max; i++)
            {
                Sprite sprite = sprites[i];

                if (sprite == null)
                    break;

                if (type == 1)
                {
                    sprite.X -= 2;
                    sprite.Y += 16;
                    sprite.Opactiy -= 8;
                }

                if (type == 2)
                {
                    sprite.X -= 8;
                    sprite.Y += 16;
                    sprite.Opactiy -= 8;
                }

                if (type == 3)
                {
                    sprite.X -= 2;
                    sprite.Y += 8;
                    sprite.Opactiy -= 8;
                }

                int x = sprite.X - ox;
                int y = sprite.Y - oy;

                if (sprite.Opactiy < 64 || x < -50 || x > 750 || y < -300 || y > 500)
                {
                    sprite.X = Rand.Next(800) - 50 + ox;
                    sprite.Y = Rand.Next(800) - 200 + oy;
                    sprite.Opactiy = 255;
                }
            }
        }
    }
}
