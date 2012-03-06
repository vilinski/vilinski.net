using System.Reflection;

using Microsoft.Win32;

namespace ScreenPaste
{
    public static class AutoStart
    {
        /// <summary>
        /// Indicates wether the autostart is enabled.
        /// </summary>
        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.RUN_LOCATION);
                if (registryKey == null)
                    return false;
                var str = (string) registryKey.GetValue(Const.PRODUCT_NAME);
                return str != null && str == Assembly.GetExecutingAssembly().Location;
            }
        }

        public static void EnableAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Const.RUN_LOCATION);
            if (key != null)
                key.SetValue(Const.PRODUCT_NAME, Assembly.GetExecutingAssembly().Location);
        }

        public static void DisableSetAutoStart()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Const.RUN_LOCATION);
            if (key != null)
                key.DeleteValue(Const.PRODUCT_NAME);
        }
    }
}