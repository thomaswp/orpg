using System;
using System.Collections.Generic;
using System.Text;

namespace DataClasses
{
    /// <summary>
    /// A data class containing a list of user-created <see cref="T:Game_Data.Actor">Actors</see>.
    /// </summary>
    [Serializable()]
    public class Actor : ICloneable
    {
        /// <summary>
        /// The actor's ID.
        /// </summary>
        public int id;
        /// <summary>
        /// The actor's name.
        /// </summary>
        public string name;
        /// <summary>
        /// The ID of the actor's class.
        /// </summary>
        public int classId;
        /// <summary>
        /// The actor's starting level.
        /// </summary>
        public int initialLevel;
        /// <summary>
        /// The actor's ending level.
        /// </summary>
        public int finalLevel;
        /// <summary>
        /// A value used in determining the exp curve of the Actor.
        /// </summary>
        public int expBasis;
        /// <summary>
        /// A value used in determining the exp curve of the Actor.
        /// </summary>
        public int expInflation;
        /// <summary>
        /// A string pointing to the actor's character sprite.
        /// </summary>
        public string characterName;
        /// <summary>
        /// The hue (0-360) of the actor's character sprite.
        /// </summary>
        public int characterHue;
        /// <summary>
        /// A string pointing to the actor's battler sprite.
        /// </summary>
        public string battlerName;
        /// <summary>
        /// The hue (0-360) of the actor's battler sprite.
        /// </summary>
        public int battlerHue;
        /// <summary>
        /// A 99x6 2-D array containing the HP, SP, STR, DEX, AGI, and INT of the character
        /// respectively for levels 1-99 respectively.
        /// </summary>
        public int[,] parameters;
        /// <summary>
        /// The ID of the starting weapon of the actor.
        /// </summary>
        public int weaponId;
        /// <summary>
        /// The ID of the starting shield of the actor.
        /// </summary>
        public int armor1Id;
        /// <summary>
        /// The ID of the starting helmet of the actor.
        /// </summary>
        public int armor2Id;
        /// <summary>
        /// The ID of the starting armor of the actor.
        /// </summary>
        public int armor3Id;
        /// <summary>
        /// The ID of the starting accessory of the actor.
        /// </summary>
        public int armor4Id;
        /// <summary>
        /// Tells if the actor's weapon is locked and cannot be changed.
        /// </summary>
        public bool weaponFix;
        /// <summary>
        /// Tells if the actor's shield is locked and cannot be changed.
        /// </summary>
        public bool armor1Fix;
        /// <summary>
        /// Tells if the actor's helmet is locked and cannot be changed.
        /// </summary>
        public bool armor2Fix;
        /// <summary>
        /// Tells if the actor's armor is locked and cannot be changed.
        /// </summary>
        public bool armor3Fix;
        /// <summary>
        /// Tells if the actor's accessory is locked and cannot be changed.
        /// </summary>
        public bool armor4Fix;

        /// <summary>
        /// Initializes the acotor.
        /// </summary>
        public Actor()
        {
            //initialize
            id = 0;
            name = "";
            classId = 1;
            initialLevel = 1;
            finalLevel = 99;
            expBasis = 30;
            expInflation = 30;
            characterName = "";
            characterHue = 0;
            battlerName = "";
            battlerHue = 0;
            //fill out the param's as the original does
            parameters = new int[6,100];
            for (int i = 1; i <= 99; i++)
            {
                parameters[0,i] = 500+i*50;
                parameters[1,i] = 500+i*50;
                parameters[2,i] = 50+i*5;
                parameters[3,i] = 50+i*5;
                parameters[4,i] = 50+i*5;
                parameters[5,i] = 50+i*5;
            }
            weaponId = 0;
            armor1Id = 0;
            armor2Id = 0;
            armor3Id = 0;
            armor4Id = 0;
            weaponFix = false;
            armor1Fix = false;
            armor2Fix = false;
            armor3Fix = false;
            armor4Fix = false;
        }

        /// <summary>
        /// Creates and returns a clone of this class.
        /// This is used by the ORPG program to edit data safely.
        /// </summary>
        /// <returns>A clone of this class.</returns>
        public object Clone()
        {
            Actor a = (Actor)this.MemberwiseClone();
            a.parameters = (int[,])this.parameters.Clone();
            return a;
        }
    }
}
