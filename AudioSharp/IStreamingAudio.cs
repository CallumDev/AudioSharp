using System;

namespace AudioSharp
{
	public delegate bool BufferNeededHandler(IStreamingAudio instance, out byte[] buffer);
	public interface IStreamingAudio : IDisposable
	{
		void Play();
		void Pause();
		void Stop();
		event BufferNeededHandler BufferNeeded;
		event EventHandler PlaybackFinished;
		PlayState GetState();
		float Volume { get; set; }
	}
}

