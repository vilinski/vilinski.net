using System;
using System.Drawing;

namespace ScreenPaste.SendTo
{
	public interface ISendTo
	{
		string Send(Bitmap image);
	}

	public interface ISendToCapabilities
	{
		bool Enabled { get; }
		Guid ID { get; }
		string Name { get; }
	}
}