using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// A class that contains and groups Sprites for display.
    /// </summary>
    public class Viewport : IComparable
    {
        #region Properties

        int _id;
        /// <summary>
        /// Gets the ID of the Viewport. This value is unique to each viewport.
        /// </summary>
        public int ID
        { get { return _id; } }

        int _ids = -1;
        /// <summary>
        /// Get the index in the Sprites Array of the last Sprite assigned to this Viewport.
        /// </summary>
        public int IDs
        { get { return _ids; } }

        Color _color = Colors.Clear;
        /// <summary>
        /// Gets or sets the color to blend with the each Sprite in this Viewport. Higher 
        /// <see cref="P:Game_Player.Color.Alpha">Alpha</see> values create a more distinct 
        /// color blend.
        /// Note: This value is blended with the <see cref="P:Game_Player.Sprite.Color">Color</see>
        /// Property of each Sprite, as well.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private Tone _tone;
        public Tone Tone
        {
            get { return _tone; }
            set { _tone = value; }
        }

        private Rect _rect;
        public Rect Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        public int X { get { return Rect.X; } }
        public int Y { get { return Rect.Y; } }
        public int Width { get { return Rect.Width; } }
        public int Height { get { return Rect.Height; } }

        private int _ox;
        public int OX
        {
            get { return _ox; }
            set { _ox = value; }
        }

        private int _oy;
        public int OY
        {
            get { return _oy; }
            set { _oy = value; }
        }

        int _z = 0;
        /// <summary>
        /// Gets or sets how far front or back the Sprites in this Viewport are rendered. 
        /// Higher values will bring the Sprites forward, redering them on top of Sprites
        /// in other Viewports. Values can be positive or negative.
        /// Note: this value only effect Sprites as compared to Sprites in other Viewports.
        /// The Sprite's <see cref="P:Game_Player.Sprite.Z">Z</see> 
        /// property will determine whether the Sprite is rendered in front of
        /// or behind Sprites in its own Viewport.
        /// </summary>
        public int Z
        {
            get { return _z; }
            set { _z = value; }
        }

        Boolean _visible = true;
        /// <summary>
        /// Gets or sets the visibility of the this Viewport.
        /// Note: changing this property does not change the 
        /// <see cref="P:Game_Player.Sprite.Visible">Visible</see> Property of the Sprites
        /// contianed in this Viewport, it simply overrides it.
        /// </summary>
        public Boolean Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        List<Sprite> _sprites  = new List<Sprite>();
        /// <summary>
        /// An array containing the Sprites contained in this Viewport.
        /// </summary>
        public List<Sprite> Sprites
        { get { return _sprites; } }

        Boolean _disposed = false;
        /// <summary>
        /// Indicates whether this Viewport and therefore all of its Sprites are
        /// <see cref="M:Game_Player.Viewport.Dispose">Disposed</see>
        /// and their resources released.
        /// </summary>
        public Boolean Disposed
        { get { return _disposed; } }

        static Viewport _default = new Viewport(Graphics.ScreenRect);
        /// <summary>
        /// The default Viewport used when declaring 
        /// <see cref="Game_Player.Sprite">Sprites</see> and 
        /// <see cref="Game_Player.Window">Windows</see> without an argument.
        /// </summary>
        public static Viewport Default
        { get { _default.Z = 1; return _default; } }

        #endregion

        public Viewport(int x, int y, int width, int height) : this(new Rect(x, y, width, height)) { }

        /// <summary>
        /// Defines a Viewport.
        /// </summary>
        public Viewport(Rect rect)
        {
            _id = Graphics.AddViewport(this);
            _rect = rect;
        }

        /// <summary>
        /// Adds a Sprite to this Viewport's contained Sprites, returning the ID it should assign
        /// itself. Sprites must be assigned to a Viewport to be rendered.
        /// Note: This method is automatically called when a Sprite is created with this
        /// Viewport as its <see cref="M:Game_Player.Sprite.#ctor">viewport argument</see>.
        /// Calling it again will assign it to two Viewports, possibly causing unintended results.
        /// Use the <see cref="M:Game_Player.Sprite.Reassign">Sprite's Reassign Method</see> instead.
        /// </summary>
        /// <param name="sprite">The Sprite to be added.</param>
        /// <returns>The ID to be assigned to the added sprite.</returns>
        public int AddSprite(Sprite sprite)
        {
            _sprites.Add(sprite);
            _ids++;
            return IDs;
        }

        /// <summary>
        /// Disposes this Viewport and calls each Sprite's 
        /// <see cref="M:Game_Player.Viewport.Dispose">Dispose</see> method, releasing their
        /// resources.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].Disposed == false)
                { Sprites[i].Dispose(); }
            }
            _disposed = true;
        }

        /// <summary>
        /// Updates the Viewport and handles any effects it's implementing.
        /// </summary>
        public void Update()
        {
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Viewport))
                return -1;

            int comp = Z - ((Viewport)obj).Z;
            if (comp != 0) return comp;
            return base.GetHashCode() - obj.GetHashCode();
        }
    }
}
