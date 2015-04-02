using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace HueTest
{
    public partial class Form1 : Form
    {
        Bitmap bmp = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void nudHue_ValueChanged(object sender, EventArgs e)
        {
            int hue = (int)this.nudHue.Value;
            float[][] matrix = HueMatrix(hue);
            
            ColorMatrix cMatrix = new ColorMatrix(matrix);
         
            ImageAttributes attr = new ImageAttributes();
            attr.SetColorMatrix(cMatrix);

            if (bmp == null)
                bmp = (Bitmap)this.pictureBox1.Image;

            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);
            Graphics.FromImage(newBitmap).DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attr);

            this.pictureBox1.Image = newBitmap;
        }

        private float[][] HueMatrix(int hue)
        {
            float[][] mat = new float[][]
            {
                new float[] {1, 0, 0, 0},
                new float[] {0, 1, 0, 0},
                new float[] {0, 0, 1, 0},
                new float[] {0, 0, 0, 1}
            };

            float mag, xrs, xrc, yrs, yrc, zrs, zrc;

            /* rotate the grey vector into positive Z */
            mag = (float)Math.Sqrt(2.0);
            xrs = 1.0f / mag;
            xrc = 1.0f / mag;
            mat = XRotateMat(mat, xrs, xrc);

            mag = (float)Math.Sqrt(3.0);
            yrs = -1.0f / mag;
            yrc = (float)Math.Sqrt(2.0) / mag;
            mat = YRotateMat(mat, yrs, yrc);

            /* rotate the hue */
            zrs = (float)Math.Sin(hue * Math.PI / 180.0);
            zrc = (float)Math.Cos(hue * Math.PI / 180.0);
            mat = ZRotateMat(mat, zrs, zrc);

            /* rotate the grey vector back into place */
            mat = YRotateMat(mat, -yrs, yrc);
            mat = XRotateMat(mat, -xrs, xrc);

            return new float[][]
            {
                new float[] {mat[0][0], mat[0][1], mat[0][2], mat[0][3], 0},
                new float[] {mat[1][0], mat[1][1], mat[1][2], mat[1][3], 0},
                new float[] {mat[2][0], mat[2][1], mat[2][2], mat[2][3], 0},
                new float[] {mat[3][0], mat[3][1], mat[3][2], mat[3][3], 0},
                new float[] {0, 0, 0, 0, 1}
            };
        }

        private float[][] MatrixMult(float[][] a, float[][] b)
        {
            int x, y;
            float[][] temp = new float[4][];

            for (y = 0; y < 4; y++)
            {
                temp[y] = new float[5];
                for (x = 0; x < 4; x++)
                {
                    temp[y][x] = b[y][0] * a[0][x]
                               + b[y][1] * a[1][x]
                               + b[y][2] * a[2][x]
                               + b[y][3] * a[3][x];
                }
            }

            return temp;
        }

        private float[][] XRotateMat(float[][] mat, float rs, float rc)
        {
            float[][] mmat = new float[][]
                {
                    new float[4],
                    new float[4],
                    new float[4],
                    new float[4]
                };

            mmat[0][0] = 1.0f;
            mmat[0][1] = 0.0f;
            mmat[0][2] = 0.0f;
            mmat[0][3] = 0.0f;

            mmat[1][0] = 0.0f;
            mmat[1][1] = rc;
            mmat[1][2] = rs;
            mmat[1][3] = 0.0f;

            mmat[2][0] = 0.0f;
            mmat[2][1] = -rs;
            mmat[2][2] = rc;
            mmat[2][3] = 0.0f;

            mmat[3][0] = 0.0f;
            mmat[3][1] = 0.0f;
            mmat[3][2] = 0.0f;
            mmat[3][3] = 1.0f;
            return MatrixMult(mmat, mat);
        }


        private float[][] YRotateMat(float[][] mat, float rs, float rc)
        {
            float[][] mmat = new float[][]
                {
                    new float[4],
                    new float[4],
                    new float[4],
                    new float[4]
                };

            mmat[0][0] = rc;
            mmat[0][1] = 0.0f;
            mmat[0][2] = -rs;
            mmat[0][3] = 0.0f;

            mmat[1][0] = 0.0f;
            mmat[1][1] = 1.0f;
            mmat[1][2] = 0.0f;
            mmat[1][3] = 0.0f;

            mmat[2][0] = rs;
            mmat[2][1] = 0.0f;
            mmat[2][2] = rc;
            mmat[2][3] = 0.0f;

            mmat[3][0] = 0.0f;
            mmat[3][1] = 0.0f;
            mmat[3][2] = 0.0f;
            mmat[3][3] = 1.0f;
            return MatrixMult(mmat, mat);
        }

        private float[][] ZRotateMat(float[][] mat, float rs, float rc)
        {
            float[][] mmat = new float[][]
                {
                    new float[4],
                    new float[4],
                    new float[4],
                    new float[4]
                };

            mmat[0][0] = rc;
            mmat[0][1] = rs;
            mmat[0][2] = 0.0f;
            mmat[0][3] = 0.0f;

            mmat[1][0] = -rs;
            mmat[1][1] = rc;
            mmat[1][2] = 0.0f;
            mmat[1][3] = 0.0f;

            mmat[2][0] = 0.0f;
            mmat[2][1] = 0.0f;
            mmat[2][2] = 1.0f;
            mmat[2][3] = 0.0f;

            mmat[3][0] = 0.0f;
            mmat[3][1] = 0.0f;
            mmat[3][2] = 0.0f;
            mmat[3][3] = 1.0f;
            mmat[3][3] = 1.0f;
            return MatrixMult(mmat, mat);
        }

        private string MatrixToString(float[][] matrix)
        {
            string s = "";

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    s += matrix[i][j].ToString("0.000") + " ";
                }
                s += "\n";
            }

            return s;
        }
    }
}
