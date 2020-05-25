using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{
    /// <summary>
    /// 
    /// Initialises terraria data from the data.txt file
    /// 
    /// </summary> 
    public class Init
    {
        public List<baseItem> loaditemIDs()
        {
            List<baseItem> completed = new List<baseItem> { };

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WinTerrEdit.itemIDs.txt";
            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            List<String> dataArray = result.Split('\n').ToList();

            foreach(string str in dataArray)
            {
                completed.Add(new baseItem(str.Split(',')));
            }

            return completed;
        }

        public List<itemPrefix> loadItemPrefixes()
        {
            List<itemPrefix> completed = new List<itemPrefix> { };

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WinTerrEdit.itemPrefixes.txt";
            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            List<String> dataArray = result.Split('\n').ToList();

            foreach (string str in dataArray)
            {
                string[] split = str.Split(',');
                completed.Add(new itemPrefix(Int32.Parse(split[0]), split[1]));
            }

            return completed;
        }
    }
}
