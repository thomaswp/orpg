using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DataClasses;

namespace ORPG
{
    public static class AutoTiles
    {
        public const int NORM_INDEX = 384;

        enum Dirs
        {
            BottomRight = 0, Right = 1, TopRight = 2, Bottom = 3,
            Top = 4, BottomLeft = 5, Left = 6, TopLeft = 7
        };

        public static int AutoTileId(Map Map, int layer, int x, int y, int baseId)
        {
            if (baseId >= NORM_INDEX || baseId == 0)
                return baseId;

            baseId = baseId / 48 * 48;

            int[] offXs = new int[] { 1, 1, 1, 0, 0, -1, -1, -1 };
            int[] offYs = new int[] { 1, 0, -1, 1, -1, 1, 0, -1 };
            bool[] auto = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                int nx = x + offXs[i];
                int ny = y + offYs[i];

                if (nx >= 0 && nx < Map.width &&
                    ny >= 0 && ny < Map.height)
                {
                    int id = Map.data[nx, ny, layer];
                    if (id >= baseId && id < baseId + 48)
                    {
                        auto[i] = true;
                    }
                }
                else
                {
                    auto[i] = true;
                }
            }

            return baseId + AutoTileIdOffset(auto);
        }

        private static int AutoTileIdOffset(bool[] auto)
        {
            if (auto[(int)Dirs.Top])
            {
                if (auto[(int)Dirs.Right])
                {
                    if (auto[(int)Dirs.Bottom])
                    {
                        if (auto[(int)Dirs.Left])
                        {
                            int n = 0;
                            if (!auto[(int)Dirs.BottomLeft]) n += 8;
                            if (!auto[(int)Dirs.BottomRight]) n += 4;
                            if (!auto[(int)Dirs.TopRight]) n += 2;
                            if (!auto[(int)Dirs.TopLeft]) n += 1;
                            return n;
                        }
                        else
                        {
                            //Not Left
                            int n = 16;
                            if (!auto[(int)Dirs.BottomRight]) n += 2;
                            if (!auto[(int)Dirs.TopRight]) n += 1;
                            return n;
                        }
                    }
                    else
                    {
                        //Not Bottom
                        if (auto[(int)Dirs.Left])
                        {
                            int n = 28;
                            if (!auto[(int)Dirs.TopRight]) n += 2;
                            if (!auto[(int)Dirs.TopLeft]) n += 1;
                            return n;
                        }
                        else
                        {
                            //Not Left
                            if (auto[(int)Dirs.TopRight])
                                return 40;
                            else
                                return 41;
                        }
                    }
                }
                else
                {
                    //Not Right
                    if (auto[(int)Dirs.Bottom])
                    {
                        if (auto[(int)Dirs.Left])
                        {
                            int n = 24;
                            if (!auto[(int)Dirs.TopLeft]) n += 2;
                            if (!auto[(int)Dirs.BottomLeft]) n += 1;
                            return n;
                        }
                        else
                        {
                            return 32;
                        }
                    }
                    else
                    {
                        //Not Bottom
                        if (auto[(int)Dirs.Left])
                        {
                            if (auto[(int)Dirs.TopLeft])
                                return 38;
                            else
                                return 39;
                        }
                        else
                        {
                            //Not Left
                            return 44;
                        }
                    }
                }
            }
            else
            {
                //Not Top
                if (auto[(int)Dirs.Right])
                {
                    if (auto[(int)Dirs.Bottom])
                    {
                        if (auto[(int)Dirs.Left])
                        {
                            int n = 20;
                            if (!auto[(int)Dirs.BottomLeft]) n += 2;
                            if (!auto[(int)Dirs.BottomRight]) n += 1;
                            return n;
                        }
                        else
                        {
                            //Not Left
                            if (auto[(int)Dirs.BottomRight])
                                return 34;
                            else
                                return 35;
                        }
                    }
                    else
                    {
                        //Not Bottom
                        if (auto[(int)Dirs.Left])
                        {
                            return 33;
                        }
                        else
                        {
                            return 43;
                        }
                    }
                }
                else
                {
                    //Not Right
                    if (auto[(int)Dirs.Bottom])
                    {
                        if (auto[(int)Dirs.Left])
                        {
                            if (auto[(int)Dirs.BottomLeft])
                                return 36;
                            else
                                return 37;
                        }
                        else
                        {
                            return 42;
                        }
                    }
                    else
                    {
                        //Not Bottom
                        if (auto[(int)Dirs.Left])
                        {
                            return 45;
                        }
                        else
                        {
                            //Not Left
                            return 46;
                        }
                    }
                }
            }
        }

        enum Corners { UpperRight, UpperLeft, LowerRight, LowerLeft }
        enum Types { Water, Out, In, Vertical, Horizontal }

        public static Bitmap AutoTile(int id, Bitmap autotile)
        {
            id += 48;

            Bitmap bmp = new Bitmap(32, 32);
            Rectangle r1, r2, r3, r4;
            Types[] types = new Types[] { Types.Water, Types.Water, Types.Water, Types.Water };

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

            r1 = AutoRectangle(Corners.UpperLeft, types[0]);
            r2 = AutoRectangle(Corners.UpperRight, types[1]);
            r3 = AutoRectangle(Corners.LowerLeft, types[2]);
            r4 = AutoRectangle(Corners.LowerRight, types[3]);

            Graphics.FromImage(bmp).DrawImage(autotile, new Rectangle(0, 0, 16, 16), r1, GraphicsUnit.Pixel);
            Graphics.FromImage(bmp).DrawImage(autotile, new Rectangle(16, 0, 16, 16), r2, GraphicsUnit.Pixel);
            Graphics.FromImage(bmp).DrawImage(autotile, new Rectangle(0, 16, 16, 16), r3, GraphicsUnit.Pixel);
            Graphics.FromImage(bmp).DrawImage(autotile, new Rectangle(16, 16, 16, 16), r4, GraphicsUnit.Pixel);

            return bmp;
        }

        private static Rectangle AutoRectangle(Corners corner, Types type)
        {
            switch (type)
            {
                case Types.Water:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rectangle(32, 64, 16, 16);
                        case Corners.UpperRight: return new Rectangle(48, 64, 16, 16);
                        case Corners.LowerLeft: return new Rectangle(32, 80, 16, 16);
                        case Corners.LowerRight: return new Rectangle(48, 80, 16, 16);
                    }
                    break;
                case Types.Out:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rectangle(0, 32, 16, 16);
                        case Corners.UpperRight: return new Rectangle(80, 32, 16, 16);
                        case Corners.LowerLeft: return new Rectangle(0, 112, 16, 16);
                        case Corners.LowerRight: return new Rectangle(80, 112, 16, 16);
                    }
                    break;
                case Types.In:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rectangle(64, 0, 16, 16);
                        case Corners.UpperRight: return new Rectangle(80, 0, 16, 16);
                        case Corners.LowerLeft: return new Rectangle(64, 16, 16, 16);
                        case Corners.LowerRight: return new Rectangle(80, 16, 16, 16);
                    }
                    break;
                case Types.Vertical:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rectangle(0, 64, 16, 16);
                        case Corners.UpperRight: return new Rectangle(80, 64, 16, 16);
                        case Corners.LowerLeft: return new Rectangle(0, 80, 16, 16);
                        case Corners.LowerRight: return new Rectangle(80, 80, 16, 16);
                    }
                    break;
                case Types.Horizontal:
                    switch (corner)
                    {
                        case Corners.UpperLeft: return new Rectangle(32, 32, 16, 16);
                        case Corners.UpperRight: return new Rectangle(48, 32, 16, 16);
                        case Corners.LowerLeft: return new Rectangle(32, 112, 16, 16);
                        case Corners.LowerRight: return new Rectangle(48, 112, 16, 16);
                    }
                    break;
            }

            return new Rectangle();
        }
    }
}
