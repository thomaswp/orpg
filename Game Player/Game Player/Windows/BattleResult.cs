using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class BattleResult : Base
    {
        private int exp;
        private int gold;
        private DataClasses.Item[] treasures;

        public BattleResult(int exp, int gold, DataClasses.Item[] treasures)
            : base(160, 0, 320, treasures.Length * 32 + 64)
        {
            this.exp = exp;
            this.gold = gold;
            this.treasures = treasures;

            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;

            this.Y = 160 - height / 2;
            this.BackOpacity = 160;
            this.Visible = false;

            Refresh();
        }

        public void Refresh()
        {
            this.Contents.Clear();

            int x = 4;
            this.Contents.FontColor = NormalColor;
            int cx = Contents.TextSize(exp.ToString()).Width;
            this.Contents.DrawText(x, 0, cx, 32, exp.ToString());

            x += cx + 4;
            this.Contents.FontColor = SystemColor;
            cx = Contents.TextSize("EXP").Width;
            this.Contents.DrawText(x, 0, 64, 32, "EXP");
            
            x += cx + 16;
            this.Contents.FontColor = NormalColor;
            cx = this.Contents.TextSize(gold.ToString()).Width;
            this.Contents.DrawText(x, 0, cx, 32, gold.ToString());

            x += cx + 4;
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, 0, 128, 32, Data.Misc.words.gold);

            y = 32;
            foreach (DataClasses.Item item in treasures)
            {
                DrawItemName(item, 4, y);
                y += 32;
            }
        }
    }
}
