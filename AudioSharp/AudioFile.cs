using System;
using System.IO;
using AudioSharp.Containers;
namespace AudioSharp
{
	public class AudioFile
	{
		IContainerFile container;
		public AudioFile (Stream stream)
		{
			if (!stream.CanSeek)
				throw new NotSupportedException ("Stream that can seek is required");

			container = ContainerDetection.GetContainerFromStream (stream);
		}
		public CodecId CodecId {
			get {
				return container.GetCodecId ();
			}
		}
	}
}

