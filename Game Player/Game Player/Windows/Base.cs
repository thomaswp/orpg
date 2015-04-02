using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Windows
{
    public class Base : Window
    {
        String windowskinName;

        public Color NormalColor { get { return new Color(255, 255, 255, 255); } }
        public Color DisabledColor { get { return new Color(128, 128, 128, 255); } }
        public Color SystemColor { get { return new Color(192, 224, 255, 255); } }
        public Color CrisisColor { get { return new Color(255, 255, 255, 64); } }
        public Color KnockoutColor { get { return new Color(255, 255, 64, 0); } }

        public Base(int x, int y, int width, int height)
            : base()
        {
            windowskinName = Globals.GameSystem.WindowSkinName;
            base.WindowSkin = Cache.LoadWindowskin(windowskinName);
            base.X = x;
            base.Y = y;
            base.Width = width;
            base.Height = height;
            base.Z = 100;

            //ADDED
            base.Update();
        }

        public Color TextColor(int n)
        {
            switch (n)
            {
                case 0:
                    return new Color(255, 255, 255, 255);
                case 1:
                    return new Color(128, 128, 255, 255);
                case 2:
                    return new Color(255, 128, 128, 255);
                case 3:
                    return new Color(128, 255, 128, 255);
                case 4:
                    return new Color(128, 255, 255, 255);
                case 5:
                    return new Color(255, 128, 255, 255);
                case 6:
                    return new Color(255, 255, 128, 255);
                case 7:
                    return new Color(192, 192, 192, 255);
                default:
                    return NormalColor;
            }
        }

        public override void Update()
        {
            if (windowskinName != Globals.GameSystem.WindowSkinName)
            {
                windowskinName = Globals.GameSystem.WindowSkinName;
                base.WindowSkin = Cache.LoadWindowskin(windowskinName);
            }
            base.Update();
        }

        public void DrawActorGraphic(Game.Actor actor, int x, int y)
        {
            Bitmap bitmap = Cache.LoadCharacter(actor.CharacterName, actor.CharacterHue);
            int cw = bitmap.Width / 4;
            int ch = bitmap.Height / 4;
            Rect srcRect = new Rect(0, 0, cw, ch);
            this.Contents.BlockTransfer(x - cw / 2, y - ch, bitmap, srcRect);
        }

        public void DrawActorName(Game.Actor actor, int x, int y)
        {
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x, y, 120, 32, actor.Name);
        }

        public void DrawActorClass(Game.Actor actor, int x, int y)
        {
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x, y, 236, 32, actor.ClassName);
        }

        public void DrawActorLevel(Game.Actor actor, int x, int y)
        {
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, "Lv");
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 32, y, 24, 32, actor.Level.ToString(), FontAligns.Right);
        }

        public string MakeBattlerStateText(Game.Battler battler, int width, bool needNormal)
        {
            int bracketsWidth = this.Contents.TextSize("[]").Width;

            string text = "";
            foreach (int i in battler.States)
            {
                if (Data.States[i].rating >= 1)
                {
                    if (text == "")
                        text = Data.States[i].name;
                    else
                    {
                        string newText = text + "/" + Data.States[i].name;
                        int textWidth = this.Contents.TextSize(newText).Width;
                        if (textWidth > width - bracketsWidth)
                            break;
                        text = newText;
                    }
                }
            }

            if (text == "")
            {
                if (needNormal)
                    text = "[Normal]";
            }
            else
                text = "[" + text + "]";

            return text;
        }

        public void DrawActorState(Game.Actor actor, int x, int y) { DrawActorState(actor, x, y, 120); }
        public void DrawActorState(Game.Actor actor, int x, int y, int width)
        {
            string text = MakeBattlerStateText(actor, width, true);
            this.Contents.FontColor = actor.Hp == 0 ? KnockoutColor : NormalColor;
            this.Contents.DrawText(x, y, width, 32, text);
        }

        public void DrawActorExp(Game.Actor actor, int x, int y)
        {
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, y, 24, 32, "E");
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 24, y, 84, 32, actor.Exp.ToString(), FontAligns.Right);
            this.Contents.DrawText(x + 108, y, 12, 32, "/", FontAligns.Center);
            this.Contents.DrawText(x + 120, y, 84, 32, actor.NextExpS);
        }

        public void DrawActorHp(Game.Actor actor, int x, int y) { DrawActorHp(actor, x, y, 144); }
        public void DrawActorHp(Game.Actor actor, int x, int y, int width)
        {
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, Data.Misc.words.hp);

            bool flag = false;
            int hpX = 0;
            if (width - 32 >= 108)
            {
                hpX = x + width - 108;
                flag = true;
            }
            else if (width - 32 >= 48)
            {
                hpX = x + width - 48;
                flag = false;
            }

            this.Contents.FontColor = actor.Hp == 0 ? KnockoutColor :
                actor.Hp <= actor.MaxHp / 4 ? CrisisColor : NormalColor;
            this.Contents.DrawText(hpX, y, 48, 32, actor.Hp.ToString(), FontAligns.Right);

            if (flag)
            {
                this.Contents.FontColor = NormalColor;
                this.Contents.DrawText(hpX + 48, y, 12, 32, "/", FontAligns.Center);
                this.Contents.DrawText(hpX + 60, y, 48, 32, actor.MaxHp.ToString());
            }
        }

        public void DrawActorSp(Game.Actor actor, int x, int y) { DrawActorSp(actor, x, y, 144); }
        public void DrawActorSp(Game.Actor actor, int x, int y, int width)
        {
            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, y, 32, 32, Data.Misc.words.sp);

            int spX = 0;
            bool flag = false;
            if (width - 32 >= 108)
            {
                spX = x + width - 108;
                flag = true;
            }
            else if (width - 32 >= 48)
            {
                spX = x + width - 48;
                flag = false;
            }

            this.Contents.FontColor = actor.Sp == 0 ? KnockoutColor :
                actor.Sp <= actor.MaxSp / 4 ? CrisisColor : NormalColor;
            this.Contents.DrawText(spX, y, 48, 32, actor.Sp.ToString(), FontAligns.Right);

            if (flag)
            {
                this.Contents.FontColor = NormalColor;
                this.Contents.DrawText(spX + 48, y, 12, 32, "/", FontAligns.Center);
                this.Contents.DrawText(spX + 60, y, 48, 32, actor.MaxSp.ToString());
            }
        }

        public void DrawActorParameter(Game.Actor actor, int x, int y, int type)
        {
            string parameterName = "";
            int parameterValue = 0;

            switch (type)
            {
                case 0:
                    parameterName = Data.Misc.words.atk;
                    parameterValue = actor.Atk;
                    break;
                case 1:
                    parameterName = Data.Misc.words.pdef;
                    parameterValue = actor.PDef;
                    break;
                case 2:
                    parameterName = Data.Misc.words.mdef;
                    parameterValue = actor.MDef;
                    break;
                case 3:
                    parameterName = Data.Misc.words.str;
                    parameterValue = actor.Str;
                    break;
                case 4:
                    parameterName = Data.Misc.words.dex;
                    parameterValue = actor.Dex;
                    break;
                case 5:
                    parameterName = Data.Misc.words.agi;
                    parameterValue = actor.Agi;
                    break;
                case 6:
                    parameterName = Data.Misc.words.intel;
                    parameterValue = actor.Int;
                    break;
            }

            this.Contents.FontColor = SystemColor;
            this.Contents.DrawText(x, y, 120, 32, parameterName);
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 120, y, 36, 32, parameterValue.ToString(), FontAligns.Right);
        }


        public void DrawItemName(DataClasses.Item item, int x, int y)
        {
            if (item == null)
                return;

            Bitmap bitmap = Cache.LoadIcon(item.iconName);
            this.Contents.BlockTransfer(x, y + 4, bitmap, new Rect(0, 0, 24, 24));
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 28, y, 212, 32, item.name);
        }

        public void DrawWeaponName(DataClasses.Weapon item, int x, int y)
        {
            if (item == null)
                return;

            Bitmap bitmap = Cache.LoadIcon(item.iconName);
            this.Contents.BlockTransfer(x, y + 4, bitmap, new Rect(0, 0, 24, 24));
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 28, y, 212, 32, item.name);
        }

        public void DrawArmorName(DataClasses.Armor item, int x, int y)
        {
            if (item == null)
                return;

            Bitmap bitmap = Cache.LoadIcon(item.iconName);
            this.Contents.BlockTransfer(x, y + 4, bitmap, new Rect(0, 0, 24, 24));
            this.Contents.FontColor = NormalColor;
            this.Contents.DrawText(x + 28, y, 212, 32, item.name);
        }
    }
}
