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
            string filename = ScreenPasteApplicationContext.GetExecutableDirectory() +
                              string.Format(Const.FILENAME_FORMAT, Timestamp) + ".png";
            if (!Directory.Exists(ScreenPasteApplicationContext.GetExecutableDirectory()))
                Directory.CreateDirectory(ScreenPasteApplicationContext.GetExecutableDirectory());
            Bitmap.Save(filename, ImageFormat.Png);
            FileName = filename;
        }

        private Bitmap Take(Screen currentScreen)
        {
            var bounds = currentScreen.Bounds;
            var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            Graphics
                .FromImage(bitmap)
                .CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
            return bitmap;
        }
    }
}