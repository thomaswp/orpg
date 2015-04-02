using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Game_Player.Windows
{
    public class Message : Selectable
    {
        private const char CHANGE_COLOR = (char)1;
        private const char SHOW_GOLD = (char)2;

        protected bool fadeIn;
        protected bool fadeOut;
        protected bool contentsShowing;
        protected int cursorWidth;
        protected Windows.InputNumber inputNumberWindow;
        protected Windows.Gold goldWindow;

        public Message() : base(80, 304, 480, 160)
        {
            this.Contents = new Bitmap(width - 32, height - 32);
            this.Visible = false;
            this.Z = 9998;
            fadeIn = false;
            fadeOut = false;
            contentsShowing = false;
            cursorWidth = 0;
            this.active = false;
            this.Index = -1;
        }

        public override void Dispose()
        {
            TerminateMessage();
            Globals.GameTemp.messageWindowShowing = false;
            if (inputNumberWindow != null)
                inputNumberWindow.Dispose();
            base.Dispose();
        }

        public void TerminateMessage()
        {
            this.Active = false;
            this.Paused = false;
            this.Index = -1;
            this.Contents.Clear();

            contentsShowing = false;

            if (Globals.GameTemp.messageProc != null)
                Globals.GameTemp.messageProc();

            Globals.GameTemp.messageText = "";
            Globals.GameTemp.messageProc = null;
            Globals.GameTemp.choiceStart = 99;
            Globals.GameTemp.choiceMax = 0;
            Globals.GameTemp.choiceCancelType = 0;
            Globals.GameTemp.choiceProc = null;
            Globals.GameTemp.numInputStart = 99;
            Globals.GameTemp.numInputVariableId = 0;
            Globals.GameTemp.numInputDigitsMax = 0;

            if (goldWindow != null)
            {
                goldWindow.Dispose();
                goldWindow = null;
            }
        }

        public void Refresh()
        {

            this.Contents.Clear();
            this.Contents.FontColor = NormalColor;
            this.Contents.FontName = Graphics.FontFace;
            this.Contents.FontSize = Graphics.FontSize;

            int x = 0, y = 0;
            cursorWidth = 0;

            if (Globals.GameTemp.choiceStart == 0)
                x = 8;

            if (Globals.GameTemp.messageText.Length > 0)
            {
                string text = Globals.GameTemp.messageText;

                MatchCollection matches = Regex.Matches(text, @"\\[Vv]\[([0-9]+)\]");
                foreach (Match match in matches)
                {
                    int startIndex = match.Index + 3;
                    int length = match.Value.Length - 4;
                    int index = int.Parse(text.Substring(startIndex, length));
                    int value = Globals.GameVariables[index];
                    text = text.Replace(match.Value, value.ToString());
                }

                matches = Regex.Matches(text, @"\\[Nn]\[([0-9]+)\]");
                foreach (Match match in matches)
                {
                    int startIndex = match.Index + 3;
                    int length = match.Value.Length - 4;
                    int index = int.Parse(text.Substring(startIndex, length));
                    string value = Globals.GameActors[index] == null ? "" : Globals.GameActors[index].Name;
                    text = text.Replace(match.Value, value);
                }

                matches = Regex.Matches(text, @"\\[Cc]\[([0-9]+)\]");
                foreach (Match match in matches)
                {
                    int startIndex = match.Index + 3;
                    int length = match.Value.Length - 4;
                    int index = int.Parse(text.Substring(startIndex, length));
                    string value = CHANGE_COLOR.ToString() + index;
                    text = text.Replace(match.Value, value);
                }

                text = text.Replace("\\\\", "\\");
                text = Regex.Replace(text, @"\\[Gg]", SHOW_GOLD.ToString());

                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];

                    if (c == CHANGE_COLOR)
                    {
                        int color = int.Parse(text[i + 1].ToString());
                        if (color >= 0 && color <= 7)
                            this.Contents.FontColor = TextColor(color);
                        i++;
                    }
                    else if (c == SHOW_GOLD)
                    {
                        if (goldWindow == null)
                        {
                            goldWindow = new Gold();
                            goldWindow.X = 560 - goldWindow.Width;
                            if (Globals.GameTemp.inBattle)
                                goldWindow.Y = 192;
                            else
                                goldWindow.Y = this.Y >= 128 ? 32 : 384;
                            goldWindow.Opacity = this.Opacity;
                            goldWindow.BackOpacity = this.BackOpacity;
                        }
                    }
                    else if (c == '\n')
                    {
                        if (y >= Globals.GameTemp.choiceStart)
                            cursorWidth = Math.Max(cursorWidth, x);

                        y++;
                        x = 0;

                        if (y >= Globals.GameTemp.choiceStart)
                            x = 8;
                    }
                    else
                    {
                        this.Contents.DrawText(4 + x, 32 * y, 40, 32, c.ToString());
                        x += this.Contents.CharacterLength(c);
                    }
                }

                if (Globals.GameTemp.choiceMax > 0)
                {
                    itemMax = Globals.GameTemp.choiceMax;
                    this.active = true;
                    this.Index = 0;
                }

                if (Globals.GameTemp.numInputVariableId > 0)
                {
                    int digitsMax = Globals.GameTemp.numInputDigitsMax;
                    int number = Globals.GameVariables[Globals.GameTemp.numInputVariableId];
                    inputNumberWindow = new InputNumber(digitsMax);
                    inputNumberWindow.Number = number;
                    inputNumberWindow.X = this.X + 8;
                    inputNumberWindow.Y = this.Y + Globals.GameTemp.numInputStart * 32;
                }
            }
        }

        public void ResetWindow()
        {
            if (Globals.GameTemp.inBattle)
                this.Y = 16;
            else
            {
                switch (Globals.GameSystem.MessagePosition)
                {
                    case 0: this.Y = 16; break;
                    case 1: this.Y = 160; break;
                    case 2: this.Y = 304; break;
                }
            }

            if (Globals.GameSystem.MessageFrame == 0)
                this.opacity = 255;
            else
                this.opacity = 0;

            this.backOpacity = 160;
        }

        public override void Update()
        {
            base.Update();

            if (fadeIn)
            {
                this.ContentsOpacity += 24;
                if (inputNumberWindow != null)
                    inputNumberWindow.ContentsOpacity += 24;
                if (this.ContentsOpacity == 255)
                    fadeIn = false;
            }

            if (inputNumberWindow != null)
            {
                inputNumberWindow.Update();

                if (Input.Triggered(Keys.C))
                {
                    Audio.SE.Play(Data.Misc.decisionSe);
                    Globals.GameVariables[Globals.GameTemp.numInputVariableId] = inputNumberWindow.Number;
                    Globals.GameMap.NeedRefresh = true;

                    inputNumberWindow.Dispose();
                    inputNumberWindow = null;
                    TerminateMessage();
                }

                return;
            }

            if (contentsShowing)
            {
                if (Globals.GameTemp.choiceMax == 0)
                    this.Paused = true;

                if (Input.Triggered(Keys.B))
                {
                    if (Globals.GameTemp.choiceMax > 0 && Globals.GameTemp.choiceCancelType > 0)
                    {
                        Audio.SE.Play(Data.Misc.cancelSe);
                        Globals.GameTemp.choiceProc(Globals.GameTemp.choiceCancelType - 1);
                        TerminateMessage();
                    }
                }

                if (Input.Triggered(Keys.C))
                {
                    if (Globals.GameTemp.choiceMax > 0)
                    {
                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.GameTemp.choiceProc(this.Index);
                    }
                    TerminateMessage();
                }

                return;
            }

            if (fadeOut == false && Globals.GameTemp.messageText != "")
            {
                contentsShowing = true;
                Globals.GameTemp.messageWindowShowing = true;
                ResetWindow();
                Refresh();
                this.Visible = true;
                this.ContentsOpacity = 0;
                if (inputNumberWindow != null)
                    inputNumberWindow.ContentsOpacity = 0;
                fadeIn = true;
                return;
            }

            if (this.Visible)
            {
                fadeOut = true;
                this.Opacity -= 48;
                if (this.Opacity == 0)
                {
                    this.Visible = false;
                    fadeOut = false;
                    Globals.GameTemp.messageWindowShowing = false;
                }
                return;
            }
        }

        public override void UpdateCursorRect()
        {
            if (Index >= 0)
            {
                int n = Globals.GameTemp.choiceStart + Index;
                this.CursorRect = new Rect(8, n * 32, cursorWidth + 8, 32);
            }
            else
                this.CursorRect.Empty();
        }
    }
}
