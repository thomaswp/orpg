using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace Game_Player
{
    /// <summary>
    /// A class that helps draw sprites. Instances of this class should not be created. Use Globals.Graphics instead.
    /// </summary>
    public class Graphics
    {
        Viewport[] _viewports = new Viewport[] { };
        /// <summary>
        /// A collection of all viewports being drawn.
        /// </summary>
        public Viewport[] Viewports
        { get { return _viewports; } }

        /// <summary>
        /// The number of sprites left to be drawn this frame.
        /// </summary>
        public int DrawSize
        { get { return toDraw.Length; } }

        int ids = -1;

        Sprite[] toDraw = new Sprite[] { };

        Texture2D[][] textures = new Texture2D[][] { };

        /// <summary>
        /// A adds a viewport to be drawn. This method should not be used outside of the Viewport Class.
        /// </summary>
        /// <param name="viewport">The viewport to be added.</param>
        /// <returns></returns>
        public int AddViewport(Viewport viewport)
        {
            Array.Resize<Viewport>(ref _viewports, _viewports.Length + 1);
            _viewports[_viewports.Length - 1] = viewport;
            ids++;
            return ids;
        }

        /// <summary>
        /// Updates the Graphics Class. Generally this is done through Globals.GameSystem.Update(),
        /// and does not need to be explicitly called.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < Viewports.Length; i++)
            {
                if (Viewports[i].Disposed == false)
                { Viewports[i].Update(); }
            }
        }

        void AddTextures()
        {
            Array.Resize<Texture2D[]>(ref textures, Viewports.Length);
            for (int i = 0; i < Viewports.Length; i++)
            {
                Array.Resize<Texture2D>(ref textures[i], Viewports[i].Sprites.Length);
            }
        }

        /// <summary>
        /// Gets a Microsoft.XNA.Framework.Graphics.Texture2D from a sprite.
        /// This method is used by the rendering engine and should not be called.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        public Texture2D SpriteTexture(Sprite s, GraphicsDevice graphicsDevice)
        {
            CreateTextureFromBitmap(graphicsDevice, s.Bitmap.SystemBitmap, ref textures[s.Viewport.ID][s.ID]);
            return textures[s.Viewport.ID][s.ID];
        }

        /// <summary>
        /// Prepared the list of sprites to be drawn.
        /// This method is used by the rendering engine and should not be called.
        /// </summary>
        public void GenerateSpriteList()
        {
            AddTextures();
            for (int i = 0; i < toDraw.Length; i++)
            {
                if (toDraw[i] != null)
                { if (!toDraw[i].Disposed) { toDraw[i].Dispose(); } }
            }
            toDraw = new Sprite[] { };
            Viewport[] vptTemp = (Viewport[])Viewports.Clone();
            int[] vptZs = new int[vptTemp.Length];
            for (int k = 0; k < vptTemp.Length; k++)
            { vptZs[k] = vptTemp[k].Z; }
            Sprite[] spriteTemp;
            int[] spriteZs;
            Array.Sort(vptZs, vptTemp);
            for (int i = 0; i < vptTemp.Length; i++)
            {
                if (vptTemp[i].Visible & !vptTemp[i].Disposed)
                {
                    spriteTemp = (Sprite[])vptTemp[i].Sprites.Clone();
                    spriteZs = new int[spriteTemp.Length];
                    for (int k = 0; k < spriteTemp.Length; k++)
                    { spriteZs[k] = spriteTemp[k].Z; }
                    Array.Sort(spriteZs, spriteTemp);
                    for (int j = 0; j < spriteTemp.Length; j++)
                    { 
                        if (spriteTemp[j].Visible & !spriteTemp[j].Disposed)
                        {
                            Array.Resize<Sprite>(ref toDraw, toDraw.Length + 1);
                            toDraw[toDraw.Length - 1] = spriteTemp[j];
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// Identifies whether there is another sprite to be rendered this frame.
        /// This method is used by the rendering engine and should not be called.
        /// </summary>
        /// <returns></returns>
        public Boolean NextSpriteExists()
        {
            return toDraw.Length > 0;
        }

        /// <summary>
        /// Gets a sprite from the stack to be rendered, then removes it from the list of sprites to be rendered.
        /// This method is used by the rendering engine and should not be called.
        /// </summary>
        /// <returns></returns>
        public Sprite PushSprite()
        { 
            Sprite s = toDraw[0];
            for (int i = 0; i < toDraw.Length - 1; i++)
            {
                toDraw[i] = toDraw[i + 1];
            }
            Array.Resize<Sprite>(ref toDraw, toDraw.Length - 1);
            return s;
        }

        //Modifies the Texture2D to contain the same image as the bitmap parameter, resized if necessary.
        //Courtesy of JSpiel (http://forums.xna.com/forums/t/12154.aspx)
        Texture2D CreateTextureFromBitmap(GraphicsDevice graphics_device, System.Drawing.Bitmap bitmap, ref Texture2D texture)
        {
            bool dispose_bitmap = false;
            try
            {
                if (bitmap != null)
                {
                    //Resize the bitmap if necessary, then capture its final size 
                    if (graphics_device.GraphicsDeviceCapabilities.TextureCapabilities.RequiresPower2)
                    {

                        System.Drawing.Size new_size; //New size will be next largest power of two, so bitmap will always be scaled up, never down
                        new_size = new Size((int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Width) / Math.Log(2.0))), (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bitmap.Height) / Math.Log(2.0))));
                        bitmap = new System.Drawing.Bitmap(bitmap, new_size);
                        dispose_bitmap = true;
                    }
                    System.Drawing.Size bitmap_size = bitmap.Size;

                    Boolean create = false;
                    if (texture == null)
                    // If there's no texture, create a new one.
                    { create = true; }
                    else
                    {
                        // Or if the texture has changed sizes, create a new one. 
                        if (texture.Width != bitmap_size.Width || texture.Height != bitmap_size.Height)
                        {
                            //Attempt to dispose the old texture first.
                            try { texture.Dispose(); }
                            catch { }
                            create = true; 
                        }
                    }
                    // If needed, create a new texture.
                    if (create) 
                    { texture = new Texture2D(graphics_device, bitmap_size.Width, bitmap_size.Height, 1, TextureUsage.None, SurfaceFormat.Color); }

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
                    graphics_device.Textures[0] = null;
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
