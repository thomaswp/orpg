using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ORPG.HeroDialogs
{
    public partial class Experience : Form
    {
        public int ExpBase 
        { 
            get { return (int)this.nudBase.Value; }
            set { this.nudBase.Value = Math.Min(Math.Max(10, value), 50); }
        }

        public int ExpSteep 
        { 
            get { return (int)this.nudSteep.Value; }
            set { this.nudSteep.Value = Math.Min(Math.Max(10, value), 50); }
        }

        public Experience()
        {
            InitializeComponent();

            this.nudBase.ValueChanged += new EventHandler(nudBase_ValueChanged);
            this.nudSteep.ValueChanged += new EventHandler(nudSteep_ValueChanged);
            this.buttonOk.Click += new EventHandler(CloseOK);
            this.buttonCancel.Click += new EventHandler(CloseCancel);
        }

        public new DialogResult ShowDialog()
        {
            return base.ShowDialog();
        }

        void nudSteep_ValueChanged(object sender, EventArgs e)
        {
            this.expChartTotal.ExpSteep = ExpSteep;
            this.expChartNext.ExpSteep = ExpSteep;
        }

        void nudBase_ValueChanged(object sender, EventArgs e)
        {
            this.expChartTotal.ExpBase = ExpBase;
            this.expChartNext.ExpBase = ExpBase;
        }

        public void CloseOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        public void CloseCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}