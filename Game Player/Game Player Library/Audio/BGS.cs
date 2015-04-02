using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    public static class BGS
    {
        public static double Volume
        {
            get
            {
                if (audio != null)
                { return audio.Volume; }
                return 0;
            }
            set
            {
                if (audio != null)
                { audio.Volume = value; }
            }
        }

        public static double Pan
        {
            get
            {
                if (audio != null)
                { return audio.Pan; }
                return 0;
            }
            set
            {
                if (audio != null)
                { audio.Pan = value; }
            }
        }

        public static double Tempo
        {
            get
            {
                if (audio != null)
                { return audio.Tempo; }
                return 0;
            }
            set
            {
                if (audio != null)
                { audio.Tempo = value; }
            }
        }

        public static double Pitch
        {
            get
            {
                if (audio != null)
                { return audio.Pitch; }
                return 0;
            }
            set
            {
                if (audio != null)
                { audio.Pitch = value; }
            }
        }

        public static Boolean Paused
        {
            set
            {
                if (audio != null)
                    audio.Paused = value;
            }
            get
            {
                if (audio != null)
                    return audio.Paused;
                return false;
            }
        }

        //public static string RootPath = @"C:\Program Files\Common Files\Enterbrain\RGSS\Standard\Audio\BGS\";
        //public static string FileExtension = ".ogg";

        static string[] exts = { "", ".ogg" };
        static string[] paths = { Paths.Root, Paths.RTP };

        static Audio audio;

        public static void Play(AudioFile file)
        {
            Play(file, false);
        }

        public static void Play(AudioFile file, bool repeat)
        {
            Play(file.name, file.volume, repeat);
        }

        public static void Play(String filepath)
        {
            Play(filepath, 100, false);
        }

        public static void Play(String filepath, double volume, Boolean repeat)
        {
            if (audio != null)
            { audio.Dispose(); }
            string path = Paths.FindValidPath(@"Audio\BGS\" + filepath, paths, exts);
            if (path == "")
                return;
            audio = new Audio(path);
            audio.Volume = volume;
            //audio.Tempo = tempo;
            audio.Play(repeat);
        }

        public static void Stop()
        {
            if (audio != null)
                audio.Stop();
        }

        public static void Fade(int frames)
        {
            if (audio != null)
                audio.Fade(frames);
        }
    }
}
