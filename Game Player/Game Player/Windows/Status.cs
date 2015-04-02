using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    //Basically we have to reqrite Window to update when the fields change instead of when it updates
    //I'm somewhat convinced that this'll require some level of rewriting. Ugh.

    public class Status : Base
    {
        Game.Actor actor;

        public Status(Game.Actor actor)
            : base(0, 0, 640, 480)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            this.actor = actor;
            Refresh();
        }

        public void Refresh()
        {
            this.Contents.Clear();
            DrawActorGraphic(actor, 40, 112);
            DrawActorName(actor, 4, 0);
            DrawActorClass(actor, 4 + 144, 0);
            DrawActorLevel(actor, 96, 32);
            DrawActorState(actor, 96, 64);
            DrawActorHp(actor, 96, 112, 172); 
            DrawActorSp(actor, 96, 144, 172);
            for (int i = 0; i <= 6; i++)
            {
                DrawActorParameter(actor, 96, i * 32 + (i <= 2 ? 192 : 208), i);
            }
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(320, 48, 80, 32, "EXP");
            this.Contents.DrawText(320, 80, 80, 32, "NEXT");
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(320 + 80, 48, 84, 32, actor.ExpS, FontAligns.Right);
            this.Contents.DrawText(320 + 80, 80, 84, 32, actor.NextExpS, FontAligns.Right);
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(320, 160, 96, 32, "Equipment");
            DrawWeaponName(Data.Weapons[actor.WeaponId], 320 + 16, 208);
            DrawArmorName(Data.Armors[actor.Armor1Id], 320 + 16, 256);
            DrawArmorName(Data.Armors[actor.Armor2Id], 320 + 16, 304);
            DrawArmorName(Data.Armors[actor.Armor3Id], 320 + 16, 352);
            DrawArmorName(Data.Armors[actor.Armor4Id], 320 + 16, 400);
        }

        public void Dummy()
        {
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(32, 112, 96, 32, Data.Misc.words.weapon);
            this.Contents.DrawText(32, 176, 96, 32, Data.Misc.words.armor1);
            this.Contents.DrawText(32, 240, 96, 32, Data.Misc.words.armor2);
            this.Contents.DrawText(32, 304, 96, 32, Data.Misc.words.armor3);
            this.Contents.DrawText(32, 368, 96, 32, Data.Misc.words.armor4);
            DrawWeaponName(Data.Weapons[actor.WeaponId], 320 + 16, 144);
            DrawArmorName(Data.Armors[actor.Armor1Id], 320 + 24, 208);
            DrawArmorName(Data.Armors[actor.Armor2Id], 320 + 24, 272);
            DrawArmorName(Data.Armors[actor.Armor3Id], 320 + 24, 336);
            DrawArmorName(Data.Armors[actor.Armor4Id], 320 + 24, 400);
        }
    }
}
