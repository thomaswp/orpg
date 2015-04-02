using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Scenes 
{

    public class Title : Game_Player.Scene
    {
        //Sprite for the background image
        Sprite sprite;
        //Command window, displaying options on the screen
        Windows.Command commandWindow;
        //Boolean telling if there are any saves to continue
        Boolean continueEnabled;

        public Title()
        {
            //Load data files created in the Game Editor
            Data.Load(Paths.DataDir);

            //Fade the title screen in
            Graphics.Transition(20);

            //Create the background Sprite, set it to the title screen and make it fill the screen
            sprite = new Sprite();
            sprite.Bitmap = Cache.LoadTitle(Data.Misc.titleName);
            sprite.Resize(Graphics.ScreenWidth, Graphics.ScreenHeight);

            //Define the command prompt strings
            string s1 = "New Game";
            string s2 = "Continue";
            string s3 = "Shutdown";

            //Create a new command window with our strings
            commandWindow = new Windows.Command(192, new string[3] { s1, s2, s3 });
            //Make the background transluscent
            commandWindow.BackOpacity = 160;
            //Set the X coordinate so that the window is in the middle of the screen
            commandWindow.X = (Graphics.ScreenWidth - commandWindow.Width) / 2;
            //Set the Y coordinate
            commandWindow.Y = Graphics.ScreenHeight * 3 / 5;

            //Looks for save files. If any are found continueEnabled will be set to true
            continueEnabled = false;
            for (int i = 0; i < 3; i++)
                if (System.IO.File.Exists("Save" + i.ToString() + ".orpgdata"))
                    continueEnabled = true;

            //If continueEnabled is true, the commandWindow starts at index = 1
            if (continueEnabled)
                commandWindow.Index = 1;
            //If not, the menu item is disabled
            else
                commandWindow.DisableItem(1);

            //Plays the title music
#if (!DEBUG)
            Audio.BGM.Play(Data.Misc.titleBgm, true);
#endif
        }

        /// <summary>
        /// Performs any final tasks before a scene change.
        /// </summary>
        public override void End()
        {
            //Dispose background and commandWindow
            sprite.Dispose();
            commandWindow.Dispose();
        }

        void BattleTest()
        {
        }

        void CommandNewGame()
        {
            Audio.SE.Play(Data.Misc.decisionSe);
            Audio.BGM.Stop();

            Globals.GameTemp = new Game.Temp();
            Globals.GameSystem = new Game.System();
            Globals.GameSwitches = new Game.Switches();
            Globals.GameVariables = new Game.Variables();
            Globals.GameSelfSwitches = new Game.SelfSwitches();
            Globals.GameScreen = new Game.Screen();
            Globals.GameActors = new Game.Actors();
            Globals.GameParty = new Game.Party();
            Globals.GameTroop = new Game.Troop();
            Globals.GameMap = new Game.Map();
            Globals.GamePlayer = new Game.Player();

            Globals.GameParty.SetupStartingMembers();
            Globals.GameMap.Setup(Data.Misc.startMapId);
            Globals.GamePlayer.MoveTo(Data.Misc.startX, Data.Misc.startY);
            Globals.GamePlayer.Refresh();
            Globals.GameMap.Autoplay();
            Globals.GameMap.Update();

            for (int i = 1; i < 32; i++)
                Globals.GameParty.GainItem(i, 2);

            Globals.Scene = new Scenes.Map();
        }

        void CommandContinue()
        {
            //If there are no save slots to load...
            if (!continueEnabled)
            {
                //Play a buzzer sound
                Audio.SE.Play(Data.Misc.buzzerSe);
                //And return
                return;
            }

            //Play a decision sound
            Audio.SE.Play(Data.Misc.decisionSe);
            //And switch scenes
            Globals.Scene = new Scenes.Load();
        }

        void CommandShutDown()
        {
            //Play decision sound
            Audio.SE.Play(Data.Misc.decisionSe);

            //Fade everything out
            Audio.BGM.Fade(30);
            Graphics.Transition(60);

            Globals.Scene = null;
        }

        /// <summary>
        /// Updates the scene. This is called every frame.
        /// </summary>
        public override void Update()
        {
            //Update the background and command window
            sprite.Update();
            commandWindow.Update();

            //If the action key is pressed...
            if (Input.Triggered(Keys.C))
            {
                //Depending on the index of the commandWindow, the appropriate command is executed
                switch (commandWindow.Index)
                {
                    case 0: CommandNewGame(); break;
                    case 1: CommandContinue(); break;
                    case 2: CommandShutDown(); break;
                }
            }
        }
    }
}
