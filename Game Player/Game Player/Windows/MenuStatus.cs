using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class MenuStatus : Selectable
    {
        public MenuStatus()
            : base(0, 0, 480, 480)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            Refresh();
            this.Active = false;
            this.Index = -1;
        }

        public void Refresh()
        {
            this.Contents.Clear();
            itemMax = Globals.GameParty.Actors.Length;
            for (int i = 0; i < Globals.GameParty.Actors.Length; i++)
            {
                int x = 64;
                int y = i * 116;
                Game.Actor actor = Globals.GameParty.Actors[i];
                DrawActorGraphic(actor, x - 40, y + 80);
                DrawActorName(actor, x, y);
                DrawActorClass(actor, x + 144, y);
                DrawActorLevel(actor, x, y + 32);
                DrawActorState(actor, x + 90, y + 32);
                DrawActorExp(actor, x, y + 64);
                DrawActorHp(actor, x + 236, y + 32);
                DrawActorSp(actor, x + 236, y + 64);
            }
        }

        public override void UpdateCursorRect()
        {
            if (Index < 0)
                this.CursorRect.Empty();
            else
                this.CursorRect = new Rect(0, Index * 116, this.Width - 32, 96);
        }
    }
}
