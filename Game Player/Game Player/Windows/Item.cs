using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Windows
{
    public class Items : Selectable
    {
        public ItemType Item
        {
            get { return data[this.Index]; }
        }

        private ItemType[] data;

        public Items()
            : base(0, 64, 640, 416)
        {
            columnMax = 2;
            Refresh();
            this.Index = 0;

            if (Globals.GameTemp.inBattle)
            {
                this.Y = 64;
                this.Height = 256;
                this.BackOpacity = 160;
            }
        }

        public void Refresh()
        {
            if (this.Contents != null)
            {
                this.Contents.Dispose();
                this.Contents = null;
            }

            data = new DataClasses.Item[] { };

            for (int i = 1; i < Data.Items.Length; i++)
                if (Globals.GameParty.ItemNumber(i) > 0)
                    data = data.Plus<ItemType>(Data.Items[i]);

            if (!Globals.GameTemp.inBattle)
            {
                for (int i = 1; i < Data.Weapons.Length; i++)
                    if (Globals.GameParty.WeaponNumber(i) > 0)
                        data = data.Plus<ItemType>(Data.Weapons[i]);

                for (int i = 1; i < Data.Armors.Length; i++)
                    if (Globals.GameParty.ArmorNumber(i) > 0)
                        data = data.Plus<ItemType>(Data.Armors[i]);
            }

            itemMax = data.Length;
            if (itemMax > 0)
            {
                this.Contents = new Bitmap(width - 32, RowMax * 32);
                this.Contents.FontName = Graphics.FontFace;
                this.Contents.FontSize = Graphics.FontSize;
                for (int i = 0; i < itemMax; i++)
                    DrawItem(i);
            }
        }

        public void DrawItem(int index)
        {
            ItemType item = data[index];
            int number = -1;
            if (item is DataClasses.Item)
                number = Globals.GameParty.ItemNumber(item.id);
            if (item is DataClasses.Weapon)
                number = Globals.GameParty.ItemNumber(item.id);
            if (item is DataClasses.Armor)
                number = Globals.GameParty.ItemNumber(item.id);

            if (item is DataClasses.Item && Globals.GameParty.CanUseItem(item.id))
                this.Contents.FontColor = NormalColor;
            else
                this.Contents.FontColor = DisabledColor;

            int x = 4 + index % 2 * (288 + 32);
            int y = index / 2 * 32;
            Rect rect = new Rect(x, y, this.width / columnMax - 32, 32);
            this.Contents.ClearRect(rect);
            Bitmap bitmap = Cache.LoadIcon(item.iconName);
            int opacity = Contents.FontColor == NormalColor ? 255 : 128;
            this.Contents.BlockTransfer(x, y + 4, bitmap, new Rect(0, 0, 24, 24), opacity);
            this.Contents.DrawText(x + 28, y, 212, 32, item.name, FontAligns.Left);
            this.Contents.DrawText(x + 240, y, 16, 32, ":", FontAligns.Center);
            this.Contents.DrawText(x + 256, y, 24, 32, number.ToString(), FontAligns.Right);
        }

        private void UpdateHelp()
        {
            HelpWindow.SetText(this.Item == null ? "" : this.Item.description);
        }
    }

}
