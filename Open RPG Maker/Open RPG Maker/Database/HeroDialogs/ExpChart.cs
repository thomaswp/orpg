using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ORPG.HeroDialogs
{
    public partial class ExpChart : UserControl
    {
        int _expBase = 25;
        /// <summary>
        /// The experience base multiplier.
        /// </summary>
        public int ExpBase
        {
            get { return _expBase; }
            set
            {
                _expBase = Math.Min(Math.Max(10, value), 50);
                RefreshData();
            }
        }

        int _expSteep = 25;
        /// <summary>
        /// The experience steepness exponentiator.
        /// </summary>
        public int ExpSteep
        {
            get { return _expSteep; }
            set
            {
                _expSteep = Math.Min(Math.Max(10, value), 50);
                RefreshData();
            }
        }

        bool _total = false;
        /// <summary>
        /// Total experience needed or just for next level.
        /// </summary>
        public bool Total
        {
            get { return _total; }
            set
            {
                _total = value;
                RefreshData();
            }
        }

        Color _color = Color.Red;
        /// <summary>
        /// The color of the numbers.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RefreshData();
            }
        }

        public ExpChart()
        {
            InitializeComponent();

            this._expBase = 29;
            this._expSteep = 32;
            Curve(2);
        }

        void RefreshData()
        {
            //columns and rows
            const int COLUMNS = 5, ROWS = 20;

            richTextBox.Text = "";

            //the start string for rtf data
            String text = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fmodern\fprq1\fcharset0 Courier New;}{\f1\fswiss\fcharset0 Arial;}}" + "\n";
            //the color data (for color code /cf1)
            text += @"{\colortbl ;" + 
                @"\red" + Color.R.ToString() + 
                @"\green" + Color.G.ToString() + 
                @"\blue" + Color.B.ToString() + ";}";
            //the text size (8 * 2 = 16)
            text += @"\fs16" + "\n";

            //for each row and column
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    //get the number this represents (we can't go in
                    //order because it's text)
                    int n = i + j * ROWS + 1;
                    //if this is a valid number
                    if (n <= 98 || (n <= 99 && Total))
                    {
                        double value;
                        //set the value for this level... the next level must
                        //actualyl for for the "next level" so add one
                        if (Total)
                            value = Curve(n);
                        else
                            value = Curve(n + 1);

                        //get teh strings with padding
                        String nStr = "L" + n.ToString().PadLeft(2) + ":";
                        String vStr = ((int)value).ToString().PadLeft(8);

                        //add it to the text
                        text += @"\cf0 " + nStr + @"\cf1 " + vStr;

                        //add needed spacing
                        if (j != COLUMNS - 1)
                            text += "  ";
                    }
                }
                //add a return
                if (i != ROWS - 1)
                    text += @"\par";
            }
            //finish of the rtf coding
            text += "}";

            //replace the rtf coding
            richTextBox.Rtf = text;
        }

        double Curve(int n)
        {
            //this is the formula in RMXP
            double pow = 2.4 + ExpSteep / 100.0;
            double exp = ExpBase * Math.Pow(n + 3, pow) / Math.Pow(5, pow);
            exp = Math.Round(exp, 5);

            //if this is just for the level, return it
            if (!Total)
                return exp;
            else
            {
                //otherwise we need to add the previous levels to get the total
                if (n == 1)
                    return 0;
                else
                    return exp + Curve(n - 1);
            }
        }
    }
}
