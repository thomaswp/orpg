using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Arrow
{
    public abstract class Base : Sprite
    {
        #region properties

        protected int index;
        public int Index 
        { 
            get { return index; }
            set
            {
                index = value;
                Update();
            }
        }

        protected Windows.Help helpWindow;
        public Windows.Help HelpWindow 
        { 
            get { return helpWindow; }
            set
            {
                helpWindow = value;
                if (helpWindow != null)
                    UpdateHelp();
            }
        }

        #endregion

        private int blinkCount;

        public abstract void UpdateHelp();

        public Base(Viewport viewport)
            : base(viewport)
        {
            this.bitmap = Cache.LoadWindowskin(Globals.GameSystem.WindowSkinName);
            this.OX = 16;
            this.OY = 64;
            this.Z = 2500;
            blinkCount = 0;
            index = 0;
            helpWindow = null;

            Update();
        }

        public override void Update()
        {
            blinkCount = (blinkCount + 1) % 8;

            if (blinkCount < 4)
                this.bmpSourceRect = new Rect(128, 96, 32, 32);
            else
                this.bmpSourceRect = new Rect(160, 96, 32, 32);

            if (helpWindow != null)
                UpdateHelp();
                
        }
    }
}
