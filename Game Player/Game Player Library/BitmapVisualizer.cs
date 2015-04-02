using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Game_Player
{
    /// <summary>
    /// A Visualizer for a Bitmap.  
    /// </summary>
    public class BitmapVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            Bitmap data = (Bitmap)objectProvider.GetObject();

            using (Form displayForm = new Form())
            {
                displayForm.Text = data.ToString();
                displayForm.Size = new Size(data.Width + 14, data.Height + 40);
                displayForm.BackColor = System.Drawing.Color.Red;

                System.Drawing.Bitmap bmp = data.SystemBitmap;
                PictureBox box = new PictureBox();
                box.SizeMode = PictureBoxSizeMode.AutoSize;
                box.Image = bmp;
                box.Parent = displayForm;

                windowService.ShowDialog(displayForm);
            }
        }

        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(BitmapVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
