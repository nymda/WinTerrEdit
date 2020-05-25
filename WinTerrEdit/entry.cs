using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class entry : Form
    {
        itemHandler ih = new itemHandler(true);
        public List<Byte> rawDecrypted = new List<Byte> { };
        public List<invItem> inventory = new List<invItem> { };
        public List<PictureBox> pbCollection = new List<PictureBox> { };
        public int nameEndOffset = 0;
        public string playerName = "";
        public int invSelectedIndex = 0;
        crypto cr = new crypto();

        public entry()
        {
            InitializeComponent();
        }
        private void Entry_Load(object sender, EventArgs e)
        {
            pbCollection.AddRange(new List<PictureBox> { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47, pictureBox48, pictureBox49, pictureBox50 });
            foreach(baseItem itm in ih.globalTerrariaItems)
            {
                cbItem.Items.Add(itm.name);
            }
            cbItem.SelectedIndex = 0;
        }
        public void loadData(string path)
        {
            byte[] decrypted = cr.decryptFile(path);
            rawDecrypted = decrypted.ToList();

            byte[] printables = new byte[] { 0x27, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x5c, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5c, 0x5d, 0x5e, 0x5f, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e };
            int startpos = 25;

            StringBuilder nameBuild = new StringBuilder();

            for (int i = startpos; i < 51; i++)
            {
                if (printables.Contains(decrypted[i]))
                {
                    nameBuild.Append(Encoding.ASCII.GetString(new byte[] { decrypted[i] }));
                }
                else
                {
                    nameEndOffset = i;
                    break;
                }
            }

            playerName = nameBuild.ToString();

            int dataBeginOffset = nameEndOffset + 211;
            int dataEndOffset = dataBeginOffset + 500;

            int extCounter = 0;
            List<int> tmp = new List<int> { };
            for (int i = dataBeginOffset; i < dataEndOffset; i++)
            {
                extCounter++;
                tmp.Add(decrypted[i]);
                if (extCounter == 10)
                {
                    invItem iv = new invItem(tmp, ih);
                    inventory.Add(iv);
                    tmp = new List<int> { };
                    extCounter = 0;
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Input File";
                dlg.Filter = "Player | *.plr";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    inventory.Clear();
                    loadData(dlg.FileName);
                }
            }
            this.Text = "WinTerrEdit | " + playerName;

            for(int i = 0; i < 50; i++)
            {
                pbCollection[i].Image = inventory[i].item.icon;
            }
        }
       
        public void updateInvDisplay()
        {
            cbItem.SelectedItem = inventory[invSelectedIndex].item.name;
            nudQuant.Value = inventory[invSelectedIndex].quantity;
        }

        private void btnSetItem_Click(object sender, EventArgs e)
        {
            inventory[invSelectedIndex].item = ih.searchByName(cbItem.SelectedItem.ToString());
            inventory[invSelectedIndex].quantity = (int)nudQuant.Value;
            for (int i = 0; i < 50; i++)
            {
                pbCollection[i].Image = inventory[i].item.icon;
            }
        }

        //yes, there are 50 click events down there
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 0; updateInvDisplay();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 1; updateInvDisplay();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 2; updateInvDisplay();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 3; updateInvDisplay();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 4; updateInvDisplay();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 5; updateInvDisplay();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 6; updateInvDisplay();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 7; updateInvDisplay();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 8; updateInvDisplay();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 9; updateInvDisplay();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 10; updateInvDisplay();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 11; updateInvDisplay();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 12; updateInvDisplay();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 13; updateInvDisplay();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 14; updateInvDisplay();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 15; updateInvDisplay();
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 16; updateInvDisplay();
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 17; updateInvDisplay();
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 18; updateInvDisplay();
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 19; updateInvDisplay();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 20; updateInvDisplay();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 21; updateInvDisplay();
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 22; updateInvDisplay();
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 23; updateInvDisplay();
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 24; updateInvDisplay();
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 25; updateInvDisplay();
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 26; updateInvDisplay();
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 27; updateInvDisplay();
        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 28; updateInvDisplay();
        }

        private void pictureBox30_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 29; updateInvDisplay();
        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 30; updateInvDisplay();
        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 31; updateInvDisplay();
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 32; updateInvDisplay();
        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 33; updateInvDisplay();
        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 34; updateInvDisplay();
        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 35; updateInvDisplay();
        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 36; updateInvDisplay();
        }

        private void pictureBox38_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 37; updateInvDisplay();
        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 38; updateInvDisplay();
        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 39; updateInvDisplay();
        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 40; updateInvDisplay();
        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 41; updateInvDisplay();
        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 42; updateInvDisplay();
        }

        private void pictureBox44_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 43; updateInvDisplay();
        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 44; updateInvDisplay();
        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 45; updateInvDisplay();
        }

        private void pictureBox47_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 46; updateInvDisplay();
        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 47; updateInvDisplay();
        }

        private void pictureBox49_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 48; updateInvDisplay();
        }

        private void pictureBox50_Click(object sender, EventArgs e)
        {
            invSelectedIndex = 49; updateInvDisplay();
        }

        public List<Byte> reEncode()
        {
            List<Byte> buffer = new List<Byte> { };
            List<Byte> save = rawDecrypted;
            foreach (invItem iv in inventory)
            {
                List<Byte> tmp = iv.recompile(ih);
                Console.WriteLine(string.Join(",", tmp));
                foreach (byte b in tmp)
                {
                    buffer.Add(b);
                }
            }
            int dataBeginOffset = nameEndOffset + 211;
            int dataEndOffset = dataBeginOffset + 500;
            int extCount = 0;

            for (int i = dataBeginOffset; i < dataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }
            return save;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save player file";
                dlg.Filter = "Terraria player | *.plr";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string savepath = dlg.FileName;
                    cr.encryptAndSave(reEncode().ToArray(), savepath);
                }
            }
        }
    }
}
