using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using ScreenPaste.SendTo;

namespace ScreenPaste
{
    public class ScreenPasteApplicationContext : ApplicationContext
    {
        public static string LastScreenshotUri = "";
        public static int Window;
        public static NotifyIcon NIcon;
        private static ContextMenu _contextMenu;

        static ScreenPasteApplicationContext()
        {
        }

        public ScreenPasteApplicationContext()
        {
        	var directory = GetSavePath();
        	if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            createContextMenu();
            createNotificationIcon();
            ThreadExit += OnApplicationExit;
        }

        public static string GetSavePath()
        {
            return Path.GetDirectoryName(Application.ExecutablePath) + "\\screenshots\\";
        }

        public void HandleHotkey()
        {
            ProcessScreenshot();
        }

        private void createContextMenu()
        {
            _contextMenu = new ContextMenu();
            MenuItem menuItem1 = _contextMenu.MenuItems.Add("Скопировать URL скриншота...");
            menuItem1.Click += copyUrlMenuItem_Click;
            menuItem1.Enabled = false;
            menuItem1.Name = "copyUrl";
            _contextMenu.MenuItems.Add("Открыть папку со скринами...").Click += openFolderMenuItem_Click;
            _contextMenu.MenuItems.Add("Выделить область").Click += selresizeItem_Click;
            _contextMenu.MenuItems.Add("-");
			var sendTo = _contextMenu.MenuItems.Add("Отправить...");
        	foreach (var sender in SendToFactory.Instance.Senders.Select(s=>s.Metadata).OrderBy(m=>m.Name))
        	{
        		var item = sendTo.MenuItems.Add(sender.Name);
        		item.Tag = sender.ID;
        		item.Click +=
        			(s, e) =>
        				{
        					var bitmap = new Screenshot().Take();
        					ShowNotificationBaloon("Загрузка на " + item.Name + "...");
        					var str = SendToFactory.Instance.SendTo(bitmap, (Guid) item.Tag);
        					LastScreenshotUri = str;
        					if (!string.IsNullOrEmpty(str))
        						ShowNotificationBaloon("Скрин загружен: " + str + "\nКликни для открытия скриншота.");
        				};
        	}
			_contextMenu.MenuItems.Add("-");
			_contextMenu.MenuItems.Add("Перейти на сайт www.htv.su").Click += link_siteMenuItem_Click;
            _contextMenu.MenuItems.Add("-");
            MenuItem menuItem2 = _contextMenu.MenuItems.Add("Запускать с Windows");
            menuItem2.Checked = AutoStart.IsAutoStartEnabled;
            menuItem2.Click += autostartMenuItem_Click;
            _contextMenu.MenuItems.Add("Выйти").Click += exitMenuItem_Click;
        }

        private void createNotificationIcon()
        {
            NIcon = new NotifyIcon
                        {
                            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                            Text = "Клик для создания скриншота. Правый клик для открытия меню.",
                            Visible = true
                        };
            NIcon.Click += nIcon_Click;
            if (_contextMenu != null)
                NIcon.ContextMenu = _contextMenu;
            NIcon.BalloonTipClicked += nIcon_BalloonClick;
        }

        private void ProcessScreenshot()
        {
            var screenshot = new Screenshot();
            screenshot.TakeAndSave();
            ShowNotificationBaloon("Загрузка скрина...");
            string str = new SendToImgur().Send(screenshot.Bitmap);
            LastScreenshotUri = str;
			if (!string.IsNullOrEmpty(str))
				ShowNotificationBaloon("Скрин загружен: " + str + "\nКликни для открытия скриншота на сайте www.htv.su");
            NIcon.ContextMenu.MenuItems.Find("copyUrl", true)[0].Enabled = LastScreenshotUri != "";
        }

        private void nIcon_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs) e).Button != MouseButtons.Left)
                return;
            ProcessScreenshot();
        }

        public static void ShowNotificationBaloon(string message, string title = "Скринлоадер")
        {
            NIcon.BalloonTipIcon = ToolTipIcon.Info;
            NIcon.BalloonTipTitle = title;
            NIcon.BalloonTipText = message;
            NIcon.ShowBalloonTip(10000);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            ExitThread();
            Application.Exit();
        }

        private void openFolderMenuItem_Click(object sender, EventArgs e)
        {
            openLink(GetSavePath());
        }

        private void selresizeItem_Click(object sender, EventArgs e)
        {
            if (Window != 0)
                return;
            (new Form1()).Show();
            Window = 1;
        }

        private void link_siteMenuItem_Click(object sender, EventArgs e)
        {
            openLink("http://htv.su/");
        }

        private void copyUrlMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(LastScreenshotUri);
        }

        private static void autostartMenuItem_Click(object sender, EventArgs e)
        {
            if (AutoStart.IsAutoStartEnabled)
                AutoStart.DisableSetAutoStart();
            else
                AutoStart.EnableAutoStart();
            ((MenuItem) sender).Checked = AutoStart.IsAutoStartEnabled;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            NIcon.Dispose();
        }

        private static void nIcon_BalloonClick(object sender, EventArgs e)
        {
            openLink(LastScreenshotUri);
        }

        private static void openLink(string sUrl)
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
    }
}