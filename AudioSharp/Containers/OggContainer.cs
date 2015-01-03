using System;
using System.IO;
namespace AudioSharp.Containers
{
	class OggContainer : IContainerFile
	{
		public OggContainer (BinaryReader reader)
		{
		}
		public CodecId GetCodecId()
		{
			return CodecId.Vorbis;
		}
	}
}

