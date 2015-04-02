using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// Defines a point in 2D space.
    /// </summary>
    public struct Point
    {
        int _x;
        /// <summary>
        /// The X-cooridinate of the point.
        /// </summary>
        public int X
        { get { return _x; } set { _x = value; } }

        int _y;
        /// <summary>
        /// The Y-coordinate of the point.
        /// </summary>
        public int Y
        { get { return _y; } set { _y = value; } }

        /// <summary>
        /// Initializes the point with coordinates.
        /// </summary>
        /// <param name="x">The X-cooridinate.</param>
        /// <param name="y">The Y-cooridinate.</param>
        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Initializes the point with coordinates.
        /// </summary>
        /// <param name="x">The X-cooridinate.</param>
        /// <param name="y">The Y-cooridinate.</param>
        public Point(double x, double y)
        {
            _x = (int)x;
            _y = (int)y;
        }

        /// <summary>
        /// Returns a <see cref="System.Drawing.Point">System.Drawing.Point</see> with X- and
        /// Y-coordinates matching those of this Point.
        /// </summary>
        /// <returns>The converted point.</returns>
        public System.Drawing.Point ToSystemPoint()
        {
            return new System.Drawing.Point(X, Y);
        }

        /// <summary>
        /// Returns a <see cref="Microsoft.Xna.Framework.Vector2">Microsoft.Xna.Framework.Vector2</see> 
        /// with X- and Y-coordinates matching those of this Point.
        /// </summary>
        /// <returns>The converted vector.</returns>
        public Microsoft.Xna.Framework.Vector2 ToVector2()
        {
            return new Microsoft.Xna.Framework.Vector2(X, Y);
        }

        /// <summary>
        /// Adds two points by their X- and Y-coordinates.
        /// </summary>
        /// <param name="a">The first point to be added.</param>
        /// <param name="b">The second point to be added.</param>
        /// <returns>A new point with the added X and Y values of the points.</returns>
        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtracts two points by their X- and Y-coordinates.
        /// </summary>
        /// <param name="a">The first point to be subtracted.</param>
        /// <param name="b">The second point to be subtracted.</param>
        /// <returns>A new point with the subtracted X and Y values of the points.</returns>
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    }
}
