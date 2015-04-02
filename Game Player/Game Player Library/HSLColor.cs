using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Game_Player
{
    //Class courtacy of Rich Newman
    //http://richnewman.wordpress.com/hslcolor-class/
    public class HSLColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private double hue = 1.0;
        private double saturation = 1.0;
        private double luminosity = 1.0;

        private const double scale = 240.0;

        public double Hue
        {
            get { return hue * scale; }
            set { hue = CheckRange(value / scale); }
        }
        public double Saturation
        {
            get { return saturation * scale; }
            set { saturation = CheckRange(value / scale); }
        }
        public double Luminosity
        {
            get { return luminosity * scale; }
            set { luminosity = CheckRange(value / scale); }
        }

        private double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        public string ToRGBString()
        {
            System.Drawing.Color color = (System.Drawing.Color)this;
            return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        public static implicit operator Color(HSLColor hslColor)
        {
            return new Color(hslColor);
        }

        #region Casts to/from System.Drawing.Color
        public static implicit operator System.Drawing.Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }
            return System.Drawing.Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }
        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5)  //<=??
                temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
            return temp2;
        }

        public static implicit operator HSLColor(System.Drawing.Color color)
        {
            HSLColor hslColor = new HSLColor();
            hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            return hslColor;
        }
        #endregion

        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = (HSLColor)System.Drawing.Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        public HSLColor() { }
        public HSLColor(System.Drawing.Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }
        public HSLColor(int red, int green, int blue)
        {
            SetRGB(red, green, blue);
        }
        public HSLColor(double hue, double saturation, double luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }


        #region transform
        public static float[][] HueMatrix(int hue)
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

        private static float[][] MatrixMult(float[][] a, float[][] b)
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

        private static float[][] XRotateMat(float[][] mat, float rs, float rc)
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


        private static float[][] YRotateMat(float[][] mat, float rs, float rc)
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

        private static float[][] ZRotateMat(float[][] mat, float rs, float rc)
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

        private static string MatrixToString(float[][] matrix)
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
        #endregion
    }
}