using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player.Game
{
    public class Picture
    {
        #region Properties
        int number;
        public int Number
        {
            get { return number; }
        }

        string name;
        public string Name
        {
            get { return name; }
        }

        int origin;
        public int Origin
        {
            get { return origin; }
        }

        double x;
        public double X
        {
            get { return x; }
        }

        double y;
        public double Y
        {
            get { return y; }
        }

        double zoomX;
        public double ZoomX
        {
            get { return zoomX; }
        }

        double zoomY;
        public double ZoomY
        {
            get { return zoomY; }
        }

        double opacity;
        public double Opacity
        {
            get { return opacity; }
        }

        int blendType;
        public int BlendType
        {
            get { return blendType; }
        }

        Tone tone;
        public Tone Tone
        {
            get { return tone; }
        }

        int angle;
        public int Angle
        {
            get { return angle; }
        }
        #endregion

        int duration;
        double targetX;
        double targetY;
        double targetZoomX;
        double targetZoomY;
        double targetOpacity;
        Tone toneTarget;
        int toneDuration;
        int rotateSpeed;

        public Picture(int number) 
        {
            this.number = number;
            zoomX = 100;
            zoomY = 100;
            opacity = 255.0;
            blendType = 1;
            targetX = X;
            targetY = Y;
            targetZoomX = ZoomX;
            targetZoomY = ZoomY;
            targetOpacity = Opacity;
            toneTarget = Tone;
        }

        public void Show(string name, int origin, double x, double y,
            double zoomX, double zoomY, double opacity, int blendType)
        {
            this.name = name;
            this.origin = origin;
            this.x = x;
            this.y = y;
            this.zoomX = zoomX;
            this.zoomY = zoomY;
            this.opacity = opacity;
            this.blendType = blendType;

            duration = 0;
            targetX = X;
            targetY = Y;
            targetZoomX = ZoomX;
            targetZoomY = ZoomY;
            targetOpacity = Opacity;
            tone = new Tone(0, 0, 0, 0);
            toneTarget = new Tone(0, 0, 0, 0);
            toneDuration = 0;
            angle = 0;
            rotateSpeed = 0;
        }

        public void Move(int duration, int origin, double x, double y,
            double zoomX, double zoomY, double opacity, int blendType)
        {
            this.duration = duration;
            this.origin = origin;
            this.x = x;
            this.y = y;
            this.zoomX = zoomX;
            this.zoomY = zoomY;
            this.opacity = opacity;
            this.blendType = blendType;
        }

        public void Roate(int speed)
        {
            rotateSpeed = speed;
        }

        public void StartToneChange(Tone tone, int duration)
        {
            toneTarget = tone;
            toneDuration = duration;
            if (toneDuration == 0)
                tone = toneTarget;
        }

        public void Erase()
        {
            name = "";
        }

        public void Update() 
        {
            if (duration > 0)
            {
                int d = duration;
                x = (x * (d - 1) + targetX) / d;
                y = (y * (d - 1) + targetY) / d;
                zoomX = (zoomX * (d - 1) + zoomX) / d;
                zoomY = (zoomY * (d - 1) + zoomY) / d;
                opacity = (opacity * (d - 1) + opacity) / d;
                duration--;
            }

            if (toneDuration > 0)
            {
                int d = duration;
                Tone newTone = new Tone();
                newTone.Red = (tone.Red * (d - 1) + toneTarget.Red) / d;
                newTone.Green = (tone.Green * (d - 1) + toneTarget.Green) / d;
                newTone.Blue = (tone.Blue * (d - 1) + toneTarget.Blue) / d;
                newTone.Gray = (tone.Gray * (d - 1) + toneTarget.Gray) / d;
                tone = newTone;
                toneDuration--;
            }

            if (rotateSpeed != 0)
            {
                angle += rotateSpeed / 2;
                while (angle < 360)
                    angle += 360;
                angle %= 360;
            }
        }
    }
}
