using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public class Tilemap
    {
        Viewport viewport;
        public Viewport Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }

        bool isDisposed = false;
        public bool IsDisposed { get { return isDisposed; } }

        private Bitmap tileset;
        public Bitmap Tileset
        {
            get { return tileset; }
            set { tileset = value; }
        }

        private Bitmap[] autoTiles;
        public Bitmap[] AutoTiles
        {
            get { return autoTiles; }
            set { autoTiles = value; }
        }

        private int[, ,] mapData;
        public int[, ,] MapData
        {
            get { return mapData; }
            set { mapData = value; }
        }

        private int[,] flashData;
        public int[,] FlashData
        {
            get { return flashData; }
            set { flashData = value; }
        }

        private int[] priorities;
        public int[] Priorities
        {
            get { return priorities; }
            set { priorities = value; }
        }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                foreach (Sprite sprite in sprites)
                    if (sprite != null)
                        sprite.Visible = visible;
            }
        }

        private int oX;
        public int OX
        {
            get { return oX; }
            set 
            {
                foreach (Sprite sprite in sprites)
                    if (sprite != null)
                        sprite.X -= value - oX;
                oX = value;
            }
        }

        private int oY;
        public int OY
        {
            get { return oY; }
            set 
            {
                foreach (Sprite sprite in sprites)
                    if (sprite != null)
                        sprite.Y -= value - oY;
                oY = value;
            }
        }

        private Sprite[, ,] sprites;
        private const int FRAME_LENGTH = 30;
        private int frame = 0;

        public Tilemap(Viewport viewport)
        {
            this.viewport = viewport;
        }

        enum Corners { UpperRight, UpperLeft, LowerRight, LowerLeft }
        enum Types { Water, Out, In, Vertical, Horizontal }

        private Bitmap AutoTile(int id, Bitmap autotile)
        {
            id += 48;

            Bitmap bmp = new Bitmap(32 * autotile.Width / 96, 32);
            Rect r1, r2, r3, r4;
            Types[] types = new Types[] {Types.Water, Types.Water, Types.Water, Types.Water};

            for (int i = 0; i < autotile.Width / 96; i++)
            {
                switch (id)
                {
                    case 48:
                        types = new Types[] { Types.Water, Types.Water, Types.Water, Types.Water }; break;
                    case 49:
                        types = new Types[] { Types.In, Types.Water, Types.Water, Types.Water }; break;
                    case 50:
                        types = new Types[] { Types.Water, Types.In, Types.Water, Types.Water }; break;
                    case 51:
                        types = new Types[] { Types.In, Types.In, Types.Water, Types.Water }; break;
                    case 52:
                        types = new Types[] { Types.Water, Types.Water, Types.Water, Types.In }; break;
                    case 53:
                        types = new Types[] { Types.In, Types.Water, Types.Water, Types.In }; break;
                    case 54:
                        types = new Types[] { Types.Water, Types.In, Types.Water, Types.In }; break;
                    case 55:
                        types = new Types[] { Types.In, Types.In, Types.Water, Types.In }; break;
                    case 56:
                        types = new Types[] { Types.Water, Types.Water, Types.In, Types.Water }; break;
                    case 57:
                        types = new Types[] { Types.In, Types.Water, Types.In, Types.Water }; break;
                    case 58:
                        types = new Types[] { Types.Water, Types.In, Types.In, Types.Water }; break;
                    case 59:
                        types = new Types[] { Types.In, Types.In, Types.In, Types.Water }; break;
                    case 60:
                        types = new Types[] { Types.Water, Types.Water, Types.In, Types.In }; break;
                    case 61:
                        types = new Types[] { Types.In, Types.Water, Types.In, Types.In }; break;
                    case 62:
                        types = new Types[] { Types.Water, Types.In, Types.In, Types.In }; break;
                    case 63:
                        types = new Types[] { Types.In, Types.In, Types.In, Types.In }; break;
                    case 64:
                        types = new Types[] { Types.Vertical, Types.Water, Types.Vertical, Types.Water }; break;
                    case 65:
                        types = new Types[] { Types.Vertical, Types.In, Types.Vertical, Types.Water }; break;
                    case 66:
                        types = new Types[] { Types.Vertical, Types.Water, Types.Vertical, Types.In }; break;
                    case 67:
                        types = new Types[] { Types.Vertical, Types.In, Types.Vertical, Types.In }; break;
                    case 68:
                        types = new Types[] { Types.Horizontal, Types.Horizontal, Types.Water, Types.Water }; break;
                    case 69:
                        types = new Types[] { Types.Horizontal, Types.Horizontal, Types.Water, Types.In }; break;
                    case 70:
                        types = new Types[] { Types.Horizontal, Types.Horizontal, Types.In, Types.Water }; break;
                    case 71:
                        types = new Types[] { Types.Horizontal, Types.Horizontal, Types.In, Types.In }; break;
                    case 72:
                        types = new Types[] { Types.Water, Types.Vertical, Types.Water, Types.Vertical }; break;
                    case 73:
                        types = new Types[] { Types.Water, Types.Vertical, Types.In, Types.Vertical }; break;
                    case 74:
                        types = new Types[] { Types.In, Types.Vertical, Types.Water, Types.Vertical }; break;
                    case 75:
                        types = new Types[] { Types.In, Types.Vertical, Types.In, Types.Vertical }; break;
                    case 76:
                        types = new Types[] { Types.Water, Types.Water, Types.Horizontal, Types.Horizontal }; break;
                    case 77:
                        types = new Types[] { Types.In, Types.Water, Types.Horizontal, Types.Horizontal }; break;
                    case 78:
                        types = new Types[] { Types.Water, Types.In, Types.Horizontal, Types.Horizontal }; break;
                    case 79:
                        types = new Types[] { Types.In, Types.In, Types.Horizontal, Types.Horizontal }; break;
                    case 80:
                        types = new Types[] { Types.Vertical, Types.Vertical, Types.Vertical, Types.Vertical }; break;
                    case 81:
                        types = new Types[] { Types.Horizontal, Types.Horizontal, Types.Horizontal, Types.Horizontal }; break;
                    case 82:
                        types = new Types[] { Types.Out, Types.Horizontal, Types.Vertical, Types.Water }; break;
                    case 83:
                        types = new Types[] { Types.Out, Types.Horizontal, Types.Vertical, Types.In }; break;
                    case 84:
                        types = new Types[] { Types.Horizontal, Types.Out, Types.Water, Types.Vertical }; break;
                    case 85:
                        types = new Types[] { Types.Horizontal, Types.Out, Types.In, Types.Vertical }; break;
                    case 86:
                        types = new Types[] { Types.Water, Types.Vertical, Types.Horizontal, Types.Out }; break;
                    case 87:
                        types = new Types[] { Types.In, Types.Vertical, Types.Horizontal, Types.Out }; break;
                    case 88:
                        types = new Types[] { Types.Vertical, Types.Water, Types.Out, Types.Horizontal }; break;
                    case 89:
                        types = new Types[] { Types.Vertical, Types.In, Types.Out, Types.Horizontal }; break;
                    case 90:
                        types = new Types[] { Types.Out, Types.Out, Types.Vertical, Types.Vertical }; break;
                    case 91:
                        types = new Types[] { Types.Out, Types.Horizontal, Types.Out, Types.Horizontal }; break;
                    case 92:
                        types = new Types[] { Types.Vertical, Types.Vertical, Types.Out, Types.Out }; break;
                    case 93:
                        types = new Types[] { Types.Horizontal, Types.Out, Types.Horizontal, Types.Out }; break;
                    case 94:
                        types = new Types[] { Types.Out, Types.Out, Types.Out, Types.Out }; break;
                    case 95:
                        types = new Types[] { Types.Out, Types.Out, Types.Out, Types.Out }; break;
                }

                r1 = AutoRect(Corners.UpperLeft, types[0]);
                r2 = AutoRect(Corners.UpperRight, types[1]);
                r3 = AutoRect(Corners.LowerLeft, types[2]);
                r4 = AutoRect(Corners.LowerRight, types[3]);

                r1.X += i * 96;
                r2.X += i * 96;
                r3.X += i * 96;
                r4.X += i * 96;

                bmp.BlockTransfer(0 + i * 32, 0, autotile, r1);
                bmp.BlockTransfer(16 + i * 32, 0, autotile, r2);
                bmp.BlockTransfer(0 + i * 32, 16, autotile, r3);
                bmp.BlockTransfer(16 + i * 32, 16, autotile, r4);
            }
            return bmp;
        }

        private Rect AutoRect(Corners corner, Types type)
        {
            switch (type)
            {
                case Types.Water:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rect(32, 64, 16, 16);
                        case Corners.UpperRight: return new Rect(48, 64, 16, 16);
                        case Corners.LowerLeft: return new Rect(32, 80, 16, 16);
                        case Corners.LowerRight: return new Rect(48, 80, 16, 16);
                    }
                    break;
                case Types.Out:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rect(0, 32, 16, 16);
                        case Corners.UpperRight: return new Rect(80, 32, 16, 16);
                        case Corners.LowerLeft: return new Rect(0, 112, 16, 16);
                        case Corners.LowerRight: return new Rect(80, 112, 16, 16);
                    }
                    break;
                case Types.In:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rect(64, 0, 16, 16);
                        case Corners.UpperRight: return new Rect(80, 0, 16, 16);
                        case Corners.LowerLeft: return new Rect(64, 16, 16, 16);
                        case Corners.LowerRight: return new Rect(80, 16, 16, 16);
                    }
                    break;
                case Types.Vertical:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rect(0, 64, 16, 16);
                        case Corners.UpperRight: return new Rect(80, 64, 16, 16);
                        case Corners.LowerLeft: return new Rect(0, 80, 16, 16);
                        case Corners.LowerRight: return new Rect(80, 80, 16, 16);
                    }
                    break;
                case Types.Horizontal:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rect(32, 32, 16, 16);
                        case Corners.UpperRight: return new Rect(48, 32, 16, 16);
                        case Corners.LowerLeft: return new Rect(32, 112, 16, 16);
                        case Corners.LowerRight: return new Rect(48, 112, 16, 16);
                    }
                    break;
            }

            return new Rect();
        }

        public  void MakeMap()
        {
            sprites = new Sprite[mapData.GetLength(0), mapData.GetLength(1), mapData.GetLength(2)];

            for (int d = 0; d < mapData.GetLength(2); d++)
            {
                for (int y = 0; y < mapData.GetLength(1); y++)
                {
                    for (int x = 0; x < mapData.GetLength(0); x++)
                    {
                        int tileId = mapData[x, y, d];

                        if (tileId >= 384)
                        {
                            int tileX = (tileId - 384) % 8 * 32;
                            int tileY = (tileId - 384) / 8 * 32;
                            Rect rect = new Rect(tileX, tileY, 32, 32);

                            Sprite sprite = new Sprite(viewport, tileset);
                            sprite.BmpSourceRect = rect;
                            sprite.Rect = new Rect(x * 32, y * 32, 32, 32);
                            sprite.Z = d + priorities[tileId] * 32 + y * 32;
                            sprites[x, y, d] = sprite;
                        }
                        else if (tileId >= 48)
                        {
                            Bitmap bmp = AutoTile(tileId % 48, autoTiles[tileId / 48 - 1]);

                            Sprite sprite = new Sprite(viewport, bmp);
                            sprite.BmpSourceRect = new Rect(0, 0, 32, 32);
                            sprite.Rect = new Rect(x * 32, y * 32, 32, 32);
                            sprite.Z = d + priorities[tileId] * 32 + y * 32;
                            sprites[x, y, d] = sprite;
                        }
                    }
                }
            }
        }

        public Tilemap() : this(Viewport.Default) { }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                //is there anything to dispose here?
            }
        }

        public void Update()
        {
            frame = (frame + 1) % (FRAME_LENGTH * 4);
            int plus = (frame / FRAME_LENGTH) * 32;

            
            for (int d = 0; d < mapData.GetLength(2); d++)
            {
                for (int y = 0; y < mapData.GetLength(1); y++)
                {
                    for (int x = 0; x < mapData.GetLength(0); x++)
                    {
                        Sprite sprite = sprites[x, y, d];
                        if (sprite != null)
                        {
                            if (mapData[x, y, d] < 384 && sprite.Bitmap.Width > 32)
                                sprite.BmpSourceRect = new Rect(plus, 0, 32, 32);

                            sprite.Update();
                        }
                    }
                }
            }
        }
    }
}
