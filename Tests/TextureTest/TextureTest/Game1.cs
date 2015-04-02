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

namespace TextureTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Bitmap gpBmp;
        System.Drawing.Bitmap sdBmp;
        Texture2D bmpText;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            Random rand = new Random();
            DateTime timer;
            TimeSpan span;
            int iters = 10000;

            for (int j = 0; j < 5; j++)
            {
                timer = DateTime.Now;

                gpBmp = new Bitmap(255, 255);
                for (int i = 0; i < iters; i++)
                {
                    int x = rand.Next(gpBmp.Width);
                    int y = rand.Next(gpBmp.Height);
                    int width = rand.Next(gpBmp.Width - x);
                    int height = rand.Next(gpBmp.Height - y);
                    Game_Player.Color gpColor = new Game_Player.Color(rand.Next(255), rand.Next(255), rand.Next(255));//, rand.Next(255));

                    gpBmp.FillRect(x, y, width, height, gpColor);
                }
                span = (DateTime.Now - timer);
                Console.WriteLine(1000 * span.Seconds + span.Milliseconds);

                timer = DateTime.Now;
                sdBmp = new System.Drawing.Bitmap(255, 255);
                for (int i = 0; i < iters; i++)
                {
                    int x = rand.Next(gpBmp.Width);
                    int y = rand.Next(gpBmp.Height);
                    int width = rand.Next(gpBmp.Width - x);
                    int height = rand.Next(gpBmp.Height - y);
                    Game_Player.Color gpColor = new Game_Player.Color(rand.Next(255), rand.Next(255), rand.Next(255));//, rand.Next(255));

                    //System.Drawing.Point p1 = new System.Drawing.Point(x1, y1);
                    //System.Drawing.Point p2 = new System.Drawing.Point(x2, y2);

                    System.Drawing.Graphics.FromImage(sdBmp).FillRectangle(gpColor.ToSolidBrush(), new System.Drawing.Rectangle(
                        x, y, width, height));

                    //int iBound = x + width;
                    //int jBound = y + height;

                    //System.Drawing.Color sdColor = gpColor.ToSystemColor();

                    //for (int l = x; l < iBound; l++)
                    //{
                    //    for (int m = y; m < jBound; m++)
                    //    {
                    //        sdBmp.SetPixel(l, m, sdColor);
                    //    }
                    //}

                }
                span = (DateTime.Now - timer);
                Console.WriteLine(1000 * span.Seconds + span.Milliseconds);
            }

            bmpText = CreateTextureFromBitmap(sdBmp);

            base.Initialize();
        }

        Texture2D realTexture;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            realTexture = Content.Load<Texture2D>("001-Fighter01");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Graphics.Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            //spriteBatch.Draw(realTexture, new Rectangle(0, 0, 500, 500), new Microsoft.Xna.Framework.Graphics.Color(255, 255, 255));
            spriteBatch.Draw(gpBmp.ToTexture2D(graphics.GraphicsDevice), new Rectangle(100, 100, gpBmp.Width, gpBmp.Height), Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.Draw(bmpText, new Rectangle(400, 100, sdBmp.Width, sdBmp.Height), Microsoft.Xna.Framework.Graphics.Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //Modifies the Texture2D to contain the same image as the bitmap parameter, resized if necessary.
        //Courtesy of JSpiel (http://forums.xna.com/forums/t/12154.aspx)
        private Texture2D CreateTextureFromBitmap(System.Drawing.Bitmap bitmap)
        {
            bool dispose_bitmap = false;
            Texture2D texture = null;

            try
            {
                if (bitmap != null)
                {
                    //Resize the bitmap if necessary, then capture its final size 
                    if (graphics.GraphicsDevice.GraphicsDeviceCapabilities.TextureCapabilities.RequiresPower2)
                    {

                        System.Drawing.Size new_size; //New size will be next largest power of two, so bitmap will always be scaled up, never down
                        new_size = new System.Drawing.Size((int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Width) / Math.Log(2.0))), (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Height) / Math.Log(2.0))));
                        bitmap = new System.Drawing.Bitmap(bitmap, new_size);
                        dispose_bitmap = true;
                    }
                    System.Drawing.Size bitmap_size = bitmap.Size;

                    texture = new Texture2D(GraphicsDevice, bitmap_size.Width, bitmap_size.Height, 1, TextureUsage.None, SurfaceFormat.Color);

                    //Lock the bitmap data and copy it out to a byte array 
                    System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap_size.Width, bitmap_size.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    byte[] pixel_bytes = null;
                    try
                    {
                        pixel_bytes = new byte[bmpdata.Stride * bmpdata.Height];
                        System.Runtime.InteropServices.Marshal.Copy(bmpdata.Scan0, pixel_bytes, 0, bmpdata.Stride * bmpdata.Height);
                    }
                    catch { } //If error occurs allocating memory, bitmap will still be unlocked properly
                    bitmap.UnlockBits(bmpdata);
                    GraphicsDevice.Textures[0] = null;
                    //Set the texture's data to the byte array containing the bitmap data that was just copied 
                    if (pixel_bytes != null) texture.SetData<byte>(pixel_bytes);
                }
            }
            catch
            {
                //Error occured; existing texture must be considered invalid 
            }
            finally
            {
                if (dispose_bitmap)
                    bitmap.Dispose();
            }
            return texture;
        }
    }
}
