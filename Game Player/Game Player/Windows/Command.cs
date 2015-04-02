using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Windows
{
    public class Command : Selectable
    {
        string[] commands;

        public Command(int width, string[] commands)
            : base(0, 0, width, commands.Length * 32 + 32)
        {
            this.itemMax = commands.Length;
            this.commands = commands;
            this.Contents = new Bitmap(width - 32, this.itemMax * 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            Refresh();
            this.Index = 0;
        }

        public void Refresh()
        {
            this.Contents.Clear();
            for (int i = 0; i < this.itemMax; i++)
            {
                DrawItem(i, NormalColor);
            }
        }

        public void DrawItem(int index, Color color)
        {
            this.Contents.FontColor = color;
            Rect rect = new Rect(4, 32 * index, this.Contents.Width - 8, 32);
            this.Contents.ClearRect(rect);
            this.Contents.DrawText(rect, commands[index]);
        }

        public void DisableItem(int index)
        {
            DrawItem(index, DisabledColor);
        }
    }
}
