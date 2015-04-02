//TODO
//Work with audio
//Window Background Modes
//Finish Window Classes (oh boy!)
//Test Reassign and make disposed items not work.
//Data in general
//Make transitions between scenes make sense

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Threading;
using Game_Player.Utils;

using Game_Player.Game;

namespace Game_Player
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int FPS = 60;

        GraphicsDeviceManager graphics;
        int frames = 0;
        int miliseconds = 0;
        int lastToggle = 0;
        const int TOGGLE_TIME = 3000;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Graphics.DeviceManager = graphics;

            Graphics.ScreenRect = new Rect(0, 0, 640, 480);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Globals.GameSystem.GameStart();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Graphics.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Graphics.Effects = Content.Load<Effect>("Effects");
            Graphics.TransEffect = Content.Load<Effect>("Trans");
            // TODO: use this.Content to load your game content here
        }
       

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Globals.Audio.Dispose();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (!Globals.GameSystem.Update())
                this.Exit();
            if (Graphics.FPS != FPS)
            {
                FPS = Graphics.FPS;
                this.TargetElapsedTime = new TimeSpan((long)Math.Pow(10, 7) / FPS);
            }


            if (lastToggle < TOGGLE_TIME)
                lastToggle += gameTime.ElapsedGameTime.Milliseconds;
            if ((Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) ||
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt)) &&
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) &&
                lastToggle >= TOGGLE_TIME)
            {
                graphics.ToggleFullScreen();
                lastToggle = 0;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //UsageData.StartUsage("mydraw");
            Graphics.Draw();
            //Console.WriteLine(UsageData.EndUsage("mydraw"));

            base.Draw(gameTime);
            UpdateFPS(gameTime);
        }

        void UpdateFPS(GameTime gameTime)
        {
            frames++;
            miliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (miliseconds >= 1000)
            {
                this.Window.Title = Paths.Title + " - " + (frames * 1000 / miliseconds).ToString() + " FPS";
                frames = 0; miliseconds -= 1000;
            }
        }
    }
}
