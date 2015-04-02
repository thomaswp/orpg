using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class BattleStatus : Base
    {
        private bool[] levelUpFlags;
        private int itemMax;

        public BattleStatus()
            : base(0, 320, 640, 160)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;

            levelUpFlags = new bool[4];
            Refresh();
        }

        public void LevelUp(int actorIndex)
        {
            levelUpFlags[actorIndex] = true;
        }

        public void Refresh()
        {
            this.Contents.Clear();
            itemMax = Globals.GameParty.Actors.Length;

            for (int i = 0; i < Globals.GameParty.Actors.Length; i++)
            {
                Game.Actor actor = Globals.GameParty.Actors[i];
                int actorX = i * 160 + 4;
                DrawActorName(actor, actorX, 0);
                DrawActorHp(actor, actorX, 64, 120);
                DrawActorSp(actor, actorX, 64, 120);

                if (levelUpFlags[i])
                {
                    this.Contents.FontColor = NormalColor;
                    this.Contents.DrawText(actorX, 96, 120, 32, "LEVEL UP!");
                }
                else
                {
                    DrawActorState(actor, actorX, 96);
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (Globals.GameTemp.battleMainPhase)
            {
                if (this.contentsOpacity > 191)
                {
                    this.contentsOpacity -= 4;
                }
            }
            else if (this.contentsOpacity < 255)
            {
                this.contentsOpacity += 4;
            }
        }
    }
}
