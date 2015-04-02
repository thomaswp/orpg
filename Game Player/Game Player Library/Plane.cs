using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public class Plane : Sprite
    {
        public Plane(Viewport viewport) : base(viewport) 
        {
            this.Rect = Graphics.ScreenRect;
        }

        public Plane() : this(Viewport.Default) { }
    }
}
