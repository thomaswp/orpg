// Copyright (c) 2003-2006 TrayGames, LLC 
// All rights reserved. Reproduction or transmission of this file, or a portion
// thereof, is forbidden without prior written permission of TrayGames, LLC.
//
// Author: Perry L. Marchant
// Date: June 2 2005

using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace TG.Sound
{
    //[Description(1, "Class for managing the playback of an Ogg Vorbis encoded sound file."),
    //Remarks("This class requires Managed DirectX to be installed because it uses a DirectSound " +
    //"device for audio playback during Ogg Vorbis decoding."),
    //Example("http://developer.traygames.com/Docs/?doc=OggLib")]
	public class OggPlayManager : IDisposable
	{
		#region Members and events code

		private const int WaitTimeout = 5000; 
		private Device directSoundDevice;
		private SampleSize oggFileSampleSize;
        private bool globalFocus;
        private bool disposed;
        private List<Thread> playbackThreads;

        //[Description(1, "Describes the two possible sample rates that can be used for playback.")] 
        public enum SampleSize { EightBits, SixteenBits }

        //[Description(1, "Event notification when the <b>PlayOggFile</b> method has completed.")] 
        public event EventHandler<OggPlayEventArgs> PlayOggFileResult;

        //[Description(1, "Event notification when the client wishes to interrupt playback.")]
        public event EventHandler<OggPlayEventArgs> StopOggFileNow;

		#endregion

		#region Properties code	

        //[Description(1, "Indicates the requested sample size for the audio data."),
        //Remarks("8-bit sample size has lower quality but is faster and takes less memory than 16-bit sample size." +
        //"If your games ogg files are encoded with an 8-bit sample size, then choose 8 (you can also choose 16, but that is " +
        //"wasteful and gains nothing if the Ogg sources are only 8-bit). If your game's Ogg files are encoded with a " +
        //"16-bit sample size, then choose 16 to get the full sound quality during playback, or choose 8 (or give the user " +
        //"the option of choosing 8) if you want to minimize playback resource requirements. If your game's Ogg files " +
        //"are a mixture (some are encoded with 8-bit sample size and others are encoded with 16-bit sample size), then " +
        //"choose whichever you think is best (either setting, EightBits or SixteenBits, will play all the ogg files).")]        
        public SampleSize OggFileSampleSize
		{
			get { return oggFileSampleSize; }
			set { oggFileSampleSize = value; }
		}

        //[Description(1, "Indicates whether or not to play buffer on loss of focus.")]
        public bool GlobalFocus
        {
            get { return globalFocus; }
            set { globalFocus = value; }
        }

        #endregion

		#region Intialization code

        //[Description(1, "Constructor. Creates a new DirectX Sound device, sets the cooperative level, global focus, " +
        //"and sample size. <b>owner</b> is used by the DirectSound SetCooperativeLevel method, which defines this parameter " +
        //"as the <b>System.Windows.Forms.Control</b> of the application that is using the Device object. This should be " +
        //"your applications main window. <b>globalFocus</b> will allow the buffer to play even if the owner loses focus. " +
        //"<b>wantedOggSampleSize</b> is either EightBits or SixteenBits, the sample rate you desire."),
        //Remarks("8-bit sample size has lower quality but is faster and takes less memory than 16-bit sample size." +
        //"If your game's ogg files are encoded with an 8-bit sample size, then choose 8 (you can also choose 16, but that is " +
        //"wasteful and gains nothing if the Ogg sources are only 8-bit). If your game's Ogg files are encoded with a " +
        //"16-bit sample size, then choose 16 to get the full sound quality during playback, or choose 8 (or give the user " +
        //"the option of choosing 8) if you want to minimize playback resource requirements. If your game's Ogg files " +
        //"are a mixture (some are encoded with 8-bit sample size and others are encoded with 16-bit sample size), then " +
        //"choose whichever you think is best (either setting, EightBits or SixteenBits, will play all the ogg files)." +
        //"IMPORTANT: To stay compatible with software decoders you should encode your ogg file at 44khz or lower!")]
        public OggPlayManager(/*Control owner,*/ bool wantGlobalFocus, SampleSize wantedOggSampleSize)
        {
            // NOTE: You will get the following warning when targeting the .NET 2.0
            // platform: 'Managed Debugging Assistant 'LoaderLock' has detected a 
            // problem in 'TGOggPlayer.vshost.exe'. This seems to be an issue in the 
            // current Managed DirectX library and will have to be ignored for now.

            if (null == (directSoundDevice = new Device(DSoundHelper.DefaultPlaybackDevice)))
            {
                throw new Exception("Unable to create Microsoft.DirectX.DirectSound Device object.");
            }

            // DirectSound documentation recommends Priority for games
            //this.directSoundDevice.SetCooperativeLevel(owner, 
            //    null != owner ? CooperativeLevel.Priority : CooperativeLevel.Normal);
            this.directSoundDevice.SetCooperativeLevel(new Control(), CooperativeLevel.Priority);
            
            // Set OggSampleSize 8 or 16 bit, see description for details
            this.oggFileSampleSize = wantedOggSampleSize;

            // Set whether or not to play buffer on loss of focus
            this.globalFocus = wantGlobalFocus;

            // Create a new list to hold thread objects
            if (null == (playbackThreads = new List<Thread>()))
            {
                throw new Exception("Unable to create playback thread array list.");
            }
        }

        //[Description(1, "Overloaded constructor. Defaults <b>globalFocus</b> to false and the <b>wantedSampleSize</b> to 16-bit. " +
        //"You provide your applications main window for the <b>owner</b> parameter.")]
        public OggPlayManager(/*Control owner*/)
            : this(/*owner, */false, SampleSize.SixteenBits)
        {
        }

        //[Description(1, "Overloaded constructor. Defaults <b>globalFocus</b> to false. " +
        //"You provide your applications main window for the owner parameter and desired sample size.")]
        public OggPlayManager(/*Control owner, */SampleSize wantedOggSampleSize)
            : this(/*owner, */false, wantedOggSampleSize)
        {
        }

        #endregion

		#region IDisposable implementation

		protected virtual void Dispose(bool disposing)
		{
			lock(this)
			{
				// Do nothing if the object has already been disposed of
				if (disposed)
					return;

                Debug.WriteLine("OggPlayer.cs: Disposing object");

                // If disposing equals true, dispose all managed 
                // and unmanaged resources.

				if (disposing)
				{
					// Release disposable objects used by this instance here

					// Cleanup DirectSound Device
					if (directSoundDevice != null)
					{
						directSoundDevice.Dispose();
						directSoundDevice = null;
					}
				}

				// Release unmanaged resource here. Don't access reference type fields

				// Remember that the object has been disposed of
				disposed = true;
			}
		}

        //[Description(1, "You must call this method when you are done using the class " +
        //"instance, it will dispose the DirectSound device and other resources.")]
		public void Dispose()
		{
            // A derived class should not be able to override this method.
            Dispose(true);

            // Take yourself off of the finalization queue to prevent 
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
		}

		#endregion

		#region Ogg playback commands code

        //[Description(1, "Plays the specified Ogg Vorbis file. Accepts the name of the sound file to play and an " +
        //"arbitrary Id value (caller determined)."),
        //Remarks("See the overloaded <b>PlayOggFile</b> method for more details."),
        //ReturnValue("Returns immediately, decoding and playback are done in a separate thread.")]
        public void PlayOggFile(string fileName, int playId)
        {
            PlayOggFile(fileName, playId, 0, 0);
        }

        //[Description(1, "Plays the specified Ogg Vorbis file. Accepts the <b>fileName</b> of the sound file to play, an arbitrary " +
        //"Id value (determined by the caller), volume level and balance as parameters. The <b>playId</b> is returned " +
        //"in the raised event so your handler code can use it to identify which specific <b>PlayOggFile</b> call resulted " +
        //"in that handled event. The <b>volumeLevel</b> can be set in a range from 0 (full volume) to -10,000 (silent)." +
        //"Unlike volume, <b>balance</b> ranges from -10,000 (full left) to +10,000 (full right), with zero being center."),
        //Remarks("Add your event handler to the public event <b>PlayOggFileResult</b>. Also see the overloaded version of " +
		//"this method if you want to play an Ogg Vorbis file from memory instead."),
		//ReturnValue("Returns immediately, decoding and playback are done in a separate thread.")]
		public void PlayOggFile(string fileName, int playId, int volumeLevel, int balance)
		{
			// Create an event argument class identified by the playId
			OggPlayEventArgs OggPlayArgs = new OggPlayEventArgs(playId, fileName, volumeLevel, balance);

			// Decode the Ogg Vorbis file in a separate thread
            OggPlayThreadInfo OggPlayThread = new OggPlayThreadInfo(
				OggPlayArgs, fileName, OggFileSampleSize == SampleSize.EightBits ? 8 : 16,
				globalFocus, directSoundDevice, this);

            // Start the thread
            Thread PlaybackThread = new Thread(new ThreadStart(OggPlayThread.PlayOggDecodeThreadProc));
            if (null != PlaybackThread)
            {
                playbackThreads.Add(PlaybackThread);
                PlaybackThread.Start();
            }
		}

        //[Description(1, "Plays the specified Ogg Vorbis data from the specified memory stream instead of a file."),
        //Remarks("See the overloaded <b>PlayOggFile</b> method for more details."),
        //ReturnValue("Returns immediately, decoding and playback are done in a separate thread.")]
        public void PlayOggFile(byte[] data, int playId, int volumeLevel, int balance)
		{
			// Create an event argument class identified by the playId
			OggPlayEventArgs OggPlayArgs = new OggPlayEventArgs(playId, volumeLevel, balance);

			// Decode the Ogg Vorbis memory stream in a separate thread
			OggPlayThreadInfo OggPlayThread = new OggPlayThreadInfo(
				OggPlayArgs, data, OggFileSampleSize == SampleSize.EightBits ? 8 : 16,
				globalFocus, directSoundDevice, this);

            // Start the thread
			Thread PlaybackThread = new Thread(new ThreadStart(OggPlayThread.PlayOggDecodeThreadProc));
            if (null != PlaybackThread)
            {
                playbackThreads.Add(PlaybackThread);
                PlaybackThread.Start();
            }
        }

        //[Description(1, "Blocks the calling thread until all outstanding playback threads terminate, while " +
        //"continuing to perform standard message pumping."),
        //Remarks("Call this function after calling the <b>StopOggFile</b> method on all of your Ogg Vorbis files.")]
        public void WaitForAllOggFiles()
        {
            // Wait for all playback threads to terminate
            foreach (Object obj in playbackThreads)
            {
                Thread PlaybackThread = (Thread)obj;
                if (null != PlaybackThread)
                    PlaybackThread.Join();
            }
        }

        //[Description(1, "Blocks the calling thread until all outstanding playback threads terminate, or the " +
        //"specified time elapses, while continuing to perform standard <b>SendMessage</b> pumping."),
        //Remarks("See the overloaded <b>WaitForAllOggFiles</b> method for more details.")]
        public void WaitForAllOggFiles(TimeSpan timeOut)
        {
            // Wait for all playback threads to terminate
            foreach (Object obj in playbackThreads)
            {
                Thread PlaybackThread = (Thread)obj;
                if (null != PlaybackThread)
                    PlaybackThread.Join(timeOut);
            }
        }

        //[Description(1, "Stops the specified Ogg Vorbis file. playId is the Id of the Ogg Vorbis file to stop playback on.")]
        public void StopOggFile(int playId)
        {
            // Let the playback thread know we want to cancel playback
            OggPlayEventArgs StopArgs = new OggPlayEventArgs(playId, true);
            StopOggFileNow(this, StopArgs);
        }

        #endregion

		/// <summary>
        /// Class for decoding and playing back of Ogg audio data. This class contains the method
        /// used for the thread start call in the PlayOggFile method of the OggPlayManager class.
		/// </summary>
		private class OggPlayThreadInfo
		{
			#region Members and events code

			private string fileName;
			private byte[] memFile;
			private int bitsPerSample;  // either 8 or 16
			private bool stopNow;
            private bool wantGlobalFocus;
            private Device directSoundDevice;
			private OggPlayEventArgs oggInfo;
			private OggPlayManager oggPlay;

			#endregion

			#region Initialization code

            //[Description(1, "Overloaded constructor. Validates parameters and initializes object members " +
            //"that are common whether your are loading from a file or memory.")]
			public OggPlayThreadInfo(OggPlayEventArgs oggInfo, int bitsPerSample, 
                bool wantGlobalFocus, Device directSound, OggPlayManager oggPlay)
			{
				// Verify parameters
				if (null == oggInfo)
                    throw new System.ArgumentNullException("oggInfo");

                if (null == directSound)
                    throw new System.ArgumentNullException("directSound");

                if (null == oggPlay)
                    throw new System.ArgumentNullException("oggPlay");

                // Initialize this objects data members
				this.oggInfo = oggInfo;
				this.bitsPerSample = bitsPerSample;
				this.directSoundDevice = directSound;
                this.wantGlobalFocus = wantGlobalFocus;
                this.oggPlay = oggPlay;
                
				// Add the interrupt event handler
				oggPlay.StopOggFileNow += new EventHandler<OggPlayEventArgs>(InterruptOggFilePlayback);
			}

            //[Description(1, "Overloaded constructor. Validates parameters and initializes object members " +
            //"when you want to load Ogg audio data from a file.")]
            public OggPlayThreadInfo(OggPlayEventArgs oggInfo, string fileName,
                int bitsPerSample, bool wantGlobalFocus, Device directSound, OggPlayManager oggPlay)
                : this(oggInfo, bitsPerSample, wantGlobalFocus, directSound, oggPlay)
            {
                if (null == fileName)
                    throw new System.ArgumentNullException("fileName");
                this.fileName = fileName;
            }

            //[Description(1, "Constructor. Validates parameters and initializes object members " +
            //"when you want to load Ogg audio data from memory.")]
            public OggPlayThreadInfo(OggPlayEventArgs oggInfo, byte[] memFile,
                int bitsPerSample, bool wantGlobalFocus, Device directSound, OggPlayManager oggPlay)
                : this(oggInfo, bitsPerSample, wantGlobalFocus, directSound, oggPlay)
            {
                if (null == memFile)
                    throw new System.ArgumentNullException("memFile");
                this.memFile = memFile;
                this.fileName = string.Empty;
            }

			#endregion

			#region Interrupt handler code

			//[Description(1, "Stops thread playback thread immediately.")]
			public void InterruptOggFilePlayback(object sender, OggPlayEventArgs e)
			{
				if (e.PlayId == oggInfo.PlayId)
				{
					stopNow = true;
				}
			}

			#endregion

            #region Decode and playback code

            //[Description(1, "Playback thread for use by the PlayOggFile method of OggPlayManager class.")]
			public void PlayOggDecodeThreadProc()
			{
				// Call external C++ functions to decode the Ogg Vorbis data
				unsafe 
				{
                    // Declare members that must be disposed later
					void *vf = null;
                    SecondaryBuffer SecBuf = null;
					BufferDescription MyDescription = null;
					Notify MyNotify = null;
                    MemoryStream PcmStream = null;
                    AutoResetEvent
                        SecBufNotifyAtBegin = new AutoResetEvent(false),
                        SecBufNotifyAtOneThird = new AutoResetEvent(false),
                        SecBufNotifyAtTwoThirds = new AutoResetEvent(false);
                    int ErrorCode = 0;

                    try
                    {
                        // Initialize the file for Ogg Vorbis decoding using
                        // data from either a file name or memory stream.
                        if (null != memFile)
                            ErrorCode = NativeMethods.memory_stream_for_ogg_decode(memFile, memFile.Length, &vf);
                        else if (null != fileName)
                            ErrorCode = NativeMethods.init_for_ogg_decode(fileName, &vf);

                        // If any error occurred then set the reason for failure
                        // and return it to the calling application.
                        if (ErrorCode != 0)
                        {
                            // Build the reason string
                            oggInfo.ReasonForFailure = ErrorCodeToMessage(ErrorCode);

                            // Raise the finished play event to return status
                            if (null != oggPlay.PlayOggFileResult)
                            {
                                oggInfo.ReasonForFailure += " Id= " + oggInfo.PlayId.ToString();
                                oggPlay.PlayOggFileResult(this, oggInfo);
                            }

                            return;
                        }

                        // DirectSound documentation recommends from 1 to 2 seconds 
                        // for buffer size, so 1.2 is an arbitrary but good choice.
                        double SecBufHoldThisManySeconds = 1.2;
                        int PcmBytes, ChannelsCount = 0, SamplingRate = 0,
                            PreviousChannelCount = 0, PreviousSamplingCount = 0,
                            AverageBytesPerSecond = 0, BlockAlign = 0,
                            PcmStreamNextConsumPcmPosition = 0,
                            SecBufByteSize = 0, SecBufNextWritePosition = 0,
                            SecBufPlayPositionWhenNextWritePositionSet = 0;
                        WaitHandle[] SecBufWaitHandles = {SecBufNotifyAtBegin,
													SecBufNotifyAtOneThird,
													SecBufNotifyAtTwoThirds};
                        bool FirstTime = true, AtEOF = false, SecBufInitialLoad = true;

                        // Vorbisfile API documentation recommends 
                        // a PCM buffer of 4096 bytes.
                        byte[] PcmBuffer = new byte[4096];
                        PcmStream = new MemoryStream();
                        WaveFormat MyWaveFormat = new WaveFormat();

                        Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder successfully initialized. Id= " + oggInfo.PlayId.ToString());

                        // Decode the Ogg Vorbis data into its PCM data
                        while (true)
                        {
                            // Check for stop request
                            if (stopNow)
                            {
                                // Client has decided to stop playback!
                                if (null != SecBuf)
                                    SecBuf.Stop();
                                Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder playback interrupted. Id= " + oggInfo.PlayId.ToString());
                                break;
                            }

                            // Get the next chunk of PCM data, pin these so the GC can't 
                            // relocate them.
                            fixed (byte* buf = &PcmBuffer[0])
                            {
                                fixed (int* HoleCount = &oggInfo.ErrorHoleCount)
                                {
                                    fixed (int* BadLinkCount = &oggInfo.ErrorBadLinkCount)
                                    {
                                        // The sample size of the returned PCM data -- either 8-bit 
                                        // or 16-bit samples -- is set by BitsPerSample.
                                        PcmBytes = NativeMethods.ogg_decode_one_vorbis_packet(
                                            vf, buf, PcmBuffer.Length,
                                            bitsPerSample,
                                            &ChannelsCount, &SamplingRate,
                                            HoleCount, BadLinkCount);
                                    }
                                }
                            }

                            // Set AtEOF if we don't have anymore PCM data
                            if (PcmBytes == 0)
                            {
                                AtEOF = true;
                            }

                            // Check if we are at EOF before we even started
                            if (FirstTime && AtEOF)
                            {
                                if (null != oggPlay.PlayOggFileResult)
                                {
                                    oggInfo.ReasonForFailure = "The Ogg Vorbis file or memory stream has no usable data. Id= " + oggInfo.PlayId.ToString();
                                    oggPlay.PlayOggFileResult(this, oggInfo);
                                }
                                return;
                            }

                            // We must be aware that multiple bitstream sections do not 
                            // necessarily use the same number of channels or sampling rate.							
                            if (!FirstTime &&
                                (ChannelsCount != PreviousChannelCount
                                || SamplingRate != PreviousSamplingCount))
                            {
                                // Format changed so we must stop playback!
                                if (null != SecBuf)
                                    SecBuf.Stop();

                                if (null != oggPlay.PlayOggFileResult)
                                {
                                    oggInfo.ReasonForFailure = "The Ogg Vorbis file has a format change and DirectSound can't handle this. Id= " + oggInfo.PlayId.ToString();
                                    oggPlay.PlayOggFileResult(this, oggInfo);
                                }

                                return;
                            }

                            // Use the PCM data
                            if (FirstTime)
                            {
                                // Handle first time through this loop
                                Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder playing ogg file or memory stream Id= " + oggInfo.PlayId.ToString());

                                // Compute format items
                                BlockAlign = ChannelsCount * (bitsPerSample / 8);
                                AverageBytesPerSecond = SamplingRate * BlockAlign;
                                int HoldThisManySamples = (int)(SamplingRate * SecBufHoldThisManySeconds);

                                // Set the format items
                                MyWaveFormat.AverageBytesPerSecond = AverageBytesPerSecond;
                                MyWaveFormat.BitsPerSample = (short)bitsPerSample;
                                MyWaveFormat.BlockAlign = (short)BlockAlign;
                                MyWaveFormat.Channels = (short)ChannelsCount;
                                MyWaveFormat.SamplesPerSecond = SamplingRate;
                                MyWaveFormat.FormatTag = WaveFormatTag.Pcm;

                                // Describe the capabilities of this buffer
                                MyDescription = new BufferDescription();
                                MyDescription.Format = MyWaveFormat;
                                MyDescription.BufferBytes = SecBufByteSize = HoldThisManySamples * BlockAlign;
                                MyDescription.CanGetCurrentPosition = true;
                                MyDescription.ControlPositionNotify = true;
                                MyDescription.ControlVolume = true;
                                MyDescription.ControlPan = true;
                                // Tells the primary buffer to continue play this buffer, 
                                // even if the application loses focus.
                                MyDescription.GlobalFocus = wantGlobalFocus;

                                try
                                {
                                    // Pass in the the buffer description, and the
                                    // the Device we created earlier. Also set the
                                    // volume and balance for this buffer.
                                    SecBuf = new SecondaryBuffer(MyDescription, directSoundDevice);
                                    SecBuf.Volume = oggInfo.VolumeLevel;
                                    SecBuf.Pan = oggInfo.Balance;
                                }
                                catch (ControlUnavailableException)
                                {
                                    // NOTE: The buffer control (volume, pan, etc) being requested is not 
                                    // available. Controls must be specified when the buffer is created.
                                    // We will try to create the buffer again below without the controls.

                                    if (SecBuf != null)
                                    {
                                        SecBuf.Dispose();
                                        SecBuf = null;
                                    }
                                }

                                if (SecBuf == null)
                                {
                                    // Must have been a ControlUnavailableException so retry
                                    MyDescription.ControlVolume = false;
                                    MyDescription.ControlPan = false;
                                    SecBuf = new SecondaryBuffer(MyDescription, directSoundDevice);
                                }

                                // Set 3 notification points, at 0, 1/3, and 2/3 SecBuf size
                                BufferPositionNotify[] MyBufferPositions = new BufferPositionNotify[3];
                                MyBufferPositions[0].Offset = 0;
                                MyBufferPositions[0].EventNotifyHandle = SecBufNotifyAtBegin.SafeWaitHandle.DangerousGetHandle();
                                MyBufferPositions[1].Offset = (HoldThisManySamples / 3) * BlockAlign;
                                MyBufferPositions[1].EventNotifyHandle = SecBufNotifyAtOneThird.SafeWaitHandle.DangerousGetHandle();
                                MyBufferPositions[2].Offset = ((HoldThisManySamples * 2) / 3) * BlockAlign;
                                MyBufferPositions[2].EventNotifyHandle = SecBufNotifyAtTwoThirds.SafeWaitHandle.DangerousGetHandle();

                                // Set notification for points above
                                MyNotify = new Notify(SecBuf);
                                MyNotify.SetNotificationPositions(MyBufferPositions);

                                // Prepare for next iteration
                                PreviousChannelCount = ChannelsCount;
                                PreviousSamplingCount = SamplingRate;
                                FirstTime = false;
                            }
                            else if (AtEOF)
                            {
                                // Handle the end of file
                                Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder playback at end of file. Id= " + oggInfo.PlayId.ToString());

                                Debug.Assert(SecBufPlayPositionWhenNextWritePositionSet >= 0
                                    && SecBufPlayPositionWhenNextWritePositionSet < SecBufByteSize
                                    && SecBufNextWritePosition >= 0
                                    && SecBufNextWritePosition < SecBufByteSize);

                                // Start playback if there wasn't enough PCM 
                                // data to fill SecBuf the first time.
                                if (SecBufInitialLoad)
                                {
                                    Debug.Assert(SecBufPlayPositionWhenNextWritePositionSet == 0
                                        && SecBufNextWritePosition > 0);

                                    // Play method does the playing in its own thread
                                    SecBuf.Play(0, BufferPlayFlags.Looping);
                                    Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder starting playback. Id= " + oggInfo.PlayId.ToString());
                                }

                                // Poll for end of current playback
                                int PlayPosition, LoopbackCount = 0,
                                    PreviousPlayPosition = SecBufPlayPositionWhenNextWritePositionSet;

                                for (; ; PreviousPlayPosition = PlayPosition)
                                {
                                    // Yield 10 millisecond timeslice, an arbitrary 
                                    // but good choice.
                                    Thread.Sleep(10);

                                    PlayPosition = SecBuf.PlayPosition;

                                    if (PlayPosition < PreviousPlayPosition)
                                        ++LoopbackCount;

                                    if (SecBufPlayPositionWhenNextWritePositionSet <= SecBufNextWritePosition)
                                    {
                                        if (PlayPosition >= SecBufNextWritePosition || LoopbackCount > 0)
                                            break;
                                    }
                                    else
                                    {
                                        if ((PlayPosition < SecBufPlayPositionWhenNextWritePositionSet
                                            && PlayPosition >= SecBufNextWritePosition) || LoopbackCount > 1)
                                            break;
                                    }
                                }

                                // Finished playing so we can stop playback!
                                if (null != SecBuf)
                                    SecBuf.Stop();
                                Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder finished playback. Id= " + oggInfo.PlayId.ToString());
                                break;
                            }

                            // Copy the new PCM data into a PCM memory stream
                            PcmStream.SetLength(0);
                            PcmStream.Write(PcmBuffer, 0, PcmBytes);
                            PcmStream.Position = 0;
                            PcmStreamNextConsumPcmPosition = 0;

                            Debug.Assert(PcmStream.Length == PcmBytes);

                            if (SecBufInitialLoad)
                            {
                                // Handle initial load of secondary buffer
                                int WriteCount = (int)Math.Min(
                                    PcmStream.Length,
                                    SecBufByteSize - SecBufNextWritePosition);

                                Debug.Assert(WriteCount >= 0);

                                if (WriteCount > 0)
                                {
                                    Debug.Assert(PcmStream.Position == 0);

                                    SecBuf.Write(
                                        SecBufNextWritePosition,
                                        PcmStream,
                                        WriteCount,
                                        LockFlag.None);

                                    SecBufNextWritePosition += WriteCount;
                                    PcmStreamNextConsumPcmPosition += WriteCount;
                                }

                                if (SecBufByteSize == SecBufNextWritePosition)
                                {
                                    // Finished filling the buffer
                                    SecBufInitialLoad = false;
                                    SecBufNextWritePosition = 0;

                                    // So start the playback in its own thread
                                    SecBuf.Play(0, BufferPlayFlags.Looping);

                                    // Yield rest of timeslice so playback can  
                                    // start right away.
                                    Thread.Sleep(0);
                                }
                                else
                                {
                                    continue;  // Get more PCM data
                                }
                            }

                            // Exhaust the current PCM data by writing the data into SecBuf
                            for (; PcmStreamNextConsumPcmPosition < PcmStream.Length; )
                            {
                                int WriteCount = 0,
                                    PlayPosition = SecBuf.PlayPosition,
                                    WritePosition = SecBuf.WritePosition;

                                if (SecBufNextWritePosition < PlayPosition
                                    && (WritePosition >= PlayPosition || WritePosition < SecBufNextWritePosition))
                                    WriteCount = PlayPosition - SecBufNextWritePosition;
                                else if (SecBufNextWritePosition > WritePosition
                                    && WritePosition >= PlayPosition)
                                    WriteCount = (SecBufByteSize - SecBufNextWritePosition) + PlayPosition;

                                Debug.Assert(WriteCount >= 0 && WriteCount <= SecBufByteSize);

                                if (WriteCount > 0)
                                {
                                    WriteCount = (int)Math.Min(
                                        WriteCount,
                                        PcmStream.Length - PcmStreamNextConsumPcmPosition);

                                    PcmStream.Position = PcmStreamNextConsumPcmPosition;

                                    SecBuf.Write(
                                        SecBufNextWritePosition,
                                        PcmStream,
                                        WriteCount,
                                        LockFlag.None);

                                    SecBufNextWritePosition =
                                        (SecBufNextWritePosition + WriteCount) % SecBufByteSize;
                                    SecBufPlayPositionWhenNextWritePositionSet = PlayPosition;
                                    PcmStreamNextConsumPcmPosition += WriteCount;
                                }
                                else
                                {
                                    WaitHandle.WaitAny(SecBufWaitHandles, new TimeSpan(0, 0, 5), true);
                                }
                            }
                        }

                        // Finito
                        if (null != oggPlay.PlayOggFileResult)
                        {
                            oggInfo.Success = true;
                            oggInfo.StopRequest = stopNow;
                            oggPlay.PlayOggFileResult(this, oggInfo);
                        }

                        Debug.WriteLine("OggPlayer.cs: Finished playback of sound file. Id= " + oggInfo.PlayId.ToString());
                    }
                    catch (NoDriverException ex)
                    {
                        // NOTE: The Notify.SetNotificationPositions method will throw 
                        // this exception if there is not driver available for playback. 
                        // There is not much we can do here except clean up and leave.

                        if (null != oggPlay.PlayOggFileResult)
                        {
                            // Raise an event back to the client indicating failure
                            oggInfo.ReasonForFailure = ex.Message;
                            oggPlay.PlayOggFileResult(this, oggInfo);
                        }

                        Debug.WriteLine("OggPlayer.cs: Failed to playback sound file. " + ex.ToString());
                    }
                    catch (ControlUnavailableException ex)
                    {
                        // NOTE: The buffer control (volume, pan, etc) being requested is not 
                        // available. Controls must be specified when the buffer is created.

                        if (null != oggPlay.PlayOggFileResult)
                        {
                            // Raise an event back to the client indicating failure
                            oggInfo.ReasonForFailure = ex.Message;
                            oggPlay.PlayOggFileResult(this, oggInfo);
                        }

                        Debug.WriteLine("OggPlayer.cs: Failed to playback sound file. " + ex.ToString());
                    }
                    catch (ArgumentException ex)
                    {
                        // NOTE: If an attempt is made to create a SecondaryBuffer with 
                        // Control3D set to true in the BufferDescription and you attempt 
                        // to load the buffer with a stero sound, the constructor fails 
                        // throwing an ArgumentException.

                        if (null != oggPlay.PlayOggFileResult)
                        {
                            // Raise an event back to the client indicating failure
                            oggInfo.ReasonForFailure = ex.Message;
                            oggPlay.PlayOggFileResult(this, oggInfo);
                        }

                        Debug.WriteLine("OggPlayer.cs: Failed to playback sound file. " + ex.ToString());
                    }
                    catch (Exception ex)
                    {
                        // Report the exception but allow the client to continue.
                        Debug.WriteLine("PlayOggDecodeThreadProc(): Failed to playback sound file." + Environment.NewLine +
                            "Exception = " + ex.GetType().ToString() + Environment.NewLine +
                            "Full Text = " + ex.Message + Environment.NewLine + ex.StackTrace);

                        if (null != oggPlay.PlayOggFileResult)
                        {
                            // Raise an event back to the client indicating failure
                            oggInfo.ReasonForFailure = ex.Message;
                            oggPlay.PlayOggFileResult(this, oggInfo);
                        }
                    }
                    finally
                    {
                        // Cleanup Ogg Vorbis decoder library
                        ErrorCode = NativeMethods.final_ogg_cleanup(vf);
                        Debug.Assert(ErrorCode == 0);

                        // Cleanup AutoReset event items
                        if (null != SecBufNotifyAtBegin)
                        {
                            if (null != SecBufNotifyAtBegin.SafeWaitHandle)
                                SecBufNotifyAtBegin.SafeWaitHandle.Close();
                            SecBufNotifyAtBegin.Close();
                            SecBufNotifyAtBegin = null;
                        }

                        if (null != SecBufNotifyAtOneThird)
                        {
                            if (null != SecBufNotifyAtOneThird.SafeWaitHandle)
                                SecBufNotifyAtOneThird.SafeWaitHandle.Close();
                            SecBufNotifyAtOneThird.Close();
                            SecBufNotifyAtOneThird = null;
                        }

                        if (null != SecBufNotifyAtTwoThirds)
                        {
                            if (null != SecBufNotifyAtTwoThirds.SafeWaitHandle)
                                SecBufNotifyAtTwoThirds.SafeWaitHandle.Close();
                            SecBufNotifyAtTwoThirds.Close();
                            SecBufNotifyAtTwoThirds = null;
                        }

                        // Cleanup DirectSound related items
                        if (null != SecBuf)
                        {
                            SecBuf.Dispose();
                            SecBuf = null;
                        }

                        if (null != PcmStream)
                        {
                            PcmStream.Close();
                            PcmStream = null;
                        }

                        if (null != MyDescription)
                        {
                            MyDescription.Dispose();
                            MyDescription = null;
                        }

                        if (null != MyNotify)
                        {
                            MyNotify.Dispose();
                            MyNotify = null;
                        }

                        Debug.WriteLine("OggPlayer.cs: Ogg Vorbis decoder objects cleaned up.");
                    } // Finally
				}  // Unsafe  
            } // PlayOggDecodeThreadProc

            #endregion

            #region Helper code

            private String ErrorCodeToMessage(int ErrorCode)
            {
                String Message = "Ogg Vorbis decoder initialization for ogg file or memory stream failed: ";

                switch (ErrorCode)
                {
                    case NativeMethods.ifod_err_open_failed:
                        Message += "Unable to find or open the specified ogg file.";
                        break;

                    case NativeMethods.ifod_err_malloc_failed:
                        Message += "A call to malloc failed. May indicatate an 'Out Of Memory' error.";
                        break;

                    case NativeMethods.ifod_err_read_failed:
                        Message += "A read from the media returned an error.";
                        break;

                    case NativeMethods.ifod_err_not_vorbis_data:
                        Message += "The bitstream does not appear to be Ogg Vorbis data.";
                        break;

                    case NativeMethods.ifod_err_vorbis_version_mismatch:
                        Message += "Ogg Vorbis library version mismatch.";
                        break;

                    case NativeMethods.ifod_err_invalid_vorbis_header:
                        Message += "Invalid Ogg Vorbis bitstream header.";
                        break;

                    case NativeMethods.ifod_err_internal_fault:
                        Message +=
                            "Internal logic fault. May indicate a defect or heap/stack corruption.";
                        break;

                    case NativeMethods.ifod_err_unspecified_error:
                        Message += "Ogg Vorbis library function call (ov_open) returned an undocumented error.";
                        break;
                }

                return Message;
            }

            #endregion
        } 
    }
} 
