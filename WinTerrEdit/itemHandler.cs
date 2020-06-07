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
        public List<itemPrefix> globalItemPrefixes = new List<itemPrefix> { };
        public Init intialize = new Init();

        public itemHandler(bool load)
        {
            if (load)
            {
                globalTerrariaItems = intialize.loaditemIDs();
                globalItemPrefixes = intialize.loadItemPrefixes();
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
        public baseItem searchItemByID(int id)
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
        public baseItem searchItemByName(string name)
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

        public itemPrefix searchPrefixByID(int id)
        {
            foreach (itemPrefix i in globalItemPrefixes)
            {
                if (i.ID == id)
                {
                    return i;
                }
            }
            return globalItemPrefixes[0];
        }

        public itemPrefix searchPrefixByName(string name)
        {
            foreach (itemPrefix i in globalItemPrefixes)
            {
                if (i.name == name)
                {
                    return i;
                }
            }
            return globalItemPrefixes[0];
        }

        public bool calcByteOffset(List<List<int>> invDat)
        {
            int additor = 0;
            bool foundItem = false;

            foreach(List<int> invchunk in invDat)
            {
                foreach(int i in invchunk)
                {
                    if(i != 0)
                    {
                        foundItem = true;
                    }
                }

                additor += invchunk[2];
                additor += invchunk[3];
                additor += invchunk[6];
                additor += invchunk[7];
                additor += invchunk[9];
            }

            Console.WriteLine(additor);

            if (!foundItem)
            {
                return true;
            }
            if(additor == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class invItem
    {
        public baseItem item { get; set; }
        public int quantity { get; set; }
        public itemPrefix prefix { get; set; }
        
        public invItem(List<int> terrData, itemHandler handler)
        {
            int id = handler.resolveEncodedData(terrData[0], terrData[1]);
            item = handler.searchItemByID(id);
            quantity = handler.resolveEncodedData(terrData[4], terrData[5]);
            prefix = handler.searchPrefixByID(terrData[8]);
        }

        //returns the inventory item as a set of 10 bytes for reinserting into raw data
        public List<Byte> recompile(itemHandler handler)
        {
            Random r = new Random();

            List<Byte> final = new List<Byte> { };
            List<int> encodedItem = handler.encodeData(item.ID);
            List<int> encodedQuant = handler.encodeData(quantity);
            //bytes 2, 3, 6, 7 and 9 seem to make the item dissapear if they are anything other than 0x00
            final.Add((byte)encodedItem[0]);
            final.Add((byte)encodedItem[1]);
            final.Add(0x00);
            final.Add(0x00);
            final.Add((byte)encodedQuant[0]);
            final.Add((byte)encodedQuant[1]);
            final.Add(0x00);
            final.Add(0x00);
            final.Add((byte)prefix.ID);
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
    public class itemPrefix
    {
        public int ID { get; }
        public string name { get; }

        public itemPrefix(int ID, string name)
        {
            this.ID = ID;
            this.name = name;
        }
    }
}
