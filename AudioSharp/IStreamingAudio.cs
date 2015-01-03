using System;

namespace AudioSharp
{
	public interface IStreamingAudio : IDisposable
	{
		void SubmitBuffer(byte[] buffer);
		void Play();
		void Pause();
		void Stop();
		event EventHandler BufferNeeded;
		PlayState GetState();
	}
}

