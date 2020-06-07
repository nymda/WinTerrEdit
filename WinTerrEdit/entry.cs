using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class entry : Form
    {
        //general stuff
        itemHandler ih = new itemHandler(true);
        public List<Byte> rawDecrypted = new List<Byte> { };
        public readonly string playerfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/Terraria/Players";

        //static player variables
        public string playerName = "";
        public gamemodes.gamemode playerMode;

        //modifiable player variables
        public List<invItem> inventory = new List<invItem> { };
        public List<int> playerHealth = new List<int> { };
        public List<int> playerMana = new List<int> { };
        public List<Color> playerColours = new List<Color> { }; //hair, skin, eyes, shirt, undershirt, pants, shoes

        //only populated if unlock all is used
        public List<Byte> unlockAllData = new List<Byte> { };

        //other
        public int additionOffset = 0;
        public List<List<int>> debugInvData = new List<List<int>> { };
        public List<ListViewItem> lvis = new List<ListViewItem> { };
        public List<Panel> pnCollection = new List<Panel> { };
        public List<PictureBox> pbCollection = new List<PictureBox> { };
        public int nameEndOffset = 0;
        public int invSelectedIndex = -1;
        crypto cr = new crypto();
        public bool isSaved = true;
        public loading ld;

        public entry()
        {
            InitializeComponent();
            new Thread(new ThreadStart(delegate
            {
                Application.Run(ld = new loading(ih.globalTerrariaItems.Count()));
            })).Start();
        }
        private void Entry_Load(object sender, EventArgs e)
        {
            pbCollection.AddRange(new List<PictureBox> { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20, pictureBox21, pictureBox22, pictureBox23, pictureBox24, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32, pictureBox33, pictureBox34, pictureBox35, pictureBox36, pictureBox37, pictureBox38, pictureBox39, pictureBox40, pictureBox41, pictureBox42, pictureBox43, pictureBox44, pictureBox45, pictureBox46, pictureBox47, pictureBox48, pictureBox49, pictureBox50 }); //lol
            pnCollection.AddRange(new List<Panel> { hairPnl, skinPnl, eyesPnl, shirtPnl, undershirtPnl, pantsPnl, shoesPnl });
            int cnt = 0;
            itemLV.View = View.Details;
            itemLV.Columns[0].Width = itemLV.Width - 25;
            imageList1.ImageSize = new Size(32, 32);
            itemLV.SmallImageList = imageList1;
            itemLV.BeginUpdate();
            Stopwatch st = new Stopwatch();
            st.Start();
            foreach (baseItem itm in ih.globalTerrariaItems)
            {
                cbItem.Items.Add(itm.name);
                imageList1.Images.Add(itm.icon);
                ListViewItem tmp = new ListViewItem();
                tmp.Text = itm.name;
                tmp.ImageIndex = cnt;
                itemLV.Items.Add(tmp);
                lvis.Add(tmp);
                cnt++;
            }
            st.Stop();
            Console.WriteLine("load took " + st.Elapsed);
            itemLV.Sorting = SortOrder.Ascending;
            itemLV.Sort();
            itemLV.EndUpdate();
            this.Invoke(new MethodInvoker(delegate ()
            {
                ld.Close();
            }));

            //just show the fucking form to the user
            User32.AllowSetForegroundWindow((uint)Process.GetCurrentProcess().Id);
            User32.SetForegroundWindow(Handle);
            User32.ShowWindow(Handle, User32.SW_SHOWNORMAL);

            foreach (itemPrefix ipf in ih.globalItemPrefixes)
            {
                cbPrefixes.Items.Add(ipf.name);
            }
            cbItem.SelectedIndex = 0;
            cbPrefixes.SelectedIndex = 0;
            nudQuant.MouseWheel += new MouseEventHandler(this.ScrollHandlerFunction);
            nudHealthCur.MouseWheel += new MouseEventHandler(this.ScrollHandlerFunction);
            nudHealthMax.MouseWheel += new MouseEventHandler(this.ScrollHandlerFunction);
            nudManaCur.MouseWheel += new MouseEventHandler(this.ScrollHandlerFunction);
            nudManaMax.MouseWheel += new MouseEventHandler(this.ScrollHandlerFunction);
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

            playerMode = (gamemodes.gamemode)decrypted[nameEndOffset];

            lblGm.Text = "Mode: " + playerMode.ToString();

            playerName = nameBuild.ToString();

            tbName.Text = playerName;

            int InvDataBeginOffset = nameEndOffset + (211 + additionOffset);
            int InvDataEndOffset = InvDataBeginOffset + 500;

            int extCounter = 0;

            //initial inventory data load
            List<int> invTmp = new List<int> { };
            for (int i = InvDataBeginOffset; i < InvDataEndOffset; i++)
            {
                extCounter++;
                invTmp.Add(decrypted[i]);
                if (extCounter == 10)
                {
                    debugInvData.Add(invTmp);
                }
            }

            //test if the +2 byte offset is needed
            bool needExtraOffset = ih.calcByteOffset(debugInvData);
            if (needExtraOffset)
            {
                additionOffset = 2;
            }
            else
            {
                additionOffset = 0;
            }

            //actually load the data
            inventory = new List<invItem> { };
            debugInvData = new List<List<int>> { };

            InvDataBeginOffset = nameEndOffset + (211 + additionOffset);
            InvDataEndOffset = InvDataBeginOffset + 500;

            extCounter = 0;

            invTmp = new List<int> { };
            for (int i = InvDataBeginOffset; i < InvDataEndOffset; i++)
            {
                extCounter++;
                invTmp.Add(decrypted[i]);
                if (extCounter == 10)
                {
                    invItem iv = new invItem(invTmp, ih);
                    inventory.Add(iv);
                    debugInvData.Add(invTmp);
                    invTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            int ColourDataBeginOffset = nameEndOffset + 40;
            int ColourDataEndOffset = ColourDataBeginOffset + 21;
            List<int> colTmp = new List<int> { };
            for (int i = ColourDataBeginOffset; i < ColourDataEndOffset; i++)
            {
                extCounter++;
                colTmp.Add(decrypted[i]);
                if (extCounter == 3)
                {
                    Color col = Color.FromArgb(colTmp[0], colTmp[1], colTmp[2]);
                    playerColours.Add(col);
                    colTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            hairPnl.BackColor = playerColours[0];
            skinPnl.BackColor = playerColours[1];
            eyesPnl.BackColor = playerColours[2];
            shirtPnl.BackColor = playerColours[3];
            undershirtPnl.BackColor = playerColours[4];
            pantsPnl.BackColor = playerColours[5];
            shoesPnl.BackColor = playerColours[6];

            int HealthDataBeginOffset = nameEndOffset + 18;
            int HealthDataEndOffset = HealthDataBeginOffset + 8;
            List<int> helTmp = new List<int> { };
            for (int i = HealthDataBeginOffset; i < HealthDataEndOffset; i++)
            {
                extCounter++;
                helTmp.Add(decrypted[i]);
                if (extCounter == 4)
                {
                    int tmpHelth = ih.resolveEncodedData(helTmp[0], helTmp[1]);
                    playerHealth.Add(tmpHelth);
                    helTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            nudHealthCur.Value = playerHealth[0];
            nudHealthMax.Value = playerHealth[1];

            int ManaDataBeginOffset = nameEndOffset + 26;
            int ManaDataEndOffset = ManaDataBeginOffset + 8;
            List<int> manTmp = new List<int> { };
            for (int i = ManaDataBeginOffset; i < ManaDataEndOffset; i++)
            {
                extCounter++;
                manTmp.Add(decrypted[i]);
                if (extCounter == 4)
                {
                    int tmpMan = ih.resolveEncodedData(manTmp[0], manTmp[1]);
                    playerMana.Add(tmpMan);
                    manTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            nudManaCur.Value = playerMana[0];
            nudManaMax.Value = playerMana[1];
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = playerfolder;
                dlg.Title = "Open Input File";
                dlg.Filter = "Player | *.plr";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //reset variables
                    additionOffset = 0;
                    rawDecrypted = new List<Byte> { };
                    playerName = "";
                    inventory = new List<invItem> { };
                    playerHealth = new List<int> { };
                    playerMana = new List<int> { };
                    playerColours = new List<Color> { };
                    nameEndOffset = 0;
                    invSelectedIndex = 0;
                    isSaved = true;
                    unlockAllData = new List<Byte> { };
                    debugInvData = new List<List<int>> { };
                    inventory.Clear();

                    loadData(dlg.FileName);
                    gbInvHold.Enabled = true;
                    gbPlayer.Enabled = true;
                    gb_slot.Enabled = true;
                    gbItems.Enabled = true;
                }
            }

            for(int i = 0; i < 50; i++)
            {
                pbCollection[i].Image = inventory[i].item.icon;
            }

            btnSave.Enabled = true;
            invSelectedIndex = 0;
            updateInvDisplay();
        }
       
        public void updateInvDisplay()
        {
            cbItem.SelectedItem = inventory[invSelectedIndex].item.name;
            nudQuant.Value = inventory[invSelectedIndex].quantity;
            cbPrefixes.SelectedItem = inventory[invSelectedIndex].prefix.name;
            gb_slot.Text = "Slot " + (invSelectedIndex + 1);
            for (int i = 0; i < 50; i++)
            {
                pbCollection[i].Image = inventory[i].item.icon;
            }
        }

        private void btnSetItem_Click(object sender, EventArgs e)
        {
            if(cbItem.SelectedIndex.ToString() != "")
            {
                inventory[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
            }
            if(cbPrefixes.SelectedItem.ToString() != "")
            {
                inventory[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
            }
            inventory[invSelectedIndex].quantity = (int)nudQuant.Value;
            for (int i = 0; i < 50; i++)
            {
                pbCollection[i].Image = inventory[i].item.icon;
            }
        }

        private void item_Click(object sender, EventArgs e)
        {
            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "Box" }, StringSplitOptions.None);
            invSelectedIndex = Int32.Parse(npart[1]) - 1;
            updateInvDisplay();
        }

        public List<Byte> reEncode()
        {
            List<Byte> buffer = new List<Byte> { };
            List<Byte> save = rawDecrypted;
            foreach (invItem iv in inventory)
            {
                List<Byte> tmp = iv.recompile(ih);
                foreach (byte b in tmp)
                {
                    buffer.Add(b);
                }
            }

            foreach(Color c in playerColours)
            {
                List<int> tmpCol = new List<int> { };
                tmpCol.Add(c.R);
                tmpCol.Add(c.G);
                tmpCol.Add(c.B);
                foreach(int i in tmpCol)
                {
                    buffer.Add((byte)i);
                }
            }

            buffer.Add((byte)ih.encodeData(playerHealth[0])[0]);
            buffer.Add((byte)ih.encodeData(playerHealth[0])[1]);
            buffer.Add(0x00);
            buffer.Add(0x00);
            buffer.Add((byte)ih.encodeData(playerHealth[1])[0]);
            buffer.Add((byte)ih.encodeData(playerHealth[1])[1]);
            buffer.Add(0x00);
            buffer.Add(0x00);
            buffer.Add((byte)ih.encodeData(playerMana[0])[0]);
            buffer.Add((byte)ih.encodeData(playerMana[0])[1]);
            buffer.Add(0x00);
            buffer.Add(0x00);
            buffer.Add((byte)ih.encodeData(playerMana[1])[0]);
            buffer.Add((byte)ih.encodeData(playerMana[1])[1]);
            buffer.Add(0x00);
            buffer.Add(0x00);

            int dataBeginOffset = nameEndOffset + (211 + additionOffset);
            int dataEndOffset = dataBeginOffset + 500;
            int extCount = 0;

            Console.WriteLine(buffer.Count());

            for (int i = dataBeginOffset; i < dataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int ColourDataBeginOffset = nameEndOffset + 40;
            int ColourDataEndOffset = ColourDataBeginOffset + 21;
            for (int i = ColourDataBeginOffset; i < ColourDataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int HealthDataBeginOffset = nameEndOffset + 18;
            int HealthDataEndOffset = HealthDataBeginOffset + 8;
            for (int i = HealthDataBeginOffset; i < HealthDataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int ManaDataBeginOffset = nameEndOffset + 26;
            int ManaDataEndOffset = ManaDataBeginOffset + 8;
            for (int i = ManaDataBeginOffset; i < ManaDataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int unlockAllBeginOffser = nameEndOffset + 2557;
            save.InsertRange(unlockAllBeginOffser, unlockAllData);

            //insert padding if needed
            while(save.Count() % 16 != 0)
            {
                save.Add(0);
            }

            return save;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.InitialDirectory = playerfolder;
                dlg.Title = "Save player file";
                dlg.Filter = "Terraria player | *.plr";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string savepath = dlg.FileName;
                    cr.encryptAndSave(reEncode().ToArray(), savepath);
                    isSaved = true;
                    saveNotifier sn = new saveNotifier();
                    sn.ShowDialog();
                }
            }
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "Box" }, StringSplitOptions.None);
            int tmp = Int32.Parse(npart[1]) - 1;

            if(tmp == invSelectedIndex)
            {
                e.Graphics.DrawRectangle(Pens.Red, 0, 0, 31, 31);
            }
        }

        private void gb_slot_Enter(object sender, EventArgs e)
        {

        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbItem.SelectedIndex.ToString() != "")
                {
                    inventory[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
                }
                for (int i = 0; i < 50; i++)
                {
                    pbCollection[i].Image = inventory[i].item.icon;
                }
                isSaved = false;
                if(inventory[invSelectedIndex].quantity == 0 && inventory[invSelectedIndex].item.name != "Empty")
                {
                    inventory[invSelectedIndex].quantity += 1;
                    nudQuant.Value += 1;
                }
                if(inventory[invSelectedIndex].item.name == "Empty")
                {
                    inventory[invSelectedIndex].quantity = 0;
                    nudQuant.Value = 0;
                }
            }
            catch
            {

            }
        }

        private void cbPrefixes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbPrefixes.SelectedItem.ToString() != "")
                {
                    inventory[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
                }
                for (int i = 0; i < 50; i++)
                {
                    pbCollection[i].Image = inventory[i].item.icon;
                }
                isSaved = false;
            }
            catch
            {

            }
        }

        private void nudQuant_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                inventory[invSelectedIndex].quantity = (int)nudQuant.Value;
                for (int i = 0; i < 50; i++)
                {
                    pbCollection[i].Image = inventory[i].item.icon;
                }
                isSaved = false;
            }
            catch
            {

            }
        }

        private void ScrollHandlerFunction(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            NumericUpDown _sender = (sender as NumericUpDown);

            if (_sender.Name.Contains("Mana"))
            {
                if (handledArgs.Delta > 0)
                {
                    if (_sender.Value <= 249)
                    {
                        _sender.Value += 1;
                    }
                }
                else
                {
                    if (_sender.Value > 0)
                    {
                        _sender.Value += -1;
                    }
                }
            }
            else if (_sender.Name.Contains("Health"))
            {
                if (handledArgs.Delta > 0)
                {
                    if (_sender.Value <= 499)
                    {
                        _sender.Value += 1;
                    }
                }
                else
                {
                    if (_sender.Value > 0)
                    {
                        _sender.Value += -1;
                    }
                }
            }
            else
            {
                if (handledArgs.Delta > 0)
                {
                    if (_sender.Value <= 25534)
                    {
                        _sender.Value += 1;
                    }
                }
                else
                {
                    if (_sender.Value > 0)
                    {
                        _sender.Value += -1;
                    }
                }
            }
        }

        private void onClose(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
                closeWarn cw = new closeWarn();
                if(cw.ShowDialog() == DialogResult.OK) 
                {
                    Environment.Exit(0);
                }
            }
        }

        private void colourSelecter_Click(object sender, EventArgs e)
        {
            string elementName = (sender as Panel).Name;
            int extCount = pnCollection.IndexOf(sender as Panel);
            colourSelecter.Color = playerColours[extCount];
            colourSelecter.FullOpen = true;
            colourSelecter.AnyColor = true;
            colourSelecter.CustomColors = new int[] { ColorTranslator.ToOle(playerColours[0]), ColorTranslator.ToOle(playerColours[1]), ColorTranslator.ToOle(playerColours[2]), ColorTranslator.ToOle(playerColours[3]), ColorTranslator.ToOle(playerColours[4]), ColorTranslator.ToOle(playerColours[5]), ColorTranslator.ToOle(playerColours[6]) };
            if (colourSelecter.ShowDialog() == DialogResult.OK)
            {
                playerColours[extCount] = colourSelecter.Color;

                hairPnl.BackColor = playerColours[0];
                skinPnl.BackColor = playerColours[1];
                eyesPnl.BackColor = playerColours[2];
                shirtPnl.BackColor = playerColours[3];
                undershirtPnl.BackColor = playerColours[4];
                pantsPnl.BackColor = playerColours[5];
                shoesPnl.BackColor = playerColours[6];
            }
        }

        private void nudHealthCur_ValueChanged(object sender, EventArgs e)
        {
            playerHealth[0] = (int)nudHealthCur.Value;
        }

        private void nudHealthMax_ValueChanged(object sender, EventArgs e)
        {
            playerHealth[1] = (int)nudHealthMax.Value;
        }

        private void nudManaCur_ValueChanged(object sender, EventArgs e)
        {
            playerMana[0] = (int)nudManaCur.Value;
            playerMana[0] = (int)nudManaCur.Value;
        }

        private void nudManaMax_ValueChanged(object sender, EventArgs e)
        {
            playerMana[1] = (int)nudManaMax.Value;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbItem.SelectedItem = "Empty";
            cbPrefixes.SelectedItem = "None";
            nudQuant.Value = 0;
        }

        private void entry_kDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F1)
            {
                about ab = new about();
                ab.Show();
            }
            if(e.KeyCode == Keys.F2)
            {
                hexView hx = new hexView(debugInvData, rawDecrypted.ToArray(), nameEndOffset);
                hx.Show();
            }
        }

        private void lb_activ(object sender, EventArgs e)
        {
            cbItem.SelectedItem = itemLV.SelectedItems[0].Text;
        }

        public static class User32
        {
            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;
            public const int SW_SHOWNORMAL = 1;
            public const int SW_SHOWMAXIMIZED = 3;
            public const int SW_RESTORE = 9;

            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern bool AllowSetForegroundWindow(uint dwProcessId);
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 2)
            {
                var results = lvis.Where(x => x.Text.ToLower().Contains(textBox1.Text)).ToList();
                itemLV.Items.Clear();
                foreach (var i in results)
                {
                    itemLV.Items.Add(i);
                }         
            }
            else
            {
                itemLV.Items.Clear();
                itemLV.Items.AddRange(lvis.ToArray());
            }
        }
    }
}
