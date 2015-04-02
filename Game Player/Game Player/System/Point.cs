using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public class Point
    {
        int _x;
        public int X
        { get { return _x; } set { _x = value; } }
        int _y;
        public int Y
        { get { return _y; } set { _y = value; } }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public System.Drawing.Point ToSystemPoint()
        {
            return new System.Drawing.Point(X, Y);
        }

        public Microsoft.Xna.Framework.Vector2 ToVector2()
        {
            return new Microsoft.Xna.Framework.Vector2(X, Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    }
}
