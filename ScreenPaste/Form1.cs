using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ScreenPaste
{
    public partial class Form1 : Form
    {
        private const int RESIZE_WIDTH = 17;

        public Form1()
        {
            ScreenPasteApplicationContext.NIcon.BalloonTipClicked += nIcon_BalloonClick;
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //http://msdn.microsoft.com/ru-ru/library/6k15y9et.aspx
            if (e.Button != MouseButtons.Left)
                return;
            User32.ReleaseCapture();
            User32.SendMessage(Handle, Const.WM_NCLBUTTONDOWN, Const.HT_CAPTION, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = Width.ToString() + "x" + Height.ToString();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(Width, Height);
            Graphics graphics1 = Graphics.FromImage(bitmap);
            ScreenPasteApplicationContext.ShowNotificationBaloon("Загрузка скрина...");
            Hide();
            ScreenPasteApplicationContext.Window = 0;
            Graphics graphics2 = graphics1;
            Size size = Size;
            graphics2.CopyFromScreen(Bounds.X, Bounds.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            _timestamp = DateTime.Now;
            _fileName = ScreenPasteApplicationContext.GetExecutableDirectory() +
                        string.Format(Const.FILENAME_FORMAT, _timestamp) + ".png";
            if (!Directory.Exists(ScreenPasteApplicationContext.GetExecutableDirectory()))
                Directory.CreateDirectory(ScreenPasteApplicationContext.GetExecutableDirectory());
            bitmap.Save(_fileName, ImageFormat.Png);
            string str = PostingServies.PostToImgur(bitmap);
            ScreenPasteApplicationContext.LastScreenshotUri = str;
            ScreenPasteApplicationContext.ShowNotificationBaloon(
                "Скрин загружен: " + str + "\nКликни для открытия скриншота на сайте www.htv.su");
            ScreenPasteApplicationContext.NIcon.ContextMenu.MenuItems.Find("copyUrl", true)[0].Enabled =
                ScreenPasteApplicationContext.LastScreenshotUri != "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(Width, Height);
            Graphics graphics1 = Graphics.FromImage(bitmap);
            Hide();
            Graphics graphics2 = graphics1;
            Rectangle bounds = Bounds;
            int x = bounds.X;
            bounds = Bounds;
            int y = bounds.Y;
            Size size = Size;
            graphics2.CopyFromScreen(x, y, 0, 0, size, CopyPixelOperation.SourceCopy);
            _timestamp = DateTime.Now;
            string filename = ScreenPasteApplicationContext.GetExecutableDirectory() +
                              string.Format(Const.FILENAME_FORMAT, _timestamp) + ".png";
            if (!Directory.Exists(ScreenPasteApplicationContext.GetExecutableDirectory()))
                Directory.CreateDirectory(ScreenPasteApplicationContext.GetExecutableDirectory());
            bitmap.Save(filename, ImageFormat.Png);
            _fileName = filename;
        }

        private void nIcon_BalloonClick(object sender, EventArgs e)
        {
        }

        private void panResize_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ResizeLocation = e.Location;
                ResizeLocation.Offset(-Width, -Height);
                if (ResizeLocation.X > -RESIZE_WIDTH || ResizeLocation.Y > -RESIZE_WIDTH)
                    return;
                ResizeLocation = Point.Empty;
            }
            else
                ResizeLocation = Point.Empty;
        }

        private void panResize_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = Width + "x" + Height;
            if (e.Button == MouseButtons.Left && !ResizeLocation.IsEmpty)
            {
                if (Cursor == Cursors.SizeNWSE)
                    Size = new Size(e.Location.X - ResizeLocation.X, e.Location.Y - ResizeLocation.Y);
                else if (Cursor == Cursors.SizeWE)
                {
                    Size = new Size(e.Location.X - ResizeLocation.X, Size.Height);
                }
                else
                {
                    if (Cursor != Cursors.SizeNS)
                        return;
                    Size = new Size(Size.Width, e.Location.Y - ResizeLocation.Y);
                }
            }
            else if (e.X - Width > -RESIZE_WIDTH && e.Y - Height > -RESIZE_WIDTH)
                Cursor = Cursors.SizeNWSE;
            else if (e.X - Width > -RESIZE_WIDTH)
                Cursor = Cursors.SizeWE;
            else if (e.Y - Height > -RESIZE_WIDTH)
                Cursor = Cursors.SizeNS;
            else
                Cursor = Cursors.Default;
        }

        private void openLink(string sUrl)
        {
            try
            {
                Process.Start(sUrl);
            }
            catch (Win32Exception ex)
            {
                if (ex.ErrorCode != -2147467259)
                    return;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panResize_MouseUp(object sender, MouseEventArgs e)
        {
            ResizeLocation = Point.Empty;
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Const.WM_MOVING)
            {
                var rect = (Rect) Marshal.PtrToStructure(m.LParam, typeof (Rect));
                // Попытка выхода за левую границу экрана ...
                if (rect.Left < 0)
                {
                    rect.Left = 0;
                    rect.Right = Width;
                }
                // Попытка выхода за верхнюю границу экрана ...
                if (rect.Top < 0)
                {
                    rect.Top = 0;
                    rect.Bottom = Height;
                }
                // Попытка выхода за правую границу экрана ...
                if (rect.Right > Screen.PrimaryScreen.Bounds.Right)
                {
                    rect.Left = Screen.PrimaryScreen.Bounds.Right - Width;
                    rect.Right = Screen.PrimaryScreen.Bounds.Right;
                }
                // Попытка выхода за нижнюю границу экрана ...
                if (rect.Bottom > Screen.PrimaryScreen.Bounds.Bottom)
                {
                    rect.Bottom = Screen.PrimaryScreen.Bounds.Bottom;
                    rect.Top = Screen.PrimaryScreen.Bounds.Bottom - Height;
                }
                Marshal.StructureToPtr(rect, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
    }
}