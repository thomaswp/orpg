using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Windows
{
    public class Selectable : Base
    {
        protected int itemMax, columnMax;

        Help _helpWindow;
        public Help HelpWindow
        {
            get { return _helpWindow; }
            set
            {
                _helpWindow = value;
                if (HelpWindow != null && base.Active)
                {
                    UpdateHelp();
                }
            }
        }

        int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (HelpWindow != null && base.Active)
                {
                    UpdateHelp();
                }
                UpdateCursorRect();
            }
        }

        public int TopRow
        { 
            get { return this.OY / 32; }
            set
            {
                int row = value;
                row = Math.Max(0, row);
                row = Math.Min(RowMax - 1, row);
                this.OY = row * 32;
            }
        }

        public int RowMax
        { get { return (itemMax - columnMax + 1) / columnMax; } }

        public int PageRowMax
        { get { return (this.Height - 32) / 32; } }

        public int PageItemMax
        { get { return PageRowMax * columnMax; } }

        public Selectable(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            itemMax = 1;
            columnMax = 1;
            Index = -1;
        }

        void UpdateHelp()
        {
            if (HelpWindow != null)
            {
                if (!HelpWindow.Disposed)
                { HelpWindow.Update(); }
            }
        }

        public virtual void UpdateCursorRect()
        {
            if (Index < 0)
            {
                this.CursorRect.Empty();
                return;
            }

            int row = Index / columnMax;
            TopRow = Math.Min(TopRow, row);

            if (row > TopRow + (PageRowMax - 1))
                TopRow = row - (PageRowMax - 1);

            int cursorWidth = this.Width / columnMax - 32;
            int x = Index % columnMax * (cursorWidth + 32);
            int y = Index / columnMax * 32 - this.OY;

            this.CursorRect = new Rect(x, y, cursorWidth, 32);
        }

        public override void Update()
        {
            base.Update();
            if (this.Active && itemMax > 0 && Index >= 0)
            {
                if (Input.Repeated(Keys.Down))
                {
                    if ((columnMax == 1 && Input.Triggered(Keys.Down)) ||
                        Index < itemMax - columnMax)
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index = (Index + columnMax) % itemMax;
                    }
                }
                if (Input.Repeated(Keys.Up))
                {
                    if ((columnMax == 1 && Input.Triggered(Keys.Up)) ||
                        Index >= columnMax)
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index = (Index - columnMax + itemMax) % itemMax;
                    }
                }
                if (Input.Repeated(Keys.Right))
                {
                    if (columnMax >= 2 && Index < itemMax - 1)
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index += 1;
                    }
                }
                if (Input.Repeated(Keys.Left))
                {
                    if (columnMax >= 2 && Index > 0)
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index -= 1;
                    }
                }
                if (Input.Repeated(Keys.R))
                {
                    if (this.TopRow + (this.PageRowMax - 1) < (this.RowMax - 1))
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index = Math.Min(Index + this.PageItemMax, itemMax - 1);
                        this.TopRow += this.PageRowMax;
                    }
                }
                if (Input.Repeated(Keys.L))
                {
                    if (this.TopRow > 0)
                    {
                        Audio.SE.Play(Data.Misc.cursorSe);
                        Index = Math.Max(Index - this.PageItemMax, 0);
                        this.TopRow -= this.PageRowMax;
                    }
                }
            }
            if (this.Active && HelpWindow != null)
            { HelpWindow.Update(); }
            UpdateCursorRect();
        }
    }
}
