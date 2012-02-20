using Microsoft.Win32;
using System.Reflection;

namespace ScreenPaste
{
    public static class AutoStart
    {
        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.RUN_LOCATION);
                if (registryKey == null)
                    return false;
                var str = (string)registryKey.GetValue(Const.PRODUCT_NAME);
                return str != null && str == Assembly.GetExecutingAssembly().Location;
            }
        }

        public static void EnableAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(Const.RUN_LOCATION);
            if (key != null)
                key.SetValue(Const.PRODUCT_NAME, Assembly.GetExecutingAssembly().Location);
        }

        public static void DisableSetAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(Const.RUN_LOCATION);
            if (key != null)
                key.DeleteValue(Const.PRODUCT_NAME);
        }
    }
}
