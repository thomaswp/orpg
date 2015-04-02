using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Scenes
{
    public class Menu : Scene
    {
        int menuIndex;
        Windows.Command commandWindow;
        Windows.Playtime playtimeWindow;
        Windows.Steps stepsWindow;
        Windows.Gold goldWindow;
        Windows.MenuStatus statusWindow;

        public Menu() : this(0) { }
        public Menu(int menuIndex)
        {
            Graphics.Transition();

            this.menuIndex = menuIndex;

            string s1 = Data.Misc.words.item;
            string s2 = Data.Misc.words.skill;
            string s3 = Data.Misc.words.equip;
            string s4 = "Status";
            string s5 = "Save";
            string s6 = "End Game";
            commandWindow = new Windows.Command(160, new string[] { s1, s2, s3, s4, s5, s6 });
            commandWindow.Index = menuIndex;

            if (Globals.GameParty.Actors.Length == 0)
                for (int i = 0; i < 4; i++)
                    commandWindow.DisableItem(i);

            if (Globals.GameSystem.SaveDisabled)
                commandWindow.DisableItem(4);

            playtimeWindow = new Windows.Playtime();
            playtimeWindow.X = 0;
            playtimeWindow.Y = 224;

            stepsWindow = new Windows.Steps();
            stepsWindow.X = 0;
            stepsWindow.Y = 320;

            goldWindow = new Windows.Gold();
            goldWindow.X = 0;
            goldWindow.Y = 416;

            statusWindow = new Windows.MenuStatus();
            statusWindow.X = 160;
            statusWindow.Y = 0;
        }

        public override void End()
        {
            commandWindow.Dispose();
            playtimeWindow.Dispose();
            stepsWindow.Dispose();
            goldWindow.Dispose();
            statusWindow.Dispose();
        }

        public override void Update()
        {
            commandWindow.Update();
            playtimeWindow.Update();
            stepsWindow.Update();
            goldWindow.Update();
            statusWindow.Update();

            if (commandWindow.Active)
            {
                UpdateCommand();
                return;
            }

            if (statusWindow.Active)
            {
                UpdateStatus();
                return;
            }
        }

        private void UpdateCommand()
        {
            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                Globals.Scene = new Scenes.Map();
                return;
            }

            if (Input.Triggered(Keys.C))
            {
                if (Globals.GameParty.Actors.Length == 0 && commandWindow.Index < 4)
                {
                    Audio.SE.Play(Data.Misc.buzzerSe);
                    return;
                }

                switch (commandWindow.Index)
                {
                    case 0:
                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Item();
                        break;
                    case 1:
                    case 2:
                    case 3:
                        Audio.SE.Play(Data.Misc.decisionSe);
                        commandWindow.Active = false;
                        statusWindow.Active = true;
                        statusWindow.Index = 0;
                        break;
                    case 4:
                        if (Globals.GameSystem.SaveDisabled)
                        {
                            Audio.SE.Play(Data.Misc.buzzerSe);
                            return;
                        }

                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Save();

                        break;
                    case 5:
                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Ending();
                        break;
                }
            }
        }

        private void UpdateStatus()
        {
            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                commandWindow.Active = true;
                statusWindow.Active = false;
                statusWindow.Index = -1;
                return;
            }
            if (Input.Triggered(Keys.C))
            {
                switch (commandWindow.Index)
                {
                    case 1:
                        if (Globals.GameParty.Actors[statusWindow.Index].Restriction >= 2)
                        {
                            Audio.SE.Play(Data.Misc.buzzerSe);
                            return;
                        }

                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Skill(statusWindow.Index);
                        break;
                    case 2:
                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Equip(statusWindow.Index);
                        break;
                    case 3:
                        Audio.SE.Play(Data.Misc.decisionSe);
                        Globals.Scene = new Scenes.Status(statusWindow.Index);
                        break;
                }
            }
        }
    }
}
