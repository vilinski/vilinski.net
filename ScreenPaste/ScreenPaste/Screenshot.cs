using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace ScreenPaste
{
    internal class Screenshot
    {
        public DateTime Timestamp { get; set; }

        public Bitmap Bitmap { get; private set; }

        public string FileName { get; private set; }

        public void TakeAndSave()
        {
            Bitmap = Take(Screen.PrimaryScreen);
            Timestamp = DateTime.Now;
            Save();
        }

        private void Save()
        {
        	var path = ScreenPasteApplicationContext.GetSavePath();
        	string filename = path +
                              string.Format(Const.FILENAME_FORMAT, Timestamp) + ".png";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Bitmap.Save(filename, ImageFormat.Png);
            FileName = filename;
        }

    	public Bitmap Take()
    	{
    		return Take(Screen.PrimaryScreen.Bounds);
    	}

    	public Bitmap Take(Screen screen)
        {
    		return Take(screen.Bounds);
        }

    	public static Bitmap Take(Rectangle bounds)
    	{
    		var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
    		Graphics
    			.FromImage(bitmap)
    			.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
    		return bitmap;
    	}
    }
}