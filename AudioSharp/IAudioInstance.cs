using System;

namespace AudioSharp
{
	public interface IAudioInstance : IDisposable
	{
		void SubmitBuffer(byte[] buffer);
		void Play();
		void Pause();
		void Stop();
		event EventHandler BufferNeeded;
		PlayState GetState();
	}
}

