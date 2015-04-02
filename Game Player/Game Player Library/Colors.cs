using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// A class for predefined colors. The class is static, and therefore should not be instanced.
    /// </summary>
    public static class Colors
    {
        /// <summary>
        /// Predefined color. RBG: (255, 255, 255).
        /// </summary>
        public static Color White
        {
            get { return new Color(255, 255, 255, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (0, 0, 0).
        /// </summary>
        public static Color Black
        {
            get { return new Color(0, 0, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (255, 0, 0).
        /// </summary>
        public static Color Red
        {
            get { return new Color(255, 0, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (0, 255, 0).
        /// </summary>
        public static Color Green
        {
            get { return new Color(0, 255, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (0, 0, 255).
        /// </summary>
        public static Color Blue
        {
            get { return new Color(0, 0, 255, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (255, 128, 0).
        /// </summary>
        public static Color Orange
        {
            get { return new Color(255, 128, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (255, 255, 0).
        /// </summary>
        public static Color Yellow
        {
            get { return new Color(255, 255, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (127, 61, 0).
        /// </summary>
        public static Color Brown
        {
            get { return new Color(127, 61, 0, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (255, 128, 128).
        /// </summary>
        public static Color Purple
        {
            get { return new Color(255, 128, 128, 255); }
        }

        /// <summary>
        /// Predefined color. RBG: (128, 128, 128).
        /// </summary>
        public static Color Gray
        {
            get { return new Color(128, 128, 128, 255); }
        }

        /// <summary>
        /// Predefined color. RBGA: (255, 255, 255, 0).
        /// </summary>
        public static Color Clear
        {
            get { return new Color(255, 255, 255, 0); }
        }
    }
}
