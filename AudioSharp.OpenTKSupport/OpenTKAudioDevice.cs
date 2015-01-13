using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
namespace AudioSharp.OpenTKSupport
{
	/// <summary>
	/// An IAudioDevice implementation using OpenAL
	/// </summary>
	public class OpenTKAudioDevice : IAudioDevice
	{
		internal AudioContext context;
		internal bool ready = false;
		bool createContext;
		bool running = true;
		//ConcurrentQueues to avoid threading errors
		ConcurrentQueue<OpenTKStreamingAudio> toRemove = new ConcurrentQueue<OpenTKStreamingAudio> ();
		ConcurrentQueue<OpenTKStreamingAudio> toAdd = new ConcurrentQueue<OpenTKStreamingAudio> ();
		List<OpenTKStreamingAudio> instances = new List<OpenTKStreamingAudio> ();
		public OpenTKAudioDevice(bool createContext = true)
		{
			this.createContext = createContext;
			new Thread (new ThreadStart (UpdateThread)).Start ();
		}

		void UpdateThread()
		{
			if(createContext)
				context = new AudioContext ();
			ready = true;
			while (running) {
				//remove from items to update
				while (toRemove.Count > 0) {
					OpenTKStreamingAudio item;
					if (toRemove.TryDequeue (out item))
						instances.Remove (item);
				}
				//insert into items to update
				while (toAdd.Count > 0) {
					OpenTKStreamingAudio item;
					if (toAdd.TryDequeue (out item))
						instances.Add(item);
				}
				//update
				for (int i = 0; i < instances.Count; i++) {
					instances [i].Update ();
				}
				Thread.Sleep (0);
				CheckALError ();
			}
		}
		internal static void CheckALError()
		{
			ALError error;
			if ((error = AL.GetError()) != ALError.NoError)
				throw new InvalidOperationException(AL.GetErrorString(error));
		}
		internal void Add (OpenTKStreamingAudio audio)
		{
			toAdd.Enqueue (audio);
		}
		internal void Remove(OpenTKStreamingAudio audio)
		{
			toRemove.Enqueue (audio);
		}
		public IStreamingAudio CreateStreamer (SoundFormat format, int sampleRate)
		{
			return new OpenTKStreamingAudio (this, format, sampleRate);
		}

		public void Dispose()
		{
			running = false;
		}
	}
}

