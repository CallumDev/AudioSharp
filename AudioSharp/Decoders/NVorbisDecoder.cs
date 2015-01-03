﻿using System;
using System.IO;
using AudioSharp.Decoders.NVorbis;
namespace AudioSharp.Decoders
{
	class NVorbisDecoder : IDecoder
	{
		VorbisReader reader;
		SoundFormat format;
		float[] readBuffer = null;
		public NVorbisDecoder (Stream stream)
		{
			//reset the stream
			stream.Seek (0, SeekOrigin.Begin);
			reader = new VorbisReader (stream, false);
			if (reader.Channels == 1) {
				format = SoundFormat.Mono16;
			} else if (reader.Channels == 2) {
				format = SoundFormat.Stereo16;
			} else {
				throw new NotSupportedException (string.Format ("{0} channels not supported", reader.Channels));
			}
		}
		public CodecId CodecId
		{
			get {
				return CodecId.Vorbis;
			}
		}
		public TimeSpan Duration
		{
			get {
				return reader.TotalTime;
			}
		}
		public SoundFormat Format
		{
			get {
				return format;
			}
		}
		public int SampleRate
		{
			get {
				return reader.SampleRate;
			}
		}
		public void Reset()
		{
			reader.DecodedTime = TimeSpan.Zero;
		}
		public unsafe int Read(int length, out byte[] buffer)
		{
			if (readBuffer == null || readBuffer.Length < (length / 2))
				readBuffer = new float[(length / 2)];
			int sampleCount = reader.ReadSamples (readBuffer, 0, (length / 2));
			buffer = new byte[sampleCount * 2];
			fixed(float* inPtr = readBuffer) {
				fixed(byte* outPtr = buffer) {
					CastBuffer (inPtr, outPtr, sampleCount);
				}
			}
			return (sampleCount * 2);
		}
		unsafe void CastBuffer(float* inBuffer, byte* outBuffer, int length)
		{
			short* outPtr = (short*)outBuffer;
			for (int i = 0; i < length; i++) {
				var temp = (int)(32767f * inBuffer[i]);
				if (temp > short.MaxValue) temp = short.MaxValue;
				else if (temp < short.MinValue) temp = short.MinValue;
				outPtr[i] = (short)temp;
			}
		}
	}
}

