using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class PartyCommand : Selectable
    {
        private string[] commands;

        public PartyCommand()
            : base(0, 0, 640, 64)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            this.backOpacity = 160;

            commands = new string[] { "Attack", "Escape" };
            itemMax = 2;
            columnMax = 2;

            DrawItem(0, NormalColor);
            DrawItem(1, Globals.GameTemp.battleCanEscape ? NormalColor : DisabledColor);

            this.Active = false;
            this.Visible = false;
            this.Index = 0;
        }

        public void DrawItem(int index, Color color)
        {
            this.Contents.FontColor = color;
            Rect rect = new Rect(160 + index * 160 + 4, 0, 128 - 10, 32);
            this.Contents.FillRect(rect, Colors.Clear);
            this.Contents.DrawText(rect, commands[index], FontAligns.Center);
        }

        public override void UpdateCursorRect()
        {
            this.CursorRect = new Rect(160 + Index * 160, 0, 128, 32);
        }
    }
}
