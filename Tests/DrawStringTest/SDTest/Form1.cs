using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SDTEst
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();

            if (DialogResult != DialogResult.Cancel)
            {
                this.button1.BackColor = this.colorDialog1.Color;
                //this.BackColor = this.colorDialog1.Color;
                ColorText(this.colorDialog1.Color);
            }
        }

        private void ColorText(Color color)
        {
            //create a new bitmap
            Bitmap bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);

            //clear it to green
            Graphics.FromImage(bmp).Clear(Color.FromArgb(0, 255, 0));
            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0));
            //write the text on it in red
            Graphics.FromImage(bmp).DrawString("Lala!!", new Font(new FontFamily("Arial"), 12), brush, new PointF(0, 0));

            float[] cm = new float[] { color.R / 255.0f, color.G / 255.0f, color.B / 255.0f };
            //creat a color matrix where red is replaced with the color you want for the text
            //and blue and green are replaced with a 0-alpha version of that color
            float[][] matrix = new float[][]
            {
                new float[] {cm[0], cm[1], cm[2], 1, 0},
                new float[] {cm[0], cm[1], cm[2], 0, 0},
                new float[] {cm[0], cm[1], cm[2], 0, 0},
                new float[] {0, 0, 0, 0, 0},
                new float[] {0, 0, 0, 0, 1}
            };

            //now get the bitmap onto which you're going to draw the text
            Bitmap newBmp = new Bitmap(this.pictureBox1.Image);
            //create an ImageAttributes and give it the color matrix above
            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();
            attr.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(matrix));
            //draw the above bitmap onto this new bitmap using the color matrix
            Graphics.FromImage(newBmp).DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attr);
            Graphics.FromImage(newBmp).DrawString("Lala!!", new Font(new FontFamily("Arial"), 12), new SolidBrush(color), new PointF(0, 40));

            this.pictureBox1.Image = newBmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);

            string text = this.textBox1.Text;

            g.Clear(Color.FromArgb(0, 0, 0, 0));
            Font font = new Font(new FontFamily("Arial"), 36);
            
            g.DrawString(text, font, Brushes.Black, new PointF(0, 0));
            text += "|";

            int width = (int)g.MeasureString(text, font).Width;
            int height = (int)g.MeasureString(text, font).Height;

            Bitmap b = new Bitmap(width, height);
            Graphics.FromImage(b).DrawString(text, font, Brushes.Black, new PointF(0, 0));
            while (b.GetPixel(width - 1, height / 2) == Color.FromArgb(0,0,0,0))
                width--;
            width -= (int)(g.MeasureString("||", font).Width - g.MeasureString("|", font).Width);

            Size s = new Size(width, height);
            g.DrawRectangle(Pens.Black, new Rectangle(new Point(0, 0), s));

            this.pictureBox1.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Set up string.
            string measureString = "! !";
            Font stringFont = new Font("Arial", 16.0F);

            // Set character ranges to "First" and "Second".
            CharacterRange[] characterRanges = { new CharacterRange(1, 1) };

            // Create rectangle for layout.
            float x = 50.0F;
            float y = 50.0F;
            float width = 200.0F;
            float height = 35.0F;
            RectangleF layoutRect = new RectangleF(x, y, width, height);

            // Set string format.
            StringFormat stringFormat = new StringFormat();
            stringFormat.SetMeasurableCharacterRanges(characterRanges);

            // Draw string to screen.
            e.Graphics.DrawString(measureString, stringFont, Brushes.Black, x, y, stringFormat);

            // Measure two ranges in string.
            Region[] stringRegions = e.Graphics.MeasureCharacterRanges(measureString, 
        stringFont, layoutRect, stringFormat);

            // Draw rectangle for first measured range.
            RectangleF measureRect1 = stringRegions[0].GetBounds(e.Graphics);
            e.Graphics.DrawRectangle(new Pen(Color.Red, 1), Rectangle.Round(measureRect1));

            //Draw rectangle for second measured range.
            //RectangleF measureRect2 = stringRegions[1].GetBounds(e.Graphics);
            //e.Graphics.DrawRectangle(new Pen(Color.Blue, 1), Rectangle.Round(measureRect2));

        }
    }
}
