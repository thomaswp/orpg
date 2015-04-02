using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class Steps : Base
    {
        public Steps()
            : base(0, 0, 160, 96)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            Refresh();
        }

        public void Refresh()
        {
            this.Contents.Clear();
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(4, 0, 120, 32, "Steps");
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(4, 32, 120, 32, Globals.GameParty.Steps.ToString(), FontAligns.Right);
        }
    }
}
