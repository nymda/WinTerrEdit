using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
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
        public List<buff> globalBuffs = new List<buff> { };
        public Init intialize = new Init();

        public itemHandler(bool load)
        {
            if (load)
            {
                globalTerrariaItems = intialize.loaditemIDs();
                globalItemPrefixes = intialize.loadItemPrefixes();
                globalBuffs = intialize.loadBuffIDs();
            }
        }
        public int resolveEncodedData(int b1, int b2)
        {
            return b1 + (256 * b2);
        }
        public List<int> encodeData(int inp)
        {
            if(inp >= 0)
            {
                return new List<int> { inp % 256, inp / 256 };
            }
            else
            {
                return new List<int> { 0, 0 };
            }

        }
        public baseItem searchItemByID(int id)
        {
            var res = globalTerrariaItems.Where(baseItem => baseItem.ID == id);
            return (res.Count() > 0) ? res.First() : globalTerrariaItems[0];
        }

        public baseItem searchItemByName(string name)
        {
            var res = globalTerrariaItems.Where(baseItem => baseItem.name == name);
            return (res.Count() > 0) ? res.First() : globalTerrariaItems[0];
        }

        public itemPrefix searchPrefixByID(int id)
        {
            var res = globalItemPrefixes.Where(itemPrefix => itemPrefix.ID == id);
            return (res.Count() > 0) ? res.First() : globalItemPrefixes[0];
        }

        public itemPrefix searchPrefixByName(string name)
        {
            var res = globalItemPrefixes.Where(itemPrefix => itemPrefix.name == name);
            return (res.Count() > 0) ? res.First() : globalItemPrefixes[0];
        }
        public buff searchBuffById(int id)
        {
            var res = globalBuffs.Where(buff => buff.ID == id);
            return (res.Count() > 0) ? res.First() : globalBuffs[0];
        }

        public buff searchBuffByName(string name)
        {
            var res = globalBuffs.Where(buff => buff.name == name);
            return (res.Count() > 0) ? res.First() : globalBuffs[0];
        }
    }
    public class invItem
    {
        public baseItem item { get; set; }
        public int quantity { get; set; }
        public itemPrefix prefix { get; set; }
        public bool isFavorite { get; set; }
        
        public invItem(List<int> terrData, itemHandler handler)
        {
            int id = handler.resolveEncodedData(terrData[0], terrData[1]);
            quantity = handler.resolveEncodedData(terrData[4], terrData[5]);
            prefix = handler.searchPrefixByID(terrData[8]);
            isFavorite = terrData.Last() == 1 ? true : false;

            if(quantity > 0 && id == 0)
            {
                item = handler.searchItemByID(-1);
            }
            else
            {
                item = handler.searchItemByID(id);
            }

            //Console.WriteLine(item.name + ":" + isFavorite);
        }

        //returns the inventory item as a set of 10 bytes for reinserting into raw data
        public List<Byte> recompile(itemHandler handler, encodeMethod e)
        {
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
            if (e == encodeMethod.Long)
            {
                final.Add((byte)(isFavorite == true ? 0x01 : 0x00));
            }
            
            return final;
        }
    }
    public class playerBuff
    {
        public buff buff { get; set; }
        public int duration { get; set; }

        public playerBuff(List<int> terrData, itemHandler handler)
        {
            int id = handler.resolveEncodedData(terrData[0], terrData[1]);
            buff = handler.searchBuffById(id);
            duration = handler.resolveEncodedData(terrData[4], terrData[5]);
        }

        //returns the inventory item as a set of 10 bytes for reinserting into raw data
        public List<Byte> recompile(itemHandler handler)
        {
            List<Byte> final = new List<Byte> { };
            List<int> encodedItem = handler.encodeData(buff.ID);
            List<int> encodedDuration = handler.encodeData(duration);
            //bytes 2, 3, 6, 7 and 9 seem to make the item dissapear if they are anything other than 0x00
            final.Add((byte)encodedItem[0]);
            final.Add((byte)encodedItem[1]);
            final.Add(0x00);
            final.Add(0x00);
            final.Add((byte)encodedDuration[0]);
            final.Add((byte)encodedDuration[1]);
            final.Add(0x00);
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
            Console.WriteLine(insertion[0]);
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

    public class buff
    {
        public int ID { get; }
        public string name { get; }
        public string name_internal { get; }
        public buffStatus buffStatus { get; }
        public Bitmap icon { get; }

        public buff(string[] insertion)
        {
            //loads item data from array
            this.ID = Int32.Parse(insertion[0]);
            this.name = insertion[1];
            this.name_internal = insertion[2];
            this.buffStatus = (insertion[3] == "Buff") ? buffStatus.Buff : buffStatus.Debuff;
            byte[] bdata = Convert.FromBase64String(insertion[4]);
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

    public enum encodeMethod
    {
        Long,
        Short
    }

    public enum buffStatus
    {
        Buff,
        Debuff
    }
}
