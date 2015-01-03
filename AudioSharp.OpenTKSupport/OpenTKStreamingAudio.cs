using System;
using System.Linq;
using System.Collections.Concurrent;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
namespace AudioSharp.OpenTKSupport
{
	public class OpenTKStreamingAudio : IStreamingAudio
	{
		public event BufferNeededHandler BufferNeeded;
		ALFormat bufferFormat;
		int sampleRate;
		int sourceId;
		int[] bufferIds;
		PlayState currentState = PlayState.Stopped;
		OpenTKAudioDevice device;
		float volume = 1f;
		public float Volume {
			get {
				return volume;
			} set {
				if (value != volume) {
					volume = value;
					AL.Source (sourceId, ALSourcef.Gain, volume);
				}
			}
		}
		internal OpenTKStreamingAudio (OpenTKAudioDevice device, SoundFormat format, int sampleRate)
		{
			switch (format) {
			case SoundFormat.Mono16:
				bufferFormat = ALFormat.Mono16;
				break;
			case SoundFormat.Mono8:
				bufferFormat = ALFormat.Mono8;
				break;
			case SoundFormat.Stereo16:
				bufferFormat = ALFormat.Stereo16;
				break;
			case SoundFormat.Stereo8:
				bufferFormat = ALFormat.Stereo8;
				break;
			}
			while (!device.ready)
				;
			uint sid;
			AL.GenSource (out sid);
			sourceId = (int)sid;
			OpenTKAudioDevice.CheckALError ();
			bufferIds = AL.GenBuffers (3);
			OpenTKAudioDevice.CheckALError ();
			this.device = device;
			this.sampleRate = sampleRate;
		}
		bool finished = false;
		bool threadRunning = false;
		internal void Update()
		{
			//manage state
			if (currentState == PlayState.Stopped) {
				AL.SourceStop (sourceId);
				device.Remove (this);
				threadRunning = false;
				return;
			}
			var state = AL.GetSourceState (sourceId);
			OpenTKAudioDevice.CheckALError ();
			if (currentState == PlayState.Paused) {
				if (state != ALSourceState.Paused)
					AL.SourcePause (sourceId);
				return;
			}
			if (currentState == PlayState.Playing && state != ALSourceState.Playing) {
				AL.SourcePlay (sourceId);
			}
			//load buffers
			int processed_count;
			AL.GetSource (sourceId, ALGetSourcei.BuffersProcessed, out processed_count);
			while (processed_count > 0) {
				int bid = AL.SourceUnqueueBuffer (sourceId);
				if (bid != 0 && !finished) {
					byte[] buffer;
					finished = !BufferNeeded (this, out buffer);
					AL.BufferData (bid, bufferFormat, buffer, buffer.Length, sampleRate);
					AL.SourceQueueBuffer (sourceId, bid);
				}
				--processed_count;
			}
			//are we finished?
			int queued_count;
			AL.GetSource (sourceId, ALGetSourcei.BuffersQueued, out queued_count);
			if (queued_count == 0 && finished) {
				device.Remove (this);
				currentState = PlayState.Stopped;
				threadRunning = false;
			}
		}

		public void Play ()
		{
			if (currentState == PlayState.Playing)
				return;
			if (currentState == PlayState.Stopped) {
				currentState = PlayState.Playing;
				for (int i = 0; i < bufferIds.Length; i++) {
					byte[] buffer;
					BufferNeeded (this, out buffer);
					AL.BufferData (bufferIds [i], bufferFormat, buffer, buffer.Length, sampleRate);
					OpenTKAudioDevice.CheckALError ();
					AL.SourceQueueBuffer (sourceId, bufferIds [i]);
					OpenTKAudioDevice.CheckALError ();
					AL.SourcePlay (sourceId);
					OpenTKAudioDevice.CheckALError ();
				}
				device.Add (this);
				threadRunning = true;
			}
			currentState = PlayState.Playing;

		}

		public void Pause ()
		{
			currentState = PlayState.Paused;
		}

		public void Stop ()
		{
			if (currentState == PlayState.Stopped)
				return;
			currentState = PlayState.Stopped;
			while (threadRunning)
				;
			AL.SourceStop (sourceId);
			device.Remove (this);
			Empty ();
		}

		void Empty()
		{
			int queued;
			AL.GetSource(sourceId, ALGetSourcei.BuffersQueued, out queued);
			if (queued > 0)
			{
				try
				{
					AL.SourceUnqueueBuffers(sourceId, queued);
					OpenTKAudioDevice.CheckALError ();
				}
				catch (InvalidOperationException)
				{
					//work around OpenAL bug
					int processed;
					AL.GetSource(sourceId, ALGetSourcei.BuffersProcessed, out processed);
					var salvaged = new int[processed];
					if (processed > 0)
					{
						AL.SourceUnqueueBuffers(sourceId, processed, salvaged);
						OpenTKAudioDevice.CheckALError ();
					}
					AL.SourceStop(sourceId);
					OpenTKAudioDevice.CheckALError ();
					Empty();
				}
			}
		}
		public PlayState GetState ()
		{
			return currentState;
		}

		public void Dispose ()
		{
			Stop ();
			AL.DeleteBuffers (bufferIds);
			AL.DeleteSource (sourceId);
		}
	}
}