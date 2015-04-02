using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    enum Sides {Right, Left, Up, Down, Center, TopLeft, TopRight, BotLeft, BotRight}

    public class Window
    {
#region Properties
        Viewport _viewport;
        public Viewport Viewport
        {
            get { return _viewport; }
        }

        Boolean _disposed = false;
        public Boolean Disposed
        {
            get { return _disposed; }
        }

        Bitmap _windowSkin;
        public Bitmap WindowSkin
        {
            get { return _windowSkin; }
            set { _windowSkin = value; LoadWindow(); LoadCursor(); LoadArrow(); }
        }

        public Bitmap Contents
        {
            get { LoadContents(); return contentsSprite.Bitmap; }
            set { LoadContents(); contentsSprite.Bitmap = value; }
        }

        Rect _cursorRect;
        public Rect CursorRect
        {
            get { return _cursorRect; }
            set { _cursorRect = value; }
        }

        Boolean _active = true;
        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }

        Boolean _visible = true;
        public Boolean Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        Boolean _paused = false;
        public Boolean Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }

        public Boolean HasCursor
        {
            get { return CursorRect != null; }
        }

        int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        int _y;
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        int _width;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        int _height;
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Rect Rect
        {
            get { return new Rect(X, Y, Width, Height); }
            set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
        }

        int _z;
        public int Z
        {
            get { return _z; }
            set
            {
                if (disposeViewport) { Viewport.Z = value; }
                _z = value;
            }
        }

        public int OX
        {
            get { return contentsSprite.OX; }
            set { contentsSprite.OX = value; }
        }

        public int OY
        {
            get { return contentsSprite.OY; }
            set { contentsSprite.OY = value; }
        }

        double _opacity = 1;
        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        double _backOpacity = 1;
        public double BackOpacity
        {
            get { return _backOpacity; }
            set { _backOpacity = value; }
        }

        double _contentsOpacity = 1;
        public double ContentsOpacity
        {
            get { return _contentsOpacity; }
            set { _contentsOpacity = value; }
        }
#endregion

#region Constants
        int rX { get { return X + WS_BORDER_WIDTH; } }
        int rY { get { return Y + WS_BORDER_WIDTH; } }
        int rWidth { get { return Width - 2 * WS_BORDER_WIDTH; } }
        int rHeight { get { return Height - 2 * WS_BORDER_WIDTH; } }

        const int WS_BORDER_WIDTH = 16;
        const int BG_BORDER_WIDTH = 2;
        const int C_BORDER_WIDTH = 2;
        Rect CENTER_RECT = new Rect(0, 0, 128, 128);
        Rect BORDER_RECT = new Rect(128, 0, 64, 64);
        Rect CURSOR_RECT = new Rect(128, 64, 32, 32);
        Rect ARROW_RECT = new Rect(160, 64, 32, 32);

        const int CURSOR_STATES = 30;
        const double MIN_CURSOR_OPACITY = 0.3;
        const int DEFAULT_VPT_Z = 100;
        const int ARROW_SLOW = 10;
#endregion

        Sprite contentsSprite;
        Sprite arrowSprite;
        Sprite[] windowSprites = new Sprite[9];
        Sprite[] cursorSprites = new Sprite[9];

        double arrowState = 0;
        int cursorState = 0;

        Boolean disposeViewport = false;

        public Window(Viewport viewport)
        {
            _viewport = viewport;
        }

        public Window()
        {
            _viewport = new Viewport();
            _viewport.Z = DEFAULT_VPT_Z;
            disposeViewport = true;
        }

        public virtual void Update()
        {
            UpdateWindow();
            UpdateCursor();
            UpdateContents();
            UpdateArrow();
        }

        private void LoadArrow()
        {
            try
            {
                if (arrowSprite != null) { if (!arrowSprite.Disposed) { arrowSprite.Dispose(); } }
                Bitmap bmp = new Bitmap(ARROW_RECT.Width, ARROW_RECT.Height);
                bmp.BlockTransfer(0, 0, WindowSkin, ARROW_RECT, 1);
                arrowSprite = new Sprite(Viewport, bmp);
                arrowState = 0;
            }
            catch
            {
                Globals.GameSystem.MsgBox("Could not use assigned Windowskin");
                throw new Exception("Could not use assigned Windowskin");
            }
        }

        private void LoadContents()
        {
            if (contentsSprite != null) { if (!contentsSprite.Disposed) { return; } }
            contentsSprite = new Sprite(Viewport, new Bitmap(rWidth, rHeight));
        }

        private void LoadCursor()
        {
            LoadFrameSprites(ref cursorSprites, CURSOR_RECT, null, C_BORDER_WIDTH);
        }

        private void LoadWindow()
        {
            LoadFrameSprites(ref windowSprites, BORDER_RECT, CENTER_RECT, WS_BORDER_WIDTH);
        }

        private void LoadFrameSprites(ref Sprite[] sprites, Rect borderRect, Rect centerRect, int w)
        {
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    if (sprites[i] != null)
                    {
                        if (!sprites[i].Disposed)
                        { sprites[i].Dispose(); }
                    }
                }
                Rect rect = new Rect(0, 0, 0, 0);
                Bitmap bmp;
                int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                for (int i = 0; i < 9; i++)
                {
                    switch ((Sides)i)
                    {
                        case Sides.Left:
                            {
                                x1 = 0;
                                x2 = w;
                                y1 = w;
                                y2 = borderRect.Height - w;
                                break;
                            }
                        case Sides.Right:
                            {
                                x1 = borderRect.Width - w;
                                x2 = borderRect.Width;
                                y1 = w;
                                y2 = borderRect.Height - w;
                                break;
                            }
                        case Sides.Up:
                            {
                                x1 = w;
                                x2 = borderRect.Width - w;
                                y1 = 0;
                                y2 = w;
                                break;
                            }
                        case Sides.Down:
                            {
                                x1 = w;
                                x2 = borderRect.Width - w;
                                y1 = borderRect.Height - w;
                                y2 = borderRect.Height;
                                break;
                            }
                        case Sides.TopLeft:
                            {
                                x1 = 0;
                                x2 = w;
                                y1 = 0;
                                y2 = w;
                                break;
                            }
                        case Sides.TopRight:
                            {
                                x1 = borderRect.Width - w;
                                x2 = borderRect.Width;
                                y1 = 0;
                                y2 = w;
                                break;
                            }
                        case Sides.BotLeft:
                            {
                                x1 = 0;
                                x2 = w;
                                y1 = borderRect.Height - w;
                                y2 = borderRect.Height;
                                break;
                            }
                        case Sides.BotRight:
                            {
                                x1 = borderRect.Width - w;
                                x2 = borderRect.Width;
                                y1 = borderRect.Height - w;
                                y2 = borderRect.Height;
                                break;
                            }
                        case Sides.Center:
                            {
                                if (centerRect == null)
                                {
                                    rect = new Rect(
                                        borderRect.X + w, 
                                        borderRect.Y + w, 
                                        borderRect.Width - 2 * w, 
                                        borderRect.Height - 2 * w);
                                }
                                else
                                { rect = centerRect; }
                                break;
                            }
                    }
                    if (i != (int)Sides.Center) { rect = new Rect(x1 + borderRect.X, y1 + borderRect.Y, x2 - x1, y2 - y1); }
                    bmp = new Bitmap(rect.Width, rect.Height);
                    bmp.BlockTransfer(0, 0, WindowSkin, rect, 1);
                    sprites[i] = new Sprite(Viewport, bmp);
                }
            }
            catch
            {
                Globals.GameSystem.MsgBox("Could not use assigned windowskin.");
                throw new Exception("Could not use assigned windowskin.");
            }
        }

        private void UpdateFrameSprites(ref Sprite[] sprites, Rect rect, int border, int indent)
        {
            int cornerWidth = border; int cornerHeight = border;
            int x = rect.X; int y = rect.Y;
            int width = rect.Width; int height = rect.Height;
            int rX = x + border; int rY = y + border;
            int rWidth = rect.Width - 2 * border; int rHeight = rect.Height - 2 * border;
            if (width >= border * 2)
            {
                sprites[(int)Sides.Left].Rect =
                    new Rect(x, rY, border, rHeight);
                sprites[(int)Sides.Right].Rect =
                    new Rect(x + width - border, rY, border, rHeight);
            }
            else
            {
                cornerWidth = width / 2;
                sprites[(int)Sides.Left].Rect =
                    new Rect(x, rY, x + width / 2, y + rHeight);
                sprites[(int)Sides.Right].Rect =
                    new Rect(x + width / 2, rY, width / 2, rHeight);
            }
            if (height >= border * 2)
            {
                sprites[(int)Sides.Up].Rect =
                    new Rect(rX, y, rWidth, border);
                sprites[(int)Sides.Down].Rect =
                    new Rect(rX, y + height - border, rWidth, border);
            }
            else
            {
                cornerHeight = height / 2;
                sprites[(int)Sides.Up].Rect =
                    new Rect(rX, y, rWidth, height / 2);
                sprites[(int)Sides.Down].Rect =
                    new Rect(rX, y + height / 2, rWidth, height / 2);
            }
            sprites[(int)Sides.TopLeft].Rect =
                new Rect(x, y, cornerWidth, cornerHeight);
            sprites[(int)Sides.TopRight].Rect =
                new Rect(x + width - cornerWidth, y, cornerWidth, cornerHeight);
            sprites[(int)Sides.BotLeft].Rect =
                new Rect(x, y + height - cornerHeight, cornerWidth, cornerHeight);
            sprites[(int)Sides.BotRight].Rect =
                new Rect(x + width - cornerWidth, y + height - cornerHeight, cornerWidth, cornerHeight);
            sprites[(int)Sides.Center].Rect = new Rect(
                x + indent, 
                y + indent, 
                width - 2 * indent, 
                height - 2 * indent);
        }

        private void UpdateWindow()
        {
            for (int i = 0; i < 9; i++)
            {
                if (windowSprites[i] == null) { return; }
                if (windowSprites[i].Disposed) { return; }
            }
            for (int i = 0; i < 9; i++)
            {
                windowSprites[i].Z = Z;
                if (i != (int)Sides.Center) { windowSprites[i].Z += 1; }
                windowSprites[i].Visible = Visible;
                windowSprites[i].Opactiy = BackOpacity * Opacity;
                windowSprites[i].Update();
            }
            UpdateFrameSprites(ref windowSprites, Rect, WS_BORDER_WIDTH, BG_BORDER_WIDTH);

        }

        private void UpdateCursor()
        {
            double perc = 0;
            if (HasCursor & Active)
            {
                cursorState = (cursorState + 1) % (2 * CURSOR_STATES);
                perc = cursorState % CURSOR_STATES;
                perc /= CURSOR_STATES;
                
            }
            for (int i = 0; i < 9; i++)
            {
                if (cursorSprites[i]== null) { return; }
                if (cursorSprites[i].Disposed) { return; }
                if (HasCursor & Active)
                {
                    if (cursorState >= CURSOR_STATES)
                    { cursorSprites[i].Opactiy = perc * (1 - MIN_CURSOR_OPACITY) + MIN_CURSOR_OPACITY; }
                    else
                    { cursorSprites[i].Opactiy = (1 - perc) * (1 - MIN_CURSOR_OPACITY) + MIN_CURSOR_OPACITY; }
                    cursorSprites[i].Opactiy *= Opacity;
                }
                else
                { cursorSprites[i].Opactiy = Opacity; }
                if (i == (int)Sides.Center)
                { cursorSprites[i].Z = Z + 4; }
                else
                { cursorSprites[i].Z = Z + 5; }
                cursorSprites[i].Visible = Visible & HasCursor;
                cursorSprites[i].Update();
            }
            if (CursorRect != null)
            {
                Rect rect = new Rect(rX + CursorRect.X, rY + CursorRect.Y, CursorRect.Width, CursorRect.Height);
                if (rect.X < X) { rect.X = X; }
                if (rect.Y < Y) { rect.Y = Y; }
                if (rect.Right > Rect.Right) { rect.Width -= rect.Right - Rect.Right; }
                if (rect.Bottom > Rect.Bottom) { rect.Height -= rect.Bottom - Rect.Bottom; }
                UpdateFrameSprites(ref cursorSprites, rect, C_BORDER_WIDTH, C_BORDER_WIDTH);
            }
        }

        private void UpdateContents()
        {
            if (contentsSprite == null) { return; }
            if (contentsSprite.Disposed) { return; }
            contentsSprite.Location = new Point(rX, rY);
            contentsSprite.BmpSourceRect = new Rect(0, 0, rWidth, rHeight);
            contentsSprite.Z = Z + 3;
            contentsSprite.Visible = Visible;
            contentsSprite.Opactiy = ContentsOpacity * Opacity;
            contentsSprite.Update();
        }

        private void UpdateArrow()
        {
            if (arrowSprite == null) { return; }
            if (arrowSprite.Disposed) { return; }
            if (Paused)
            {
                arrowState = (arrowState + 1) % (4 * ARROW_SLOW);
                Rect rect = new Rect(0, 0, 0, 0);
                rect.X = ((int)(arrowState / ARROW_SLOW) % 2) * arrowSprite.Width;
                rect.Y = (int)(arrowState / ARROW_SLOW / 2) * arrowSprite.Height;
                rect.Width = arrowSprite.Bitmap.Width / 2;
                rect.Height = arrowSprite.Bitmap.Height / 2;
                arrowSprite.BmpSourceRect = rect;
            }
            arrowSprite.X = rX + (rWidth - arrowSprite.Bitmap.Width / 2) / 2;
            arrowSprite.Y = Y + Height - arrowSprite.Height;
            arrowSprite.Z = Z + 2;
            arrowSprite.Visible = Visible & Paused;
            arrowSprite.Opactiy = Opacity;
            arrowSprite.Update();
        }

        public virtual void Dispose()
        {
            if (disposeViewport)
            { Viewport.Dispose(); }
            else
            {
                if (contentsSprite != null) { if (!contentsSprite.Disposed) { contentsSprite.Dispose(); } }
                for (int i = 0; i < 9; i++)
                {
                    if (windowSprites[i] != null)
                    {
                        if (!windowSprites[i].Disposed)
                        { windowSprites[i].Dispose(); }
                        
                    }
                    if (cursorSprites[i] != null)
                    {
                        if (!cursorSprites[i].Disposed)
                        { cursorSprites[i].Dispose(); }
                    }
                }
            }
            _disposed = true;
        }

    }
}
