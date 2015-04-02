using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class DebugLeft : Selectable
    {
        int swtichMax;
        int variableMax;

        public DebugLeft() : base(0, 0, 192, 480)
        {
            this.Index = 0;
            Refresh();
        }

        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }

            swtichMax = (Data.Misc.switches.Length - 1 + 9) / 10;
            variableMax = (Data.Misc.variables.Length - 1 + 9) / 10;
            itemMax = swtichMax + variableMax;

            this.Contents = new Bitmap(width - 32, itemMax * 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;

            for (int i = 0; i < swtichMax; i++)
            {
                string text = "S " + (i * 10 + 1).ToString("0000") + "-" + (i * 10 + 10).ToString("0000");
                this.Contents.DrawText(4, i * 32, 152, 32, text);
            }

            for (int i = 0; i < variableMax; i++)
            {
                string text = "S " + (i * 10 + 1).ToString("0000") + "-" + (i * 10 + 10).ToString("0000");
                this.Contents.DrawText(4, (swtichMax + i) * 32, 152, 32, text);
            }
        }

        public int Mode
        {
            get { return this.Index < swtichMax ? 0 : 1; }
        }

        public int TopId
        {
            get { return this.Index < swtichMax ? this.Index * 10 + 1 : (this.Index - swtichMax) * 10 + 1; }
        }
    }
}
