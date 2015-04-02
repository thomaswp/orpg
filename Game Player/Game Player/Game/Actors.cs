using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    /// <summary>
    /// Contains a list of all <see cref="T:Game.Actor">Game.Actors</see>
    /// and their current conditions. This varies from <see cref="T:DataClasses.Actors">
    /// Data.Actors</see> in that its data is an instance and changes during gameplay.
    /// </summary>
    public class Actors
    {
        Actor[] data = new Actor[1000];

        public Actor this[int actorId]
        {
            get
            {
                if (actorId >= data.Length || Data.Actors[actorId] == null)
                    return null;
                if (data[actorId] == null)
                    data[actorId] = new Actor(actorId);
                return data[actorId];
            }
        }
    }
}
