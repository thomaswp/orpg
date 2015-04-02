using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Overlap_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics.FromImage(pictureBox2.Image).Clear(Color.FromArgb(100, 255, 0, 0));

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(pictureBox1.Image).Clear(Color.FromArgb(255, 0, 0, 255));

            Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.SendToBack();
            Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.SendToBack();
            Refresh();
        }
    }
}
