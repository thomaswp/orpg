using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public class Viewport
    {
        #region Properties

        int _id;
        public int ID
        { get { return _id; } }

        int _ids = -1;
        public int IDs
        { get { return _ids; } }

        Color _color = Colors.Clear;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        int _z = 0;
        public int Z
        {
            get { return _z; }
            set { _z = value; }
        }

        Boolean _visible = true;
        public Boolean Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        Sprite[] _sprites  = new Sprite[] { };
        public Sprite[] Sprites
        { get { return _sprites; } }

        Boolean _disposed = false;
        public Boolean Disposed
        { get { return _disposed; } }

        #endregion

        public Viewport()
        {
            _id = Globals.Graphics.AddViewport(this);
        }

        public int AddSprite(Sprite sprite)
        {
            Array.Resize<Sprite>(ref _sprites, _sprites.Length + 1);
            _sprites[_sprites.Length - 1] = sprite;
            _ids++;
            return IDs;
        }

        public void Dispose()
        {
            for (int i = 0; i < Sprites.Length; i++)
            {
                if (Sprites[i].Disposed == false)
                { Sprites[i].Dispose(); }
            }
            _disposed = true;
        }

        public void Update()
        {
        }
    }
}
