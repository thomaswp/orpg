using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataClasses;

namespace ORPG
{
    public partial class TextureSelector : ScrollableControl
    {
        public MapEditor MapEditor { get; set; }

        int tilesetId;
        public int TilesetId
        {
            get { return tilesetId; }
            set
            {
                tilesetId = value;

                if (tilesetId > 0)
                {
                    LoadTexture();

                    this.Size = new Size(this.Width, Texture.Height);
                    this.AutoScrollMinSize = new Size(0, Texture.Height);
                    this.pictureBoxTexture.Visible = true;
                }
                else
                {
                    this.AutoScrollMinSize = new Size(0, 0);
                    this.pictureBoxTexture.Visible = false;
                }
            }
        }

        Tileset Tileset { get { return Game_Player.Data.Tilesets[tilesetId]; } }

        Bitmap Texture
        {
            get { return (Bitmap)this.pictureBoxTexture.Image; }
            set { this.pictureBoxTexture.Image = value; }
        }

        Rectangle selection;
        public Rectangle Selection
        {
            get { return selection; }
            set
            {
                selection = value;
                Refresh();
            }
        }

        int row = 0;
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        int column = 0;
        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        TreeView mapSelector;
        public TreeView MapSelector
        {
            get { return mapSelector; }
            set { mapSelector = value; }
        }

        private bool mouseDown = false;
        private Point mouseStart;

        private string[] exts { get { return new string[] { "", ".jpg.", ".jpeg", ".bmp", ".png" }; } }
        private string[] paths { get { return new string[] { Game_Player.Paths.Root, Game_Player.Paths.RTP }; } }

        public TextureSelector()
        {
            InitializeComponent();
        }

        void LoadTexture()
        {
            string path = Game_Player.Paths.FindValidPath(@"Graphics\Tilesets\" + Tileset.tilesetName, paths, exts);
            Bitmap bmp = new Bitmap(path);
            Texture = new Bitmap(bmp.Width, bmp.Height + 33);

            LoadAutoTile();

            Graphics g = Graphics.FromImage(Texture);
            g.DrawImage(bmp, new Point(0, 32));
        }

        void LoadAutoTile()
        {
            string[] names = Tileset.autotileNames;
            Graphics g = Graphics.FromImage(Texture);

            g.Clear(Color.FromArgb(255, 246, 246, 246));

            for (int i = 0; i < names.Length; i++)
            {
                string name = names[i];
                Bitmap tile = new Bitmap(Game_Player.Paths.FindValidPath(@"Graphics\Autotiles\" + name, paths, exts));

                g.DrawImage(tile, new Rectangle(32 * (i + 1), 0, 32, 32), new Rectangle(0, 0, 32, 32), GraphicsUnit.Pixel);
            }
        }

        public void LoadMaps()
        {
            mapSelector.Nodes.Clear();

            TreeNode title = mapSelector.Nodes.Add("0", Game_Player.Paths.Title, 0);
            TreeNode[] nodes = new TreeNode[Game_Player.Data.Maps.Length + 1];
            nodes[0] = title;

            foreach (Map m in Game_Player.Data.Maps)
            {
                nodes[m.id] = nodes[m.parentId].Nodes.Add(m.id.ToString(), m.name, 1);
                nodes[m.id].SelectedImageIndex = 1;
            }

            nodes[0].ExpandAll();
        }

        public void MapRenamed(object sender, NodeLabelEditEventArgs e)
        {
            int id = int.Parse(e.Node.Name);
            string name = e.Label;

            if (id == 0)
                Game_Player.Paths.Title = name; //doessn't save yet to file
            else
                Game_Player.Data.Maps[id].name = name;
        }

        public void MapChanged(object sender, TreeViewEventArgs e)
        {
            int id = int.Parse(e.Node.Name);

            if (id == 0)
                TilesetId = 0;
            else
                TilesetId = Game_Player.Data.Maps[id].tilesetId;

            if (Selection.IsEmpty)
                Selection = new Rectangle(0, 0, 32, 32);

            MapEditor.MapId = id;
        }

        private void pictureBoxTexture_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int n = 0;
            g.DrawRectangle(Pens.Black, selection.X + n + 1, selection.Y + n + 1, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1); n++;
            g.DrawRectangle(Pens.White, selection.X + n + 1, selection.Y + n + 1, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1); n++;
            g.DrawRectangle(Pens.White, selection.X + n + 1, selection.Y + n + 1, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1); n++;
            g.DrawRectangle(Pens.Black, selection.X + n + 1, selection.Y + n + 1, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1);
        }

        private void pictureBoxTexture_MouseDown(object sender, MouseEventArgs e)
        {
            mouseStart = new Point(e.X, e.Y);
            mouseDown = true;
            UpdateSelection(e);
        }

        private void pictureBoxTexture_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateSelection(e);
        }

        void UpdateSelection(MouseEventArgs e)
        {
            if (mouseDown)
            {
                int x1 = e.X, x2 = mouseStart.X, y1 = e.Y, y2 = mouseStart.Y;

                Point start = new Point(Math.Max(0, Math.Min(x1, x2)), Math.Max(0, Math.Min(y1, y2)));
                start = new Point((int)(start.X / 32) * 32, (int)(start.Y / 32) * 32);

                Point end = new Point(Math.Min(Texture.Width - 1, Math.Max(x1, x2)), Math.Min(Texture.Height - 1, Math.Max(y1, y2)));
                end = new Point((int)(end.X / 32 + 1) * 32, (int)(end.Y / 32 + 1) * 32);

                Selection = new Rectangle(start.X, start.Y, end.X - start.X, end.Y - start.Y);
            }
        }

        private void pictureBoxTexture_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
