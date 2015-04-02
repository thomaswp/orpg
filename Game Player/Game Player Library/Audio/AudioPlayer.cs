using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public class AudioPlayer
    {
        public double Volume
        {
            get 
            {
                if (audio != null)
                    return audio.Volume;
                return 0;
            }
            set
            {
                if (audio != null)
                    audio.Volume = value;
            }
        }

        public double Pan
        {
            get
            {
                if (audio != null)
                    return audio.Pan;
                return 0;
            }
            set
            {
                if (audio != null)
                    audio.Pan = value;
            }
        }

        public double Tempo
        {
            get
            {
                if (audio != null)
                    return audio.Tempo;
                return 0;
            }
            set
            {
                if (audio != null)
                    audio.Tempo = value;
            }
        }

        public double Pitch
        {
            get
            {
                if (audio != null)
                    return audio.Pitch;
                return 0;
            }
            set
            {
                if (audio != null)
                    audio.Pitch = value;
            }
        }

        public string Playing { get; private set; }

        private static string[] exts = { ".mid", ".midi", ".ogg", ".vorbis", ".wav", ".wave", ".mp3" };
        private static string[] paths = { Paths.Root, Paths.RTP };

        private Audio audio;
        private string folder;

        public AudioPlayer(string folder)
        {
            this.folder = folder;
        }

        public void Play(AudioFile file)
        {
            Play(file, false);
        }

        public void Play(AudioFile file, bool repeat)
        {
            Play(file.name, file.volume, file.pitch, 100, repeat);
        }

        public void Play(String filepath)
        {
            Play(filepath, 100, 100);
        }

        public void Play(String filepath, double volume, double pitch)
        {
            Play(filepath, volume, pitch, 100, false);
        }

        public void Play(String filepath, double volume, double pitch, double tempo)
        {
            Play(filepath, volume, pitch, tempo, false);
        }

        public void Play(String filepath, double volume, double pitch, double tempo, Boolean repeat)
        {
            if (audio != null)
                audio.Dispose();

            string path = Paths.FindValidPath(@"Audio\" + folder + @"\" + filepath, paths, exts);
            if (path == "")
            {
                MsgBox.Show("Could not find '" + filepath + "'");
                return;
            }

            audio = new Audio(path);
            audio.Volume = volume;
            audio.Tempo = tempo;
            audio.Pitch = pitch;
            audio.Play(repeat);
            Playing = filepath;
        }

        public void Stop()
        {
            if (audio != null)
                audio.Stop();
        }

        public void Fade(int frames)
        {
            if (audio != null)
                audio.Fade(frames);
        }
    }
}
