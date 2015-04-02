using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    enum Sides
    {
        Right = 0,
        Left = 1,
        Up = 2,
        Down = 3,
        Center = 4,
        TopLeft = 5,
        TopRight = 6,
        BotLeft = 7,
        BotRight = 8
    }

    /// <summary>
    /// A class used for the creation and usage of the many Sprites that make up a Window.
    /// Windows are used to display messages within the game.
    /// </summary>
    public class Window
    {
        #region Properties
        protected Viewport viewport;
        /// <summary>
        /// Gets or sets the Viewport that displays this Window.
        /// </summary>
        public Viewport Viewport
        {
            get { return viewport; }
        }

        protected Boolean disposed = false;
        /// <summary>
        /// Gets whether the Sprites of this Window are 
        /// <see cref="P:GamePlayer.Sprite.Disposed">disposed</see> and their resources released.
        /// </summary>
        public Boolean Disposed
        {
            get { return disposed; }
        }

        protected Bitmap windowSkin;
        /// <summary>
        /// Gets or sets the <see cref="GamePlayer.Bitmap">Bitmap</see> that contains the
        /// needed parts of the Window. Changin this value will reload the Window.
        /// </summary>
        public Bitmap WindowSkin
        {
            get { return windowSkin; }
            set
            {
                windowSkin = value;
                LoadWindow();
                LoadCursor();
                LoadArrow();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GamePlayer.Bitmap">Bitmap</see> that is displayed
        /// on the inside of this window. Manipulate this Bitmap to make the Window display text.
        /// Note: There is 16 pixel border inside of the Window before the start of the Sprite
        /// containing the Contents.
        /// </summary>
        public Bitmap Contents
        {
            get { LoadContents(); return contentsSprite.Bitmap; }
            set 
            { 
                LoadContents(); contentsSprite.Bitmap = value;
                UpdateProperties();
            }
        }

        Rect cursorRect = new Rect(-1, -1, -1, -1);
        /// <summary>
        /// This rectangle defines where, in reference to the Window, the cursor will show.
        /// The cursor is a flashing rectagle used to highlight selected information.
        /// The cursor is not visible until this Rect is set. 
        /// Note: The cursor is also not visible unless the
        /// <see cref="P:GamePlayer.Window.Active">Active Property</see> is set to true.
        /// </summary>
        public Rect CursorRect
        {
            get { return cursorRect; }
            set 
            {
                if (cursorRect == new Rect(-1, -1, -1, -1))
                    Active = true; 
                cursorRect = value;
                UpdateProperties();
            }
        }


        protected Boolean active = false;
        /// <summary>
        /// Defines whether this Window is "active." Active means that the the cursor will show
        /// if a Rect is assigned to the 
        /// <see cref="P:GamePlayer.Window.CursorRect">CursorRect Property</see>.
        /// </summary>
        public Boolean Active
        {
            get { return active; }
            set 
            { 
                active = value;
                UpdateProperties();
            }
        }

        protected Boolean visible = true;
        /// <summary>
        /// Gets ot sets the visibility of this Window and all Sprites it contains.
        /// </summary>
        public Boolean Visible
        {
            get { return visible; }
            set 
            { 
                visible = value;
                UpdateProperties();
            }
        }

        protected Boolean paused = false;
        /// <summary>
        /// Gets or sets whether the Window's Arrow is visible. This property gets its name
        /// from the its main use: showing when waiting for player input. Thus, the Window
        /// is <i>paused</i> until it recieves input.
        /// </summary>
        public Boolean Paused
        {
            get { return paused; }
            set 
            {
                paused = value;
                UpdateProperties();
            }
        }

        ///// <summary>
        ///// Gets whether or not the cursor is showing.
        ///// See the <see cref="P:GamePlayer.Window.CursorRect">CursorRect Property</see> and the
        ///// <see cref="P:GamePlayer.Window.Active">Active Property</see> for more information.
        ///// </summary>
        //public Boolean HasCursor
        //{
        //    get { return Active; }
        //}

        protected int x;
        /// <summary>
        /// Gets or sets the X-coordinate of the Window.
        /// </summary>
        public int X
        {
            get { return x; }
            set 
            { 
                x = value;
                UpdateProperties();
            }
        }

        protected int y;
        /// <summary>
        /// Gets or sets the Y-coordinate of the Window.
        /// </summary>
        public int Y
        {
            get { return y; }
            set 
            { 
                y = value;
                UpdateProperties();
            }
        }

        protected int width;
        /// <summary>
        /// Gets or sets the Width of the Window.
        /// </summary>
        public int Width
        {
            get { return width; }
            set 
            { 
                width = value;
                UpdateProperties();
            }
        }

        protected int height;
        /// <summary>
        /// Gets or sets the Height of the Window.
        /// </summary>
        public int Height
        {
            get { return height; }
            set 
            { 
                height = value;
                UpdateProperties();
            }
        }

        /// <summary>
        /// Gets or sets a Rect containing the coordinates and dimensions of the Window.
        /// </summary>
        public Rect Rect
        {
            get { return new Rect(X, Y, Width, Height); }
            set 
            { 
                X = value.X; 
                Y = value.Y; 
                Width = value.Width; 
                Height = value.Height;
                UpdateProperties();
            }
        }

        protected int z;
        /// <summary>
        /// Gets or sets how far front or back this Window is rendered. Higher values will
        /// bring the Window forward, redering it on top of other Sprites or Windows. Values 
        /// can be positive or negative.
        /// Note: this value only effect Windows as compared to other Sprites in its 
        /// <see cref="GamePlayer.Viewport">Viewport.</see>
        /// The Viewport's <see cref="P:GamePlayer.Viewport.Z">Z</see> 
        /// property will determine whether the Window is rendered in front of
        /// or behind Sprites in other Viewports.
        /// </summary>
        public int Z
        {
            get { return z; }
            set 
            { 
                z = value;
                UpdateProperties();
            }
        }

        protected int oX = 0;
        public int OX
        {
            get { return oX; }
            set 
            { 
                oX = value;
                UpdateProperties();
            }
        }

        protected int oY = 0;
        public int OY
        {
            get { return oY; }
            set 
            { 
                oY = value;
                UpdateProperties();
            }
        }

        protected int opacity = 255;
        /// <summary>
        /// Gets or sets the opactiy of this Window. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the Window fully covers the pixels underneath it, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the Window would be fully transparent.
        /// </summary>
        public int Opacity
        {
            get { return opacity; }
            set 
            { 
                opacity = value.MinMax(0, 255);
                UpdateProperties();
            }
        }

        protected int backOpacity = 255;
        /// <summary>
        /// Gets or sets the opactiy of this Window's border Sprites. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the sprites fully covers the pixels underneath them, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the sprites would be fully transparent.
        /// Note: This value is multiplied by the <see cref="P:GamePlayer.Window.Opacity">Window's Opacity Property</see>
        /// to get the final Opacity of the background Sprites.
        /// </summary>
        public int BackOpacity
        {
            get { return backOpacity; }
            set 
            { 
                backOpacity = value;
                UpdateProperties();
            }
        }

        protected int contentsOpacity = 255;
        /// <summary>
        /// Gets or sets the opactiy of this Window's Content Sprite. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the sprite fully covers the pixels underneath it, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the sprite would be fully transparent.
        /// Note: This value is multiplied by the <see cref="P:GamePlayer.Window.Opacity">Window's Opacity Property</see>
        /// to get the final Opacity of the Contents Sprite.
        /// </summary>
        public int ContentsOpacity
        {
            get { return contentsOpacity; }
            set 
            { 
                contentsOpacity = value.MinMax(0, 255);
                UpdateProperties();
            }
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
        const int MIN_CURSOR_OPACITY = 85;
        const int ARROW_SLOW = 10;
        #endregion

        Sprite contentsSprite;
        Sprite arrowSprite;
        Sprite[] windowSprites = new Sprite[9];
        Sprite[] cursorSprites = new Sprite[9];

        double arrowState = 0;
        int cursorState = 0;

        /// <summary>
        /// Declares a new Window, preferably with a Viewport. If no Viewport is provided,
        /// one will be created for this Window, and later it will be disposed.
        /// </summary>
        public Window() : this(Viewport.Default) { }

        /// <summary>
        /// Declares a new Window, preferably with a Viewport. If no Viewport is provided,
        /// one will be created for this Window, and later it will be disposed.
        /// </summary>
        /// <param name="viewport">The Viewport to assign.</param>
        public Window(Viewport viewport)
        {
            this.viewport = viewport;
        }

        /// <summary>
        /// Updates the Viewport and oll of its components.
        /// </summary>
        public virtual void Update()
        {
            UpdateWindow();
            UpdateCursor();
            UpdateArrow();
            UpdateContents();
        }

        private void UpdateProperties()
        {
            UpdateWindow(false);
            UpdateCursor(false);
            UpdateArrow(false);
            UpdateContents(false);
        }

        /// <summary>
        /// Reassigns this Window and all its Sprites to another Viewport.
        /// </summary>
        /// <param name="viewport">The Viewport to be assigned.</param>
        public void Reassign(Viewport viewport)
        {
            Reassign_S(contentsSprite, viewport);
            Reassign_S(arrowSprite, viewport);
            for (int i = 0; i < windowSprites.Length; i++)
            { Reassign_S(windowSprites[i], viewport); }
            for (int i = 0; i < cursorSprites.Length; i++)
            { Reassign_S(cursorSprites[i], viewport); }
            this.viewport = viewport;
        }

        void Reassign_S(Sprite s, Viewport v)
        {
            if (s != null)
            {
                if (!s.Disposed)
                { s.Reassign(v); }
            }
        }

        private void LoadArrow()
        {
            try
            {
                if (arrowSprite != null) { if (!arrowSprite.Disposed) { arrowSprite.Dispose(); } }
                Bitmap bmp = new Bitmap(ARROW_RECT.Width, ARROW_RECT.Height);
                bmp.BlockTransfer(0, 0, WindowSkin, ARROW_RECT);
                arrowSprite = new Sprite(Viewport, bmp);
                arrowState = 0;
            }
            catch
            {
                MsgBox.Show("Could not use assigned Windowskin");
                throw new Exception("Could not use assigned Windowskin");
            }
        }

        private void LoadContents()
        {
            if (contentsSprite != null) 
                if (!contentsSprite.Disposed) 
                    return;

            contentsSprite = new Sprite(Viewport, new Bitmap(rWidth, rHeight));
        }

        private void LoadCursor()
        {
            LoadFrameSprites(ref cursorSprites, CURSOR_RECT, new Rect(), C_BORDER_WIDTH);
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
                                if (centerRect.Empty())
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
                    bmp.BlockTransfer(0, 0, WindowSkin, rect);
                    sprites[i] = new Sprite(Viewport, bmp);
                }
            }
            catch
            {
                MsgBox.Show("Could not use assigned windowskin.");
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

        private void UpdateWindow() { UpdateWindow(true); }
        private void UpdateWindow(bool updateSprite)
        {
            for (int i = 0; i < 9; i++)
            {
                if (windowSprites[i] == null) { return; }
                if (windowSprites[i].Disposed) { return; }
            }
            for (int i = 0; i < 9; i++)
            {
                windowSprites[i].Z = Z;
                if (i != (int)Sides.Center) 
                    windowSprites[i].Z += 1;

                windowSprites[i].Visible = Visible;
                windowSprites[i].Opactiy = BackOpacity * Opacity / 255;

                if (updateSprite)
                    windowSprites[i].Update();
            }
            UpdateFrameSprites(ref windowSprites, Rect, WS_BORDER_WIDTH, BG_BORDER_WIDTH);

        }

        private void UpdateCursor() { UpdateCursor(true); }
        private void UpdateCursor(bool updateSprite)
        {
            double perc = 0;
            if (Active)
            {
                if (updateSprite)
                    cursorState = (cursorState + 1) % (2 * CURSOR_STATES);

                perc = cursorState % CURSOR_STATES;
                perc /= CURSOR_STATES;

            }
            for (int i = 0; i < 9; i++)
            {
                if (cursorSprites[i] == null) 
                    return;
                if (cursorSprites[i].Disposed) 
                    return;

                if (Active)
                {
                    if (cursorState >= CURSOR_STATES)
                        cursorSprites[i].Opactiy = (int)(perc * (255 - MIN_CURSOR_OPACITY) + MIN_CURSOR_OPACITY);
                    else
                        cursorSprites[i].Opactiy = (int)((1 - perc) * (255 - MIN_CURSOR_OPACITY) + MIN_CURSOR_OPACITY);
                    
                    cursorSprites[i].Opactiy *= Opacity / 255;
                }
                else
                    cursorSprites[i].Opactiy = Opacity;

                if (i == (int)Sides.Center)
                    cursorSprites[i].Z = Z + 4;
                else
                    cursorSprites[i].Z = Z + 5;

                cursorSprites[i].Visible = Visible && Active;
                
                if (updateSprite)
                    cursorSprites[i].Update();
            }
            if (!CursorRect.Empty())
            {
                Rect rect = new Rect(rX + CursorRect.X, rY + CursorRect.Y, CursorRect.Width, CursorRect.Height);
                
                if (rect.X < X) 
                    rect.X = X;
                if (rect.Y < Y) 
                    rect.Y = Y;
                if (rect.Right > Rect.Right) 
                    rect.Width -= rect.Right - Rect.Right;
                if (rect.Bottom > Rect.Bottom) 
                    rect.Height -= rect.Bottom - Rect.Bottom;

                UpdateFrameSprites(ref cursorSprites, rect, C_BORDER_WIDTH, C_BORDER_WIDTH);
            }
        }

        private void UpdateContents() { UpdateContents(true); }
        private void UpdateContents(bool updateSprite)
        {
            if (contentsSprite == null)
                return;
            if (contentsSprite.Disposed) 
                return;

            contentsSprite.Location = new Point(rX, rY);
            contentsSprite.BmpSourceRect = new Rect(oX, oY, rWidth, rHeight);
            contentsSprite.Z = Z + 3;
            contentsSprite.Visible = Visible;
            contentsSprite.Opactiy = ContentsOpacity * Opacity / 255;
            
            if (updateSprite)
                contentsSprite.Update();
        }

        private void UpdateArrow() { UpdateArrow(true); }
        private void UpdateArrow(bool updateSprite)
        {
            if (arrowSprite == null) 
                return;
            if (arrowSprite.Disposed) 
                return;

            if (Paused && updateSprite)
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
            arrowSprite.Visible = Visible && Paused;
            arrowSprite.Opactiy = Opacity;

            if (updateSprite)
                arrowSprite.Update();
        }

        /// <summary>
        /// Disposes this Window, all of its sprites and their resources.
        /// </summary>
        public virtual void Dispose()
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
            disposed = true;
        }

    }
}
