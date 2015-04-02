using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Game_Player;

namespace CSGSSTemplate
{
    /// <summary>
    /// This is a sample class created for the immediate implementation
    /// of the CSGSS Library for a 2D game.
    /// 
    /// NOTE: The library will only function with XNA 3.0!
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Initializes the Game class.
        /// </summary>
        public Game1()
        {
            //Set the root content directory to your game's Content folder.
            //In theory, the only content you need to load will be Effect files.
            Content.RootDirectory = "Content";

            //Create a device manager for the Graphics engine.
            Graphics.DeviceManager = new GraphicsDeviceManager(this);
            //Set the size of the visible window to (640, 480).
            Graphics.ScreenRect = new Rect(0, 0, 640, 480);
            
            //Set the keys that the Input class recognizes.
            //See the GetKeys() method below.
            Input.GetKeys(GetKeys());
        }

        /// <summary>
        /// Here you can add any initialization code you need.
        /// The engine requires none by default.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Initialize Graphics' SpriteBatch
            Graphics.SpriteBatch = new SpriteBatch(GraphicsDevice);
            //And load its two needed effect files. These files
            //are found under the content folder in the game directory
            //and can be modified to suit your needs.
            Graphics.Effects = Content.Load<Effect>("Effects");
            Graphics.TransEffect = Content.Load<Effect>("Trans");
            //Load any other effect files here.
        }

        /// <summary>
        /// Use this method to unload any content.
        /// By default there is none to unload.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        int framesTillToggle = 0;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit if the escape key is pressed.
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                this.Exit();

            //If either Alt key is down and the player presses enter, this will toggle full
            //screen mode. To prevent lag, this can only be done once every 100 frames.
            if ((Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) ||
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt)) &&
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter)
                && framesTillToggle == 0)
            {
                Graphics.DeviceManager.ToggleFullScreen();
                framesTillToggle = 100;

            }
            if (framesTillToggle > 0)
                framesTillToggle--;

            //Update Input.
            Input.Update();

            //Update your game's logic.
            //This is found in a System class created
            //for demonstrational purposes in this project.
            System.Update();

            //Update Output (Audio and Graphics)
            Audio.Update();
            Graphics.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Allow Graphics to draw its content.
            Graphics.Draw();
            //And call the base's Draw() method.
            base.Draw(gameTime);
        }

        /// <summary>
        /// Use this method to define the keys that correspond to each item in Input.Keys
        /// </summary>
        /// <returns></returns>
        public static string[][] GetKeys()
        {
            string[][] keys = new string[20][] {
                new string[] {"Up"},                            //Up
                new string[] {"Down"},                          //Down
                new string[] {"Right"},                         //Right
                new string[] {"Left"},                          //Left
                new string[] {"Z"},                             //A
                new string[] {"Escape", "Num0"},                //B
                new string[] {"Space", "Enter"},                //C
                new string[] {"A"},                             //X
                new string[] {"S"},                             //Y
                new string[] {"D"},                             //Z
                new string[] {"Q"},                             //R
                new string[] {"W"},                             //L
                new string[] {"RightShift", "LeftShift"},       //Shift
                new string[] {"RightControl", "LeftControl"},   //Ctrl
                new string[] {"RightAlt", "LeftAlt"},           //Alt
                new string[] {"F5"},                            //F5
                new string[] {"F6"},                            //F6
                new string[] {"F7"},                            //F7
                new string[] {"F8"},                            //F8
                new string[] {"F9"}};                           //F9
            return keys;
        }


    }
}
