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
    public partial class BasicStatistics : Form
    {
        public int SelectedTab
        {
            get { return this.tabControlMain.SelectedIndex; }
            set { this.tabControlMain.SelectedIndex = value; }
        }

        public int[][] Values
        {
            get
            {
                return new int[][] {
                    statHP.Values,
                    statSP.Values,
                    statStr.Values,
                    statDex.Values,
                    statAgi.Values,
                    statInt.Values };
            }
            set
            {
                statHP.Values = value[0];
                statSP.Values = value[1];
                statStr.Values = value[2];
                statDex.Values = value[3];
                statAgi.Values = value[4];
                statInt.Values = value[5];
            }
        }

        public ORPG.Stat[] Stats
        {
            get
            {
                return new ORPG.Stat[]
                {
                    statHP,
                    statSP,
                    statStr,
                    statDex,
                    statAgi,
                    statInt
                };
            }
        }

        int lastIndex = 0;

        public BasicStatistics()
        {
            InitializeComponent();
            this.buttonCancel.Click += new EventHandler(CloseCancel);
            this.buttonOk.Click += new EventHandler(CloseOK);
            this.tabControlMain.SelectedIndexChanged += new EventHandler(TabChanged);
        }

        void TabChanged(object sender, EventArgs e)
        {
            Stats[SelectedTab].Level = Stats[lastIndex].Level;
            lastIndex = SelectedTab;
        }

        public new DialogResult ShowDialog()
        {
            return base.ShowDialog();
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