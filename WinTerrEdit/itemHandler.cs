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
    /// Contains structures for handling items
    /// 
    /// </summary>
    public class itemHandler
    {
        public List<baseItem> globalTerrariaItems = new List<baseItem> { };
        public Init intialize = new Init();

        public itemHandler(bool load)
        {
            if (load)
            {
                globalTerrariaItems = intialize.load();
            }
        }
        public int resolveEncodedData(int b1, int b2)
        {
            int ID = 0;
            ID += b1;
            ID += 256 * b2;
            return ID;
        }
        public List<int> encodeData(int inp)
        {
            int count256 = 0;
            while (inp > 256)
            {
                inp -= 256;
                count256 += 1;
            }
            return new List<int> { inp, count256 };
        }
        public baseItem searchByID(int id)
        {
            foreach (baseItem i in globalTerrariaItems)
            {
                if (i.ID == id)
                {
                    return i;
                }
            }
            return globalTerrariaItems[0];
        }
        public baseItem searchByName(string name)
        {
            foreach (baseItem i in globalTerrariaItems)
            {
                if (i.name == name)
                {
                    return i;
                }
            }
            return globalTerrariaItems[0];
        }
    }
    public class invItem
    {
        public baseItem item { get; set; }
        public int quantity { get; set; }
        public int modifier { get; set; }
        
        public invItem(List<int> terrData, itemHandler handler)
        {
            int id = handler.resolveEncodedData(terrData[0], terrData[1]);
            item = handler.searchByID(id);
            quantity = handler.resolveEncodedData(terrData[4], terrData[5]);
            modifier = terrData[8];
        }

        //returns the inventory item as a set of 10 bytes for reinserting into raw data
        public List<Byte> recompile(itemHandler handler)
        {
            List<Byte> final = new List<Byte> { };
            List<int> encodedItem = handler.encodeData(item.ID);
            List<int> encodedQuant = handler.encodeData(quantity);
            final.Add((byte)encodedItem[0]);
            final.Add((byte)encodedItem[1]);
            final.Add(0x00);
            final.Add(0x00);
            final.Add((byte)encodedQuant[0]);
            final.Add((byte)encodedQuant[1]);
            final.Add(0x00);
            final.Add(0x00);
            final.Add((byte)modifier);
            final.Add(0x00);
            return final;
        }
    }
    public class baseItem
    {
        public int ID { get; }
        public string name { get; }
        public string name_internal { get; }
        public Bitmap icon { get; }

        public baseItem(string[] insertion)
        {
            //loads item data from array
            this.ID = Int32.Parse(insertion[0]);
            this.name = insertion[1];
            this.name_internal = insertion[2];
            byte[] bdata = Convert.FromBase64String(insertion[3]);
            using (MemoryStream ms = new MemoryStream(bdata))
            {
                this.icon = new Bitmap(ms);
            }
        }
    }
}
