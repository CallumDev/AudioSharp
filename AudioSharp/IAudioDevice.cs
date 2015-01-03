using System;

namespace AudioSharp
{
	public interface IAudioDevice
	{
		IAudioInstance CreateInstance();
	}
}

