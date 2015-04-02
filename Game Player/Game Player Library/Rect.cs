using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// Defines a rectangle in 2D space.
    /// </summary>
    public struct Rect
    {
        int _x;
        /// <summary>
        /// Gets or sets the X-coordinate of the rect.
        /// </summary>
        public int X
        { get { return _x; } set { _x = value; } }

        int _y;
        /// <summary>
        /// Gets or sets the Y-coordinate of the rect.
        /// </summary>
        public int Y
        { get { return _y; } set { _y = value; } }

        int _width;
        /// <summary>
        /// Gets or sets the Width of the rect.
        /// </summary>
        public int Width
        { get { return _width; } set { _width = value; } }

        int _height;
        /// <summary>
        /// Gets or sets the Height of the rect.
        /// </summary>
        public int Height
        { get { return _height; } set { _height = value; } }

        /// <summary>
        /// Gets the right edge of the Rect: X + Width.
        /// </summary>
        public int Right
        { get { return X + Width; } }

        /// <summary>
        /// Gets the bottom edge of the Rect: Y + Height.
        /// </summary>
        public int Bottom
        { get { return Y + Height; } }

        /// <summary>
        /// Created a new Rect width the given coordinates and dimensions.
        /// If no coordinates are given they are assumed to be (0, 0).
        /// </summary>
        /// <param name="x">The X-coordinate of the Rect.</param>
        /// <param name="y">The Y-coordinate of the Rect.</param>
        /// <param name="width">The Width of the Rect.</param>
        /// <param name="height">The Height of the Rect.</param>
        public Rect(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Created a new Rect width the given coordinates and dimensions.
        /// If no coordinates are given they are assumed to be (0, 0).
        /// </summary>
        /// <param name="width">The Width of the Rect.</param>
        /// <param name="height">The Height of the Rect.</param>
        public Rect(int width, int height)
        {
            _x = 0;
            _y = 0;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Returns a <see cref="System.Drawing.Rectangle">System.Drawing.Rectangle</see> created with
        /// the coordinates and dimensions of this Rect.
        /// </summary>
        /// <returns>The returned rectangle.</returns>
        public System.Drawing.Rectangle ToSystemRect()
        {
            return new System.Drawing.Rectangle(X, Y, Width, Height);
        }

        /// <summary>
        /// Returns a <see cref="Microsoft.Xna.Framework.Rectangle">
        /// Microsoft.Xna.Framework.Rectangle</see> created with the coordinates and dimensions
        /// of this Rect.
        /// </summary>
        /// <returns>The returned rectangle.</returns>
        public Microsoft.Xna.Framework.Rectangle ToXNARect()
        {
            return new Microsoft.Xna.Framework.Rectangle(X, Y, Width, Height);
        }

        public Boolean Empty()
        {
            return this == new Rect();
        }

        public static bool operator ==(Rect a, Rect b)
        {
            return a.X == b.X &
                   a.Y == b.Y &
                   a.Width == b.Width &
                   a.Height == b.Height;
        }

        public static bool operator !=(Rect a, Rect b)
        {
            return a.X != b.X ||
                   a.Y != b.Y ||
                   a.Width != b.Width ||
                   a.Height != b.Height;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
