using System;

namespace AudioSharp
{
	public delegate bool BufferNeededHandler(IStreamingAudio instance);
	public interface IStreamingAudio : IDisposable
	{
		void SubmitBuffer(byte[] buffer);
		void Play();
		void Pause();
		void Stop();
		event BufferNeededHandler BufferNeeded;
		PlayState GetState();
	}
}

