using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Scenes
{
    public class Ending : Scene
    {
        private Windows.Command commandWindow;

        public Ending()
        {
            Graphics.Transition();

            String s1 = "To Title";
            String s2 = "Shutdown";
            String s3 = "Cancel";

            commandWindow = new Windows.Command(192, new string[] { s1, s2, s3 });
            commandWindow.X = 320 - commandWindow.Width / 2;
            commandWindow.Y = 240 - commandWindow.Height / 2;
        }

        public override void End() 
        {
            commandWindow.Dispose();

            if (Globals.Scene is Scenes.Title)
                Graphics.Transition();
        }

        public override void Update() 
        {
            commandWindow.Update();

            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                Globals.Scene = new Scenes.Menu(5);
                return;
            }

            if (Input.Triggered(Keys.C))
            {
                switch (commandWindow.Index)
                {
                    case 0:
                        CommandToTitle();
                        break;
                    case 1:
                        CommandShutDown();
                        break;
                    case 2:
                        CommandCancel();
                        break;
                }
                return;
            }
        }

        public void CommandToTitle()
        {
            Audio.SE.Play(Data.Misc.decisionSe);

            Audio.BGM.Fade(800);
            Audio.BGS.Fade(800);
            Audio.ME.Fade(800);

            Globals.Scene = new Scenes.Title();
        }

        public void CommandShutDown()
        {
            Audio.SE.Play(Data.Misc.decisionSe);

            Audio.BGM.Fade(800);
            Audio.BGS.Fade(800);
            Audio.ME.Fade(800);

            Globals.Scene = null;
        }

        public void CommandCancel()
        {
            Audio.SE.Play(Data.Misc.decisionSe);

            Globals.Scene = new Scenes.Menu(5);
        }
    }
}
