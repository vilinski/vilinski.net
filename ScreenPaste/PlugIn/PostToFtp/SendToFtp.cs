using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using System.ComponentModel.Composition;

using ScreenPaste;

namespace PostToFtp
{
	[Export("SendTo")]
	public class SendToFtp : ISendTo
	{
		public SendToFtp()
		{
			Enabled = true;
			ID = new Guid("E757E354-5A45-41B2-9554-BCF5CAA0BC4E");
		}

		public bool Send(Bitmap image)
		{
			return true;
		}

		public bool Enabled { get; set; }

		public Guid ID { get; set; }
	}
}
