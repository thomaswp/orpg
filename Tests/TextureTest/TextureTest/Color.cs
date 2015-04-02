using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// A class that defines colors by their Alpha, Red, Green and Blue values.
    /// </summary>
    [Serializable()]
    public struct Color
    {
        int _red;
        /// <summary>
        /// The colors Red value. Values are in the range [0-255].
        /// </summary>
        public int Red
        { get { return _red; } set { _red = makeColorValue(value); } }

        int _green;
        /// <summary>
        /// The colors Green value. Values are in the range [0-255].
        /// </summary>
        public int Green
        { get { return _green; } set { _green = makeColorValue(value); } }

        int _blue;
        /// <summary>
        /// The colors Blue value. Values are in the range [0-255].
        /// </summary>
        public int Blue
        { get { return _blue; } set { _blue = makeColorValue(value); } }

        int _alpha;
        /// <summary>
        /// The colors Alpha value. Values are in the range [0-255].
        /// </summary>
        public int Alpha
        { get { return _alpha; } set { _alpha = makeColorValue(value); } }

        
        /// <summary>
        /// Defines a Color with Alpha, Red, Green and Blue vlaues. Alpha defaults to 255.
        /// Values can be derived from a System.Drawing.Color.
        /// </summary>
        /// <param name="r">The Red value.</param>
        /// <param name="g">The Green value.</param>
        /// <param name="b">The Blue value.</param>
        public Color(int r, int g, int b)
        {
            _red = r;
            _green = g;
            _blue = b;
            _alpha = 255;
        }
        
        /// <summary>
        /// Defines a Color with Alpha, Red, Green and Blue vlaues. Alpha defaults to 255.
        /// Values can be derived from a System.Drawing.Color.
        /// </summary>
        /// <param name="a">The Alpha value.</param>
        /// <param name="r">The Red value.</param>
        /// <param name="g">The Green value.</param>
        /// <param name="b">The Blue value.</param>
        public Color(int r, int g, int b, int a)
        {
            _red = r;
            _green = g;
            _blue = b;
            _alpha = a;
        }

        /// <summary>
        /// Defines a Color with Alpha, Red, Green and Blue vlaues. Alpha defaults to 255.
        /// Values can be derived from a System.Drawing.Color.
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color">System.Drawing.Color</see> from which to derive ARBG values.</param>
        public Color(System.Drawing.Color color)
        {
            _red = color.R;
            _green = color.G;
            _blue = color.B;
            _alpha = color.A;
        }

        /// <summary>
        /// Returns an equivalent System.Drawing.Color.
        /// </summary>
        /// <returns>The returned color.</returns>
        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(Alpha, Red, Green, Blue);
        }

        /// <summary>
        /// Returns an equivalent <see cref="System.Drawing.SolidBrush">System.Drawing.SolidBrush</see>.
        /// </summary>
        /// <returns>The returned solid brush.</returns>
        public System.Drawing.SolidBrush ToSolidBrush()
        {
            return new System.Drawing.SolidBrush(this.ToSystemColor());
        }

        /// <summary>
        /// Returns an equivalent <see cref="System.Drawing.Pen">System.Drawing.Pen</see>.
        /// </summary>
        /// <returns>The returned pen.</returns>
        public System.Drawing.Pen ToPen()
        {
            return new System.Drawing.Pen(this.ToSystemColor());
        }

        public Microsoft.Xna.Framework.Graphics.Color ToXNAColor()
        {
            Microsoft.Xna.Framework.Graphics.Color c = new Microsoft.Xna.Framework.Graphics.Color();
            c.R = (byte)Red;
            c.B = (byte)Blue;
            c.G = (byte)Green;
            c.A = (byte)Alpha;

            return c;
        }

        int makeColorValue(int i)
        {
            if (i > 255) { i = 255; }
            if (i < 0) { i = 0; }
            return i;
        }

        public static bool operator ==(Color c1, Color c2)
        {
            return
                c1.Red == c2.Red &&
                c1.Green == c2.Green &&
                c1.Blue == c2.Blue &&
                c1.Alpha == c2.Alpha;
        }

        public static bool operator !=(Color c1, Color c2)
        {
            return !(c1 == c2);
        }
    }
}
