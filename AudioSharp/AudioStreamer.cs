using System;

namespace AudioSharp
{
	/// <summary>
	/// Main class for streaming audio. Streams audio from an <see cref="AudioSharp.AudioFile"/> to <see cref="AudioSharp.IAudioDevice"/>  
	/// </summary>
	public class AudioStreamer : IDisposable
	{
		/// <summary>
		/// Occurs when playback is finished.
		/// </summary>
		public event EventHandler PlaybackFinished;

		IStreamingAudio streamer;
		IAudioDevice device;
		AudioFile currentFile;
		float volume = 1f;

		/// <summary>
		/// Gets the size of the PCM buffers.
		/// </summary>
		/// <value>The size of the buffers in bytes.</value>
		public int BufferSize {
			get;
			private set;
		}
		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>A volume between 0.0 and 1.0</value>
		public float Volume {
			get {
				return volume;
			}
			set {
				volume = value;
				if (streamer != null)
					streamer.Volume = value;
			}
		}
		/// <summary>
		/// The AudioFile currently being used
		/// </summary>
		/// <value>The current file.</value>
		public AudioFile CurrentFile
		{
			get {
				return currentFile;
			}
		}
		/// <summary>
		/// Gets the current state of this AudioStreamer
		/// </summary>
		/// <value>Playing, Paused, or Stopped</value>
		public PlayState State
		{
			get {
				if (streamer != null)
					return streamer.GetState ();
				else
					return PlayState.Stopped;
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="AudioSharp.AudioStreamer"/> class.
		/// </summary>
		/// <param name="device">An <see cref="AudioSharp.IAudioDevice" to stream audio to/> </param>
		/// <param name="bufferSize">Size of a PCM buffer in bytes</param>
		public AudioStreamer (IAudioDevice device, int bufferSize = 44100)
		{
			this.device = device;
			BufferSize = bufferSize;
		}
		/// <summary>
		/// Sets up the AudioStreamer using the specified file
		/// </summary>
		/// <param name="file">An <see cref="AudioSharp.AudioFile"/> to use.</param>
		public void Open(AudioFile file)
		{
			if (streamer != null) {
				streamer.BufferNeeded -= GrabBuffer;
				streamer.PlaybackFinished -= OnPlaybackFinished;
				streamer.Dispose ();
			}
			currentFile = file;
			streamer = device.CreateStreamer (file.Decoder.Format, file.Decoder.SampleRate);
			streamer.Volume = volume;
			streamer.BufferNeeded += GrabBuffer;
			streamer.PlaybackFinished += OnPlaybackFinished;
			Play ();
		}
		void OnPlaybackFinished(object sender, EventArgs e)
		{
			if (PlaybackFinished != null)
				PlaybackFinished (this, new EventArgs ());
		}
		bool GrabBuffer(IStreamingAudio instance, out byte[] buffer)
		{
			int length = currentFile.Decoder.Read (BufferSize, out buffer);
			return (length == BufferSize);
		}
		/// <summary>
		/// Plays/Resumes the Audio
		/// </summary>
		public void Play()
		{
			streamer.Play ();
		}
		/// <summary>
		/// Pauses the audio
		/// </summary>
		public void Pause()
		{
			streamer.Pause ();
		}
		/// <summary>
		/// Stops the audio
		/// </summary>
		public void Stop()
		{
			streamer.Stop ();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="AudioSharp.AudioStreamer"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="AudioSharp.AudioStreamer"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="AudioSharp.AudioStreamer"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="AudioSharp.AudioStreamer"/> so the
		/// garbage collector can reclaim the memory that the <see cref="AudioSharp.AudioStreamer"/> was occupying.</remarks>
		public void Dispose()
		{
			if (streamer != null) {
				streamer.BufferNeeded -= GrabBuffer;
				streamer.PlaybackFinished -= OnPlaybackFinished;
				streamer.Dispose ();
			}
		}
	}
}

