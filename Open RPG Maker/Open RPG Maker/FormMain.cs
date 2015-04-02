//TODO
//right now working on database

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ORPG.Dialogs;
using Game_Player;
using System.Diagnostics;

namespace ORPG
{
    public partial class FormMain : Form
    {
        New newDialog = new New();
        Confirm confirmDialog = new Confirm();
        Database database = new Database();
        Resources resources = new Resources();

        Boolean _fileOpen = false;
        Boolean FileOpen
        {
            get
            {
                return _fileOpen;
            }
            set
            {
                _fileOpen = value;
                EnableButtons();
                this.textureSelector.LoadMaps();
            }
        }

        public FormMain()
        {
            InitializeComponent();
            GenerateEvents();
        }

        void GenerateEvents()
        {
            this.buttonNew.Click += new EventHandler(NewProject);
            this.tsmiNew.Click += new EventHandler(NewProject);

            this.buttonOpen.Click += new EventHandler(OpenProject);
            this.tsmiOpen.Click += new EventHandler(OpenProject);

            this.tsmiClose.Click += new EventHandler(CloseProject);

            this.tsmiSave.Click += new EventHandler(SaveProject);
            this.buttonSave.Click += new EventHandler(SaveProject);

            this.tsmiDatabase.Click += new EventHandler(OpenDatabase);
            this.buttonDatabase.Click += new EventHandler(OpenDatabase);

            this.tsmiTransfer.Click += new EventHandler(Transfer);

            this.tsmiResources.Click += new EventHandler(OpenResources);
            this.buttonResources.Click += new EventHandler(OpenResources);

            this.treeViewMain.AfterLabelEdit += new NodeLabelEditEventHandler(this.textureSelector.MapRenamed);
            this.treeViewMain.AfterSelect += new TreeViewEventHandler(this.textureSelector.MapChanged);

            this.buttonBottom.Click += new EventHandler(ChangeLayer);
            this.buttonMiddle.Click += new EventHandler(ChangeLayer);
            this.buttonTop.Click += new EventHandler(ChangeLayer);
            this.buttonEvent.Click += new EventHandler(ChangeLayer);

            this.tsmiBottom.Click += new EventHandler(ChangeLayer);
            this.tsmiMiddle.Click += new EventHandler(ChangeLayer);
            this.tsmiTop.Click += new EventHandler(ChangeLayer);
            this.tsmiEvent.Click += new EventHandler(ChangeLayer);

            this.tsmiDimOther.Click += new EventHandler(ChangeDimOthers);

            this.tsmiShowAll.Click += new EventHandler(ChangeShowAll);
            this.tsmiShowThis.Click += new EventHandler(ChangeShowAll);

            this.tsmiPencil.Click += new EventHandler(ChangeDrawTool);
            this.tsmiRect.Click += new EventHandler(ChangeDrawTool);
            this.tsmiElipse.Click += new EventHandler(ChangeDrawTool);
            this.tsmiSpill.Click += new EventHandler(ChangeDrawTool);
            this.tsmiSelect.Click += new EventHandler(ChangeDrawTool);
            this.buttonPencil.Click += new EventHandler(ChangeDrawTool);
            this.buttonRect.Click += new EventHandler(ChangeDrawTool);
            this.buttonElipse.Click += new EventHandler(ChangeDrawTool);
            this.buttonSpill.Click += new EventHandler(ChangeDrawTool);
            this.buttonSelect.Click += new EventHandler(ChangeDrawTool);

            this.tsmiTest.Click += new EventHandler(TestGame);
            this.buttonTest.Click += new EventHandler(TestGame);

            this.mapEditor.OnAction += new MapEditor.OnActionHandler(UpdateUndoRedo);
            this.tsmiRedo.Click += new EventHandler(Redo);
            this.buttonRedo.Click += new EventHandler(Redo);
            this.tsmiUndo.Click += new EventHandler(Undo);
            this.buttonUndo.Click += new EventHandler(Undo);
        }

        void Redo(object sender, EventArgs e)
        {
            this.mapEditor.Redo();
        }

        void Undo(object sender, EventArgs e)
        {
            this.mapEditor.Undo();
        }

        void UpdateUndoRedo(MapEditor.Action action)
        {
            this.tsmiUndo.Enabled = this.mapEditor.CanUndo;
            this.buttonUndo.Enabled = this.mapEditor.CanUndo;

            this.tsmiRedo.Enabled = this.mapEditor.CanRedo;
            this.buttonRedo.Enabled = this.mapEditor.CanRedo;
        }

        void TestGame(object sender, EventArgs e)
        {
            SaveProject(sender, e);

            try
            {
                string path = Paths.Root + @"Game Player\Game Player.exe";
                Process p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.WorkingDirectory = Paths.Root + @"Game Player";
                p.Start();
            }
            catch 
            {
                MessageBox.Show("Could not start game.");
            }
        }

        void ChangeShowAll(object sender, EventArgs e)
        {
            bool showAll = (sender == this.tsmiShowAll);

            this.tsmiShowAll.Checked = showAll;
            this.tsmiShowThis.Checked = !showAll;

            this.mapEditor.ShowAllLayers = showAll;
        }

        void ChangeDimOthers(object sender, EventArgs e)
        {
            this.tsmiDimOther.Checked = !this.tsmiDimOther.Checked;
            this.mapEditor.DimOtherLayers = this.tsmiDimOther.Checked;
        }

        void ChangeDrawTool(object sender, EventArgs e)
        {
            ToolStripItem s = (ToolStripItem)sender;
            int tag = int.Parse(s.Tag.ToString());

            this.mapEditor.DrawTool = (DrawTool)tag;

            ToolStripItem[] items = new ToolStripItem[] {
                this.tsmiPencil,
                this.tsmiRect,
                this.tsmiElipse,
                this.tsmiSpill,
                this.tsmiSelect,
                this.buttonPencil,
                this.buttonRect,
                this.buttonElipse,
                this.buttonSpill,
                this.buttonSelect
            };

            foreach (ToolStripItem item in items)
            {
                bool c;
                c = item.Tag.ToString() == tag.ToString();
                if (item is ToolStripMenuItem)
                    ((ToolStripMenuItem)item).Checked = c;
                else
                    ((ToolStripButton)item).Checked = c;
            }    

        }

        void ChangeLayer(object sender, EventArgs e)
        {
            this.buttonBottom.Checked = false;
            this.buttonMiddle.Checked = false;
            this.buttonTop.Checked = false;
            this.buttonEvent.Checked = false;

            this.tsmiBottom.Checked = false;
            this.tsmiMiddle.Checked = false;
            this.tsmiTop.Checked = false;
            this.tsmiEvent.Checked = false;

            if (sender == this.buttonBottom || sender == this.tsmiBottom)
            {
                this.mapEditor.Layer = 0;
                this.buttonBottom.Checked = true;
                this.tsmiBottom.Checked = true;
            }
            if (sender == this.buttonMiddle || sender == this.tsmiMiddle)
            {
                this.mapEditor.Layer = 1;
                this.buttonMiddle.Checked = true;
                this.tsmiMiddle.Checked = true;
            }
            if (sender == this.buttonTop || sender == this.tsmiTop)
            {
                this.mapEditor.Layer = 2;
                this.buttonTop.Checked = true;
                this.tsmiTop.Checked = true;
            }
            if (sender == this.buttonEvent || sender == this.tsmiEvent)
            {
                this.mapEditor.Layer = 3;
                this.buttonEvent.Checked = true;
                this.tsmiEvent.Checked = true;
            }
        }

        void OpenResources(object o, EventArgs e)
        {
            resources.ShowDialog();
        }

        void Transfer(object o, EventArgs e)
        {
            string oldFilter = this.openFileDialog.Filter;
            this.openFileDialog.Filter = @"Tranfer Files|*.orpgtrans|All Files|*.*";

            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Game_Player.Scanner.Load(this.openFileDialog.FileName);
            }

            this.openFileDialog.Filter = oldFilter;
        }

        void NewProject(object o, EventArgs e)
        {
            if (CheckOpen())
                return;
            int n = 1;
            while (Directory.Exists(Paths.Root + "Project" + n.ToString()))
            { n++; }
            newDialog.ProjectName = "Project" + n.ToString();
            newDialog.GameTitle = "Project" + n.ToString();
            newDialog.ProjectPath = Paths.Root;
            newDialog.ShowDialog();
            if (newDialog.DialogResult == DialogResult.OK)
            {
                try
                {
                    string path = newDialog.ProjectPath + newDialog.ProjectName;
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "\\Audio");
                    foreach (string p in Paths.AudioPaths)
                    {
                        Directory.CreateDirectory(path + "\\Audio\\" + p);
                    }
                    Directory.CreateDirectory(path + "\\Graphics");
                    foreach (string p in Paths.GraphicsPaths)
                    {
                        Directory.CreateDirectory(path + "\\Graphics\\" + p);
                    }
                    Directory.CreateDirectory(path + "\\Data");
                    File.Create(path + "\\" + newDialog.ProjectName + ".orpgproj");
                    Game_Player.Paths.Load(path);
                    FileOpen = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error in creating your project:\n" + 
                        ex.Message);
                }
            }
        }

        void OpenProject(object o, EventArgs e)
        {
            if (CheckOpen())
                return;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String dir = openFileDialog.FileName;
                int cut = dir.LastIndexOf('\\');
                Game_Player.Paths.Load(dir.Substring(0, cut + 1));
                Game_Player.Data.Load(Paths.DataDir);
                FileOpen = true;
            }
        }

        void CloseProject(object o, EventArgs e)
        {
            if (CheckOpen())
                return;
            Paths.Root = "";
            FileOpen = false;
        }

        void SaveProject(object o, EventArgs e)
        {
            Game_Player.Data.Save(Paths.DataDir);
        }

        void OpenDatabase(object o, EventArgs e)
        {
            database.ShowDialog();
        }

        Boolean CheckOpen()
        {
            if (FileOpen)
            {
                confirmDialog.ShowDialog("The current project will be closed. Continue?");
                if (confirmDialog.DialogResult != DialogResult.OK)
                    return true;
            }
            return false;
        }

        void EnableButtons()
        {
            this.tsmiSave.Enabled = FileOpen;
            this.buttonSave.Enabled = FileOpen;
            this.tsmiClose.Enabled = FileOpen;

            this.tsmiBottom.Enabled = FileOpen;
            this.buttonBottom.Enabled = FileOpen;
            this.tsmiMiddle.Enabled = FileOpen;
            this.buttonMiddle.Enabled = FileOpen;
            this.tsmiTop.Enabled = FileOpen;
            this.buttonTop.Enabled = FileOpen;
            this.tsmiEvent.Enabled = FileOpen;
            this.buttonEvent.Enabled = FileOpen;

            this.tsmiShowAll.Enabled = FileOpen;
            this.tsmiShowThis.Enabled = FileOpen;
            this.tsmiDimOther.Enabled = FileOpen;

            this.tsmiZoomIn.Enabled = FileOpen;
            this.buttonZoomIn.Enabled = FileOpen;
            this.tsmiZoomOut.Enabled = FileOpen;
            this.buttonZoomOut.Enabled = FileOpen;

            this.tsmiDatabase.Enabled = FileOpen;
            this.buttonDatabase.Enabled = FileOpen;
            this.tsmiResources.Enabled = FileOpen;
            this.buttonResources.Enabled = FileOpen;
            this.tsmiScript.Enabled = FileOpen;
            this.buttonScript.Enabled = FileOpen;
            this.tsmiMusic.Enabled = FileOpen;
            this.buttonMusic.Enabled = FileOpen;

            this.tsmiTest.Enabled = FileOpen;
            this.buttonTest.Enabled = FileOpen;
            this.tsmiTitle.Enabled = FileOpen;
            this.tsmiRTP.Enabled = FileOpen;
            this.tsmiFolder.Enabled = FileOpen;

            this.tsmiPencil.Enabled = FileOpen;
            this.buttonPencil.Enabled = FileOpen;
            this.tsmiRect.Enabled = FileOpen;
            this.buttonRect.Enabled = FileOpen;
            this.tsmiElipse.Enabled = FileOpen;
            this.buttonElipse.Enabled = FileOpen;
            this.tsmiSpill.Enabled = FileOpen;
            this.buttonSpill.Enabled = FileOpen;
            this.tsmiSelect.Enabled = FileOpen;
            this.buttonSelect.Enabled = FileOpen;

            this.splitContainerLeft.Visible = FileOpen;
            if (FileOpen)
                this.tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            else
            {
                this.tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                this.mapEditor.MapId = 0;
            }
        }

    }
}