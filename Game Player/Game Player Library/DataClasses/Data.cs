using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Game_Player.DataClasses;

namespace Game_Player
{
    public class Data
    {
        Actors _actors = new Actors();
        public Actors Actors { get { return _actors; } }

        Animations _animations = new Animations();
        public Animations Animations { get { return _animations; } }

        Classes _classes = new Classes();
        public Classes Classes { get { return _classes; } }

        CommonEvents _commonEvents = new CommonEvents();
        public CommonEvents CommonEvents { get { return _commonEvents; } }

        Enemies _enemies = new Enemies();
        public Enemies Enemies { get { return _enemies; } }

        Items _items = new Items();
        public Items Items { get { return _items; } }

        Misc _misc = new Misc();
        public Misc Misc { get { return _misc; } }

        Skills _skills = new Skills();
        public Skills Skills { get { return _skills; } }

        States _states = new States();
        public States States { get { return _states; } }

        Tilesets _tilesets = new Tilesets();
        public Tilesets Tilesets { get { return _tilesets; } }

        Troops _troops = new Troops();
        public Troops Troops { get { return _troops; } }

        Weapons _weapons = new Weapons();
        public Weapons Weapons { get { return _weapons; } }

        public const string RTP = "C:\\Program Files\\Common Files\\Enterbrain\\RGSS\\Standard\\";

        public Data()
        {

        }

        public string[][] GetKeys()
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

        public Rect GetScreen()
        {
            return new Rect(800, 600);
        }
    }
}
