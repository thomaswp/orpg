using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class Target : Selectable
    {
        public Target()
            : base(0, 0, 336, 480)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            this.Z += 10;
            this.itemMax = Globals.GameParty.Actors.Length;
            Refresh();
        }

        public void Refresh()
        {
            this.Contents.Clear();

            for (int i = 0; i < Globals.GameParty.Actors.Length; i++)
            {
                int x = 4;
                int y = i * 116;
                Game.Actor actor = Globals.GameParty.Actors[i];
                DrawActorName(actor, x, y);
                DrawActorClass(actor, x + 144, y);
                DrawActorLevel(actor, x + 8, y + 32);
                DrawActorState(actor, x + 8, y + 64);
                DrawActorHp(actor, x + 152, y + 32);
                DrawActorSp(actor, x + 152, y + 64);
            }
        }

        public override void UpdateCursorRect()
        {
            if (Index <= -2)
                this.CursorRect = new Rect(0, (Index + 10) * 116, this.width - 32, 96);
            else if (Index == -1)
                this.CursorRect = new Rect(0, 0, this.width - 32, itemMax * 116 - 20);
            else
                this.CursorRect = new Rect(0, Index * 116, this.width - 32, 96);
        }
    }
}
