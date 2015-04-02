using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataClasses;

namespace Game_Player.Scenes
{
    public class Item : Scene
    {
        private Windows.Help helpWindow;
        private Windows.Items itemWindow;
        private Windows.Target targetWindow;
        private ItemType item;

        public Item()
        {
            Graphics.Transition();

            helpWindow = new Windows.Help();
            itemWindow = new Windows.Items();
            itemWindow.HelpWindow = helpWindow;

            targetWindow = new Windows.Target();
            targetWindow.Visible = false;
            targetWindow.Active = false;
        }

        public override void End() 
        {
            helpWindow.Dispose();
            itemWindow.Dispose();
            targetWindow.Dispose();
        }

        public override void Update() 
        {
            helpWindow.Update();
            itemWindow.Update();
            targetWindow.Update();

            if (itemWindow.Active)
            {
                UpdateItem();
                return;
            }

            if (targetWindow.Active)
            {
                UpdateTarget();
                return;
            }
        }

        private void UpdateItem()
        {
            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                Globals.Scene = new Scenes.Menu(0);
                return;
            }

            if (Input.Triggered(Keys.C))
            {
                this.item = itemWindow.Item;

                if (!(this.item is DataClasses.Item))
                {
                    Audio.SE.Play(Data.Misc.buzzerSe);
                    return;
                }

                if (!Globals.GameParty.CanUseItem(this.item.id))
                {
                    Audio.SE.Play(Data.Misc.buzzerSe);
                    return;
                }

                Audio.SE.Play(Data.Misc.decisionSe);

                DataClasses.Item item = (DataClasses.Item)this.item; //added
                if (item.scope >= 3)
                {
                    itemWindow.Active = false;
                    targetWindow.X = (itemWindow.Index + 1) % 2 * 304;
                    targetWindow.Visible = true;
                    targetWindow.Active = true;

                    if (item.scope == 4 || item.scope == 6)
                        targetWindow.Index = -1;
                    else
                        targetWindow.Index = 0;
                }
                else
                {
                    if (item.commonEventId > 0)
                    {
                        Globals.GameTemp.commonEventId = item.commonEventId;
                        Audio.SE.Play(item.menuSe);

                        if (item.consumable)
                        {
                            Globals.GameParty.LoseItem(item.id, 1);
                            itemWindow.DrawItem(itemWindow.Index);
                        }

                        Globals.Scene = new Scenes.Map();
                        return;
                    }
                }
            }
        }

        private void UpdateTarget()
        {
            DataClasses.Item item = (DataClasses.Item)this.item; //added

            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                if (!Globals.GameParty.CanUseItem(item.id))
                    itemWindow.Refresh();

                itemWindow.Active = true;
                targetWindow.Visible = false;
                targetWindow.Active = false;
                return;
            }

            if (Input.Triggered(Keys.C))
            {
                if (Globals.GameParty.ItemNumber(item.id) == 0)
                {
                    Audio.SE.Play(Data.Misc.buzzerSe);
                    return;
                }

                bool used = false;

                if (targetWindow.Index == -1)
                {
                    used = false;
                    foreach (Game.Actor i in Globals.GameParty.Actors)
                        used |= i.ItemEffect(item);
                }

                if (targetWindow.Index >= 0)
                {
                    Game.Actor target = Globals.GameParty.Actors[targetWindow.Index];
                    used = target.ItemEffect(item);
                }

                if (used)
                {
                    Audio.SE.Play(item.menuSe);

                    if (item.consumable)
                    {
                        Globals.GameParty.LoseItem(item.id, 1);
                        itemWindow.DrawItem(itemWindow.Index);
                    }

                    targetWindow.Refresh();

                    if (Globals.GameParty.AreAllDead)
                    {
                        Globals.Scene = new Scenes.Gameover();
                        return;
                    }

                    if (item.commonEventId > 0)
                    {
                        Globals.GameTemp.commonEventId = item.commonEventId;

                        Globals.Scene = new Scenes.Map();
                        return;
                    }
                }
                else
                {
                    Audio.SE.Play(Data.Misc.buzzerSe);
                }
            }
        }
    }
}
