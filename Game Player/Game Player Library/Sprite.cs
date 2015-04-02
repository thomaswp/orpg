using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// An enum of the different flip-modes used to draw sprites.
    /// </summary>
    public enum FlipModes 
    { 
        /// <summary>
        /// No flip: the Sprite is drawn normally.
        /// </summary>
        None, 

        /// <summary>
        /// The Sprite is flipped Vertically, reflecting itself about its X-axis.
        /// </summary>
        Vertically, 

        /// <summary>
        /// The Sprite is flipped Horizontally, reflecting itself about its Y-axis.
        /// </summary>
        Horizontally 
    }

    /// <summary>
    /// Defines a Sprite for displaying Bitmaps and adding effects.
    /// </summary>
    public class Sprite : IComparable
    {

        #region Properties

        protected Bitmap bitmap;
        /// <summary>
        /// Gets or sets the Bitmap that is rendered when the Sprite is rendered.
        /// </summary>
        public Bitmap Bitmap
        {
            get { return bitmap; }
            set 
            { 
                bitmap = value;

                if (bitmap != null)
                {
                    if (bmpSourceRect.Empty()) 
                        Resize(value.Width, value.Height);
                    else
                        Resize(bmpSourceRect.Width, bmpSourceRect.Height);
                }
            }
        }

        protected Rect bmpSourceRect = new Rect();
        /// <summary>
        /// Gets or sets he Rect of the bitmap that is rendered. If null, this property returns
        /// the Bitmap's Rect property.
        /// </summary>
        public Rect BmpSourceRect
        {
            get 
            {
                if (bmpSourceRect.Empty())
                {
                    if (Bitmap != null)
                        return Bitmap.Rect;
                    else
                        return new Rect(0, 0);
                }
                else
                { return bmpSourceRect; }

            }
            set { bmpSourceRect = value; }
        }

        protected int id;
        /// <summary>
        /// Gets the ID of the Sprite. This property is unique to each sprite in a Viewport.
        /// </summary>
        public int ID
        { get { return id; } }

        protected int x;
        /// <summary>
        /// Gets or sets the X-coordinate where the Sprite is rendered.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        protected int y;
        /// <summary>
        /// Gets or sets the Y-coordinate where the Sprite is rendered 
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        protected int z;
        /// <summary>
        /// Gets or sets how far front or back this Sprite is rendered. Higher values will
        /// bring the Sprite forward, redering it on top of other Sprites. Values can be
        /// positive or negative.
        /// Note: this value only effect Sprites as compared to other Sprites in its 
        /// <see cref="GamePlayer.Viewport">Viewport.</see>
        /// The Viewport's <see cref="P:GamePlayer.Viewport.Z">Z</see> 
        /// property will determine whether the Sprite is rendered in front of
        /// or behind Sprites in other Viewports.
        /// </summary>
        public int Z
        {
            get { return z; }
            set 
            { 
                z = value;
            }
        }

        /// <summary>
        /// Gets or sets the Width of the Sprite. 
        /// Note: there is no actual Width value. Width is just a product of the Bitmap's Width
        /// and the <see cref="P:GamePlayer.Sprite.ZoomX">Sprite.ZoomX property</see>. This
        /// means that altering Width will change the ZoomX property of the sprite, and vice-versa.
        /// </summary>
        public int Width
        {
            get { return (int)(BmpSourceRect.Width * ZoomX); }
            set { ZoomX = value / (double)BmpSourceRect.Width; }
        }

        /// <summary>
        /// Gets or sets the Height of the Sprite. 
        /// Note: there is no actual Height value. Height is just a product of the Bitmap's Height
        /// and the <see cref="P:GamePlayer.Sprite.ZoomY">Sprite.ZoomY property</see>. This
        /// means that altering Height will change the ZoomY property of the sprite, and vice-versa.
        /// </summary>
        public int Height
        {
            get { return (int)(BmpSourceRect.Height * ZoomY); }
            set { ZoomY = value / (double)BmpSourceRect.Height; }
        }

        protected int oX;
        /// <summary>
        /// Gets or sets the X-coordinate of the rendered Bitmap that corresponds with the (0, 0)
        /// coordinate of the sprite, serving as an offset for the 
        /// <see cref="P:GamePlayer.BmpSourceRect">BmpSourceRect</see> property.
        /// Note: when the sprite is zoomed, the origin is still at point (0, 0) of the sprite,
        /// therefor this property can be used to zoom in on another part of the Bitmap.
        /// </summary>
        public int OX
        {
            get { return oX; }
            set { oX = value; }
        }

        protected int oY;
        /// <summary>
        /// Gets or sets the Y-coordinate of the rendered Bitmap that corresponds with the (0, 0)
        /// coordinate of the sprite, serving as an offset for the 
        /// <see cref="P:GamePlayer.BmpSourceRect">BmpSourceRect</see> property.
        /// Note: when the sprite is zoomed, the origin is still at point (0, 0) of the sprite,
        /// therefor this property can be used to zoom in on another part of the Bitmap.
        /// </summary>
        public int OY
        {
            get { return oY; }
            set { oY = value; }
        }

        protected int opactiy = 255;
        /// <summary>
        /// Gets or sets the opactiy of this Sprite. Ranges from 0.0 to 1.0. For example, an 
        /// opacity of 1.0 means the sprite fully covers the pixels underneath it, an opacity of 0.5 would 
        /// blend the two layers of pixels, while with an opactiy of 0.0 the Sprite would be fully transparent.
        /// </summary>
        public int Opactiy
        {
            get { return opactiy; }
            set { opactiy = value.MinMax(0, 255); }
        }

        protected double rotation;
        /// <summary>
        /// Gets or sets the amount the Sprite is rotated. Values range from 0.0 to 1.0
        /// and go clockwise.
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value % 1; }
        }

        /// <summary>
        /// Gets or sets a <see cref="GamePlayer.Point">Point</see> that represents the X-
        /// and Y- coordinates of the Sprite.
        /// </summary>
        public Point Location
        {
            get {return new Point(X, Y);}
            set { X = value.X; Y = value.Y; }
        }

        protected Color color = Colors.Clear;
        /// <summary>
        /// Gets or sets the color to blend with the Sprite. Higher 
        /// <see cref="P:GamePlayer.Color.Alpha">Alpha</see> values 
        /// create more distinct color blend.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        protected Boolean visible = true;
        /// <summary>
        /// Gets or sets the visibility of the Sprite.
        /// </summary>
        public Boolean Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        protected FlipModes flipMode = FlipModes.None;
        /// <summary>
        /// Gets or sets the <see cref="F:GamePlayer.FlipModes">FlipModes</see> that determines
        /// how the Sprite is flipped.
        /// </summary>
        public FlipModes FlipMode
        {
            get { return flipMode; }
            set { flipMode = value; }
        }

        protected Viewport viewport;
        /// <summary>
        /// Gets the Viewport that displays this Sprite.
        /// </summary>
        public Viewport Viewport
        { get { return viewport; } }

        protected Boolean disposed;
        /// <summary>
        /// Gets whether this Sprite is disposed and it resources released.
        /// </summary>
        public Boolean Disposed
        { get { return disposed; } }
        
        /// <summary>
        /// Gets or sets the Rect that represents the coordinates and dimensions of this Sprite.
        /// </summary>
        public Rect Rect
        {
            get { return new Rect(X, Y, Width, Height); }
            set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
        }

        protected double zoomX = 1;
        /// <summary>
        /// The amount this Sprite is stretched to the right. Values range from 0 up.
        /// The value represents how many times the Bitmap's Width, the Sprite's Width will be.
        /// Values less than one will make the Sprite smaller, and values greater than 1 will 
        /// make it larger.
        /// </summary>
        public double ZoomX
        {
            get { return zoomX; }
            set { zoomX = value; }
        }

        protected double zoomY = 1;
        /// <summary>
        /// The amount this Sprite is stretched downward. Values range from 0 up.
        /// The value represents how many times the Bitmap's Height, the Sprite's Height will be.
        /// Values less than one will make the Sprite smaller, and values greater than 1 will 
        /// make it larger.
        /// </summary>
        public double ZoomY
        {
            get { return zoomY; }
            set { zoomY = value; }
        }

        protected Tone tone = new Tone();
        public Tone Tone
        {
            get { return tone; }
            set { tone = value; }
        }

        protected int bushDepth;
        public int BushDepth 
        { 
            get { return bushDepth; } 
            set { bushDepth = Math.Max(0, value); }
        }

        protected int blendType;
        public int BlendType 
        { 
            get { return blendType; } 
            set { blendType = Math.Min(Math.Max(0, value), 2); }
        }

        //protected int blur = 0;
        //public int Blur
        //{
        //    get { return blur; }
        //    set { blur = value.MinMax(0, 30); }
        //}

        //protected Point bubble = new Point(0, 0);
        //public Point Bubble
        //{
        //    get { return bubble; }
        //    set { bubble = value; }
        //}

        //protected int bubbleRadius = 0;
        //public int BubbleRadius
        //{
        //    get { return bubbleRadius; }
        //    set { bubbleRadius = value; }
        //}
#endregion

        /// <summary>
        /// Defines the sprite. Dimensions can be defined by a 
        /// <see cref="GamePlayer.Bitmap">Bitmap</see>, coordinates/dimensions or a
        /// <see cref="GamePlayer.Rect">Rect</see>. The Sprite should be initialized with a
        /// <see cref="GamePlayer.Viewport">Viewport</see>, but if it is not, one is
        /// created and disposed when the Sprite is disposed.
        /// </summary>
        public Sprite()
        {
            viewport = Viewport.Default;
            id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(1, 1);
            X = 0; Y = 0;
        }

        /// <summary>
        /// Defines the sprite. Dimensions can be defined by a 
        /// <see cref="GamePlayer.Bitmap">Bitmap</see>, coordinates/dimensions or a
        /// <see cref="GamePlayer.Rect">Rect</see>. The Sprite should be initialized with a
        /// <see cref="GamePlayer.Viewport">Viewport</see>, but if it is not, one is
        /// created and disposed when the Sprite is disposed.
        /// </summary>
        /// <param name="viewport">The Viewport that renders the Sprite.</param>
        public Sprite(Viewport viewport)
        {
            this.viewport = viewport;
            id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(1, 1);
            X = 0; Y = 0;
        }

        /// <summary>
        /// Defines the sprite. Dimensions can be defined by a 
        /// <see cref="GamePlayer.Bitmap">Bitmap</see>, coordinates/dimensions or a
        /// <see cref="GamePlayer.Rect">Rect</see>. The Sprite should be initialized with a
        /// <see cref="GamePlayer.Viewport">Viewport</see>, but if it is not, one is
        /// created and disposed when the Sprite is disposed.
        /// </summary>
        /// <param name="viewport">The Viewport that renders the Sprite.</param>
        /// <param name="bitmap">The Bitmap that defines the Sprite's dimensions and becomes
        /// its <see cref="GamePlayer.Bitmap">Bitmap</see>.</param>
        public Sprite(Viewport viewport, Bitmap bitmap)
        {
            this.viewport = viewport;
            id = Viewport.AddSprite(this);
            Bitmap = bitmap;
            X = 0; Y = 0; 
        }

        /// <summary>
        /// Defines the sprite. Dimensions can be defined by a 
        /// <see cref="GamePlayer.Bitmap">Bitmap</see>, coordinates/dimensions or a
        /// <see cref="GamePlayer.Rect">Rect</see>. The Sprite should be initialized with a
        /// <see cref="GamePlayer.Viewport">Viewport</see>, but if it is not, one is
        /// created and disposed when the Sprite is disposed.
        /// </summary>
        /// <param name="viewport">The Viewport that renders the Sprite.</param>
        /// <param name="x">The Sprite's X-coordinate.</param>
        /// <param name="y">The Sprite's Y-coordinate.</param>
        /// <param name="width">The Sprite's Width.</param>
        /// <param name="height">The Sprite's Height.</param>
        public Sprite(Viewport viewport, int x, int y, int width, int height)
        {
            this.viewport = viewport;
            id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(width, height);
            X = x; Y = y;
        }

        /// <summary>
        /// Defines the sprite. Dimensions can be defined by a 
        /// <see cref="GamePlayer.Bitmap">Bitmap</see>, coordinates/dimensions or a
        /// <see cref="GamePlayer.Rect">Rect</see>. The Sprite should be initialized with a
        /// <see cref="GamePlayer.Viewport">Viewport</see>, but if it is not, one is
        /// created and disposed when the Sprite is disposed.
        /// </summary>
        /// <param name="viewport">The Viewport that renders the Sprite.</param>
        /// <param name="rect">The Rect that defines the Sprite's coordinates and dimensions.</param>
        public Sprite(Viewport viewport, Rect rect)
        {
            this.viewport = viewport;
            id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(rect.Width, rect.Height);
            X = rect.X; Y = rect.Y;
        }

        /// <summary>
        /// Disposes the Sprite and releases its resources.
        /// </summary>
        public virtual void Dispose()
        { 
            if (disposed == false) 
            {
                if (Bitmap != null)
                    Bitmap.Dispose(); 
                disposed = true; 
            }
        }

        /// <summary>
        /// Reassigns this Sprite to another Viewport.
        /// </summary>
        /// <param name="viewport">The Viewport to which the Sprite will be assigned.</param>
        public void Reassign(Viewport viewport)
        {
            Viewport.Sprites[ID] = null;
            this.viewport = viewport;
            id = Viewport.AddSprite(this);
        }

        /// <summary>
        /// Resizes the Sprite to the given Width and Height.
        /// </summary>
        /// <param name="width">The new Width of the Sprite.</param>
        /// <param name="height">The new Height of the Sprite.</param>
        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Updates the Sprite and handles any effects it's implementing.
        /// </summary>
        public virtual void Update()
        { }

        public int CompareTo(object obj)
        {
            if (!(obj is Sprite))
                return -1;

            Sprite other = (Sprite)obj;
            if (other.Z == Z)
                return Viewport.CompareTo(other.Viewport);

            return Z - other.Z;
        }
    }
}
