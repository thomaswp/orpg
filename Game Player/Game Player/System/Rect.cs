using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public class Rect
    {
        int _x, _y, _width, _height;
        public int X
        { get { return _x; } set { _x = value; } }
        public int Y
        { get { return _y; } set { _y = value; } }
        public int Width
        { get { return _width; } set { _width = value; } }
        public int Height
        { get { return _height; } set { _height = value; } }
        public int Right
        { get { return X + Width; } }
        public int Bottom
        { get { return Y + Height; } }

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect(int width, int height)
        {
            X = 0;
            Y = 0;
            Width = width;
            Height = height;
        }

        public System.Drawing.Rectangle ToSystemRect()
        {
            return new System.Drawing.Rectangle(X, Y, Width, Height);
        }

        public Microsoft.Xna.Framework.Rectangle ToXNARect()
        {
            return new Microsoft.Xna.Framework.Rectangle(X, Y, Width, Height);
        }
    }
}
