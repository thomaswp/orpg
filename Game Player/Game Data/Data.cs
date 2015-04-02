using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DataClasses;

namespace Game_Player
{
    /// <summary>
    /// The data class, used for accessing all the data sub-classes.
    /// It handles the loading and saving of data.
    /// </summary>
    public static class Data
    {
        static DataArray<Actor> _actors = new DataArray<Actor>(new Actor());
        public static DataArray<Actor> Actors { get { return _actors; } set { _actors = value; } }

        static DataArray<Animation> _animations = new DataArray<Animation>(new Animation() );
        public static DataArray<Animation> Animations { get { return _animations; } set { _animations = value; } }

        static DataArray<Armor> _armors = new DataArray<Armor>(new Armor());
        public static DataArray<Armor> Armors { get { return _armors; } set { _armors = value; } } 

        static DataArray<Class> _classes = new DataArray<Class>(new Class());
        public static DataArray<Class> Classes { get { return _classes; } set { _classes = value; } }

        static DataArray<CommonEvent> _commonEvents = new DataArray<CommonEvent>(new CommonEvent());
        public static DataArray<CommonEvent> CommonEvents { get { return _commonEvents; } set { _commonEvents = value; } }

        static DataArray<Enemy> _enemies = new DataArray<Enemy>(new Enemy());
        public static DataArray<Enemy> Enemies { get { return _enemies; } set { _enemies = value; } }

        static DataArray<Item> _items = new DataArray<Item>(new Item());
        public static DataArray<Item> Items { get { return _items; } set { _items = value; } }

        static DataArray<Map> _maps = new DataArray<Map>(new Map(1, 1));
        public static DataArray<Map> Maps { get { return _maps; } set { _maps = value; } }

        static DataArray<Skill> _skills = new DataArray<Skill>(new Skill());
        public static DataArray<Skill> Skills { get { return _skills; } set { _skills = value; } }

        static DataArray<State> _states = new DataArray<State>(new State());
        public static DataArray<State> States { get { return _states; } set { _states = value; } }

        static DataArray<Tileset> _tilesets = new DataArray<Tileset>(new Tileset());
        public static DataArray<Tileset> Tilesets { get { return _tilesets; } set { _tilesets = value; } }

        static DataArray<Troop> _troops = new DataArray<Troop>(new Troop());
        public static DataArray<Troop> Troops { get { return _troops; } set { _troops = value; } }

        static DataArray<Weapon> _weapons = new DataArray<Weapon>(new Weapon());
        public static DataArray<Weapon> Weapons { get { return _weapons; } set { _weapons = value; } }

        static Misc _misc = new Misc();
        public static Misc Misc { get { return _misc; } set { _misc = value; } }

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
                Stream stream;

                stream = File.Open(dir + "Actors.orpgdata", FileMode.Open);
                _actors = (DataArray<Actor>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Animations.orpgdata", FileMode.Open);
                _animations = (DataArray<Animation>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Armors.orpgdata", FileMode.Open);
                _armors = (DataArray<Armor>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Classes.orpgdata", FileMode.Open);
                _classes = (DataArray<Class>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "CommonEvents.orpgdata", FileMode.Open);
                _commonEvents = (DataArray<CommonEvent>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Enemies.orpgdata", FileMode.Open);
                _enemies = (DataArray<Enemy>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Items.orpgdata", FileMode.Open);
                _items = (DataArray<Item>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Maps.orpgdata", FileMode.Open);
                _maps = (DataArray<Map>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Misc.orpgdata", FileMode.Open);
                _misc = (Misc)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Skills.orpgdata", FileMode.Open);
                _skills = (DataArray<Skill>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "States.orpgdata", FileMode.Open);
                _states = (DataArray<State>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Tilesets.orpgdata", FileMode.Open);
                _tilesets = (DataArray<Tileset>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Troops.orpgdata", FileMode.Open);
                _troops = (DataArray<Troop>)bf.Deserialize(stream);
                stream.Close();
                stream = File.Open(dir + "Weapons.orpgdata", FileMode.Open);
                _weapons = (DataArray<Weapon>)bf.Deserialize(stream);
                stream.Close();
            }
            catch
            { }

            //while Misc is under construction, we need to load the defaults instead of from a file
            //Misc.Load();
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
                Stream stream;

                stream = File.Open(dir + "Actors.orpgdata", FileMode.Create);
                bf.Serialize(stream, _actors);
                stream.Close();
                stream = File.Open(dir + "Animations.orpgdata", FileMode.Create);
                bf.Serialize(stream, _animations);
                stream.Close();
                stream = File.Open(dir + "Armors.orpgdata", FileMode.Create);
                bf.Serialize(stream, _armors);
                stream.Close();
                stream = File.Open(dir + "Classes.orpgdata", FileMode.Create);
                bf.Serialize(stream, _classes);
                stream.Close();
                stream = File.Open(dir + "CommonEvents.orpgdata", FileMode.Create);
                bf.Serialize(stream, _commonEvents);
                stream.Close();
                stream = File.Open(dir + "Enemies.orpgdata", FileMode.Create);
                bf.Serialize(stream, _enemies);
                stream.Close();
                stream = File.Open(dir + "Items.orpgdata", FileMode.Create);
                bf.Serialize(stream, _items);
                stream.Close();
                stream = File.Open(dir + "Maps.orpgdata", FileMode.Create);
                bf.Serialize(stream, _maps);
                stream.Close();
                stream = File.Open(dir + "Misc.orpgdata", FileMode.Create);
                bf.Serialize(stream, _misc);
                stream.Close();
                stream = File.Open(dir + "Skills.orpgdata", FileMode.Create);
                bf.Serialize(stream, _skills);
                stream.Close();
                stream = File.Open(dir + "States.orpgdata", FileMode.Create);
                bf.Serialize(stream, _states);
                stream.Close();
                stream = File.Open(dir + "Tilesets.orpgdata", FileMode.Create);
                bf.Serialize(stream, _tilesets);
                stream.Close();
                stream = File.Open(dir + "Troops.orpgdata", FileMode.Create);
                bf.Serialize(stream, _troops);
                stream.Close();
                stream = File.Open(dir + "Weapons.orpgdata", FileMode.Create);
                bf.Serialize(stream, _weapons);
                stream.Close();

                //May want to add code to edit the prefs.txt file.
            }
            catch {}
        }
    }
}
