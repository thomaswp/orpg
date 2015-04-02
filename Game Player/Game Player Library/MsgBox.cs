using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Game_Player
{
    /// <summary>
    /// A static class for showing a Windows Message Box.
    /// </summary>
    public static class MsgBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        /// <summary>
        /// Shows the message box, displaying the given string and caption.
        /// Caption defaults to "Message Box"
        /// </summary>
        /// <param name="str">The string to be shown.</param>
        public static void Show(String str)
        {
            MessageBox(new IntPtr(), str, "Message Box", 0);
        }

        /// <summary>
        /// Shows the message box, displaying the given string and caption.
        /// Caption defaults to "Message Box"
        /// </summary>
        /// <param name="str">The string to be shown.</param>
        /// <param name="caption">The caption to be shown.</param>
        public static void Show(String str, String caption)
        {
            MessageBox(new IntPtr(), str, caption, 0);
        }

        public static void Show(Object obj)
        {
            Show(obj.ToString());
        }

        public static void Show(Array array)
        {
            string s = "[";

            for (int i = 0; i < array.Length; i++)
            {
                s += array.GetValue(i).ToString();

                if (i != array.Length - 1)
                    s += ", ";
            }

            s += "]";

            Show(s);
        }
    }
}
