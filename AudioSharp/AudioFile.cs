using System;
using System.IO;
using AudioSharp.Decoders;
namespace AudioSharp
{
	/// <summary>
	/// An Audio File
	/// </summary>
	public class AudioFile
	{
		IDecoder decoder;
		/// <summary>
		/// Gets the current decoder being used.
		/// </summary>
		/// <value>The decoder.</value>
		public IDecoder Decoder {
			get {
				return decoder;
			}
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="AudioSharp.AudioFile"/> class.
		/// </summary>
		/// <param name="stream">The stream to load the Audio from</param>
		public AudioFile (Stream stream)
		{
			if (!stream.CanSeek)
				throw new NotSupportedException ("Stream that can seek is required");

			decoder = DecoderDetection.GetDecoderFromStream (stream);
		}
		/// <summary>
		/// Gets the codec of the AudioFile before decoding
		/// </summary>
		/// <value>A <see cref="AudioSharp.CodecId"/> decribing the codec</value>
		public CodecId CodecId {
			get {
				return Decoder.CodecId;
			}
		}
		/// <summary>
		/// Gets the duration of the AudioFile.
		/// </summary>
		/// <value>The total duration</value>
		public TimeSpan Duration {
			get {
				return Decoder.Duration;
			}
		}
	}
}

