using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenPaste
{
    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public extern static bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public extern static bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}