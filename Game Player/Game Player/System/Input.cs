using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Game_Player
{
    /// <summary>
    /// Possible Keystates to which to compare an Input.GetState() call.
    /// </summary>
    public enum KeyStates { Lifted, Held, Triggered, Released}
    /// <summary>
    /// Possible keys that are checked for input. These keys correspond to real keyboard keys and can be assigned.
    /// </summary>
    public enum Keys { Up, Down, Right, Left, A, B, C, X, Y, Z, R, L, Shift, Ctrl, Alt, F5, F6, F7, F8, F9 }

    /// <summary>
    /// A class that handles input from the keyboard.
    /// </summary>
    public class Input
    {
        const int NUM_OF_KEYS = 20;
        const int KEYHOLDTIME = 20;
        const int KEYCHECK = 4;

        int[] held = new int[NUM_OF_KEYS];

        Microsoft.Xna.Framework.Input.Keys[][] keys = new Microsoft.Xna.Framework.Input.Keys[NUM_OF_KEYS][];
        KeyStates[] states = new KeyStates[NUM_OF_KEYS];
        KeyStates[] oldStates = new KeyStates[NUM_OF_KEYS];

        /// <summary>
        /// Updates the Input Class. Generally this is done through Globals.GameSystem.Update(),
        /// and does not need to be explicitly called.
        /// </summary>
        public void Update()
        {
            Boolean pressed;
            for (int i = 0; i < NUM_OF_KEYS; i++)
            {
                pressed = false;
                for (int j = 0; j < keys[i].Length; j++)
                {
                    if (Keyboard.GetState().IsKeyDown(keys[i][j]))
                    { pressed = true; }
                }
                if (pressed)
                {
                    if (states[i] == KeyStates.Lifted || states[i] == KeyStates.Released)
                    { states[i] = KeyStates.Triggered; }
                    else
                    { states[i] = KeyStates.Held; }
                }
                else
                {
                    if (states[i] == KeyStates.Triggered || states[i] == KeyStates.Held)
                    { states[i] = KeyStates.Released; }
                    else
                    { states[i] = KeyStates.Lifted; }
                }
            }
            for (int i = 0; i < oldStates.Length; i++)
            {
                if (oldStates[i] == states[i] & states[i] == KeyStates.Held)
                { held[i]++; }
                else
                { held[i] = 0; }
            }
            oldStates = states;
        }

        /// <summary>
        /// Gets the state of a given key.
        /// </summary>
        /// <param name="key">The key being checked.</param>
        /// <returns></returns>
        public KeyStates State(Keys key)
        {       
            return states[(int)key];
        }

        /// <summary>
        /// The length a given key has been held down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public int HoldLength(Keys key)
        {
            return held[(int)key];
        }

        /// <summary>
        /// Returns an integer (2, 4, 6, 8 or 0) depending on which of the Up, Down, Right and 
        /// Left keys are being held down. The number corresponds to the numberpad direction
        /// on a standard keyboard: 2 = Down, 4 = Left, 6 = Right, 8 = Up, and 0 indicated no
        /// directional key is being pressed. Use this method to control sprite movement in 4
        /// directions, etc. Note that this method will default to the lowest numbered direction
        /// being held and will not indicate if other directional keys are being held.
        /// </summary>
        /// <returns></returns>
        public int Dir4()
        {
            if (State(Keys.Down) == KeyStates.Held )
            { return 2; }
            if (State(Keys.Left) == KeyStates.Held )
            { return 4; }
            if (State(Keys.Right) == KeyStates.Held )
            { return 6; }
            if (State(Keys.Up) == KeyStates.Held )
            { return 8; }
            return 0;
        }

        /// <summary>
        /// Indicates if the given key is in the Held state. This includes all time after the key is
        /// pressed and continues to be pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Held;"
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns></returns>
        public Boolean Held(Keys key)
        { return State(key) == KeyStates.Held; }

        /// <summary>
        /// Indicates if the given key is in the Triggered state. This includes only the
        /// initial frame the key is pressed.
        /// Note, this is a shortcut method equivalent to: "State(key) == KeyStates.Triggered;"
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public Boolean Triggered(Keys key)
        { return State(key) == KeyStates.Triggered; }

        /// <summary>
        /// This method is used to trigger an event while a key is being held. Instead of simply 
        /// returning whether the key is being held or not, (~60 times a second) it will return 
        /// true once upon triggering, then after waiting 20 frames, once every 10 frames (~6
        /// time a second).
        /// Use this method for scrolling, etc. Imagine a cursor in a text document.
        /// </summary>
        /// <param name="key">They key to check.</param>
        /// <returns></returns>
        public Boolean Repeated(Keys key)
        {           
            int hold = HoldLength(key);
            return (hold % KEYCHECK == 0) & (hold > KEYHOLDTIME) || Triggered(key);
        }

        /// <summary>
        /// This method loads a set of keybaord keys to associate with its collection
        /// of Keys. from the Data class. This class is called by Game.System.GameStart()
        /// and should not be called unless a manual reassignment of keys is needed.
        /// </summary>
        public void GetKeys()
        {
            Microsoft.Xna.Framework.Input.Keys k = Microsoft.Xna.Framework.Input.Keys.None;
            string[][] getKeys = Globals.Data.GetKeys();
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
