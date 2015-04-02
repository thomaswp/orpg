using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.DataClasses
{
    /// <summary>
    /// A data class containing a list of user-created actors.
    /// </summary>
    [Serializable()]
    public class Actors
    {
        Actor[] actors = new Actor[0];

        /// <summary>
        /// Returns the number of actors contained in list.
        /// </summary>
        public int Length
        {
            get { return actors.Length; }
            set { Resize(value); }
        }

        /// <summary>
        /// Gets the <see cref="T:Game_Data.Actor">Game_Data.Actor</see> at the given index in the list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The actor.</returns>
        public Actor this[int index]
        {
            get 
            {
                if (index + 1 > actors.Length || index < 0)
                    return null;
                return actors[index]; 
            }
            set 
            {
                if (index + 1 > actors.Length || index < 0)
                    return;
                actors[index] = value; 
            }
        }

        /// <summary>
        /// Resizes the number of <see cref="T:Game_Data.Actor">Game_Data.Actor</see>'s in the list.
        /// </summary>
        /// <param name="size">The new size.</param>
        public void Resize(int size)
        {
            Array.Resize<Actor>(ref actors, size);
        }

        /// <summary>
        /// Adds a <see cref="T:Game_Data.Actor">Game_Data.Actor</see> to the list.
        /// </summary>
        /// <param name="actor"></param>
        public void Add(Actor actor)
        {
            this.Length++;
            this[Length - 1] = actor;
        }

        /// <summary>
        /// A method used so that foreach can enumerate through the list..
        /// </summary>
        /// <returns>The enumerator</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// A class used to aid the enumerator
        /// </summary>
        public class Enumerator
        {
            int nIndex;
            Actors actors;
            /// <summary>
            /// Initializes the enumerator.
            /// </summary>
            /// <param name="actors">The actors through which to enumerate.</param>
            public Enumerator(Actors actors)
            {
                this.actors = actors;
                nIndex = -1;
            }

            /// <summary>
            /// Enumerates once.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                nIndex++;
                return (nIndex < actors.actors.Length);
            }

            /// <summary>
            /// Returns the current actor being enumerated.
            /// </summary>
            public Actor Current
            {
                get
                {
                    return (actors.actors[nIndex]);
                }
            }
        }
    }
}
