using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public abstract class Scene
    {
        public virtual void Update()
        { }

        /// <summary>
        /// Called automatically every time Globals.Scene is changed.
        /// </summary>
        public virtual void End()
        { }
    }
}
