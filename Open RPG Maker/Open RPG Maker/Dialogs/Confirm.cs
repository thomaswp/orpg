using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ORPG.Dialogs
{
    public partial class Confirm : Form
    {
        public Confirm()
        {
            InitializeComponent();
            this.buttonCancel.Click += new EventHandler(CloseCancel);
            this.buttonOk.Click += new EventHandler(CloseOK);
        }

        public DialogResult ShowDialog(String message)
        {
            this.labelMessage.Text = message;
            return this.ShowDialog();
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