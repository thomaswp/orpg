using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Game_Player;

namespace ORPG.Dialogs
{
    public partial class Resources : Form
    {
        enum Icons { Folder = 0, Blue = 1, Red = 2 };

        String[][] folders = new String[Paths.GraphicsPaths.Length + Paths.AudioPaths.Length][];
        int audioStart = Paths.GraphicsPaths.Length;
        Preview preview = new Preview();
        Confirm confirm = new Confirm();

        public Resources()
        {
            InitializeComponent();
            InitializeEvents();

        }

        void InitializeEvents()
        {
            this.listViewFolder.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(SelectionChanged);

            this.listViewImage.DoubleClick += new EventHandler(PreviewItem);
            this.buttonPreview.Click += new EventHandler(PreviewItem);

            this.listViewImage.SelectedIndexChanged += new EventHandler(ItemSelected);
            this.listViewFolder.SelectedIndexChanged += new EventHandler(ItemSelected);

            this.buttonExport.Click += new EventHandler(Export);

            this.buttonDelete.Click += new EventHandler(Delete);

            this.buttonImport.Click += new EventHandler(Import);

            this.buttonClose.Click += new EventHandler(Close);
        }

        void Close(object sender, EventArgs e)
        {
            Close();
        }

        void Import(object sender, EventArgs e)
        {
            if (this.listViewFolder.SelectedIndices.Count != 1)
                return;

            int index = this.listViewFolder.SelectedIndices[0];
            string path = Paths.Root + this.listViewFolder.SelectedItems[0].Text + "\\";

            string filter;
            if (index < audioStart)
            {
                filter = "Graphics Files|*.bmp; *.jpg; *.jpeg; *.png|All Files|*.*";
            }
            else
            {
                filter = "Audio Files|*mid; *.midi; *.ogg; *.wav; *.wma|All Files|*.*";
            }

            this.openFileDialog.Filter = filter;
            if (this.openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string source = this.openFileDialog.FileName;
            int slash = source.LastIndexOf('\\');
            string name = source.Substring(slash + 1, source.Length - slash - 1);

            try
            {
                Image img = this.pictureBoxPreview.Image;
                this.pictureBoxPreview.Image = null;
                if (img != null)
                    img.Dispose();
                File.Copy(source, path + name);
                RefreshResources();
                this.listViewFolder.Items[index].Selected = true;
            }
            catch
            {
                MsgBox.Show("Failed to import file.");
            }
        }

        void Delete(object sender, EventArgs e)
        {
            if (this.listViewFolder.SelectedIndices.Count != 1 ||
                this.listViewImage.SelectedIndices.Count != 1)
                return;

            string[] paths = folders[this.listViewFolder.SelectedIndices[0]];
            string path = paths[this.listViewImage.SelectedIndices[0]];

            if (confirm.ShowDialog("Are you sure you want to delete this file?") != DialogResult.OK)
                return;

            try
            {
                Image img = this.pictureBoxPreview.Image;
                this.pictureBoxPreview.Image = null;
                if (img != null)
                    img.Dispose();
                GC.Collect();
                File.Delete(path);
                int index = this.listViewFolder.SelectedIndices[0];
                RefreshResources();
                this.listViewFolder.Items[index].Selected = true;
            }
            catch
            {
                MsgBox.Show("Failed to delete file.");
            }
        }

        void Export(object sender, EventArgs e)
        {
            if (this.listViewFolder.SelectedIndices.Count != 1 ||
                this.listViewImage.SelectedIndices.Count != 1)
                return;

            string[] paths = folders[this.listViewFolder.SelectedIndices[0]];
            string path = paths[this.listViewImage.SelectedIndices[0]];

            int dot = path.LastIndexOf('.');
            if (dot == -1)
                return;
            string ext = path.Substring(dot, path.Length - dot);

            saveFileDialog.Filter = "File Type|*" + ext + "|All files|*.*";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string newPath = saveFileDialog.FileName;
                File.Copy(path, newPath);
            }
            catch
            {
                MsgBox.Show("Failed to export file.");
            }
        }

        void UpdateButtons()
        {
            int index = this.listViewFolder.SelectedIndices[0];
            this.buttonPreview.Enabled = index < audioStart;
            bool export = false;
            try
            {
                string[] paths = folders[this.listViewFolder.SelectedIndices[0]];
                string path = paths[this.listViewImage.SelectedIndices[0]];
                export = File.Exists(path);
            }
            catch
            {
                export = false;
            }
            this.buttonExport.Enabled = export;
            this.buttonDelete.Enabled = export & this.listViewImage.SelectedItems[0].ImageIndex == (int)Icons.Red;
            this.buttonImport.Enabled = index >= 0 && index < folders.Length;

        }

        void ItemSelected(object sender, EventArgs e)
        {
            if (this.listViewFolder.SelectedIndices.Count != 1 ||
                this.listViewImage.SelectedIndices.Count != 1)
                return;

            UpdateButtons();

            if (this.listViewFolder.SelectedIndices[0] >= audioStart)
                return;

            string[] paths = folders[this.listViewFolder.SelectedIndices[0]];
            string path = paths[this.listViewImage.SelectedIndices[0]];

            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(path);
                this.pictureBoxPreview.Image = bmp;
            }
            catch
            {
                this.pictureBoxPreview.Image = null;
            }
        }

        void PreviewItem(object sender, EventArgs e)
        {
            if (this.listViewFolder.SelectedIndices.Count != 1 ||
                this.listViewImage.SelectedIndices.Count != 1)
                return;

            if (this.listViewFolder.SelectedIndices[0] >= audioStart)
                return;

            string[] paths = folders[this.listViewFolder.SelectedIndices[0]];
            string path = paths[this.listViewImage.SelectedIndices[0]];
            
            preview.ShowDialog(path);
        }

        void SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.listViewImage.Clear();

            int index = e.ItemIndex;
            string[] paths = folders[index];

            foreach (string path in paths)
            {
                int icon;
                if (path.IndexOf(Paths.RTP) != -1)
                    icon = (int)Icons.Blue;
                else
                    icon = (int)Icons.Red;

                int slash = path.LastIndexOf('\\');
                string s = path.Substring(slash + 1, path.Length - slash - 1);
                this.listViewImage.Items.Add(new ListViewItem(s, icon));
            }

            this.pictureBoxPreview.Image = null;
            if (this.listViewImage.Items.Count > 0)
                this.listViewImage.Items[0].Selected = true;
        }

        public new void ShowDialog()
        {
            RefreshResources();
            base.ShowDialog();
        }

        public void RefreshResources()
        {
            int folderIndex = -1, imageIndex = -1;

            if (listViewFolder.SelectedIndices.Count != 0)
                folderIndex = this.listViewFolder.SelectedIndices[0];
            if (listViewImage.SelectedIndices.Count != 0)
                imageIndex = this.listViewImage.SelectedIndices[0];

            this.listViewFolder.Clear();
            this.listViewImage.Clear();

            //for graphics...
            for (int j = 0; j < Paths.GraphicsPaths.Length + Paths.AudioPaths.Length; j++)
            {
                string prefix;
                string s;
                int index;
                if (j < audioStart)
                {
                    index = j;
                    prefix = "Graphics\\";
                    s = Paths.GraphicsPaths[index];
                }
                else
                {
                    index = j - audioStart;
                    prefix = "Audio\\";
                    s = Paths.AudioPaths[index];
                }

                //add the item
                this.listViewFolder.Items.Add(new ListViewItem(prefix + s, (int)Icons.Folder));

                //get the player's files
                String[] f1, f2;

                if (Directory.Exists(Paths.Root + prefix + s))
                    f1 = Directory.GetFiles(Paths.Root + prefix + s);
                else
                    f1 = new String[] { };
                //and then the RTP's
                if (Directory.Exists(Paths.RTP + prefix + s))
                    f2 = Directory.GetFiles(Paths.RTP + prefix + s);
                else
                    f2 = new String[] { };

                //concate them
                String[] files = new String[f1.Length + f2.Length];
                for (int i = 0; i < f1.Length; i++)
                    files[i] = f1[i];
                for (int i = f1.Length; i < files.Length; i++)
                    files[i] = f2[i - f1.Length];

                //store them
                folders[j] = files;
            }
        }
    }
}
