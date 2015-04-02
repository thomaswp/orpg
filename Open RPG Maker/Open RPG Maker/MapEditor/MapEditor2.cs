using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataClasses;
using System.Drawing.Imaging;

//No idea how to do adative/negative blending, but otherwise not bad

namespace ORPG
{
    using Sprite = Game_Player.Sprite;

    //public enum DrawTool { Pencil = 0, Rect = 1, Elipse = 2, Spill = 3, Select = 4 }

    public partial class MapEditor2 : UserControl
    {
        private int layer;
        public int Layer
        {
            get { return layer; }
            set 
            { 
                layer = value;
                Selection = new Rectangle();
                if (mapId > 0)
                    LoadLayer();
            }
        }

        private int TilesetId { get { return Map.tilesetId; } }

        private Tileset Tileset { get { return Game_Player.Data.Tilesets[TilesetId]; } }

        private int mapId;
        public int MapId
        {
            get { return mapId; }
            set
            {
                mapId = value;
                if (mapId > 0)
                {
                    this.Size = new Size(ImageWidth, ImageHeight);
                    this.AutoScrollMinSize = new Size(ImageWidth, ImageHeight);
                    this.xnaHost.Visible = true;

                    LoadMap();
                }
                else
                {
                    this.AutoScrollMinSize = new Size(0, 0);
                    this.xnaHost.Visible = false;
                }
            }
        }

        private DrawTool drawTool;
        public DrawTool DrawTool
        {
            get { return drawTool; }
            set
            {
                drawTool = value;
            }
        }

        private Map Map { get { return Game_Player.Data.Maps[mapId]; } }

        private Rectangle selection;
        public Rectangle Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        private bool showAllLayers = true;
        public bool ShowAllLayers
        {
            get { return showAllLayers; }
            set 
            { 
                showAllLayers = value;

                if (mapId > 0)
                    LoadLayer();
            }
        }

        private bool dimOtherLayers = true;
        public bool DimOtherLayers
        {
            get { return dimOtherLayers; }
            set
            {
                dimOtherLayers = value;

                if (mapId > 0)
                    LoadLayer();
            }
        }

        public delegate void OnActionHandler(Action action);
        public event OnActionHandler OnAction;

        public TextureSelector TextureSelector { get; set; }

        private Rectangle TextureSelection { get { return TextureSelector.Selection; } }

        private int ImageWidth { get { return Map.width * 32; } }
        private int ImageHeight { get { return Map.height * 32; } }

        private Rectangle ISelection
        {
            get
            {
                Rectangle rect = selection;
                rect.X /= 32;
                rect.Y /= 32;
                rect.Width /= 32;
                rect.Height /= 32;
                return rect;
            }
        }

        public bool CanUndo { get { return actionIndex > 0; } }
        public bool CanRedo { get { return actionIndex < actions.Count; } }

        private bool mouseDown = false;
        private Point mouseStart;

        private string[] exts { get { return new string[] { "", ".jpg.", ".jpeg", ".bmp", ".png" }; } }
        private string[] paths { get { return new string[] { Game_Player.Paths.Root, Game_Player.Paths.RTP }; } }

        private const int NORM_INDEX = AutoTiles.NORM_INDEX;

        private Bitmap[] layers = new Bitmap[4];
        private Bitmap grid;
        private Game_Player.Bitmap selectionBmp;
        private Sprite[] sprites = new Sprite[6];
        private Sprite background;

        private bool[,] needRefresh;
        private int[,] saveLayer;

        public class Action
        {
            public int layer;
            public int[, ,] before, after;

            public Action(int layer, int[, ,] before)
            {
                this.layer = layer;
                this.before = (int[, ,])before.Clone();
            }

            public void finish(int[, ,] after)
            {
                this.after = (int[, ,])after.Clone();
            }
        }

        private int actionIndex;
        private Action currentAction;
        private List<Action> actions;

        private Point lastMouse;

        public MapEditor2()
        {
            InitializeComponent();
        }

        public void Undo()
        {
            if (CanUndo)
            {
                actionIndex--;
                Action action = actions[actionIndex];
                SetMapData(action.before, action.layer);

                OnAction(action);
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                Action action = actions[actionIndex];
                actionIndex++;
                SetMapData(action.after, action.layer);

                OnAction(action);
            }
        }

        private void doAction()
        {
            if (currentAction != null)
            {
                currentAction.finish(Map.data);
                while (actions.Count > actionIndex)
                    actions.RemoveAt(actions.Count - 1);
                actions.Add(currentAction);
                actionIndex++;
                OnAction(currentAction);
                currentAction = null;
            }
        }

        private void pictureBoxMap_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            //SaveAction();
            doAction();
        }

        private void pictureBoxMap_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateSelection(e);
        }

        private void pictureBoxMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (Layer < 3)
            {
                currentAction = new Action(layer, Map.data);

                mouseStart = new Point(e.X, e.Y);
                lastMouse = new Point(-1, -1);
                mouseDown = true;

                CreateSaveLayer();
                UpdateSelection(e);

            }
        }

        private void xnaHost_MouseLeave(object sender, EventArgs e)
        {
            sprites[5].Visible = false;
            Refresh();
        }

        private void DrawSelection()
        {
            selectionBmp.Clear();

            int n = 0;
            selectionBmp.DrawRect(selection.X + n, selection.Y + n, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1, Game_Player.Colors.Black); n++;
            selectionBmp.DrawRect(selection.X + n, selection.Y + n, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1, Game_Player.Colors.White); n++;
            selectionBmp.DrawRect(selection.X + n, selection.Y + n, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1, Game_Player.Colors.White); n++;
            selectionBmp.DrawRect(selection.X + n, selection.Y + n, selection.Width - 2 * n - 1, selection.Height - 2 * n - 1, Game_Player.Colors.Black); n++;

            Refresh();
        }

        private void UpdateSelection(MouseEventArgs e)
        {
 
            if (DrawTool == DrawTool.Pencil)
            {
                selection.X = e.X / 32 * 32;
                selection.Y = e.Y / 32 * 32;
                selection.Width = TextureSelector.Selection.Width;
                selection.Height = TextureSelector.Selection.Height;
            }
            if (DrawTool == DrawTool.Rect || DrawTool == DrawTool.Elipse)
            {
                if (mouseDown)
                {
                    int x1 = e.X, x2 = mouseStart.X, y1 = e.Y, y2 = mouseStart.Y;

                    Point start = new Point(Math.Max(0, Math.Min(x1, x2)), Math.Max(0, Math.Min(y1, y2)));
                    start = new Point((int)(start.X / 32) * 32, (int)(start.Y / 32) * 32);

                    Point end = new Point(Math.Min(ImageWidth - 1, Math.Max(x1, x2)), Math.Min(ImageHeight - 1, Math.Max(y1, y2)));
                    end = new Point((int)(end.X / 32 + 1) * 32, (int)(end.Y / 32 + 1) * 32);

                    selection = new Rectangle(start.X, start.Y, end.X - start.X, end.Y - start.Y);
                }
                else
                {
                    selection.X = e.X / 32 * 32;
                    selection.Y = e.Y / 32 * 32;
                    selection.Width = 32;
                    selection.Height = 32;
                }
            }
            if (DrawTool == DrawTool.Spill)
            {
                selection.X = e.X / 32 * 32;
                selection.Y = e.Y / 32 * 32;
                selection.Width = 32;
                selection.Height = 32;
            }

            sprites[5].Visible = DrawTool == DrawTool.Pencil || !mouseDown;

            selection.X = Math.Max(0, selection.X);
            selection.Y = Math.Max(0, selection.Y);
            selection.Width = Math.Min(ImageWidth - selection.X, selection.Width);
            selection.Height = Math.Min(ImageHeight - selection.Y, selection.Height);

            Point mouseI = new Point(e.X / 32, e.Y / 32);
            if (mouseI != lastMouse)
                UpdateMap();

            lastMouse = mouseI;

            DrawSelection();
        }

        private void CreateSaveLayer()
        {
            for (int i = 0; i < Map.width; i++)
            {
                for (int j = 0; j < Map.height; j++)
                {
                    saveLayer[i, j] = Map.data[i, j, layer];
                }
            }
        }

        private void UpdateMap()
        {
            if (mouseDown)
            {
                Bitmap tilesetBmp = GetTilesetBitmap();

                if (drawTool == DrawTool.Pencil)
                {
                    for (int i = 0; i < ISelection.Width; i++)
                    {
                        for (int j = 0; j < ISelection.Height; j++)
                        {
                            int selectX = i + TextureSelection.X / 32;
                            int selectY = j + TextureSelection.Y / 32;

                            int x = ISelection.X + i;
                            int y = ISelection.Y + j;
                            int tileId;

                            if (selectY > 0)
                                tileId = (selectY - 1) * 8 + selectX + NORM_INDEX;
                            else if (selectX > 0)
                                tileId = selectX * 48;
                            else
                                tileId = 0;

                            SetMapData(x, y, tileId);
                        }
                    }
                    
                    for (int i = -1; i < ISelection.Width + 1; i++)
                    {
                        for (int j = -1; j < ISelection.Height + 1; j++)
                        {
                            int x = selection.X / 32 + i;
                            int y = selection.Y / 32 + j;
                            if (OnMap(x, y))
                            {
                                int newId = AutoTiles.AutoTileId(Map, layer, x, y, Map.data[x, y, layer]);
                                SetMapData(x, y, newId);
                            }
                        }
                    }
                }
                if (DrawTool == DrawTool.Rect || DrawTool == DrawTool.Elipse)
                {
                    for (int x = 0; x < Map.width; x++)
                    {
                        for (int y = 0; y < Map.height; y++)
                        {
                            bool draw;
                            if (DrawTool == DrawTool.Rect)
                                draw = ISelection.Contains(new Point(x, y));
                            else
                            {
                                double cX = ISelection.X + (ISelection.Width - 1) / 2.0;
                                double cY = ISelection.Y + (ISelection.Height - 1) / 2.0;
                                double dx = (x - cX) / ISelection.Width * 2;
                                double dy = (y - cY) / ISelection.Height * 2;
                                draw = dx * dx + dy * dy <= 1;
                            }

                            if (draw)
                            {
                                int offX, offY;
                                if (x > mouseStart.X / 32)
                                    offX = x - ISelection.X;
                                else if (x == mouseStart.X / 32)
                                    offX = 0;
                                else
                                    offX = x - (ISelection.Right - 1);
                                if (y > mouseStart.Y / 32)
                                    offY = y - ISelection.Y;
                                else if (y == mouseStart.Y / 32)
                                    offY = 0;
                                else
                                    offY = y - (ISelection.Bottom - 1);

                                while (offX < 0)
                                    offX += TextureSelection.Width / 32;
                                while (offY < 0)
                                    offY += (TextureSelection.Height / 32);

                                int selectX = TextureSelection.X / 32 + offX % (TextureSelection.Width / 32);
                                int selectY = TextureSelection.Y / 32 + offY % (TextureSelection.Height / 32);

                                int tileId;

                                if (selectY > 0)
                                    tileId = (selectY - 1) * 8 + selectX + NORM_INDEX;
                                else if (selectX > 0)
                                    tileId = selectX * 48;
                                else
                                    tileId = 0;

                                SetMapData(x, y, tileId);
                            }
                            else
                            {
                                SetMapData(x, y, saveLayer[x, y]);
                            }
                        }
                    }
                    for (int i = -2; i < ISelection.Width + 2; i++)
                    {
                        for (int j = -2; j < ISelection.Height + 2; j++)
                        {
                            int x = selection.X / 32 + i;
                            int y = selection.Y / 32 + j;
                            if (OnMap(x, y))
                            {
                                int newId = AutoTiles.AutoTileId(Map, layer, x, y, Map.data[x, y, layer]);
                                SetMapData(x, y, newId);
                            }
                        }
                    }
                }
                if (DrawTool == DrawTool.Spill)
                {
                    int x = ISelection.X;
                    int y = ISelection.Y;
                    int selectX = TextureSelection.X / 32;
                    int selectY = TextureSelection.Y / 32;

                    int tileId;
                    if (selectY > 0)
                        tileId = (selectY - 1) * 8 + selectX + NORM_INDEX;
                    else if (selectX > 0)
                        tileId = selectX * 48;
                    else
                        tileId = 0;

                    DrawSpill(x, y, Map.data[x,y,layer], tileId);
                }
                Redraw(tilesetBmp);
            }
        }

        private void DrawSpill(int x, int y, int from, int to)
        {
            if (!OnMap(x, y))
                return;

            if (from != 0 && from < NORM_INDEX)
            {
                if (from / 48 == to / 48)
                    return;
                if (Map.data[x, y, layer] / 48 != from / 48)
                    return;
            }
            else
            {
                if (from == to)
                    return;
                if (Map.data[x, y, layer] != from)
                    return;
            }
            

            SetMapData(x, y, to);

            int[] offX = { -1, 1, 0, 0 };
            int[] offY = { 0, 0, 1, -1 };
            for (int i = 0; i < 4; i++)
            {
                DrawSpill(x + offX[i], y + offY[i], from, to);
            }


            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int nx = x + i;
                    int ny = y + j;
                    if (OnMap(nx, ny))
                        SetMapData(nx, ny, AutoTiles.AutoTileId(Map, layer, nx, ny, Map.data[nx, ny, layer]));
                }
            }
        }

        private void SetMapData(int[, ,] data, int layer)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    SetMapData(i, j, data[i,j,layer], layer);
                }
            }
            Redraw(GetTilesetBitmap(), layer);
        }

        private void SetMapData(int x, int y, int tileId)
        {
            SetMapData(x, y, tileId, layer);
        }

        private void SetMapData(int x, int y, int tileId, int layer)
        {
            if (Map.data[x, y, layer] != tileId)
            {
                Map.data[x, y, layer] = tileId;
                needRefresh[x, y] = true;
                Console.WriteLine("[" + x + "," + y + "] set to " + tileId);
            }
        }

        private void Redraw(Bitmap tilesetBmp)
        {
            Redraw(tilesetBmp, layer);
        }

        private void Redraw(Bitmap tilesetBmp, int layer)
        {
            for (int x = 0; x < Map.width; x++)
            {
                for (int y = 0; y < Map.height; y++)
                {
                    if (needRefresh[x, y])
                    {
                        int tileId = Map.data[x, y, layer];
                        Console.WriteLine("Drawing " + "[" + x + ", " + y + "] as " + tileId);

                        Rectangle dest = new Rectangle(x * 32, y * 32, 32, 32);
                        Graphics g = Graphics.FromImage(layers[layer]);
                        g.Clip = new Region(dest);
                        g.Clear(Color.White);
                        sprites[layer].Bitmap.NeedRefresh = true;
                        needRefresh[x, y] = false;

                        if (tileId >= NORM_INDEX)
                        {
                            int tileX = (tileId - NORM_INDEX) % 8 * 32;
                            int tileY = (tileId - NORM_INDEX) / 8 * 32;
                            Rectangle source = new Rectangle(tileX, tileY, 32, 32);
                            g.DrawImage(tilesetBmp, dest, source, GraphicsUnit.Pixel);
                        }
                        else if (tileId >= 48)
                        {
                            Bitmap bmp = AutoTiles.AutoTile(tileId % 48, GetAutotileBitmap(tileId));
                            g.DrawImage(bmp, dest.Location);
                        }
                    }
                }
            }
            Refresh();
        }

        private void Redraw(Bitmap tilesetBmp, int x, int y, int width, int height, bool onlyAuto)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (i >= 0 && i < Map.width &&
                        j >= 0 && j < Map.height)
                    {
                        int tileId = Map.data[i, j, layer];

                        Rectangle dest = new Rectangle(i * 32, j * 32, 32, 32);
                        Graphics g = Graphics.FromImage(layers[layer]);
                        g.Clip = new Region(dest);

                        if (tileId >= NORM_INDEX)
                        {
                            if (!onlyAuto)
                            {
                                int tileX = (tileId - NORM_INDEX) % 8 * 32;
                                int tileY = (tileId - NORM_INDEX) / 8 * 32;
                                Rectangle source = new Rectangle(tileX, tileY, 32, 32);

                                g.Clear(Color.Transparent);
                                g.DrawImage(tilesetBmp, dest, source, GraphicsUnit.Pixel);

                                sprites[layer].Bitmap.NeedRefresh = true;
                            }
                        }
                        else if (tileId >= 48)
                        {
                            Bitmap bmp = AutoTiles.AutoTile(tileId % 48, GetAutotileBitmap(tileId));

                            g.Clear(Color.Transparent);
                            g.DrawImage(bmp, new Point(i * 32, j * 32));

                            sprites[layer].Bitmap.NeedRefresh = true;
                        }
                        else
                        {
                            g.Clear(Color.Transparent);
                        }
                    }
                }
            }
            Refresh();
        }

        private bool OnMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Map.width && y < Map.height;
        }

        private Bitmap GetAutotileBitmap(int tileId)
        {
            string tilePath = Game_Player.Paths.FindValidPath(@"Graphics\Autotiles\" +
                                Tileset.autotileNames[tileId / 48 - 1], paths, exts);
            return new Bitmap(tilePath);
        }

        private Bitmap GetTilesetBitmap()
        {
            string path = Game_Player.Paths.FindValidPath(@"Graphics\Tilesets\" + Tileset.tilesetName, paths, exts);
            return new Bitmap(path);
        }

        private Bitmap GetEventBitmap(Event evnt)
        {
            string characterName = evnt.pages[0].graphic.characterName;
            string charPath = Game_Player.Paths.FindValidPath(@"Graphics\Characters\" + characterName, paths, exts);
            return charPath != "" ? new Bitmap(charPath) : null;
        }

        private void LoadLayer()
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].Visible = false;
                sprites[i].Color = Game_Player.Colors.White;
                sprites[i].Opactiy = 255;
            }

            for (int i = 0; i < (showAllLayers ? 4 : layer + 1); i++)
            {
                sprites[i].Visible = true;

                if (!(layer == i || layer == 3 || !dimOtherLayers))
                {
                    if (i < layer)
                        sprites[i].Color = new Game_Player.Color(123, 123, 123);
                    else
                        sprites[i].Opactiy = 123;
                }
            }

            sprites[4].Visible = (layer == 3);
            sprites[4].Opactiy = 255 / 4;
            sprites[5].Visible = true;


            Refresh();
        }

        void LoadMap()
        {
            Bitmap tilesetBmp = GetTilesetBitmap();

            selectionBmp = new Game_Player.Bitmap(ImageWidth, ImageHeight);

            needRefresh = new bool[Map.width, Map.height];
            saveLayer = new int[Map.width, Map.height];

            actionIndex = 0;
            actions = new List<Action>();
            OnAction(null);

            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] == null)
                {
                    sprites[i] = new Sprite();
                    sprites[i].Z = i;
                }
            }
            if (background == null)
            {
                background = new Sprite();
                background.Z = -1;
            }
            background.Bitmap = new Game_Player.Bitmap(ImageWidth, ImageHeight);
            background.Bitmap.Clear(Game_Player.Colors.White);

            for (int l = 0; l < 5; l++)
            {
                Bitmap layer = new Bitmap(Map.width * 32, Map.height * 32);

                if (l == 0)
                    Graphics.FromImage(layer).Clear(Color.White);

                for (int x = 0; x < Map.width; x++)
                {
                    for (int y = 0; y < Map.height; y++)
                    {
                        if (l == 4)
                        {
                            Graphics.FromImage(layer).DrawRectangle(Pens.Black,
                                new Rectangle(x * 32, y * 32, 31, 31));
                        }
                        else if (l != 3)
                        {
                            int tileId = Map.data[x, y, l];

                            if (tileId >= NORM_INDEX)
                            {
                                int tileX = (tileId - NORM_INDEX) % 8 * 32;
                                int tileY = (tileId - NORM_INDEX) / 8 * 32;
                                Rectangle rect = new Rectangle(tileX, tileY, 32, 32);

                                Graphics.FromImage(layer).DrawImage(tilesetBmp,
                                    new Rectangle(x * 32, y * 32, 32, 32), rect, GraphicsUnit.Pixel);
                            }
                            else if (tileId >= 48)
                            {
                                Bitmap bmp = AutoTiles.AutoTile(tileId % 48, GetAutotileBitmap(tileId));
                                Graphics.FromImage(layer).DrawImage(bmp, new Point(x * 32, y * 32));
                            }
                        }
                    }
                }

                if (l == 4)
                    grid = layer;
                else
                    layers[l] = layer;

                LoadLayer();
            }

            Bitmap eventLayer = new Bitmap(Map.width * 32, Map.height * 32);
            foreach (DataClasses.Event evnt in Map.events.Values)
            {
                if (evnt != null)
                {
                    int x = 32 * evnt.x;
                    int y = 32 * evnt.y;

                    Bitmap eventBmp = GetEventBitmap(evnt);

                    if (eventBmp != null)
                    {
                        int charX = eventBmp.Width * (1 + 2 * evnt.pages[0].graphic.pattern) / 8 - 12;
                        int charY = 4 + eventBmp.Height * (evnt.pages[0].graphic.direction - 2) / 8;
                        
                        float[][] fMat = Game_Player.HSLColor.HueMatrix(evnt.pages[0].graphic.characterHue);
                        fMat[3][3] = evnt.pages[0].graphic.opacity / 255.0f;
                        ColorMatrix cMat = new ColorMatrix(fMat);
                        ImageAttributes attr = new ImageAttributes();
                        attr.SetColorMatrix(cMat);

                        Graphics.FromImage(eventLayer).FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), new Rectangle(x + 5, y + 5, 22, 22));
                        if (eventBmp.Width < 32 * 4 || eventBmp.Height < 32 * 4)
                        {
                            int charW = Math.Min(23, eventBmp.Width / 4);
                            int charH = Math.Min(23, eventBmp.Height / 4);

                            if (charW < 23) charX = 0;
                            if (charH < 23) charY = 0;

                            int charX2 = charW < 23 ? (32 - charW) / 2 : 4;
                            int charY2 = charH < 23 ? (32 - charH) / 2 : 4;

                            Graphics.FromImage(eventLayer).DrawImage(eventBmp, new Rectangle(x + charX2, y + charY2, charW, charH),
                                charX, charY, charW, charH, GraphicsUnit.Pixel, attr);
                        }
                        else
                        {
                            Graphics.FromImage(eventLayer).DrawImage(eventBmp, new Rectangle(x + 4, y + 4, 23, 23),
                                charX, charY, 23, 23, GraphicsUnit.Pixel, attr);
                        }
                        Graphics.FromImage(eventLayer).DrawRectangle(Pens.White, new Rectangle(x + 4, y + 4, 23, 23));
                    }
                    else if (evnt.pages[0].graphic.tileId > 0)
                    {
                        float[][] fMat = Game_Player.HSLColor.HueMatrix(evnt.pages[0].graphic.characterHue);
                        fMat[3][3] = evnt.pages[0].graphic.opacity / 255.0f;
                        ColorMatrix cMat = new ColorMatrix(fMat);
                        ImageAttributes attr = new ImageAttributes();
                        attr.SetColorMatrix(cMat);

                        int tileX = (evnt.pages[0].graphic.tileId - NORM_INDEX) % 8 * 32;
                        int tileY = (evnt.pages[0].graphic.tileId - NORM_INDEX) / 8 * 32;

                        Graphics.FromImage(eventLayer).FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), new Rectangle(x + 5, y + 5, 22, 22));
                        Graphics.FromImage(eventLayer).DrawImage(tilesetBmp, new Rectangle(x + 4, y + 4, 23, 23),
                                tileX, tileY, 32, 32, GraphicsUnit.Pixel, attr);
                        Graphics.FromImage(eventLayer).DrawRectangle(Pens.White, new Rectangle(x + 4, y + 4, 23, 23));
                    }
                    else
                    {
                        Graphics.FromImage(eventLayer).FillRectangle(new SolidBrush(Color.FromArgb(64, 255, 255, 255)), new Rectangle(x + 5, y + 5, 22, 22));
                        Graphics.FromImage(eventLayer).DrawRectangle(Pens.White, new Rectangle(x + 4, y + 4, 23, 23));
                    }
                }
            }
            layers[3] = eventLayer;

            for (int i = 0; i < 4; i++)
            {
                sprites[i].Bitmap = new Game_Player.Bitmap(layers[i]);
            }
            sprites[4].Bitmap = new Game_Player.Bitmap(grid);
            sprites[5].Bitmap = selectionBmp;

            LoadLayer();
        }

        
    }
}
