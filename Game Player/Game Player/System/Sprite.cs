using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public class Sprite
    {
        public enum Flips { None, Vertically, Horizontally }

        #region Properties

        Bitmap _bitmap;
        public Bitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; Resize(value.Width, value.Height); }
        }

        Rect _bmpSourceRect = null;
        public Rect BmpSourceRect
        {
            get 
            {
                if (_bmpSourceRect == null)
                { return Bitmap.Rect; }
                else
                { return _bmpSourceRect; }

            }
            set { _bmpSourceRect = value; }
        }

        //Bitmap _drawBitmap = null;
        //public Bitmap DrawnBitmap
        //{
        //    get
        //    {
        //        if (BmpSourceRect == null)
        //        { return Bitmap; }
        //        else
        //        {
        //            if (_drawBitmap == null)
        //            {
        //                try
        //                {
        //                    _drawBitmap = new Bitmap(BmpSourceRect.Width, BmpSourceRect.Height);
        //                    _drawBitmap.BlockTransfer(0, 0, Bitmap, BmpSourceRect, 1);
        //                }
        //                catch
        //                { return Bitmap; }
        //            }
        //            return _drawBitmap;
        //        }
        //    }
        //}

        int _id;
        public int ID
        { get { return _id; } }
            
        int _x;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        int _y;
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        int _z;
        public int Z
        {
            get { return _z; }
            set 
            {
                if (disposeViewport) { Viewport.Z = value; }
                _z = value; 
            }
        }

        public int Width
        {
            get { return (int)(BmpSourceRect.Width * ZoomX); }
            set { ZoomX = value / (double)BmpSourceRect.Width; }
        }

        public int Height
        {
            get { return (int)(BmpSourceRect.Height * ZoomY); }
            set { ZoomY = value / (double)BmpSourceRect.Height; }
        }

        int _oX;
        public int OX
        {
            get { return _oX; }
            set { _oX = value; }
        }

        int _oY;
        public int OY
        {
            get { return _oY; }
            set { _oY = value; }
        }

        Double _opactiy = 1;
        public Double Opactiy
        {
            get { return _opactiy; }
            set { _opactiy = value; }
        }

        double _rotation;
        public double Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Point Location
        {
            get {return new Point(X, Y);}
            set { X = value.X; Y = value.Y; }
        }

        Color _color = Colors.Clear;
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        Boolean _visible = true;
        public Boolean Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        Flips _flipMode = Flips.None;
        public Flips FlipMode
        {
            get { return _flipMode; }
            set { _flipMode = value; }
        }

        Viewport _viewport;
        public Viewport Viewport
        { get { return _viewport; } }

        Boolean _disposed;
        public Boolean Disposed
        { get { return _disposed; } }
        
        public Rect Rect
        {
            get { return new Rect(X, Y, Width, Height); }
            set { X = value.X; Y = value.Y; Width = value.Width; Height = value.Height; }
        }

        double _zoomX = 1;
        public double ZoomX
        {
            get { return _zoomX; }
            set { _zoomX = value; }
        }

        double _zoomY = 1;
        public double ZoomY
        {
            get { return _zoomY; }
            set { _zoomY = value; }
        }
#endregion
        
        Boolean disposeViewport = false;

        public Sprite()
        {
            _viewport = new Viewport();
            _id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(1, 1);
            X = 0; Y = 0;
        }

        public Sprite(Viewport viewport)
        {
            _viewport = viewport;
            _id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(1, 1);
            X = 0; Y = 0;
        }
        public Sprite(Viewport viewport, Bitmap bitmap)
        {
            _viewport = viewport;
            _id = Viewport.AddSprite(this);
            Bitmap = bitmap;
            X = 0; Y = 0; 
        }
        public Sprite(Viewport viewport, int x, int y, int width, int height)
        {
            _viewport = viewport;
            _id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(width, height);
            X = x; Y = y;
        }
        public Sprite(Viewport viewport, Rect rect)
        {
            _viewport = viewport;
            _id = Viewport.AddSprite(this);
            Bitmap = new Bitmap(rect.Width, rect.Height);
            X = rect.X; Y = rect.Y;
        }

        public void Dispose()
        { 
            if (_disposed == false) { Bitmap.Dispose(); _disposed = true; }
            if (!Viewport.Disposed & disposeViewport) { Viewport.Dispose(); }
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Update()
        { }
    }
}
