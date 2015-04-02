using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player.Game
{
    public class Screen
    {
        #region Properties

        Tone tone;
        public Tone Tone
        {
            get { return tone; }
        }

        Color flashColor;
        public Color FlashColor
        {
            get { return flashColor; }
        }

        int shake;
        public int Shake
        {
            get { return shake; }
        }

        Picture[] pictures;
        public Picture[] Pictures
        {
            get { return pictures; }
        }

        int weatherType;
        public int WeatherType
        {
            get { return weatherType; }
        }

        double weatherMax;
        public double WeatherMax
        {
            get { return weatherMax; }
        }
        #endregion

        Tone toneTarget;
        int toneDuration;
        int flashDuration;
        int shakePower;
        int shakeSpeed;
        int shakeDuration;
        int shakeDirection;
        int weatherTypeTarget;
        double weatherMaxTarget;
        int weatherDuration;

        public Screen()
        {
            tone = new Tone();
            toneTarget = new Tone();
            toneDuration = 0;
            flashColor = new Color();
            flashDuration = 0;
            shakePower = 0;
            shakeSpeed = 0;
            shakeDuration = 0;
            shakeDirection = 1;
            shake = 0;
            pictures = new Picture[101];
            for (int i = 1; i < 101; i++)
            {
                pictures[i] = new Picture(1);
            }
            weatherType = 0;
            weatherMax = 0;
            weatherTypeTarget = 0;
            weatherMaxTarget = 0.0;
            weatherDuration = 0;
        }

        public void StartToneChange(Tone tone, int duration)
        {
            toneTarget = tone;
            toneDuration = duration;
            if (toneDuration == 0)
                tone = toneTarget;
        }

        public void StartFlash(Color color, int duration)
        {
            flashColor = color;
            flashDuration = duration;
        }

        public void StartShake(int power, int speed, int duration)
        {
            shakePower = power;
            shakeSpeed = speed;
            shakeDuration = duration;
        }

        public void Weather(int type, int power, int duration)
        {
            weatherTypeTarget = type;
            if (weatherTypeTarget != 0)
            {
               weatherType = weatherTypeTarget;
                weatherMaxTarget = (power + 1) * 4;
            }
            else
                weatherMaxTarget = 0;

            weatherDuration = duration;
            if (duration == 0)
            {
                weatherType = weatherTypeTarget;
                weatherMax = weatherMaxTarget;
            }
        }

        public void Update()
        {
            if (toneDuration > 0)
            {
                int d = toneDuration;
                Tone newTone = new Tone();
                newTone.Red = (tone.Red * (d - 1) + toneTarget.Red) / d;
                newTone.Green = (tone.Green * (d - 1) + toneTarget.Green) / d;
                newTone.Blue = (tone.Blue * (d - 1) + toneTarget.Blue) / d;
                tone = newTone;
                toneDuration--;
            }

            if (flashDuration > 0)
            {
                flashColor.Alpha = flashColor.Alpha * (flashDuration - 1) / flashDuration;
                flashDuration--;
            }

            if (shakeDuration > 0 || Shake != 0)
            {
                double delta = shakePower * shakeSpeed * shakeDirection / 10;

                if (shakeDuration <= 1 && Shake * (Shake + delta) < 0)
                    shake = 0;
                else
                    shake += (int)delta;

                if (shake > shakePower * 2)
                    shakeDirection = -1;
                if (shake < shakePower * -2)
                    shakeDirection = 1;

                if (shakeDuration > 0)
                    shakeDuration--;
            }

            if (weatherDuration > 0)
            {
                weatherMax = weatherMax * (weatherDuration - 1) / weatherDuration;
                weatherDuration--;
                if (weatherDuration == 0)
                    weatherType = weatherTypeTarget;
            }

            if (Globals.GameTemp.inBattle)
                for (int i = 51; i < 101; i++)
                    pictures[i].Update();
            else
                for (int i = 1; i < 51; i++)
                    pictures[i].Update();
        }

    }
}
