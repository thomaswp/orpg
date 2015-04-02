using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// An enum of the different font aligns with which a string can be drawn.
    /// </summary>
    public enum FontAligns {Right, Left, Center}

    /// <summary>
    /// An enum of the different font styles with which a string can be drawn.
    /// </summary>
    public enum FontStyles { Normal, Bold, Italic, Underline, StrikeThrough }

    /// <summary>
    /// A class representing a 32-bit bitmap. It encapsulates the System.Drawing.Bitmap
    /// class, allowing for easy editing. The class contains methods to modify this bitmap.
    /// </summary>
    public class Bitmap
    {
        const int MAX_FONT_SIZE = 100;
        const int MIN_FONT_SIZE = 5;

        const string SPACING_CHAR = "I";

        System.Drawing.Bitmap _systemBitmap;
        /// <summary>
        /// The System.Drawing.Bitmap that is being rendered.
        /// </summary>
        public System.Drawing.Bitmap SystemBitmap
        { get { return _systemBitmap;} set { _systemBitmap = value; } }

        /// <summary>
        /// The System.Drawing.Graphics class linked to the bitmap.
        /// </summary>
        public System.Drawing.Graphics Graphics
        { get { return System.Drawing.Graphics.FromImage(SystemBitmap); } }

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
                { return Globals.FontFace; }
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
                { return Globals.FontSize; }
            }
            set { _fontSize = value; }
        }

        Color _fontColor;
        /// <summary>
        /// Unless explicity defined as otherwise, all text drawn to the bitmap uses this font color.
        /// </summary>
        public Color FontColor
        {
            get
            {
                if (_fontColor != null)
                { return _fontColor; }
                else
                { return Colors.Black; }
            }
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
            set { SystemBitmap.SetPixel(x, y, value.ToSystemColor()); }
        }

        /// <summary>
        /// Gets a rectangle, based at point (0, 0) with the width and height of the Bitmap.
        /// </summary>
        public Rect Rect
        {
            get { return new Rect(0, 0, Width, Height); }
        }

        /// <summary>
        /// Creates a bitmap from file path, System.Drawing.Bitmap, or dimensions.
        /// An exception will be thrown if loading fails.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <exception cref=">"
        public Bitmap(int width, int height)
        {
            if (width <= 0) { width = 1; }
            if (height <= 0) { height = 1; }
            SystemBitmap = new System.Drawing.Bitmap(width, height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">The file name of the desired bitmap file.</param>
        public Bitmap(string fileName)
        {
            try
            { SystemBitmap = new System.Drawing.Bitmap(fileName); }
            catch
            { 
                Globals.GameSystem.MsgBox("Cannot load image '" + fileName + "'.");
                throw new Exception("Cannot load image '" + fileName + "'.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp">The System.Drawing.Bitmap from which to create the Bitmap.</param>
        public Bitmap(System.Drawing.Bitmap bmp)
        {
            SystemBitmap = bmp;
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

        /// <summary>
        /// Transfers a block of pixels from a source Bitmap to this bitmap at the given coordinates.
        /// </summary>
        /// <param name="startX">The X-coordinate on this Bitmap where the transfered block is placed.</param>
        /// <param name="startY">The Y-coordinate on this Bitmap where the transfered block is placed.</param>
        /// <param name="sourceBmp">The source Bitmap.</param>
        /// <param name="sourceRect">The block of pixels to take from the sourceBmp and transfer to this bitmap.</param>
        /// <param name="opacity">The opactiry used to transfer the block. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the block is fully covers the pixels underneath it, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the transfered block would be fully transparent.</param>
        public void BlockTransfer(int startX, int startY, Bitmap sourceBmp, Rect sourceRect, double opacity)
        {
            Color c;
            for (int i = 0; i < sourceRect.Width; i++)
            {
                for (int j = 0; j < sourceRect.Height; j++)
                {
                    if (startX + i < Width & 
                        startY + j < Height & 
                        sourceRect.X + i < sourceBmp.Width & 
                        sourceRect.Y + j < sourceBmp.Height)
                    {
                        c =  sourceBmp[sourceRect.X + i, sourceRect.Y + j];
                        c.Alpha = (int)(c.Alpha * opacity);
                        Graphics.FillRectangle(c.ToSolidBrush(), startX + i, startY + j, 1, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Transfers a block of pixels from a source Bitmap to this Bitmap, stretching the 
        /// block to cover the whole destination Rect.
        /// </summary>
        /// <param name="destRect">The destination rectangle into which the transfered block is placed.</param>
        /// <param name="sourceBmp">The source Bitmap, from which the transfered block is taken.</param>
        /// <param name="sourceRect">The block of pixels to take from the sourceBmp and transfer to this bitmap.</param>
        /// <param name="opacity">The opactiry used to transfer the block. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the block is fully covers the pixels underneath it, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the transfered block would be fully transparent.</param>
        public void StretchBlockTransfer(Rect destRect, Bitmap sourceBmp, Rect sourceRect, double opacity)
        {
            Color c;
            int destX, destY, sourceX, sourceY;
            for (int i = 0; i < destRect.Width; i++)
            {
                for (int j = 0; j < destRect.Height; j++)
                {
                    destX = destRect.X + i;
                    destY = destRect.Y + j;
                    sourceX = sourceRect.X + i * sourceRect.Width / destRect.Width;
                    sourceY = sourceRect.Y + j * sourceRect.Height / destRect.Height;
                    if (destX < Width &
                        destY < Height &
                        sourceX < sourceBmp.Width &
                        sourceY < sourceBmp.Height)
                    {
                        c = sourceBmp[sourceX, sourceY];
                        c.Alpha = (int)(c.Alpha * opacity);
                        Graphics.FillRectangle(c.ToSolidBrush(), destX, destY, 1, 1);
                    }
                }
            }
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
        /// 
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
        /// 
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
        /// <param name="points">An array of Points that designate the points of the polygon.</param>
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
        /// 
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
        /// 
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
        /// Draws a given string at the coordinates or Rect given. By default the string will be at the size
        /// designated by the FontSize property, but it will resize to fit inside the Width and Height.
        /// Style and alignment can be designated, but the alignment is defaulted to Left and the style to Normal.
        /// This method is intended for single-line text.
        /// </summary>
        /// <param name="x">The X-coordinate of the drawn string.</param>
        /// <param name="y">The X-coordinate of the drawn string.</param>
        /// <param name="width">The max Width of the drawn string.</param>
        /// <param name="height">The max Height of the drawn string.</param>
        /// <param name="str">The string to be drawn.</param>
        /// <param name="alignment">The FontAligns that designates how to align the drawn string.</param>
        /// <param name="style">The FontStyles that designates how to style the drawn string.</param>
        public void DrawText(int x, int y, int width, int height, string str, FontAligns alignment, FontStyles style)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontName, FontSize);
            Rect rect = new Rect(x, y, width, height);
            while ((Graphics.MeasureString(str + SPACING_CHAR, font).Width > rect.Width ||
                Graphics.MeasureString(str + SPACING_CHAR, font).Height > rect.Height) &
                font.Size >= MIN_FONT_SIZE)
            {
                font = new System.Drawing.Font(FontName, font.Size - 1);
            }
            int n = 0;
            while (Graphics.MeasureString(str, font).Height + n * 2 < rect.Height) { n++; }
            rect.Y += n;
            if (style == FontStyles.Bold)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Bold); }
            else if (style == FontStyles.Italic)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Italic); }
            else if (style == FontStyles.Underline)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Underline); }
            else if (style == FontStyles.StrikeThrough)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Strikeout); }
            else
            { font = new System.Drawing.Font(FontName, font.Size); }
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            if (alignment == FontAligns.Right)
            { sf.Alignment = System.Drawing.StringAlignment.Far; }
            else if (alignment == FontAligns.Center)
            { sf.Alignment = System.Drawing.StringAlignment.Center; }
            else
            { sf.Alignment = System.Drawing.StringAlignment.Near; }
            
            Graphics.DrawString(str, font, FontColor.ToSolidBrush(), rect.ToSystemRect(), sf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The X-coordinate of the drawn string.</param>
        /// <param name="y">The X-coordinate of the drawn string.</param>
        /// <param name="width">The max Width of the drawn string.</param>
        /// <param name="height">The max Height of the drawn string.</param>
        /// <param name="str">The string to be drawn.</param>
        public void DrawText(int x, int y, int width, int height, string str)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontName, FontSize);
            Rect rect = new Rect(x, y, width, height);
            while ((Graphics.MeasureString(str + SPACING_CHAR, font).Width > rect.Width ||
                Graphics.MeasureString(str + SPACING_CHAR, font).Height > rect.Height) & 
                font.Size >= MIN_FONT_SIZE)
            {
                font = new System.Drawing.Font(FontName, font.Size - 1);
            }
            int n = 0;
            while (Graphics.MeasureString(str, font).Height + n * 2 < rect.Height) { n++; }
            rect.Y += n;
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            sf.Alignment = System.Drawing.StringAlignment.Near;
            Graphics.DrawString(str, font, FontColor.ToSolidBrush(), rect.ToSystemRect(), sf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">The Rect where the string will be drawn.</param>
        /// <param name="str">The string to be drawn.</param>
        public void DrawText(Rect rect, string str)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontName, FontSize);
            while ((Graphics.MeasureString(str + SPACING_CHAR, font).Width > rect.Width ||
                Graphics.MeasureString(str + SPACING_CHAR, font).Height > rect.Height) &
                font.Size >= MIN_FONT_SIZE)
            {
                font = new System.Drawing.Font(FontName, font.Size - 1);
            }
            int n = 0;
            while (Graphics.MeasureString(str, font).Height + n * 2 < rect.Height) { n++; }
            rect.Y += n;
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            sf.Alignment = System.Drawing.StringAlignment.Near;
            Graphics.DrawString(str, font, FontColor.ToSolidBrush(), rect.ToSystemRect(), sf);
        }

        /// <summary>
        /// Fills a Rect with the given string in a given font size. The Rect will be filled, afterwhich
        /// any excess in the string will be cut off. Style and alignment can be designated.
        /// The font alignment is defaulted to Left and the font style to Normal.
        /// This method is intended for multi-line strings.
        /// </summary>
        /// <param name="rect">The Rect that the string fills.</param>
        /// <param name="str">The string to be drawn.</param>
        /// <param name="fontSize">The size of the string's font.</param>
        /// <param name="alignment">The FontAligns of the string. </param>
        /// <param name="style">The FontStyles of the string.</param>
        public void FillText(Rect rect, String str, int fontSize, FontAligns alignment, FontStyles style)
        {
            System.Drawing.Font font = new System.Drawing.Font(FontName, fontSize);
            if (style == FontStyles.Bold)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Bold); }
            else if (style == FontStyles.Italic)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Italic); }
            else if (style == FontStyles.Underline)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Underline); }
            else if (style == FontStyles.StrikeThrough)
            { font = new System.Drawing.Font(FontName, font.Size, System.Drawing.FontStyle.Strikeout); }
            else
            { font = new System.Drawing.Font(FontName, font.Size); }
            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
            if (alignment == FontAligns.Right)
            { sf.Alignment = System.Drawing.StringAlignment.Far; }
            else if (alignment == FontAligns.Center)
            { sf.Alignment = System.Drawing.StringAlignment.Center; }
            else
            { sf.Alignment = System.Drawing.StringAlignment.Near; }
            Graphics.DrawString(str, font, FontColor.ToSolidBrush(), rect.ToSystemRect(), sf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">The Rect that the string fills.</param>
        /// <param name="str">The string to be drawn.</param>
        /// <param name="fontSize">The size of the string's font.</param>
        public void FillText(Rect rect, String str, int fontSize)
        {
            if (fontSize < MIN_FONT_SIZE) { fontSize = MIN_FONT_SIZE; }
            Graphics.DrawString(str, new System.Drawing.Font(FontName, fontSize), FontColor.ToSolidBrush(), rect.ToSystemRect());
        }

        /// <summary>
        /// Clears the Bitmap to the given color. Color defaults to Clear.
        /// </summary>
        public void Clear()
        {
            Clear(Colors.Clear);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">The color used to clear the Bitmap.</param>
        public void Clear(Color color)
        {
            Graphics.Clear(color.ToSystemColor());
        }

        /// <summary>
        /// Clears a Rect of the Bitmap to a certain color.
        /// </summary>
        /// <param name="rect">The Rect being cleared.</param>
        public void ClearRect(Rect rect)
        {
            ClearRect(rect, Colors.Clear);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">The Rect being cleared.</param>
        /// <param name="color">The color used to clear the Bitmap.</param>
        public void ClearRect(Rect rect, Color color)
        {
            for (int i = 0; i < rect.Width; i++)
            {
                for (int j = 0; j < rect.Height; j++)
                {
                    try
                    { this[rect.X + i, rect.Y + j] = color; }
                    catch { }
                }
            }
        }
    }
}
