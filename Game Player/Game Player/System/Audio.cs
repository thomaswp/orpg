using System;
using System.Collections.Generic;
using System.Text;
using TG.Sound;
using System.IO;

namespace Game_Player
{
    /// <summary>
    /// Class for accessing audio methods for playing sounds.
    /// </summary>
    public class Audio
    {
        public string RootPath = "C:\\Program Files\\Common Files\\Enterbrain\\RGSS\\Standard\\Audio\\";

        Microsoft.DirectX.AudioVideoPlayback.Audio BGM;
        OggPlayManager oplay = new OggPlayManager();

        Boolean playerStopped = true;
        Boolean playerStopNextLoop = true;

        public Audio()
        {
            oplay.PlayOggFileResult += new EventHandler<OggPlayEventArgs>(BGSended);
            
        }

        public void Dispose()
        {
            if (BGM != null) { StopBGM(); }
            //if (SE != null) { /*Add Stop*/; }
            oplay.Dispose();
        }

        bool PlayMidi(string filePath, int volume, int balance, ref Microsoft.DirectX.AudioVideoPlayback.Audio audio)
        {
            if (filePath.IndexOf(".") == -1) { filePath += ".mid"; }
            try
            {
                if (audio != null)
                {
                    if (!audio.Disposed)
                    { 
                      audio.Stop();
                       audio.Dispose();
                    }
                }
                audio = new Microsoft.DirectX.AudioVideoPlayback.Audio(filePath, true);
                return true;
            }
            catch 
            {
                Globals.GameSystem.MsgBox("Could not find audio file '" + filePath + "'.");
                return false;
            }
        }

        public void PlayBGM(string filePath)
        {
            PlayBGM(filePath, 0, 0);
        }

        public void PlayBGM(string filePath, int volume, int balance)
        {
            filePath = RootPath + "BGM\\" + filePath;
            PlayMidi(filePath, volume, balance, ref BGM);
        }

        public void StopBGM()
        {
            if (!BGM.Disposed)
            {
                BGM.Stop();
                BGM.Dispose();
            }
        }

        public void ChangeBGMSettings(int newVolume, int newBalance)
        {
            BGM.Volume = newVolume;
            BGM.Balance = newBalance;
        }
        
        public void PlayBGS(string filePath)
        {
            PlayBGS(filePath, 0, 0);
        }

        public void PlayBGS(string filePath, int volume, int balance)
        {
            StopBGS();
            if (filePath.IndexOf(".") == -1) { filePath += ".ogg"; }
            filePath = RootPath + "BGS\\" + filePath;
            try
            {
                if (File.Exists(filePath))
                { oplay.PlayOggFile(filePath, 1, volume, balance); }
                else
                { throw new Exception(); }
                playerStopped = false;
                playerStopNextLoop = false;
            }
            catch
            {
                Globals.GameSystem.MsgBox("Could not find audio file '" + filePath + "'.");
                playerStopped = true;
                playerStopNextLoop = true;
            }
        }

        public void StopBGS()
        {
            if (!playerStopped)
            {
                oplay.StopOggFile(1);
                playerStopped = true;
            }
        }

        void BGSended(object sender, OggPlayEventArgs e)
        {
            if (e.Success & !playerStopped & !playerStopNextLoop)
            {
                oplay.PlayOggFile(e.FileName, e.PlayId, e.VolumeLevel, e.Balance);
            }
        }
    }
}
