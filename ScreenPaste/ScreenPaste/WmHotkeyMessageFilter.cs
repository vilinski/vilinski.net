using System.Security.Permissions;
using System.Windows.Forms;

namespace ScreenPaste
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public class WmHotkeyMessageFilter : IMessageFilter
    {
        private const int WM_HOTKEY = 786;
        private readonly ScreenPasteApplicationContext _appContext;

        public WmHotkeyMessageFilter(ScreenPasteApplicationContext context)
        {
            _appContext = context;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_HOTKEY)
                return false;
            _appContext.HandleHotkey();
            return true;
        }
    }

    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
