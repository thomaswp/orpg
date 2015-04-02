using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Arrow
{
    public class Enemy : Base
    {
        #region properties

        protected Game.Enemy enemy;
        public Game.Enemy GetEnemy 
        { 
            get 
            { 
                return Globals.GameTroop.Enemies[index]; 
            } 
        }

        #endregion

        public Enemy(Viewport viewport) : base(viewport) { }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < Globals.GameTroop.Enemies.Length; i++ )
            {
                if (this.enemy.Exists)
                    break;

                index++;
                index %= Globals.GameTroop.Enemies.Length;
            }

            if (Input.Repeated(Keys.Right))
            {
                Audio.SE.Play(Data.Misc.cursorSe);
                for (int i = 0; i < Globals.GameTroop.Enemies.Length; i++)
                {
                    index++;
                    index %= Globals.GameTroop.Enemies.Length;
                    if (this.enemy.Exists)
                        break;
                }
            }
            
            if (Input.Repeated(Keys.Left))
            {
                Audio.SE.Play(Data.Misc.cursorSe);
                for (int i = 0; i < Globals.GameTroop.Enemies.Length; i++)
                {
                    index += Globals.GameTroop.Enemies.Length - 1;
                    index %= Globals.GameTroop.Enemies.Length;
                    if (this.enemy.Exists)
                        break;
                }
            }

            if (this.enemy != null)
            {
                this.X = this.enemy.ScreenX;
                this.Y = this.enemy.ScreenY;
            }
        }

        public override void UpdateHelp()
        {
            helpWindow.SetEnemy(enemy);
        }
    }
}
