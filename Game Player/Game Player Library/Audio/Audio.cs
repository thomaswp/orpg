using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Game_Player
{
    public partial class Audio
    {
        #region properties
        double volume; //[0, 100] default=100
        public double Volume
        {
            get { return volume; }
            set 
            {
                volume = Math.Max(Math.Min(100, value), 0);
                if (stopFadeOnVC)
                {
                    fading = false;
                }
                updateChannel = true;
            }
        }

        double pan;//[-1,1] default=0
        public double Pan
        {
            get { return pan; }
            set 
            {
                pan = Math.Max(Math.Min(1, value), -1);
                updateChannel = true;
            }
        }

        double tempo;//[0, inf) default=100
        public double Tempo
        {
            get { return tempo; }
            set 
            {
                tempo = Math.Max(0, value);
                updateChannel = true;
            }
        }

        double pitch;//[0,inf) default=100
        public double Pitch
        {
            get { return pitch; }
            set 
            {
                pitch = Math.Max(0, value);
                updateChannel = true;
            }
        }

        Boolean paused = false;
        public Boolean Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                updateChannel = true;
            }
        }

        Boolean disposed;
        public Boolean Disposed
        {
            get { return disposed; }
        }

        bool fading = false;
        public bool Fading
        {
            get { return fading; }
        }

        private bool repeat;
        public bool Repeat
        {
            get
            {
                return repeat;
            }
            set
            {
                updateChannel = (repeat != value);
                repeat = value;
            }
        }

        public static AudioPlayer BGM { get; private set; }
        public static AudioPlayer BGS { get; private set; }
        public static AudioPlayer ME { get; private set; }
        public static AudioPlayer SE { get; private set; }

        #endregion

        //the amount the volume is faded each frame
        double fadeInc;


        //When set to false, allows the voume to be changed for
        //a fade without stopping the fade.
        //Otherwise, changing the volume stops a fade.
        private Boolean stopFadeOnVC = true;

        private FMOD.Channel channel;
        private FMOD.Sound sound;
        private FMOD.DSP dsp;

        private Boolean updateChannel;

        private float startFrequency;

        private static FMOD.System system;
        private static List<Audio> audios;

        /// <summary>
        /// Initialized an instance of audio.
        /// An exception will be thrown if it fails to initialize.
        /// </summary>
        static Audio()
        {
            audios = new List<Audio>();

            BGM = new AudioPlayer("BGM");
            BGS = new AudioPlayer("BGS");
            ME = new AudioPlayer("ME");
            SE = new AudioPlayer("SE");

            FMOD.RESULT result = FMOD.Factory.System_Create(ref system);
            if (result == FMOD.RESULT.OK)
            {
                result = system.init(4, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
                if (result == FMOD.RESULT.OK)
                    return;
            }

            throw new Exception("Failed to initialize audio");
        }

        public Audio(string filepath)
        {
            FMOD.RESULT result;

            Boolean success = false;
            if (File.Exists(filepath))
            {
                result = system.createStream(filepath, FMOD.MODE._2D | FMOD.MODE.CREATESTREAM | FMOD.MODE.HARDWARE | FMOD.MODE.LOOP_NORMAL, ref sound);
                success = (result == FMOD.RESULT.OK);
                result = system.createDSPByType(FMOD.DSP_TYPE.PITCHSHIFT, ref dsp);
                success = success && (result == FMOD.RESULT.OK);
            }
            else
            {
                MsgBox.Show("Could not load audio file '" + filepath + "'.");
                Dispose();
            }
            if (success)
            {
                this.Volume = 100.0;
                this.Pan = 0.0;
                this.Tempo = 1.0;
                this.Pitch = 0.0;

                audios.Add(this);
            }
        }

        public void Play(Boolean repeat)
        {
            if (!Paused)
            {
                system.playSound(FMOD.CHANNELINDEX.FREE, sound, false, ref channel);
                FMOD.DSPConnection con = null;
                channel.addDSP(dsp, ref con);
                channel.getFrequency(ref startFrequency);
            }

            this.repeat = repeat;
            paused = false;

            UpdateChannel();
        }

        public void Stop()
        {
            Paused = true;
            fading = false;

            if (sound != null)
                channel.setPosition(0, FMOD.TIMEUNIT.MS);
        }

        public void Fade(int frames)
        {
            fading = true;
            fadeInc = Volume / frames;
        }

        public static void Update()
        {
            foreach (Audio audio in audios)
                if (audio != null)
                    audio.UpdateInstance();

            system.update();
        }

        void UpdateInstance()
        {
            if (Fading)
            {
                stopFadeOnVC = false;
                Volume -= fadeInc;
                stopFadeOnVC = true;
                if (Volume <= 0)
                {
                    Volume = 0;
                    Stop();
                }
            }

            if (updateChannel)
            {
                UpdateChannel();
            }
        }

        public void UpdateChannel()
        {
            channel.setVolume((float)(volume / 100));
            channel.setPan((float)pan);
            channel.setFrequency((float)tempo * startFrequency / 100);
            channel.setPaused(paused);

            //If we increased the frequency, we should decrease the tempo by the same factor
            dsp.setParameter((int)(FMOD.DSP_PITCHSHIFT.PITCH), (float)(pitch / tempo).MinMax(0.5, 2));

            channel.setLoopCount(repeat ? -1 : 0);
            //not working for some reason

            updateChannel = false;
        }

        void CheckDisposed()
        {
            if (Disposed)
            {
                throw new Exception("Cannot access a disposed Audio instance.");
            }
        }

        /// <summary>
        /// When called, releases all resources being used by this
        /// Audio object. Any reference to the object will result in
        /// error. Also stops the updating of this object.
        /// </summary>
        public void Dispose()
        {
            if (this.sound != null)
                sound.release();
            if (dsp != null)
            {
                dsp.disconnectAll(true, true);
                dsp.remove();
                dsp.release();
            }
            audios.Remove(this);
            disposed = true;
        }

        public static void DisposeAudio()
        {
            system.close();
            system.release();
        }
    }
}
