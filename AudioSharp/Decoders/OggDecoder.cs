using System;
using System.IO;
namespace AudioSharp.Containers
{
	//TODO: Hook in NVorbis
	class OggDecoder : IDecoder
	{
		public OggDecoder (BinaryReader reader)
		{
		}
		public CodecId GetCodecId()
		{
			return CodecId.Vorbis;
		}
	}
}

