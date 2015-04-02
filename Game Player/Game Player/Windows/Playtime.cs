using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class Playtime : Base
    {
        int totalSec;

        public Playtime()
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
            this.Contents.DrawText(4, 0, 120, 32, "Play Time");
            totalSec = (int)Graphics.Playtime.TotalSeconds;
            int hour = Graphics.Playtime.Hours;
            int min = Graphics.Playtime.Minutes;
            int sec = Graphics.Playtime.Seconds;
            string text = hour.ToString("00") + ":" + min.ToString("00") + ":" + sec.ToString("00");
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(4, 32, 120, 32, text, FontAligns.Right);
        }

        public override void Update()
        {
            base.Update();
            if (totalSec != (int)Graphics.Playtime.TotalSeconds)
                Refresh();
        }
    }
}
