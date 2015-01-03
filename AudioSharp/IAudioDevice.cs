using System;

namespace AudioSharp
{
	public interface IAudioDevice
	{
		IStreamingAudio CreateStreamer();
	}
}

