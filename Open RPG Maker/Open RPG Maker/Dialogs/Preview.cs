using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ORPG
{
    public partial class Preview : Form
    {
        public Preview()
        {
            InitializeComponent();
            InitializeEvents();
        }

        void InitializeEvents()
        {
            this.buttonClose.Click += new EventHandler(Close);
            this.pictureBox.DoubleClick += new EventHandler(Close);
        }

        void Close(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ShowDialog(string str)
        {
            try
            {
                Bitmap bmp = new Bitmap(str);
                this.pictureBox.Image = bmp;
                this.Width = bmp.Width + 20;
                this.Height = bmp.Height + 75;

                base.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Could not load image.");
            }
        }
    }
}
