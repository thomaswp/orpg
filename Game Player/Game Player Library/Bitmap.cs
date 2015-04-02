using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Game_Player
{
    /// <summary>
    /// An enum with different font alignments for string drawing.
    /// </summary>
    public enum FontAligns 
    {
        /// <summary>
        /// Right aligned text.
        /// </summary>
        Right,
 
        /// <summary>
        /// Left aligned, or normally aligned text.
        /// </summary>
        Left, 

        /// <summary>
        /// Center aligned text.
        /// </summary>
        Center
    }

    /// <summary>
    /// An enum of the different font styles with which a string can be drawn.
    /// </summary>
    public enum FontStyles 
    { 
        /// <summary>
        /// Normal text: "Your text will look like this."
        /// Note: this description will only show styles if XML styles are enabled on you IDE.
        /// </summary>
        Normal, 

        /// <summary>
        /// Bold text. "<b>Your text will look like this.</b>"
        /// Note: this description will only show styles if XML styles are enabled on you IDE.
        /// </summary>
        Bold, 

        /// <summary>
        /// Italic text: "<i>Your text will look like this.</i>"
        /// Note: this description will only show styles if XML styles are enabled on you IDE.
        /// </summary>
        Italic, 

        /// <summary>
        /// Underlined text: "<u>Your text will look like this.</u>"
        /// Note: this description will only show styles if XML styles are enabled on you IDE.
        /// </summary>
        Underline, 

        /// <summary>
        /// Text with a strke-through: "<del>Your text will look like this.</del>"
        /// Note: this description will only show styles if XML styles are enabled on you IDE.
        /// </summary>
        StrikeThrough 
    }

    /// <summary>
    /// A class representing a 32-bit bitmap. It encapsulates the <see cref="System.Drawing.Bitmap">System.Drawing.Bitmap</see>
    /// class, allowing for easy editing. The class contains methods to modify this bitmap.
    /// </summary>
    [DebuggerVisualizer(typeof(BitmapVisualizer))]
    [Serializable]
    public class Bitmap
    {
        const int MAX_FONT_SIZE = 100;
        const int MIN_FONT_SIZE = 5;

        const string SPACING_CHAR = "";

        [NonSerialized]
        protected Microsoft.Xna.Framework.Graphics.Texture2D texture;
        public Microsoft.Xna.Framework.Graphics.Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected bool needRefresh = false;
        public bool NeedRefresh
        {
            get { return needRefresh; }
            set { needRefresh = value; }
        }

        System.Drawing.Bitmap _systemBitmap;
        /// <summary>
        /// The <see cref="System.Drawing.Bitmap">System.Drawing.Bitmap</see> that is being rendered.
        /// </summary>
        public System.Drawing.Bitmap SystemBitmap
        { get { return _systemBitmap; } set { _systemBitmap = value; NeedRefresh = true; } }

        /// <summary>
        /// The <see cref="System.Drawing.Graphics">System.Drawing.Graphics</see> class linked to the bitmap.
        /// </summary>
        public System.Drawing.Graphics Graphics
        { get { NeedRefresh = true; return System.Drawing.Graphics.FromImage(SystemBitmap); } }

        Boolean _disposed;
        /// <summary>
        /// Returns true if the bitmap has been disposed and its resources have been released.
        /// </summary>
        public Boolean Disposed
        { get { return _disposed;} }

        string _fontName;
        /// <summary>
        /// Unless explicity defined as otherwise, all text drawn to the bitmap uses this font name.
        /// </summary>
        public string FontName
        {
            get 
            {
                if (_fontName != null)
                { return _fontName; }
                else
                { return Game_Player.Graphics.FontFace; }
            }
            set { _fontName = value; }
        }

        int _fontSize;
        /// <summary>
        /// Unless explicity defined as otherwise, all text drawn to the bitmap uses this font size.
        /// </summary>
        public int FontSize
        {
            get 
            {
                if (_fontSize > 0)
                { return _fontSize; }
                else
                { return Game_Player.Graphics.FontSize; }
            }
            set { _fontSize = value; }
        }

        Color _fontColor = Colors.Black;
        /// <summary>
        /// Unless explicity defined as otherwise, all text drawn to the bitmap uses this font color.
        /// </summary>
        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        /// <summary>
        /// Gets or sets the Width of the bitmap. Minimum Width is 1.
        /// </summary>
        public int Width
        {
            get { return SystemBitmap.Width; }
            set 
            {
                int v = 1;
                if (value > v) {v = value;}
                SystemBitmap = new System.Drawing.Bitmap(SystemBitmap, new System.Drawing.Size(v, Height)); 
            }
        }

        /// <summary>
        /// Gets or sets the Height of the bitmap. Minimum Height is 1.
        /// </summary>
        public int Height
        {
            get { return SystemBitmap.Height; }
            set 
            {
                int v = 1;
                if (value > v) { v = value; }
                SystemBitmap = new System.Drawing.Bitmap(SystemBitmap, new System.Drawing.Size(Width, v)); 
            }
        }

        /// <summary>
        /// Gets or sets the color of a given pixel of the Bitmap.
        /// </summary>
        /// <param name="x">The x-coordinate of the pixel.</param>
        /// <param name="y">The y-coordinate of the pixel.</param>
        /// <returns></returns>
        public Color this[int x, int y]
        {
            get { return new Color(SystemBitmap.GetPixel(x, y)); }
            set { SystemBitmap.SetPixel(x, y, value.ToSystemColor()); NeedRefresh = true; }
        }

        /// <summary>
        /// Gets a rectangle, based at point (0, 0) with the width and height of the Bitmap.
        /// </summary>
        public Rect Rect
        {
            get { return new Rect(0, 0, Width, Height); }
        }

        /// <summary>
        /// Creates a bitmap from file path, <see cref="System.Drawing.Bitmap">System.Drawing.Bitmap</see>, or dimensions.
        /// An exception will be thrown if loading fails.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        public Bitmap(int width, int height)
        {
            if (width <= 0) { width = 1; }
            if (height <= 0) { height = 1; }
            _systemBitmap = new System.Drawing.Bitmap(width, height);
            texture = new Microsoft.Xna.Framework.Graphics.Texture2D(Game_Player.Graphics.GraphicsDevice, width, height);
        }

        /// <summary>
        /// Creates a bitmap from file path, <see cref="System.Drawing.Bitmap">System.Drawing.Bitmap</see>, or dimensions.
        /// An exception will be thrown if loading fails.
        /// </summary>
        /// <param name="fileName">The file name of the desired bitmap file.</param>
        public Bitmap(string fileName)
        {
            Utils.UsageData.StartUsage("bmp");
            string[] exts = new string[] { "", ".jpg", ".jpeg", ".bmp", ".png" };
            string[] paths = new string[] { Paths.Root, Paths.RTP};

            try
            {
                string validName = Paths.FindValidPath(fileName, paths, exts);
                FileStream stream = new FileStream(validName, FileMode.Open);
                _systemBitmap = new System.Drawing.Bitmap(stream);
                stream.Close();
                stream = new FileStream(validName, FileMode.Open);
                texture = Microsoft.Xna.Framework.Graphics.Texture2D.FromStream(Game_Player.Graphics.GraphicsDevice, stream);
                stream.Close();
                Utils.UsageData.EndUsage("bmp");
                //Console.WriteLine(Utils.UsageData.GetTotalUsage("bmp"));
                return;
            }
            catch
            {
                MsgBox.Show("Cannot load image '" + fileName + "'.");
                throw new Exception("Cannot load image '" + fileName + "'.");
            }
        }

        /// <summary>
        /// Creates a bitmap from file path, <see cref="System.Drawing.Bitmap">System.Drawing.Bitmap</see>, or dimensions.
        /// An exception will be thrown if loading fails.
        /// </summary>
        /// <param name="bmp">The System.Drawing.Bitmap from which to create the Bitmap.</param>
        public Bitmap(System.Drawing.Bitmap bmp)
        {
            SystemBitmap = bmp;
            NeedRefresh = true;
        }

        /// <summary>
        /// Disposes the Bitmap and releases all of its resources.
        /// </summary>
        public void Dispose()
        {
            if (Disposed == false)
            {
                SystemBitmap.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Resizes the Bitmap to the given Width and Height.
        /// </summary>
        /// <param name="width">The Width to which to resize.</param>
        /// <param name="height">The Height to which to resize.</param>
        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void BlockTransfer(int startX, int startY, Bitmap sourceBmp, Rect sourceRect)
        {
            BlockTransfer(startX, startY, sourceBmp, sourceRect, 255);
        }

        /// <summary>
        /// Transfers a block of pixels from a source <see cref="Game_Player.Bitmap">Bitmap</see>
        /// to this bitmap at the given coordinates.
        /// </summary>
        /// <param name="startX">The X-coordinate on this Bitmap where the transfered block is placed.</param>
        /// <param name="startY">The Y-coordinate on this Bitmap where the transfered block is placed.</param>
        /// <param name="sourceBmp">The source <see cref="Game_Player.Bitmap">Bitmap</see>.</param>
        /// <param name="sourceRect">The block of pixels to take from the sourceBmp and transfer to this bitmap.</param>
        /// <param name="opacity">The opactiy used to transfer the block. Ranges from 0 to 255. For example, an 
        /// opacity of 255 means the block fully covers the pixels underneath it, an opacity of 123 would 
        /// blend the two layers of pixels, while with an opactiy of 0 the transfered block would be fully transparent.</param>
        public void BlockTransfer(int startX, int startY, Bitmap sourceBmp, Rect sourceRect, int opacity)
        {
            StretchBlockTransfer(new Rect(startX, startY, sourceRect.Width, sourceRect.Height), sourceBmp, sourceRect, opacity);

            //if (opacity == 255)
            //{
            //    BlockTransfer(startX, startY, sourceBmp, sourceRect);
            //    return;
            //}

            //Color c;
            //for (int i = 0; i < sourceRect.Width; i++)
            //{
            //    for (int j = 0; j < sourceRect.Height; j++)
            //    {
            //        if (startX + i < Width &
            //            startY + j < Height &
            //            sourceRect.X + i < sourceBmp.Width &
            //            sourceRect.Y + j < sourceBmp.Height)
            //        {
            //            c = sourceBmp[sourceRect.X + i, sourceRect.Y + j];
            //            c.Alpha = c.Alpha * opacity / 255;
            //            Graphics.FillRectangle(c.ToSolidBrush(), startX + i, startY + j, 1, 1);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Transfers a block of pixels from a source Bitmap to this <see cref="Game_Player.Bitmap">Bitmap</see>, 
        /// stretching the block to cover the whole destination Rect.
        /// </summary>
        /// <param name="destRect">The destination rectangle into which the transfered block is placed.</param>
        /// <param name="sourceBmp">The source <see cref="Game_Player.Bitmap">Bitmap</see>, from which the transfered block is taken.</param>
        /// <param name="sourceRect">The block of pixels to take from the sourceBmp and transfer to this bitmap.</param>
        /// <param name="opacity">The opactiy used to transfer the block. Ranges from 0 to 255. For example, an 
        /// opacity of 255 means the block fully covers the pixels underneath it, an opacity of 123 would 
        /// blend the two layers of pixels, while with an opactiy of 0 the transfered block would be fully transparent.</param>
        public void StretchBlockTransfer(Rect destRect, Bitmap sourceBmp, Rect sourceRect, int opacity)
        {
            if (opacity == 255)
            {
                StretchBlockTransfer(destRect, sourceBmp, sourceRect);
                return;
            }

            float[][] matirx = new float[][]
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, opacity / 255.0f}
            };

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(matirx));

            Graphics.DrawImage(sourceBmp.SystemBitmap, destRect.ToSystemRect(), 
                sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height, 
                System.Drawing.GraphicsUnit.Pixel, attr);
        }

        public void StretchBlockTransfer(Rect destRect, Bitmap sourceBmp, Rect sourceRect)
        {
            Graphics.DrawImage(sourceBmp.SystemBitmap, destRect.ToSystemRect(), 
                sourceRect.ToSystemRect(), System.Drawing.GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Fills a rectangle the the given color.
        /// </summary>
        /// <param name="x">The X coordinate of the rectangle.</param>
        /// <param name="y">The Y coordinate of the rectangle.</param>
        /// <param name="width">The Width of the rectangle.</param>
        /// <param name="height">The Height of the rectangle.</param>
        /// <param name="color">The color of the filled rectangle. Note that the color's Alpha value will effect this.</param>
        public void FillRect(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x, y, width, height);
            Graphics.FillRectangle(color.ToSolidBrush(), rect);
        }

        /// <summary>
        /// Fills a rectangle the the given color.
        /// </summary>
        /// <param name="rect">The rectangle to be filled.</param>
        /// <param name="color">The color of the filled rectangle. Note that the color's Alpha value will effect this.</param>
        public void FillRect(Rect rect, Color color)
        {
            Graphics.FillRectangle(color.ToSolidBrush(), rect.ToSystemRect());
        }

        /// <summary>
        /// Fills an elipse with a given color.
        /// </summary>
        /// <param name="rect">The rectangle for the elipse to fill.</param>
        /// <param name="color">The color of the filled elipse. Note that the color's Alpha value will effect this.</param>
        public void FillEllipse(Rect rect, Color color)
        {
            Graphics.FillEllipse(color.ToSolidBrush(), rect.ToSystemRect());
        }

        /// <summary>
        /// Fills an elipse with a given color.
        /// </summary>
        /// <param name="centerX">The X coordinate of the <b>center</b> of the elipse.</param>
        /// <param name="centerY">The Y coordinate of the <b>center</b> of the elipse.</param>
        /// <param name="width">The Width of the elipse.</param>
        /// <param name="height">The Height of the elipse.</param>
        /// <param name="color">The color of the filled elipse. Note that the color's Alpha value will effect this.</param>
        public void FillEllipse(int centerX, int centerY, int width, int height, Color color)
        {
            Rect rect = new Rect(centerX - width / 2, centerY - height / 2, width, height);
            Graphics.FillEllipse(color.ToSolidBrush(), rect.ToSystemRect());
        }

        /// <summary>
        /// Fills a polygon designated by the given points with the given color.
        /// </summary>
        /// <param name="points">An array of <see cref="Game_Player.Point">Points</see> that designate the points of the polygon.</param>
        /// <param name="color">The color of the filled polygon. The color of the filled elipse. Note that the color's Alpha value will effect this.</param>
        public void FillPoly(Point[] points, Color color)
        {
            System.Drawing.Point[] sdPoints = new System.Drawing.Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                sdPoints[i] = points[i].ToSystemPoint();
            }
            Graphics.FillPolygon(color.ToSolidBrush(), sdPoints);
        }

        /// <summary>
        /// Draws a line from one point to another in a given color.
        /// </summary>
        /// <param name="x1">The X-coordinate of the first point.</param>
        /// <param name="y1">The Y-coordinate of the first point.</param>
        /// <param name="x2">The X-coordinate of the second point.</param>
        /// <param name="y2">The Y-coordinate of the second point.</param>
        /// <param name="color">The color of the line. Note that the color's Alpha value will effect this.</param>
        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
            Graphics.DrawLine(color.ToPen(), x1, y1, x2, y2);
        }

        /// <summary>
        /// Draws an unfilled rectangle at the given coordinates or Rect, in the given Color.
        /// </summary>
        /// <param name="x">X-coordinate of the rectangle.</param>
        /// <param name="y">Y-coordinate of the rectangle.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="color">Color of the drawn rectangle. Note that the color's Alpha value will effect this.</param>
        public void DrawRect(int x, int y, int width, int height, Color color)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x, y, width, height);
            Graphics.DrawRectangle(color.ToPen(), rect);
        }

        /// <summary>
        /// Draws an unfilled rectangle at the given coordinates or Rect, in the given Color.
        /// </summary>
        /// <param name="rect">The Rect where the rectangle will be drawn.</param>
        /// <param name="color">Color of the drawn rectangle. Note that the color's Alpha value will effect this.</param>
        public void DrawRect(Rect rect, Color color)
        {
            Graphics.DrawRectangle(color.ToPen(), rect.ToSystemRect());
        }

        /// <summary>
        /// Draws an unfilled elipse in a given color.
        /// </summary>
        /// <param name="rect">The rectangle in which to draw the elipse.</param>
        /// <param name="color">The color of the drawn elipse. Note that the color's Alpha value will effect this.</param>
        public void DrawEllipse(Rect rect, Color color)
        {
            Graphics.DrawEllipse(color.ToPen(), rect.ToSystemRect());
        }

        /// <summary>
        /// Draws an unfilled elipse in a given color.
        /// </summary>
        /// <param name="centerX">The X coordinate of the <b>center</b> of the elipse.</param>
        /// <param name="centerY">The Y coordinate of the <b>center</b> of the elipse.</param>
        /// <param name="width">The Width of the elipse.</param>
        /// <param name="height">The Height of the elipse.</param>
        /// <param name="color">The color of the drawn elipse. Note that the color's Alpha value will effect this.</param>
        public void DrawEllipse(int centerX, int centerY, int width, int height, Color color)
        {
            Rect rect = new Rect(centerX - width / 2, centerY - height / 2, width, height);
            Graphics.DrawEllipse(color.ToPen(), rect.ToSystemRect());
        }

        /// <summary>
        /// Draws a polygon designated by the given points in a given color.
        /// </summary>
        /// <param name="points">An array of Points that designate the points of the polygon.</param>
        /// <param name="color">The color of the drawn polygon. The color of the filled elipse. Note that the color's Alpha value will effect this.</param>
        public void DrawPoly(Point[] points, Color color)
        {
            System.Drawing.Point[] sdPoints = new System.Drawing.Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                sdPoints[i] = points[i].ToSystemPoint();
            }
            Graphics.DrawPolygon(color.ToPen(), sdPoints);
        }

        /// <summary>
        /// Draws a given string at the coordinates or <see cref="Game_Player.Rect">Rect</see> given. 
        /// By default the string will be at the size designated by the FontSize property, but it 
        /// will resize to fit inside the Width and Height. Style and alignment can be designated,
        /// but the alignment is defaulted to <see cref="F:Game_Player.FontAligns.Left">Left</see> 
        /// and the style to <see cref="F:Game_Player.FontStyles.Normal">Normal</see>. 
        /// This method is intended for single-line text.
        /// </summary>
        /// <param name="x">The X-coordinate of the drawn string.</param>
        /// <param name="y">The X-coordinate of the drawn string.</param>
        /// <param name="width">The max Width of the drawn string.</param>
        /// <param name="height">The max Height of the drawn string.</param>
        /// <param name="str">The string to be drawn.</param>
        /// <param name="alignment">The <see cref="Game_Player.FontAligns">FontAligns</see> that designates 
        /// how to align the drawn string.</param>
        /// <param name="style">The <see cref="Game_Player.FontStyles">FontStyles</see> that designates 
        /// how to style the drawn string.</param>
        public void DrawText(int x, int y, int width, int height, string str, FontAligns alignment, FontStyles style) { DrawText(x, y, width, height, str, alignment, style, true); }
        public void DrawText(int x, int y, int width, int height, string str, FontAligns alignment, FontStyles style, bool blend)
        {

            System.Drawing.FontStyle drawingStyle = System.Drawing.FontStyle.Regular;
            switch (style)
            {
                case FontStyles.Normal: drawingStyle = System.Drawing.FontStyle.Regular; break;
                case FontStyles.Bold: drawingStyle = System.Drawing.FontStyle.Bold; break;
                case FontStyles.Italic: drawingStyle = System.Drawing.FontStyle.Italic; break;
                case FontStyles.Underline: drawingStyle = System.Drawing.FontStyle.Underline; break;
                case FontStyles.StrikeThrough: drawingStyle = System.Drawing.FontStyle.Strikeout; break;
            }

            System.Drawing.Font font = new System.Drawing.Font(FontName, FontSize, drawingStyle);
            Rect rect = new Rect(x, y, width, height);
            //DrawRect(rect, Colors.Black);
            while (TextSize(str, font.Size, font.Name).Width > rect.Width ||
                   TextSize(str, font.Size, font.Name).Height > rect.Height &
                   font.Size >= MIN_FONT_SIZE)
            {
                font = new System.Drawing.Font(font.Name, font.Size - 1, font.Style);
            }

            int n = 0;
            while (TextSize(str, font.Size, font.Name).Height + n * 2 - 1 < rect.Height)
                n++;
            rect.Y += n;

            int length = TextSize(str, font.Size, font.Name).Width;
            int gLength = (int)Graphics.MeasureString(str, font).Width;
            if (alignment == FontAligns.Right)
                rect.X += rect.Width - gLength;
            if (alignment == FontAligns.Center)
                rect.X += (rect.Width - gLength) / 2;

            //rect.X -= ((int)Graphics.MeasureString(str, font).Width - length) / 2;
            //Graphics.DrawString(str, font, FontColor.ToSolidBrush(), rect.ToSystemRect());

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(rect.Width, rect.Height);
            System.Drawing.Graphics.FromImage(bmp).Clear(Colors.Green.ToSystemColor());
            System.Drawing.Graphics.FromImage(bmp).DrawString(str, font, Colors.Red.ToSolidBrush(), new System.Drawing.PointF(0, 0));

            float[] cm = new float[] { FontColor.Red / 255.0f, FontColor.Green / 255.0f, FontColor.Blue / 255.0f };

            float[][] matrix = new float[][]
            {
                new float[] {cm[0], cm[1], cm[2], 1, 0},
                new float[] {cm[0], cm[1], cm[2], 0, 0},
                new float[] {cm[0], cm[1], cm[2], 1, 0},
                new float[] {0, 0, 0, 0, 0},
                new float[] {0, 0, 0, 0, 1}
            };

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(matrix));
            Graphics.DrawImage(bmp, rect.ToSystemRect(), 0, 0, bmp.Width, bmp.Height, System.Drawing.GraphicsUnit.Pixel, attr);
        }

        public Rect TextSize(string text) { return TextSize(text, FontSize, FontName); }
        public static Rect TextSize(string text, float fontSize, string fontName)
        {
            System.Drawing.Bitmap holderBmp = new System.Drawing.Bitmap(1, 1);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(holderBmp);

            if (text.Length == 0)
                return new Rect(0, 0, 0, 0);

            //bool needRF = needRefresh;

            System.Drawing.SizeF size = g.MeasureString(text, 
                new System.Drawing.Font(fontName, fontSize));
            System.Drawing.Font font = new System.Drawing.Font(fontName, fontSize);

            System.Drawing.SizeF r = g.MeasureString(text, font);
            Rect rect = new Rect(0, 0, (int)(r.Width + 0.5), (int)(r.Height + 0.5));

            //needRefresh = needRF;
            holderBmp.Dispose();
            return rect;
        }

        public int CharacterLength(char c) { return CharacterLength(c, FontSize, FontName); }
        public static int CharacterLength(char c, float fontSize, string fontName)
        {
            System.Drawing.Bitmap holderBmp = new System.Drawing.Bitmap(1, 1);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(holderBmp);
            //bool needRF = needRefresh;

            if (c == ' ')
            { }

            string text = "" + c;

            System.Drawing.SizeF size = g.MeasureString(text,
                new System.Drawing.Font(fontName, fontSize));
            System.Drawing.Font font = new System.Drawing.Font(fontName, fontSize);
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            format.FormatFlags = System.Drawing.StringFormatFlags.MeasureTrailingSpaces;
            System.Drawing.CharacterRange[] ranges = new System.Drawing.CharacterRange[] { new System.Drawing.CharacterRange(0, 1) };
            format.SetMeasurableCharacterRanges(ranges);
            System.Drawing.Rectangle sdRect = new System.Drawing.Rectangle(0, 0, (int)(size.Width + 0.5), (int)(size.Height + 0.5));
            System.Drawing.Region[] regions = g.MeasureCharacterRanges(text, font, sdRect, format);
            System.Drawing.RectangleF rectF = regions[0].GetBounds(g);

            //needRefresh = needRF;
            holderBmp.Dispose();
            return (int)(rectF.Width * (c == ' ' ? 1.7 : 1) + 0.5);
        }

        /// <summary>
        /// Draws a given string at the coordinates or <see cref="Game_Player.Rect">Rect</see> given. 
        /// By default the string will be at the size designated by the FontSize property, but it 
        /// will resize to fit inside the Width and Height. Style and alignment can be designated,
        /// but the alignment is defaulted to <see cref="F:Game_Player.FontAligns.Left">Left</see> 
        /// and the style to <see cref="F:Game_Player.FontStyles.Normal">Normal</see>. 
        /// This method is intended for single-line text.
        /// </summary>
        /// <param name="x">The X-coordinate of the drawn string.</param>
        /// <param name="y">The X-coordinate of the drawn string.</param>
        /// <param name="width">The max Width of the drawn string.</param>
        /// <param name="height">The max Height of the drawn string.</param>
        /// <param name="str">The string to be drawn.</param>
        public void DrawText(int x, int y, int width, int height, string str)
        {
            DrawText(x, y, width, height, str, FontAligns.Left, FontStyles.Normal);
        }

        /// <summary>
        /// Draws a given string at the coordinates or <see cref="Game_Player.Rect">Rect</see> given. 
        /// By default the string will be at the size designated by the FontSize property, but it 
        /// will resize to fit inside the Width and Height. Style and alignment can be designated,
        /// but the alignment is defaulted to <see cref="F:Game_Player.FontAligns.Left">Left</see> 
        /// and the style to <see cref="F:Game_Player.FontStyles.Normal">Normal</see>. 
        /// This method is intended for single-line text. 
        /// </summary>
        /// <param name="rect">The Rect where the string will be drawn.</param>
        /// <param name="str">The string to be drawn.</param>
        public void DrawText(Rect rect, string str)
        {
            DrawText(rect.X, rect.Y, rect.Width, rect.Height, str, FontAligns.Left, FontStyles.Normal);
        }


        public void DrawText(Rect rect, string str, FontAligns align)
        {
            DrawText(rect.X, rect.Y, rect.Width, rect.Height, str, align);
        }

        public void DrawText(int x, int y, int width, int height, string str, FontAligns align)
        {
            DrawText(x, y, width, height, str, align, FontStyles.Normal);
        }

        /// <summary>
        /// Clears the Bitmap to the given color. Color defaults to 
        /// <see cref="Game_Player.Colors.Clear">Clear</see>.
        /// </summary>
        public void Clear()
        {
            Clear(Colors.Clear);
        }

        /// <summary>
        /// Clears the Bitmap to the given color. Color defaults to 
        /// <see cref="Game_Player.Colors.Clear">Clear</see>.
        /// </summary>
        /// <param name="color">The color used to clear the Bitmap.</param>
        public void Clear(Color color)
        {
            Graphics.Clear(color.ToSystemColor());
        }

        /// <summary>
        /// Clears a rectangle of the Bitmap to a certain color.
        /// </summary>
        /// <param name="rect">The Rect being cleared.</param>
        public void ClearRect(Rect rect)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width, Height);
            System.Drawing.Graphics.FromImage(bmp).Clear(Colors.Clear.ToSystemColor());
            if (rect.X > 0)
            {
                System.Drawing.Graphics.FromImage(bmp).DrawImage(SystemBitmap,
                    new System.Drawing.Rectangle(0, 0, rect.X, Height),
                    0, 0, rect.X, Height, System.Drawing.GraphicsUnit.Pixel);
            }
            if (rect.Right < Width)
            {
                System.Drawing.Graphics.FromImage(bmp).DrawImage(SystemBitmap,
                    new System.Drawing.Rectangle(rect.Right, 0, Width - rect.Right, Height),
                    rect.Right, 0, Width - rect.Right, Height, System.Drawing.GraphicsUnit.Pixel);
            }
            if (rect.Y > 0)
            {
                System.Drawing.Graphics.FromImage(bmp).DrawImage(SystemBitmap,
                    new System.Drawing.Rectangle(rect.X, 0, rect.Width, rect.Y),
                    rect.X, 0, rect.Width, rect.Y, System.Drawing.GraphicsUnit.Pixel);
            }
            if (rect.Bottom < Height)
            {
                System.Drawing.Graphics.FromImage(bmp).DrawImage(SystemBitmap,
                    new System.Drawing.Rectangle(rect.X, rect.Bottom, rect.Width, Height - rect.Bottom),
                    rect.X, rect.Bottom, rect.Width, Height - rect.Bottom, System.Drawing.GraphicsUnit.Pixel);
            }

            SystemBitmap = bmp;
        }

        public void HueChange(int hue)
        {
            hue %= 360;
            if (hue == 0)
                return;

            float[][] matrix = HSLColor.HueMatrix(hue);

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(matrix), 
                System.Drawing.Imaging.ColorMatrixFlag.Default, 
                System.Drawing.Imaging.ColorAdjustType.Bitmap);

            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)SystemBitmap.Clone();
            Graphics.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, Width, Height), 
                0, 0, Width, Height, System.Drawing.GraphicsUnit.Pixel, attr);
        }

        public Bitmap Clone()
        {
            Bitmap bmp = new Bitmap((System.Drawing.Bitmap)this.SystemBitmap.Clone());
            bmp.FontName = this.FontName;
            bmp.FontSize = this.FontSize;
            bmp.FontColor = this.FontColor;

            return bmp;
        }
    }
}
