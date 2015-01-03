using System;
using System.IO;
using AudioSharp.Decoders;
namespace AudioSharp
{
	public class AudioFile
	{
		IDecoder decoder;
		public IDecoder Decoder {
			get {
				return decoder;
			}
		}
		public AudioFile (Stream stream)
		{
			if (!stream.CanSeek)
				throw new NotSupportedException ("Stream that can seek is required");

			decoder = DecoderDetection.GetDecoderFromStream (stream);
		}

		public CodecId CodecId {
			get {
				return Decoder.CodecId;
			}
		}
		public TimeSpan Duration {
			get {
				return Decoder.Duration;
			}
		}
	}
}

