using System;

namespace AudioSharp
{
	/// <summary>
	/// Used for sending PCM Data to an IStreamingAudio instance
	/// </summary>
	/// <param name="instance">The IStreamingAudio that called this event</param>
	/// <param name="buffer">The PCM buffer to submit to the IStreamingAudio</param>
	/// <returns>If there is more data to be sent.<returns> 
	public delegate bool BufferNeededHandler(IStreamingAudio instance, out byte[] buffer);
	/// <summary>
	/// Streams PCM buffers to an Audio Device.
	/// </summary>
	public interface IStreamingAudio : IDisposable
	{
		/// <summary>
		/// Play/Resume the audio
		/// </summary>
		void Play();
		/// <summary>
		/// Pause the audio
		/// </summary>
		void Pause();
		/// <summary>
		/// Stop the audio
		/// </summary>
		void Stop();
		/// <summary>
		/// Occurs when a PCM buffer is needed.
		/// </summary>
		event BufferNeededHandler BufferNeeded;
		/// <summary>
		/// Occurs when playback is finished.
		/// </summary>
		event EventHandler PlaybackFinished;
		/// <summary>
		/// Gets the state of the audio
		/// </summary>
		/// <returns>Playing, Paused, or Stopped</returns>
		PlayState GetState();
		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>A volume between 0.0 and 1.0</value>
		float Volume { get; set; }
	}
}

