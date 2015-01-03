using System;
using System.IO;
using AudioSharp.Containers;
namespace AudioSharp
{
	public class AudioFile
	{
		IDecoder decoder;
		public AudioFile (Stream stream)
		{
			if (!stream.CanSeek)
				throw new NotSupportedException ("Stream that can seek is required");

			decoder = DecoderDetection.GetDecoderFromStream (stream);
		}
		public CodecId CodecId {
			get {
				return decoder.GetCodecId ();
			}
		}
	}
}

