using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// A class that defines colors by their Alpha, Red, Green and Blue values.
    /// </summary>
    public class Color
    {
        int _alpha;
        /// <summary>
        /// The colors Alpha value. Values are in the range [0-255].
        /// </summary>
        public int Alpha
        { get { return _alpha; } set { _alpha = makeColorValue(value); } }

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

        
        /// <summary>
        /// Defines a Color with Alpha, Red, Green and Blue vlaues. Alpha defaults to 255.
        /// Values can be derived from a System.Drawing.Color.
        /// </summary>
        /// <param name="r">The Red value.</param>
        /// <param name="g">The Green value.</param>
        /// <param name="b">The Blue value.</param>
        public Color(int r, int g, int b)
        {
            Alpha = 255;
            Red = r;
            Green = g;
            Blue = b;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">The Alpha value.</param>
        /// <param name="r">The Red value.</param>
        /// <param name="g">The Green value.</param>
        /// <param name="b">The Blue value.</param>
        public Color(int a, int r, int g, int b)
        {
            Alpha = a;
            Red = r;
            Green = g;
            Blue = b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">The System.Drawing.Color from which to derive ARBG values.</param>
        public Color(System.Drawing.Color color)
        {
            Alpha = color.A;
            Red = color.R;
            Green = color.G;
            Blue = color.B;
        }

        /// <summary>
        /// Returns an equivalent System.Drawing.Color.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb(Alpha, Red, Green, Blue);
        }

        /// <summary>
        /// Returns an equivalent System.Drawing.SolidBrush.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.SolidBrush ToSolidBrush()
        {
            return new System.Drawing.SolidBrush(this.ToSystemColor());
        }

        /// <summary>
        /// Returns an equivalent System.Drawing.Pen.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Pen ToPen()
        {
            return new System.Drawing.Pen(this.ToSystemColor());
        }

        int makeColorValue(int i)
        {
            if (i > 255) { i = 255; }
            if (i < 0) { i = 0; }
            return i;
        }
    }
}
