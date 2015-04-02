using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Windows
{
    public class Help : Base
    {
        private string text;
        private FontAligns align;
        private Game.Battler actor;

        public Help()
            : base(0, 0, 640, 64)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
        }

        public void SetText(string text) { SetText(text, 0); }
        public void SetText(string text, FontAligns align)
        {
            if (text != this.text || align != this.align)
            {
                this.Contents.Clear();
                this.Contents.FontColor = NormalColor;
                this.Contents.DrawText(4, 0, this.width - 40, 32, text, align);
                this.text = text;
                this.align = align;
                this.actor = null;
            }

            this.visible = true;
        }

        public void SetActor(Game.Actor actor)
        {
            if (actor != this.actor)
            {
                this.Contents.Clear();
                DrawActorName(actor, 4, 0);
                DrawActorState(actor, 140, 0);
                DrawActorHp(actor, 284, 0);
                DrawActorSp(actor, 460, 0);
                this.actor = actor;
                this.text = null;
                this.visible = true;
            }
        }

        public void SetEnemy(Game.Enemy enemy)
        {
            text = enemy.Name;
            string stateText = MakeBattlerStateText(enemy, 112, false);
            if (stateText.Length > 0)
                text += "  " + stateText;
            SetText(text, FontAligns.Center);
        }
    }
}
