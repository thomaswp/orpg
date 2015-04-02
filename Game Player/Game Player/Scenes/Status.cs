using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Scenes
{
    public class Status : Scene
    {
        protected int actorIndex;
        protected Game.Actor actor;
        protected Windows.Status statusWindow;

        public Status() : this(0) { }
        public Status(int actorIndex)
        {
            Graphics.Transition();

            this.actorIndex = actorIndex;
            actor = Globals.GameParty.Actors[actorIndex];
            statusWindow = new Game_Player.Windows.Status(actor);
        }
        
        public override void End() 
        {
            statusWindow.Dispose();
        }

        public override void Update() 
        {
            if (Input.Triggered(Keys.B))
            {
                Audio.SE.Play(Data.Misc.cancelSe);
                Globals.Scene = new Scenes.Menu(3);
                return;
            }

            if (Input.Triggered(Keys.R) || Input.Triggered(Keys.L)) //changed for efficiency
            {
                Audio.SE.Play(Data.Misc.cursorSe);
                actorIndex += Input.Triggered(Keys.R) ? 1 : Globals.GameParty.Actors.Length - 1;
                actorIndex %= Globals.GameParty.Actors.Length;

                Globals.Scene = new Scenes.Status(actorIndex);
                return;
            }
        }

    }
}
