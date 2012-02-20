using System;
using System.Windows.Forms;

namespace ScreenPaste
{
    static class Program
    {
        [STAThread]
        public static void Main()
        {
            var context = new ScreenPasteApplicationContext();
            Application.AddMessageFilter(new WmHotkeyMessageFilter(context));
            User32.RegisterHotKey(new IntPtr(0), context.GetHashCode(), Const.CTRL, (int) Keys.PrintScreen);
            Application.Run(context);
            User32.UnregisterHotKey(new IntPtr(0), context.GetHashCode());
        }
    }
}
