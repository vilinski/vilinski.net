using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ScreenPaste
{
    internal static class PostingServies
    {
        public static string PostToImgur(Bitmap bitmap)
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
                return (string)XDocument
                        .Load(new MemoryStream(webClient.UploadValues("http://htv.su/api?format=xml", data)))
                        .Descendants("img_viewer")
                        .First();
            }
        }
    }
}
