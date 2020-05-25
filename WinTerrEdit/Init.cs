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
        public List<baseItem> load()
        {
            List<baseItem> completed = new List<baseItem> { };

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "WinTerrEdit.Data.txt";
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
    }
}
