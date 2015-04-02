using System;
using System.Collections.Generic;
using System.Text;
using Game_Player.DataClasses;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game_Player
{
    /// <summary>
    /// The data class, used for accessing all the data sub-classes.
    /// It handles the loading and saving of data.
    /// </summary>
    public static class Data
    {
        static DataArray<Actor> _actors = new DataArray<Actor>(new Actor());
        public static DataArray<Actor> Actors { get { return _actors; } }

        static DataArray<Animation> _animations = new DataArray<Animation>(new Animation() );
        public static DataArray<Animation> Animations { get { return _animations; } }

        static DataArray<Class> _classes = new DataArray<Class>(new Class());
        public static DataArray<Class> Classes { get { return _classes; } }

        static DataArray<CommonEvent> _commonEvents = new DataArray<CommonEvent>(new CommonEvent());
        public static DataArray<CommonEvent> CommonEvents { get { return _commonEvents; } }

        static DataArray<Enemy> _enemies = new DataArray<Enemy>(new Enemy());
        public static DataArray<Enemy> Enemies { get { return _enemies; } }

        static DataArray<Item> _items = new DataArray<Item>(new Item());
        public static DataArray<Item> Items { get { return _items; } }

        static DataArray<Skill> _skills = new DataArray<Skill>(new Skill());
        public static DataArray<Skill> Skills { get { return _skills; } }

        static DataArray<State> _states = new DataArray<State>(new State());
        public static DataArray<State> States { get { return _states; } }

        static DataArray<Tileset> _tilesets = new DataArray<Tileset>(new Tileset());
        public static DataArray<Tileset> Tilesets { get { return _tilesets; } }

        static DataArray<Troop> _troops = new DataArray<Troop>(new Troop());
        public static DataArray<Troop> Troops { get { return _troops; } }

        static DataArray<Weapon> _weapons = new DataArray<Weapon>(new Weapon());
        public static DataArray<Weapon> Weapons { get { return _weapons; } }

        static Misc _misc = new Misc();
        public static Misc Misc { get { return _misc; } }

        /// <summary>
        /// The location of the RTP database installation.
        /// </summary>
        public const string RTP = @"C:\Program Files\Common Files\Enterbrain\RGSS\Standard\";

        /// <summary>
        /// A temporary method to define the key layout.
        /// This method is called by the Input class.
        /// </summary>
        /// <returns>an array of arrays of strings, each set corresponding to a key.</returns>
        public static string[][] GetKeys()
        {
            string[][] keys = new string[20][] {
                new string[] {"Up"},
                new string[] {"Down"},
                new string[] {"Right"},
                new string[] {"Left"},
                new string[] {"Z"},
                new string[] {"Escape", "Num0"},
                new string[] {"Space", "Enter"},
                new string[] {"A"},
                new string[] {"S"},
                new string[] {"D"},
                new string[] {"Q"},
                new string[] {"W"},
                new string[] {"RightShift", "LeftShift"},
                new string[] {"RightControl", "LeftControl"},
                new string[] {"RightAlt", "LeftAlt"},
                new string[] {"F5"},
                new string[] {"F6"},
                new string[] {"F7"},
                new string[] {"F8"},
                new string[] {"F9"}};
            return keys;
        }

        /// <summary>
        /// The dimension of the screen.
        /// </summary>
        /// <returns></returns>
        public static Rect GetScreen()
        {
            return new Rect(800, 600);
        }

        /// <summary>
        /// Loads data from the corresponding files.
        /// </summary>
        /// <param name="dir">The Data directory to load from.</param>
        public static void Load(String dir)
        {
            //create a binary formatter to serialize
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                //try to serialize each data class
                Stream stream = File.Open(dir + "Actors.orpgdata", FileMode.Open);

                _actors = (DataArray<Actor>)bf.Deserialize(stream);

                //close the stream when done
                stream.Close();
            }
            catch 
            { }

            //while Misc is under construction, we need to load the defaults instead of from a file
            Misc.Load();
        }

        /// <summary>
        /// Saves the data files to a directory.
        /// </summary>
        /// <param name="dir">The Data directory to save to.</param>
        public static void Save(String dir) 
        {
            //create a binary formatter to deserialize the class
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                //try to deserialize each data class
                Stream stream = File.Open(dir + "Actors.orpgdata", FileMode.Create);

                bf.Serialize(stream, _actors);

                //close the stream
                stream.Close();
            }
            catch { }
        }
    }
}
