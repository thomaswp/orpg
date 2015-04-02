using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Game_Player
{
    /// <summary>
    /// Possible Keystates to which to compare an Input.GetState() call.
    /// </summary>
    public enum KeyStates 
    { 
        /// <summary>
        /// Indicated that the key has been up for more than one frame.
        /// </summary>
        Lifted, 

        /// <summary>
        /// Indicates that the key has been down for more than one frame.
        /// </summary>
        Held, 

        /// <summary>
        /// Indicated that the hey was just pressed down.
        /// </summary>
        Triggered, 

        /// <summary>
        /// Indicated that the key was just lifted up.
        /// </summary>
        Released
    }

    /// <summary>
    /// Possible keys that are checked for input. These keys correspond to real keyboard keys and can be assigned.
    /// </summary>
    public enum Keys 
    { 
        //probably assigning the wrong numbers here, and yes
        //it matters because of interpreter.inputbutton
        /// <summary>
        /// An assignable key.
        /// </summary>
        Up = 1,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Down = 2,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Right = 3,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Left = 4,
        /// <summary>
        /// An assignable key.
        /// </summary>
        A = 5,
        /// <summary>
        /// An assignable key.
        /// </summary>
        B = 6,
        /// <summary>
        /// An assignable key.
        /// </summary>
        C = 7,
        /// <summary>
        /// An assignable key.
        /// </summary>
        X = 8,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Y = 9,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Z = 10,
        /// <summary>
        /// An assignable key.
        /// </summary>
        R = 11,
        /// <summary>
        /// An assignable key.
        /// </summary>
        L = 12,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Shift = 13,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Ctrl = 14,
        /// <summary>
        /// An assignable key.
        /// </summary>
        Alt = 15,
        /// <summary>
        /// An assignable key.
        /// </summary>
        F5 = 16,
        /// <summary>
        /// An assignable key.
        /// </summary>
        F6= 17,
        /// <summary>
        /// An assignable key.
        /// </summary>
        F7 = 18,
        /// <summary>
        /// An assignable key.
        /// </summary>
        F8 = 19,
        /// <summary>
        /// An assignable key.
        /// </summary>
        F9 = 20
    }

    /// <summary>
    /// A class that handles input from the keyboard.
    /// </summary>
    public class Input
    {
        const int NUM_OF_KEYS = 20;
        const int KEYHOLDTIME = 20;
        const int KEYCHECK = 4;

        private static int[] held = new int[NUM_OF_KEYS];

        private static int dir = 0;
        private static int primaryDir = 0;

        static Microsoft.Xna.Framework.Input.Keys[][] keys = new Microsoft.Xna.Framework.Input.Keys[NUM_OF_KEYS][];
        static KeyStates[] states = new KeyStates[NUM_OF_KEYS];
        static KeyStates[] oldStates = new KeyStates[NUM_OF_KEYS];

        static Input()
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
                new string[] {"W"},
                new string[] {"Q"},
                new string[] {"RightShift", "LeftShift"},
                new string[] {"RightControl", "LeftControl"},
                new string[] {"RightAlt", "LeftAlt"},
                new string[] {"F5"},
                new string[] {"F6"},
                new string[] {"F7"},
                new string[] {"F8"},
                new string[] {"F9"}};

            GetKeys(keys);
        }

        /// <summary>
        /// Updates the Input Class. Generally this is done through <c>Globals.GameSystem.Update()</c>,
        /// and does not need to be explicitly called.
        /// </summary>
        public static void Update()
        {
            Boolean pressed;
            for (int i = 0; i < NUM_OF_KEYS; i++)
            {
                pressed = false;
                for (int j = 0; j < keys[i].Length; j++)
                {
                    if (Keyboard.GetState().IsKeyDown(keys[i][j]))
                        pressed = true;
                }
                if (pressed)
                {
                    if (states[i] == KeyStates.Lifted || states[i] == KeyStates.Released)
                        states[i] = KeyStates.Triggered;
                    else
                        states[i] = KeyStates.Held;
                }
                else
                {
                    if (states[i] == KeyStates.Triggered || states[i] == KeyStates.Held)
                        states[i] = KeyStates.Released;
                    else
                        states[i] = KeyStates.Lifted;
                }
            }
            
            for (int i = 0; i < oldStates.Length; i++)
            {
                if (oldStates[i] == states[i] && states[i] == KeyStates.Held)
                    held[i]++;
                else
                    held[i] = 0;
            }

            Keys[] dirs = new Keys[9];
            dirs[2] = Keys.Down;
            dirs[4] = Keys.Left;
            dirs[6] = Keys.Right;
            dirs[8] = Keys.Up;
            
            for (int i = 2; i <= 8; i += 2)
            {
                if (Triggered(dirs[i]))
                    primaryDir = i;
            }

            if (primaryDir != 0 && (Held(dirs[primaryDir]) || Triggered(dirs[primaryDir])))
                dir = primaryDir;
            else
            {
                primaryDir = 0;
                dir = 0;
                for (int i = 2; i <= 8; i += 2)
                {
                    if (Held(dirs[i]))
                        dir = i;
                }
            }

            oldStates = states;
        }

        public static KeyStates State(int key)
        {
            return states[key - 1];
        }
        /// <summary>
        /// Gets the state of a given key.
        /// </summary>
        /// <param name="key">The key being checked.</param>
        /// <returns>The <see cref="F:Game_Player.KeyStates">KeyStates</see>
        /// of the given key.</returns>
        public static KeyStates State(Keys key)
        {       
            return states[(int)key - 1];
        }

        public static int HoldLength(int key)
        {
            return held[key - 1];
        }
        /// <summary>
        /// The length a given key has been held down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>The length of held.</returns>
        public static int HoldLength(Keys key)
        {
            return held[(int)key - 1];
        }

        /// <summary>
        /// Returns an integer (2, 4, 6, 8 or 0) depending on which of the Up, Down, Right and 
        /// Left keys are being held down. The number corresponds to the numberpad direction
        /// on a standard keyboard: 2 = Down, 4 = Left, 6 = Right, 8 = Up, and 0 indicated no
        /// directional key is being pressed. Use this method to control sprite movement in 4
        /// directions, etc. Note that this method will default to the lowest numbered direction
        /// being held and will not indicate if other directional keys are being held.
        /// </summary>
        /// <returns>The directional integer.</returns>
        public static int Dir4()
        {
            return dir;
        }

        public static bool Held(int key)
        {
            return State(key) == KeyStates.Held;
        }
        /// <summary>
        /// Indicates if the given key is in the Held state. This includes all time after the key is
        /// pressed and continues to be pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Held;"
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Held(Keys key)
        { 
            return State(key) == KeyStates.Held; 
        }

        public static Boolean Triggered(int key)
        {
            return State(key) == KeyStates.Triggered;
        }
        /// <summary>
        /// Indicates if the given key is in the Triggered state. This includes only the
        /// initial frame the key is pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Triggered;"
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Triggered(Keys key)
        { 
            return State(key) == KeyStates.Triggered; 
        }

        public static Boolean Repeated(int key)
        {
            int hold = HoldLength(key);
            return (hold % KEYCHECK == 0) && (hold > KEYHOLDTIME) || Triggered(key);
        }
        /// <summary>
        /// This method is used to trigger an event while a key is being held. Instead of simply 
        /// returning whether the key is being held or not, (~60 times a second) it will return 
        /// true once upon triggering, then after waiting 20 frames, once every 10 frames (~6
        /// time a second).
        /// Use this method for scrolling, etc. Imagine a cursor in a text document.
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns>The indicating boolean.</returns>
        public static Boolean Repeated(Keys key)
        {           
            int hold = HoldLength(key);
            return (hold % KEYCHECK == 0) && (hold > KEYHOLDTIME) || Triggered(key);
        }

        /// <summary>
        /// This method loads a set of keybaord keys to associate with its collection
        /// of Keys. from the Data class. This class is called by Game.System.GameStart()
        /// and should not be called unless a manual reassignment of keys is needed.
        /// </summary>
        public static void GetKeys(string[][] getKeys)
        {
            Microsoft.Xna.Framework.Input.Keys k = Microsoft.Xna.Framework.Input.Keys.None;
            for (int j = 0; j < NUM_OF_KEYS; j++)
            {
                keys[j] = new Microsoft.Xna.Framework.Input.Keys[] { };
                for (int i = 0; i <= 200; i++)
                {
                    if (Array.IndexOf(getKeys[j], (k + i).ToString()) != -1)
                    {
                        Array.Resize<Microsoft.Xna.Framework.Input.Keys>(ref keys[j], keys[j].Length + 1);
                        keys[j][keys[j].Length - 1] = (k + i);
                    }
                }
            }
        }
    }
}
