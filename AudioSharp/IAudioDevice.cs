using System;

namespace AudioSharp
{
	public interface IAudioDevice : IDisposable
	{
		IStreamingAudio CreateStreamer(SoundFormat format, int sampleRate);
	}
}

