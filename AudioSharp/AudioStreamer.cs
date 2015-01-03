using System;

namespace AudioSharp
{
	public class AudioStreamer : IDisposable
	{
		IStreamingAudio streamer;
		IAudioDevice device;
		AudioFile currentFile;
		public int BufferSize {
			get;
			private set;
		}
		public AudioFile CurrentFile
		{
			get {
				return currentFile;
			}
		}
		public AudioStreamer (IAudioDevice device, int bufferSize = 44100)
		{
			this.device = device;
			BufferSize = bufferSize;
		}
		public void Open(AudioFile file)
		{
			if (streamer != null) {
				streamer.BufferNeeded -= GrabBuffer;
				streamer.Dispose ();
			}
			currentFile = file;
			streamer = device.CreateStreamer (file.Decoder.Format, file.Decoder.SampleRate);
			streamer.BufferNeeded += GrabBuffer;
			Play ();
		}
		bool GrabBuffer(IStreamingAudio instance)
		{
			byte[] buffer;
			int length = currentFile.Decoder.Read (BufferSize, out buffer);
			instance.SubmitBuffer (buffer);
			return (length == BufferSize);
		}
		public void Play()
		{
			streamer.Play ();
		}
		public void Pause()
		{
			streamer.Pause ();
		}
		public void Stop()
		{
			streamer.Stop ();
		}
		public void Dispose()
		{
			if (streamer != null) {
				streamer.BufferNeeded -= GrabBuffer;
				streamer.Dispose ();
			}
		}
	}
}

