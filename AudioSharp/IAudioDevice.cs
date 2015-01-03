using System;

namespace AudioSharp
{
	public interface IAudioDevice
	{
		IStreamingAudio CreateStreamer(SoundFormat format, int sampleRate);
	}
}

