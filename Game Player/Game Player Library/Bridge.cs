using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player_Library
{
    /// <summary>
    /// A class to bridge between the Game Player Library.dll and the Game Player.exe.
    /// </summary>
    public class Bridge
    {
        /// <summary>
        /// This method initializes the Bridge with the given <see cref="Game_Player.Graphics">Graphics</see>.
        /// This method is called in the Game's initialization logic and should not be called again.
        /// </summary>
        /// <param name="g"></param>
        //public Bridge(Game_Player.Graphics g)
        //{
        //    _graphics = g;
        //}

        //private static Game_Player.Graphics _graphics = new Game_Player.Graphics();
        ///// <summary>
        ///// Graphics input and output class.
        ///// </summary>
        //public static Game_Player.Graphics Graphics
        //{
        //    get { return Bridge._graphics; }
        //}

        /// <summary>
        /// This method initializes the <see cref="Game_Player_Library.Bridge">Bridge</see>
        /// with the given <see cref="Game_Player.Graphics">Graphics</see>. This 
        /// </summary>
        private static string _fontface;
        /// <summary>
        /// Default font face for text output.
        /// </summary>
        public static string FontFace
        {
            get { return Bridge._fontface; }
            set
            {
                if (value == null)
                { Bridge._fontface = ""; }
                else
                { Bridge._fontface = value; }
            }
        }

        private static int _fontsize;
        /// <summary>
        /// Default font size for text output.
        /// </summary>
        public static int FontSize
        {
            get { return Bridge._fontsize; }
            set { Bridge._fontsize = value; }
        }
    }
}
