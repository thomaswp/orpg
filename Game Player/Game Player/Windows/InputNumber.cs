using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Windows
{
    public class InputNumber : Base
    {
        protected int number;
        public int Number
        {
            get { return number; }
            set { number = value.MinMax(0, (int)Math.Pow(10, digitsMax) - 1); }
        }

        int digitsMax;
        int cursorWidth;
        int index;

        public InputNumber(int digitsMax) : 
            base(0, 0, (new Bitmap(32, 32).CharacterLength('0') + 8) * digitsMax + 32, 4) //changed
        {
            this.digitsMax = digitsMax;
            number = 0;

            cursorWidth = new Bitmap(32, 32).CharacterLength('0') + 8;

            this.Contents = new Bitmap(width - 32, height - 32);
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;
            this.Z += 9999;
            this.Opacity = 0;

            index = 0;
            Refresh();
            UpdateCursorRect();
        }

        public void UpdateCursorRect()
        {
            this.CursorRect = new Rect(index * cursorWidth, 0, cursorWidth, 32);
        }


        public override void Update()
        {
            base.Update();

            if (Input.Repeated(Keys.Up) || Input.Repeated(Keys.Down))
            {
                Audio.SE.Play(Data.Misc.cursorSe);

                int place = (int)Math.Pow(10, digitsMax - 1 - index);
                int n = number / place % 10;
                number -= n * place;

                if (Input.Repeated(Keys.Up))
                    n = (n + 1) % 10;
                if (Input.Repeated(Keys.Down))
                    n = (n + 9) % 10;

                number += n * place;
                Refresh();
            }

            if (Input.Repeated(Keys.Right))
            {
                if (digitsMax >= 2)
                {
                    Audio.SE.Play(Data.Misc.cursorSe);
                    index = (index + 1) % digitsMax;
                }
            }

            if (Input.Repeated(Keys.Left))
            {
                if (digitsMax >= 2)
                {
                    Audio.SE.Play(Data.Misc.cursorSe);
                    index = (index + digitsMax - 1) % digitsMax;
                }
            }

            UpdateCursorRect();
        }

        public void Refresh()
        {
            this.Contents.Clear();
            this.Contents.FontColor = NormalColor;
            string s = number.ToString("".PadRight(digitsMax, '0'));
            for (int i = 0; i < s.Length; i++)
                this.Contents.DrawText(i * cursorWidth + 4, 0, 32, 32, s[i].ToString());
        }
    }
}
