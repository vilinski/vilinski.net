using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;

using ScreenPaste.SendTo;

namespace PostToFtp
{
	[Export("SendTo")]
	public class SendToFtp : ISendTo, ISendToCapabilities
	{
		public SendToFtp()
		{
			Enabled = true;
			ID = new Guid("E757E354-5A45-41B2-9554-BCF5CAA0BC4E");
			Name = "FTP";
		}

		public string Send(Bitmap image)
		{
			return string.Empty;
		}

		public bool Enabled { get; set; }

		public Guid ID { get; private set; }

		public string Name { get; private set; }
	}
}
