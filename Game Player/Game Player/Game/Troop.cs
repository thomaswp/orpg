using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Troop
    {
        protected Enemy[] enemies;
        public Enemy[] Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public Troop()
        {
            enemies = new Enemy[] { };
        }

        public void Setup(int troopId)
        {
            enemies = new Enemy[] { };
            DataClasses.Troop troop = Data.Troops[troopId];
            for (int i = 0; i < troop.members.Length; i++)
            {
                DataClasses.Enemy enemy = Data.Enemies[troop.members[i].enemyId];
                if (enemies != null)
                    enemies = enemies.Plus<Enemy>(new Enemy(troopId, i));
            }
        }

        public Enemy RandomTargetEnemy() { return RandomTargetEnemy(false);}
        public Enemy RandomTargetEnemy(bool hp0)
        {
            Enemy[] roulette = new Enemy[] { };

            foreach(Enemy enemy in enemies)
                if ((!hp0 && enemy.Exists) || (hp0 && enemy.IsHp0))
                    roulette = roulette.Plus<Enemy>(enemy);

            if (roulette.Length == 0)
                return null;

            return roulette[Rand.Next(roulette.Length)];

        }

        public Enemy RandomTargetEnemyHp0()
        {
            return RandomTargetEnemy(true);
        }

        public Enemy SmoothTargetEnemy(int enemyIndex)
        {
            Enemy enemy = enemies[enemyIndex];

            if (enemyIndex != -1 && enemy.Exists) //changed from "!= null" to "!= -1"
                return enemy;

            foreach (Enemy e in enemies)
                if (e.Exists)
                    return e;

            return null;
        }
    }
}
