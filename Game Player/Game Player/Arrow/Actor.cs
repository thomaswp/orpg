using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Arrow
{
    public class Actor : Base
    {
        #region properties

        protected Game.Actor actor;
        public Game.Actor GetActor
        { 
            get 
            { 
                return Globals.GameParty.Actors[index]; 
            } 
        }

        #endregion

        public Actor(Viewport viewport) : base(viewport) { }

        public override void Update()
        {
            base.Update();

            if (Input.Repeated(Keys.Right))
            {
                Audio.SE.Play(Data.Misc.cursorSe);
                for (int i = 0; i < Globals.GameParty.Actors.Length; i++)
                {
                    index++;
                    index %= Globals.GameParty.Actors.Length;
                }
            }
            
            if (Input.Repeated(Keys.Left))
            {
                Audio.SE.Play(Data.Misc.cursorSe);
                for (int i = 0; i < Globals.GameParty.Actors.Length; i++)
                {
                    index += Globals.GameParty.Actors.Length - 1;
                    index %= Globals.GameParty.Actors.Length;
                }
            }

            if (this.actor != null)
            {
                this.X = this.actor.ScreenX;
                this.Y = this.actor.ScreenY;
            }
        }

        public override void UpdateHelp()
        {
            helpWindow.SetActor(actor);
        }
    }
}
