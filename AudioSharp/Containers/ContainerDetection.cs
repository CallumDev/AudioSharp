using System;
using System.IO;
namespace AudioSharp.Containers
{
	class ContainerDetection
	{
		const uint RIFF_MAGIC = 0x46464952;
		const uint OGG_MAGIC = 0x5367674f;
		public static IContainerFile GetContainerFromStream(Stream stream)
		{
			var reader = new BinaryReader (stream);
			var magic = reader.ReadUInt32 ();
			if (magic == RIFF_MAGIC) {
				return new RIFFContainer (reader);
			} else if (magic == OGG_MAGIC) {
				return new OggContainer (reader);
			}
			throw new NotSupportedException ("Unsupported file format");
		}
	}
}

