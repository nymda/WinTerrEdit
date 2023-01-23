using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTerrEdit
{
    public class registryHandler
    {
        public Settings.SettingsStruct loadRegData()
        {
            var k = Registry.CurrentUser.OpenSubKey("Software\\WinTerrEdit", true);
            if(k == null)
            {
                //key doesnt exist
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\WinTerrEdit");
                key.SetValue("Settings", "00");
                key.Close();
                return new Settings.SettingsStruct(false, false);
            }
            else
            {
                //key exists, read data
                string val = k.GetValue("Settings") as string;
                if (val.Length > 2) {
                    k.SetValue("Settings", val.Substring(0, 2));
                }
                return new Settings.SettingsStruct(val[0] == '1', val[1] == '1') ;
            }
        }

        public void saveRegData(bool useAutoReloadFile, bool useExtendedName)
        {
            StringBuilder sb = new StringBuilder();
            if (useAutoReloadFile){ sb.Append("1"); }
            else{ sb.Append("0"); }
            if (useExtendedName) { sb.Append("1"); }
            else{ sb.Append("0"); }

            var k = Registry.CurrentUser.OpenSubKey("Software\\WinTerrEdit", true);
            if(k != null)
            {
                k.SetValue("Settings", sb.ToString());
                k.Close();
            }
        }
    }
}
