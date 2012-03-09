using Microsoft.Win32;
using System.Reflection;

namespace timeRecord
{
    public static class AutoStart
    {
        public static bool IsAutoStartEnabled
        {
            get
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.RunLocation);
                if (registryKey == null)
                    return false;
                var str = (string)registryKey.GetValue(Const.ProductName);
                return str != null && str == Assembly.GetExecutingAssembly().Location;
            }
        }

        public static void EnableAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(Const.RunLocation);
            if (key != null)
                key.SetValue(Const.ProductName, Assembly.GetExecutingAssembly().Location);
        }

        public static void DisableSetAutoStart()
        {
            var key = Registry.CurrentUser.CreateSubKey(Const.RunLocation);
            if (key != null)
                key.DeleteValue(Const.ProductName);
        }
    }
}
