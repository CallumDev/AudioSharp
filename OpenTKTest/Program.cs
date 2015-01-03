using System;
using System.IO;
using AudioSharp;
using AudioSharp.OpenTKSupport;
namespace OpenTKTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var device = new OpenTKAudioDevice ();
			var file = new AudioFile (File.OpenRead ("Hydrate-Kenny_Beltrey.ogg"));
			var streamer = new AudioStreamer (device);
			streamer.Open (file);
			while (!Console.KeyAvailable)
				;
			streamer.Dispose ();
			device.Dispose ();
		}
	}
}
