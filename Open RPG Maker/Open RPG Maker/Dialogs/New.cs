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
    public partial class New : Form
    {
        string lastName = "";

        string _projectPath;
        public string ProjectPath 
        { 
            get 
            { return _projectPath; }
            set 
            {
                _projectPath = value;
                this.textBoxPath.Text = value + ProjectName;
            }
        }

        public string ProjectName
        {
            get { return this.textBoxName.Text; }
            set 
            { 
                this.textBoxName.Text = value;
                this.textBoxPath.Text = ProjectPath + value;
            }
        }

        public string GameTitle
        {
            get { return this.textBoxTitle.Text; }
            set { this.textBoxTitle.Text = value; }
        }

        public New()
        {
            InitializeComponent();
            this.buttonCreate.Click += new EventHandler(Create);
            this.buttonCancel.Click += new EventHandler(Close);
            this.buttonPath.Click += new EventHandler(Browse);
            this.textBoxName.TextChanged += new EventHandler(NameChanged);
            lastName = this.textBoxName.Text;
        }

        void NameChanged(object o, EventArgs e)
        {
            if (GameTitle == lastName)
            {
                GameTitle = ProjectName;
            }
            lastName = ProjectName;
            ProjectPath = ProjectPath;
        }

        void Create(object o, EventArgs e)
        {
            if (Directory.Exists(ProjectPath + "\\" + ProjectName + "\\"))
            { 
                Game_Player.MsgBox.Show("The selected directory already exists"); 
            }
            else
            {
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    if (ProjectName.IndexOf(c) != -1)
                    {
                        Game_Player.MsgBox.Show(
                            "Invalid file name: '" + c + "' is an illigal character for files!");
                        return;
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        void Close(object o, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        void Browse(object o, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = ProjectPath;
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath != null)
            { ProjectPath = this.folderBrowserDialog.SelectedPath + "\\"; }
        }
    }
}