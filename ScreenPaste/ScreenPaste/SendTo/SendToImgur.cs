using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ScreenPaste.SendTo
{
	[Export("SendTo", typeof(ISendTo))]
    public class SendToImgur : ISendTo, ISendToCapabilities
    {
    	public SendToImgur()
    	{
    		Enabled = true;
    		ID = new Guid("2EFE2265-9185-4A6E-A07B-1A95E98BC229");
    		Name = "Imgur";
    	}

    	public string Send(Bitmap bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            using (var webClient = new WebClient())
            {
                var data = new NameValueCollection
                               {
                                   {"key", Const.IMGUR_API_KEY},
                                   {"upload", Convert.ToBase64String(memoryStream.ToArray())}
                               };
                var reply = (string)XDocument
                        .Load(new MemoryStream(webClient.UploadValues("http://htv.su/api?format=xml", data)))
                        .Descendants("img_viewer")
                        .First();
				return reply;
            }
        }

		public bool Enabled { get; private set; }

		public Guid ID { get; private set; }

		public string Name { get; private set; }
    }
}
