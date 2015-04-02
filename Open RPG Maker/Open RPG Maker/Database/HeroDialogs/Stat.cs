using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ORPG
{
    public partial class Stat : UserControl
    {
        public int Level 
        {
            get { return (int)this.numericUpDownLevel.Value; }
            set { this.numericUpDownLevel.Value = value; } 
        }

        Color _color;
        public Color Color
        {
            get { return _color; }
            set 
            { 
                _color = value;
                RefreshControl();
            }
        }

        int[] _values = new int[99];
        public int[] Values
        {
            get { return _values; }
            set 
            {
                _values = value;
                RefreshControl();
            }
        }

        Image _image;
        public Image Image
        {
            get 
            {
                if (!painting)
                    PaintPictureBox();
                return _image; 
            }
        }

        Random rand = new Random();

        int lastMouseIndex = -1;
        int lastMouseValue = -1;

        Boolean painting = false;

        public Stat()
        {
            InitializeComponent();
            _image = new Bitmap(this.pictureBoxStat.Width, this.pictureBoxStat.Height);
            this.pictureBoxStat.Image = Image;
            EventHandlers();
            RefreshControl();
        }

        void EventHandlers()
        {
            this.pictureBoxStat.Paint += new PaintEventHandler(PaintPictureBox);

            this.buttonA.Click += new EventHandler(DrawA);
            this.buttonB.Click += new EventHandler(DrawB);
            this.buttonC.Click += new EventHandler(DrawC);
            this.buttonD.Click += new EventHandler(DrawD);
            this.buttonE.Click += new EventHandler(DrawE);

            this.numericUpDownLevel.LostFocus += new EventHandler(LevelChanged);
            this.numericUpDownLevel.ValueChanged += new EventHandler(LevelChanged);
            this.numericUpDownValue.LostFocus += new EventHandler(ValueChanged);
            this.numericUpDownValue.ValueChanged += new EventHandler(ValueChanged);

            this.pictureBoxStat.MouseMove += new MouseEventHandler(DrawValue);
            this.pictureBoxStat.MouseClick += new MouseEventHandler(DrawValue);
        }

        void DrawValue(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int value = (Image.Height - e.Y) * 9999 / Image.Height;
                int index = e.X * 98 / Image.Width;
                if (lastMouseIndex != -1 && lastMouseIndex != index)
                {
                    int start = lastMouseValue;
                    double slope = (double)(value - lastMouseValue) / (index - lastMouseIndex);
                    double plus = 0;
                    for (int i = Math.Min(index, lastMouseIndex); i <= Math.Max(index, lastMouseIndex); i++)
                    {
                        Values[Math.Min(98, Math.Max(0, i))] = Math.Min(9999, Math.Max(0, start + (int)plus));
                        plus += slope;
                    }
                }
                else
                    Values[Math.Min(98, Math.Max(0, index))] = Math.Min(9999, Math.Max(0, value));

                RefreshControl();

                lastMouseIndex = index;
                lastMouseValue = value;
                //this.Parent.Text = "0";
            }
            else
            {
                lastMouseIndex = -1;
                lastMouseValue = -1;
                //this.Parent.Text = "1";
            }
        }

        void LevelChanged(object sender, EventArgs e)
        {
            this.numericUpDownValue.Value = Values[(int)this.numericUpDownLevel.Value - 1];
        }

        void ValueChanged(object sender, EventArgs e)
        {
            Values[(int)this.numericUpDownLevel.Value - 1] = (int)this.numericUpDownValue.Value;
            RefreshControl();
        }

        void DrawA(object sender, EventArgs e)
        {
            int slope = (int)(rand.NextDouble() * 20 + 65);
            int start = (int)(rand.NextDouble() * 100 + 650);
            Draw(slope, start);
        }

        void DrawB(object sender, EventArgs e)
        {
            int slope = (int)(rand.NextDouble() * 20 + 55);
            int start = (int)(rand.NextDouble() * 100 + 550);
            Draw(slope, start);
        }

        void DrawC(object sender, EventArgs e)
        {
            int slope = (int)(rand.NextDouble() * 20 + 45);
            int start = (int)(rand.NextDouble() * 100 + 450);
            Draw(slope, start);
        }

        void DrawD(object sender, EventArgs e)
        {
            int slope = (int)(rand.NextDouble() * 20 + 35);
            int start = (int)(rand.NextDouble() * 100 + 350);
            Draw(slope, start);
        }

        void DrawE(object sender, EventArgs e)
        {
            int slope = (int)(rand.NextDouble() * 20 + 25);
            int start = (int)(rand.NextDouble() * 100 + 250);
            Draw(slope, start);
        }

        void Draw(int slope, int firstValue)
        {
            for (int i = 0; i < Values.Length; i++ )
            {
                Values[i] = firstValue + (int)(i * slope);
            }
            RefreshControl();
        }

        void PaintPictureBox(object sender, PaintEventArgs e) { PaintPictureBox(); }

        public void PaintPictureBox()
        {
            if (_image == null)
                return;
            painting = true;
            Graphics g = Graphics.FromImage(Image);
            g.Clear(Color.White);
            int maxHeight = Image.Height;
            double width = (double)Image.Width / 99;
            for (int i = 0; i <= 98; i++)
            {
                int height = maxHeight * Values[i] / 9999;
                g.FillRectangle(new SolidBrush(Color), new Rectangle((int)(i * width), maxHeight - height, (int)width + 1, height));
            }
            this.pictureBoxStat.Image = Image;
            painting = false;
        }

        void RefreshControl()
        {
            this.numericUpDownValue.Value = Values[(int)this.numericUpDownLevel.Value - 1];
            this.pictureBoxStat.Refresh();
        }
    }
}
