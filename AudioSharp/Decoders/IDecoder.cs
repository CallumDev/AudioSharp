using System;

namespace AudioSharp.Decoders
{
	public interface IDecoder
	{
		CodecId CodecId { get; }
		TimeSpan Duration { get; }
		SoundFormat Format { get; }
		int SampleRate { get; }
		int Read (int length, out byte[] buffer);
		void Reset();
	}
}

