using System;

namespace AudioSharp
{
	/// <summary>
	/// A device for streaming audio to. Creates <see cref="AudioSharp.IStreamingAudio"/> instances.
	/// </summary>
	public interface IAudioDevice : IDisposable
	{
		/// <summary>
		/// Creates an IStreamingAudio to stream audio to
		/// </summary>
		/// <returns>A usable IStreamingAudio instance.</returns>
		/// <param name="format">The format of the PCM data</param>
		/// <param name="sampleRate">Sample rate in kHz (e.g. 44100)</param>
		IStreamingAudio CreateStreamer(SoundFormat format, int sampleRate);
	}
}

