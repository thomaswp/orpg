using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class Gold : Base
    {
        public Gold()
            : base(0, 0, 160, 64)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            Refresh();
        }

        public void Refresh()
        {
            this.Contents.Clear();
            int cx = Contents.TextSize(Data.Misc.words.gold).Width;
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(4, 0, 120 - cx - 2, 32, Globals.GameParty.Gold.ToString(), FontAligns.Right);
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(124 - cx, 0, cx, 32, Data.Misc.words.gold, FontAligns.Right);
        }
    }
}
