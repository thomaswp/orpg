using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Player
{
    public class Bitmap
    {
        private int width;
        public int Width 
        { 
            get { return width; } 
            //set { width = value; }
        }

        private int height;
        public int Height 
        { 
            get { return height; } 
            //set { height = value; }
        }

        protected byte[] data;
        //public byte[] Data
        //{
        //    get { return data; }
        //    set { data = value; }
        //}

        private Texture2D texture;
        private bool needRefresh;

        public Bitmap(int width, int height)
        {
            data = new byte[width * height * 4];
            this.width = width;
            this.height = height;
            needRefresh = true;
        }

        public void FillRect(Rect rect, Color color) { FillRect(rect.X, rect.Y, rect.Width, rect.Height, color); }
        public void FillRect(int x, int y, int rWidth, int rHeight, Color color)
        {
            byte red = (byte)color.Red, green = (byte)color.Green, blue = (byte)color.Blue, alpha = (byte)color.Alpha;

            rWidth = Math.Min(width - x, rWidth);
            rHeight = Math.Min(height - y, rHeight);

            int iBound = Math.Min(x + rWidth, width);
            int jBound = Math.Min(y + rHeight, height);

            int j;

            for (int i = Math.Max(0, x); i < iBound; i++)
            {
                for (j = Math.Max(0, y); j < jBound; j++)
                {
                    int index = (j * width + i) * 4;
                    data[index] = blue;
                    data[index + 1] = green;
                    data[index + 2] = red;
                    data[index + 3] = alpha;
                }
            }

            needRefresh = true;
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            byte red = (byte)color.Red, green = (byte)color.Green, blue = (byte)color.Blue, alpha = (byte)color.Alpha;


            if (x1 == x2 && y1 == y2)
            {
                SetPixel(x1, y1, red, green, blue, alpha);
                return;
            }

            if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
            {
                if (x1 > x2)
                {
                    int temp = x1;
                    x1 = x2;
                    x2 = temp;

                    temp = y1;
                    y1 = y2;
                    y2 = temp;
                }
                double slope = (y2 - y1) / (double)(x2 - x1);

                for (int x = x1; x <= x2; x++)
                {
                    int y = (int)((x - x1) * slope + y1 + 0.5);
                    SetPixel(x, y, red, green, blue, alpha);
                }
            }
            else
            {
                if (y1 > y2)
                {
                    int temp = x1;
                    x1 = x2;
                    x2 = temp;

                    temp = y1;
                    y1 = y2;
                    y2 = temp;
                }

                double slope = (x2 - x1) / (double)(y2 - y1);

                for (int y = y1; y <= y2; y++)
                {
                    int x = (int)((y - y1) * slope + x1 + 0.5);
                    SetPixel(x, y, red, green, blue, alpha);
                }
            }

            needRefresh = true;
        }

        public void SetPixel(int x, int y, byte red, byte green, byte blue, byte alpha)
        {
            int index = y * width + x;

            //faster to catch elsewhere, but not terribly
            if (index < 0 || index * 4 + 3 >= data.Length)
                return;

            index *= 4;
            data[index] = blue;
            data[index + 1] = green;
            data[index + 2] = red;
            data[index + 3] = alpha;
        }


        public Microsoft.Xna.Framework.Graphics.Texture2D ToTexture2D(GraphicsDevice device)
        {
            if (needRefresh)
            {
                if (texture == null || width != texture.Width || height != texture.Height)
                    texture = new Texture2D(device, width, height, 1, TextureUsage.None, SurfaceFormat.Color);

                texture.SetData<byte>((byte[])data.Clone());
            }

            return texture;
        }
    }
}
