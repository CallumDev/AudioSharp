﻿using System;
using System.IO;
using System.Text;
namespace AudioSharp.Containers
{
	class RIFFContainer : IContainerFile
	{
		//RIFF magic numbers
		const short CODEC_PCM = 0x1;
		//members
		CodecId codec;

		public RIFFContainer (BinaryReader reader)
		{
			byte[] buffer = new byte[4];
			//further checks
			int riffChunkSize = reader.ReadInt32 ();
			reader.Read (buffer, 0, 4);
			string riffFormat = Encoding.ASCII.GetString (buffer);
			if (riffFormat != "WAVE") {
				throw new NotSupportedException ("RIFF file is not wave");
			}
			//get codec information
			reader.Read (buffer, 0, 4);
			string fmtChunk = Encoding.ASCII.GetString (buffer);
			if (fmtChunk != "fmt ") {
				throw new NotSupportedException ("This wave file is not supported");
			}
			int fmtSize = reader.ReadInt32 ();
			ushort codecId = reader.ReadUInt16 ();
			switch (codecId) {
			case CODEC_PCM:
				codec = CodecId.PCM;
			default:
				throw new NotSupportedException ("Unsupported wave codec");
			}

		}

		public CodecId GetCodecId()
		{
			return CodecId.PCM;
		}
	}
}
