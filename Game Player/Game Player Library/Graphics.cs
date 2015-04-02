using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Drawing;
using Game_Player.Utils;

namespace Game_Player
{
    /// <summary>
    /// A class that helps draw sprites. Instances of this class should not be created. Use Globals.Graphics instead.
    /// </summary>
    public static class Graphics
    {
        private static DateTime startTime = DateTime.Now;
        public static TimeSpan Playtime
        {
            get
            {
                return DateTime.Now.Subtract(startTime);
            }
        }

        private static Rect screenRect;
        public static Rect ScreenRect
        {
            get { return screenRect; }
            set 
            { 
                screenRect = value;

                //DeviceManager.PreferredBackBufferWidth = screenRect.Width;
                //DeviceManager.PreferredBackBufferHeight = screenRect.Height;
                //DeviceManager.ApplyChanges();
                
            }
        }

        public static int ScreenWidth
        {
            get { return ScreenRect.Width; }
            set { screenRect.Width = value; }
        }

        public static int ScreenHeight
        {
            get { return ScreenRect.Height; }
            set { screenRect.Height = value; }
        }

        private static List<Viewport> _viewports = new List<Viewport>();
        /// <summary>
        /// A collection of all viewports being drawn.
        /// </summary>
        public static List<Viewport> Viewports
        { get { return _viewports; } }

        static int _fps = 60;
        /// <summary>
        /// Gets or sets the desired Frames Per Second at which to run the game.
        /// </summary>
        public static int FPS
        {
            get { return _fps; }
            set { _fps = value; }
        }

        private static string _fontface;
        /// <summary>
        /// Default font face for text output.
        /// </summary>
        public static string FontFace
        {
            get { return _fontface; }
            set
            {
                if (value == null)
                { _fontface = ""; }
                else
                { _fontface = value; }
            }
        }

        private static int _fontsize;
        /// <summary>
        /// Default font size for text output.
        /// </summary>
        public static int FontSize
        {
            get { return _fontsize; }
            set
            {
                _fontsize = value;
            }
        }

        private static Boolean _frozen = false;
        /// <summary>
        /// Returns true if the Graphics are frozen and awaiting a transition
        /// </summary>
        public static Boolean Frozen
        {
            get { return _frozen; }
        }

        public static Boolean Transitioning
        {
            get { return Math.Abs(fading) > 0; }
        }

        static int ids = -1;

        static double faded = 0;
        static double fading = 0;

        static Dictionary<Viewport, Microsoft.Xna.Framework.Graphics.Viewport> viewportsMap =
            new Dictionary<Viewport, Microsoft.Xna.Framework.Graphics.Viewport>();

        private static Microsoft.Xna.Framework.Graphics.Viewport GetViewport(Viewport vp)
        {
            Microsoft.Xna.Framework.Graphics.Viewport xnaVp;

            if (viewportsMap.ContainsKey(vp))
                xnaVp = viewportsMap[vp];
            else
            {
                xnaVp = new Microsoft.Xna.Framework.Graphics.Viewport();
                viewportsMap.Add(vp, xnaVp);
            }

            xnaVp.X = vp.X;
            xnaVp.Y = vp.Y;
            xnaVp.Width = vp.Width;
            xnaVp.Height = vp.Height;

            return xnaVp;
        }

        public static void Freeze()
        {
            lastFrame = Draw(true);
            _frozen = true;
        }

        public static void Transition() { Transition(20); }
        public static void Transition(int frames) { Transition(frames, ""); }
        public static void Transition(int frames, string transitionName)
        {
            lastFrame = Draw(true);

            _frozen = false;

            Bitmap trans;
            if (transitionName != "")
            {
                trans = new Bitmap(transitionName);
                blankTransition = true;
            }
            else
            {
                trans = new Bitmap(ScreenWidth, ScreenHeight);
                trans.Clear(Colors.Black);
                blankTransition = true;
            }

            transitionTexture = new Texture2D(GraphicsDevice, trans.Width, trans.Height);
            transitionTexture = CreateTextureFromBitmap(trans.SystemBitmap);


            faded = 1;
            fading = -1.0 / frames;
            
        }

        /// <summary>
        /// A adds a viewport to be drawn. This method should not be used outside of the Viewport Class.
        /// The method returns an id that will be assigned to the viewport.
        /// </summary>
        /// <param name="viewport">The viewport to be added.</param>
        /// <returns>The <see cref="P:Game_Player.Viewport.ID">viewport's id.</see></returns>
        public static int AddViewport(Viewport viewport)
        {
            _viewports.Add(viewport);
            ids++;
            return ids;
        }

        /// <summary>
        /// Updates the Graphics Class. Generally this is done through <c>Globals.GameSystem.Update()</c>,
        /// and does not need to be explicitly called.
        /// </summary>
        public static void Update()
        {
            for (int i = 0; i < Viewports.Count; i++)
            {
                if (Viewports[i].Disposed == false)
                { Viewports[i].Update(); }
            }
        }


        //Modifies the Texture2D to contain the same image as the bitmap parameter, resized if necessary.
        //Courtesy of JSpiel (http://forums.xna.com/forums/t/12154.aspx)
        static Texture2D CreateTextureFromBitmap(System.Drawing.Bitmap bitmap)
        {
            bool dispose_bitmap = false;
            Texture2D texture = null;

            try
            {
                if (bitmap != null)
                {
                    //Resize the bitmap if necessary, then capture its final size 
                    //if (GraphicsDevice.GraphicsDeviceCapabilities.TextureCapabilities.RequiresPower2)
                    //{

                    //    System.Drawing.Size new_size; //New size will be next largest power of two, so bitmap will always be scaled up, never down
                    //    new_size = new Size((int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Width) / Math.Log(2.0))), (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Height) / Math.Log(2.0))));
                    //    bitmap = new System.Drawing.Bitmap(bitmap, new_size);
                    //    dispose_bitmap = true;
                    //}
                    System.Drawing.Size bitmap_size = bitmap.Size;

                    texture = new Texture2D(GraphicsDevice, bitmap_size.Width, bitmap_size.Height, false, SurfaceFormat.Color);

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
                    //GraphicsDevice.Textures[0] = null;
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


        public static GraphicsDevice GraphicsDevice { get { return DeviceManager.GraphicsDevice; } }
        //public static GraphicsDeviceManager DeviceManager { get; set; }
        private static IGraphicsDeviceService deviceManager;
        public static IGraphicsDeviceService DeviceManager
        {
            get { return deviceManager; }
            set { deviceManager = value; }
        }

        public static SpriteBatch SpriteBatch { get; set; }
        public static Effect Effects { get; set; }
        private static double effectGray;
        public static double EffectGray
        {
            get { return effectGray; }
            set 
            {
                if (effectGray != value && Effects != null)
                    Effects.Parameters["gray"].SetValue((float)value);
                effectGray = value; 
            }
        }
        private static Microsoft.Xna.Framework.Color effectColor;
        public static Microsoft.Xna.Framework.Color EffectColor
        {
            get { return effectColor; }
            set 
            {
                if (effectColor != value && Effects != null)
                    Effects.Parameters["blendColor"].SetValue(value.ToVector4());
                effectColor = value; 
            }
        }
        private static int effectBlendType;
        public static int EffectBlendType
        {
            get { return effectBlendType; }
            set 
            {
                if (effectBlendType != value && Effects != null)
                    Effects.Parameters["blendType"].SetValue(value);
                effectBlendType = value;
            }
        }
        public static Effect TransEffect { get; set; }
        static Texture2D lastFrame, transitionTexture;
        static bool blankTransition;


        public static Texture2D Draw() { return Draw(false); }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="graphics">The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDeviceManager">
        /// Microsoft.Xna.Framework.Graphics.GraphicsDeviceManager</see> used to draw the frame.</param>
        /// <param name="spriteBatch">The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch">
        /// Microsoft.Xna.Framework.Graphics.SpriteBatch</see> used to draw the frame.</param>
        public static Texture2D Draw(bool textureOut)
        {
            if (Frozen || GraphicsDevice == null || DeviceManager == null || SpriteBatch == null || Effects == null)
                return lastFrame;

            if (!viewportsMap.ContainsKey(Viewport.Default))
                viewportsMap.Add(Viewport.Default, GraphicsDevice.Viewport);

            RenderTarget2D target = null, oldTarget = null;
            if (textureOut)
            {
                RenderTargetBinding[] rts = GraphicsDevice.GetRenderTargets();
                target = rts.Length == 0 ? null : (RenderTarget2D) rts[0].RenderTarget;
                oldTarget = target;
                PresentationParameters pp = GraphicsDevice.PresentationParameters;
                target = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Rg32, DepthFormat.Depth24);
                GraphicsDevice.SetRenderTarget(target);
            }

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            
            Microsoft.Xna.Framework.Color color; 
            Texture2D texture; 
            int totalAlpha; 
            SpriteEffects sEffects = SpriteEffects.None;

            BlendState blendState = BlendState.AlphaBlend, lastBlendState = BlendState.AlphaBlend;

            SpriteBatch.Begin(SpriteSortMode.Immediate, blendState);

            int t = 0;

            Viewports.Sort();
            foreach (Viewport viewport in Viewports)
            {
                if (viewport == null || viewport.Disposed || !viewport.Visible) 
                    continue;

                viewport.Sprites.Sort();
                GraphicsDevice.Viewport = GetViewport(viewport);

                List<Sprite> remove = new List<Sprite>();

                foreach (Sprite sprite in viewport.Sprites)
                {
                    t++;

                    if (sprite == null || sprite.Disposed || sprite.Bitmap == null)
                    {
                        remove.Add(sprite);
                        continue;
                    }

                    if (!sprite.Visible)
                        continue;

                    totalAlpha = sprite.Color.Alpha + sprite.Viewport.Color.Alpha;
                    if (totalAlpha == 0) { color = new Microsoft.Xna.Framework.Color(255, 255, 255, (byte)(sprite.Opactiy)); }
                    else
                    {
                        color = new Microsoft.Xna.Framework.Color(
                            (byte)((sprite.Color.Red * sprite.Color.Alpha + viewport.Color.Red * viewport.Color.Alpha) / totalAlpha),
                            (byte)((sprite.Color.Green * sprite.Color.Alpha + viewport.Color.Green * viewport.Color.Alpha) / totalAlpha),
                            (byte)((sprite.Color.Blue * sprite.Color.Alpha + viewport.Color.Blue * viewport.Color.Alpha) / totalAlpha),
                            (byte)(sprite.Opactiy));
                    }

                    color.R += (byte)(sprite.Tone.Red + viewport.Tone.Red);
                    color.G += (byte)(sprite.Tone.Green + viewport.Tone.Green);
                    color.B += (byte)(sprite.Tone.Blue + viewport.Tone.Blue);

                    Rect rect = sprite.Rect;
                    rect.X -= viewport.OX;
                    rect.Y -= viewport.OY;

                    switch (sprite.FlipMode)
                    {
                        case FlipModes.None:
                            {
                                sEffects = SpriteEffects.None;
                                break;
                            }
                        case FlipModes.Horizontally:
                            {
                                sEffects = SpriteEffects.FlipHorizontally;
                                break;
                            }
                        case FlipModes.Vertically:
                            {
                                sEffects = SpriteEffects.FlipVertically;
                                break;
                            }
                    }

                    //still need to do a better job of negative blending
                    blendState = sprite.BlendType == 1 ? BlendState.Additive : BlendState.AlphaBlend;

                    if (sprite.Bitmap.NeedRefresh)
                    {
                        sprite.Bitmap.Texture = CreateTextureFromBitmap(sprite.Bitmap.SystemBitmap);
                        sprite.Bitmap.NeedRefresh = false;
                    }
                    texture = sprite.Bitmap.Texture;

                    //There's still a problem with a lot of consecutive sprites using effect
                    //Which I'll need to fix once I have a good laggy test case
                    EffectGray = (sprite.Tone.Gray + viewport.Tone.Gray) / 255.0f;
                    EffectColor = color;
                    EffectBlendType = sprite.BlendType;
                    bool useEffect = 
                        (sprite.Tone.Gray + viewport.Tone.Gray > 0) ||
                        (color != Colors.White.ToXNAColor()) ||
                        (sprite.BlendType == 2);


                    /**
                    DeviceManager.GraphicsDevice.Textures[1] = texture;
                    Effects.Parameters["width"].SetValue(sprite.Width);
                    Effects.Parameters["height"].SetValue(sprite.Height);
                    Effects.Parameters["bushDepth"].SetValue(sprite.BushDepth);
                    Effects.Parameters["blur"].SetValue(sprite.Blur);
                    Effects.Parameters["bubbleX"].SetValue(sprite.Bubble.X);
                    Effects.Parameters["bubbleY"].SetValue(sprite.Bubble.Y);
                    Effects.Parameters["bubbleRad"].SetValue((sprite.BubbleRadius));
                    **/

                    if (blendState != lastBlendState)
                    {
                        if (!useEffect)
                        {
                            SpriteBatch.End();
                            SpriteBatch.Begin(SpriteSortMode.Immediate, blendState);
                        }
                        lastBlendState = blendState;
                    }

                    if (useEffect) 
                    {
                        SpriteBatch.End();
                        SpriteBatch.Begin(SpriteSortMode.Immediate, blendState);
                        Effects.CurrentTechnique.Passes[0].Apply();
                    }
                    
                    try
                    {
                        SpriteBatch.Draw(
                            texture,
                            rect.ToXNARect(),
                            sprite.BmpSourceRect.ToXNARect(),
                            color,
                            (float)(sprite.Rotation * 2 * Math.PI),
                            new Vector2(sprite.OX, sprite.OY),
                            sEffects,
                            0);
                    }
                    catch
                    {
                        MsgBox.Show("Graphics Error!");
                        throw new Exception("Graphics Error!");
                    }

                    if (useEffect)
                    {
                        SpriteBatch.End();
                        SpriteBatch.Begin(SpriteSortMode.Immediate, blendState);
                    }
                }

                foreach (Sprite sprite in remove)
                    viewport.Sprites.Remove(sprite);
            }
            SpriteBatch.End();

            //Console.WriteLine(t);

            if (Math.Abs(fading) > 0)
            {
                GraphicsDevice.Textures[1] = transitionTexture;
                TransEffect.Parameters["opacity"].SetValue((float)faded);

                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                if (!blankTransition)
                {
                    TransEffect.CurrentTechnique.Passes[0].Apply();
                }

                color = new Microsoft.Xna.Framework.Color((byte)255, (byte)255, (byte)255, (byte)(255.0 * faded));
                SpriteBatch.Draw(
                    lastFrame,
                    ScreenRect.ToXNARect(),
                    color);

                SpriteBatch.End();
                
                faded += fading;
                if (faded <= 0)
                {
                    faded = 0;
                    fading = 0;
                }
                else if (faded >= 1)
                {
                    faded = 1;
                    fading = 0;
                }
            }

            if (textureOut)
            {
                GraphicsDevice.SetRenderTarget(oldTarget);
                return (Texture2D) target;
            }
            else
                return null;
        }
    }
}
