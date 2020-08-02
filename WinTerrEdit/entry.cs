using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class entry : Form
    {
        /// <note>
        /// the combobox "cbItem" is no longer nessicary on the UI, however its functionality is integeral to the program. 
        /// </note>

        //general stuff
        itemHandler ih = new itemHandler(true);
        public List<Byte> rawDecrypted = new List<Byte> { };
        public readonly string playerfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Terraria\\Players";
        public string lastReadPlrPath = "";
        public bool useOverwriteFile = false;
        public bool useAutoReloadFile = false;
        public string currentFileHash = "";

        //static player variables
        public string playerName = "";
        public gamemodes.gamemode playerMode;
        public int versionCode;

        //modifiable player variables
        public List<invItem> inventory = new List<invItem> { };
        public List<invItem> safe = new List<invItem> { };
        public List<invItem> pbank = new List<invItem> { };
        public List<int> playerHealth = new List<int> { };
        public List<int> playerMana = new List<int> { };
        public List<Color> playerColours = new List<Color> { }; //hair, skin, eyes, shirt, undershirt, pants, shoes

        public invItem copyBuffer;
        public int copyIndex = -1;

        //only populated if unlock all is used
        public List<Byte> unlockAllData = new List<Byte> { };

        //other
        public int inventoryOffset;
        public int ammoOffset;
        public int coinOffset;
        public int colOffset;
        public List<List<int>> debugInvData = new List<List<int>> { };
        public List<ListViewItem> lvis = new List<ListViewItem> { };
        public List<Panel> pnCollection = new List<Panel> { };
        public List<PictureBox> pbCollection = new List<PictureBox> { };
        public int nameEndOffset = 0;
        public int invSelectedIndex = -1;
        crypto cr = new crypto();
        public bool isSaved = true;
        public loading ld;
        public ToolTip baseTT = new ToolTip();
        public bool useExtendedName = false;

        public bool twoByteNameCode = false;
        public entry()
        {
            InitializeComponent();
            new Thread(new ThreadStart(delegate
            {
                Application.Run(ld = new loading());
            })).Start();
        }
        private void Entry_Load(object sender, EventArgs e)
        {
            Console.WriteLine(playerfolder);

            baseTT.ShowAlways = true;
            baseTT.SetToolTip(btnLoad, "Open a .PLR file");
            baseTT.SetToolTip(btnReload, "Reload the last opened .PLR file");
            baseTT.SetToolTip(btnSave, "Save the currently open .PLR file");

            btnReload.UseCompatibleTextRendering = true;
            pbCollection.AddRange(new List<PictureBox> { Pb1, Pb2, Pb3, Pb4, Pb5, Pb6, Pb7, Pb8, Pb9, Pb10, Pb11, Pb12, Pb13, Pb14, Pb15, Pb16, Pb17, Pb18, Pb19, Pb20, Pb21, Pb22, Pb23, Pb24, Pb25, Pb26, Pb27, Pb28, Pb29, Pb30, Pb31, Pb32, Pb33, Pb34, Pb35, Pb36, Pb37, Pb38, Pb39, Pb40, Pb41, Pb42, Pb43, Pb44, Pb45, Pb46, Pb47, Pb48, Pb49, Pb50, Pb51, Pb52, Pb53, Pb54, Pb55, Pb56, Pb57, Pb58, Pb59, Pb60, Pb61, Pb62, Pb63, Pb64, Pb65, Pb66, Pb67, Pb68, Pb69, Pb70, Pb71, Pb72, Pb73, Pb74, Pb75, Pb76, Pb77, Pb78, Pb79, Pb80, Pb81, Pb82, Pb83, Pb84, Pb85, Pb86, Pb87, Pb88, Pb89, Pb90, Pb91, Pb92, Pb93, Pb94, Pb95, Pb96, Pb97, Pb98, Pb99, Pb100 }); //0-50 inv, 51 - 58 ammo / coins
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
                if(itm.ID != -1)
                {
                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = itm.name;
                    tmp.ImageIndex = cnt;
                    itemLV.Items.Add(tmp);
                    lvis.Add(tmp);
                }
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
            if (useAutoReloadFile)
            {
                autoFunctionTimer.Start();
            }

            currentFileHash = calcMd5OfOpenFile();
            byte[] decrypted = cr.decryptFile(path);
            rawDecrypted = decrypted.ToList();
            versionCode = ih.resolveEncodedData(decrypted[0], decrypted[1]);

            if(versionCode > 512)
            {
                throw new Exception("Invalid PLR header data");
            }

            int startpos = 25;
            int nameLen = decrypted[startpos-1];
            byte[] namebytes = new byte[nameLen];
            Array.Copy(decrypted, startpos, namebytes, 0, nameLen);
            nameEndOffset = startpos + nameLen;
            playerMode = (gamemodes.gamemode)decrypted[nameEndOffset];
            playerName = Encoding.UTF8.GetString(namebytes);
            tbName.Text = playerName;

            //neo + 838

            if(versionCode < 230)
            {
                inventoryOffset = 211;
                coinOffset = 711;
                colOffset = 40;
            }
            else
            {
                inventoryOffset = 213;
                coinOffset = 713;
                colOffset = 42;
            }

            int InvDataBeginOffset = nameEndOffset + inventoryOffset;
            int InvDataEndOffset = InvDataBeginOffset + 500;

            int extCounter = 0;

            List<int> invTmp = new List<int> { };
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

            int PbnkDataBeginOffset = nameEndOffset + 843;
            int PbnkDataEndOffset = PbnkDataBeginOffset + 400;

            extCounter = 0;

            List<int> PbnkTmp = new List<int> { };
            for (int i = PbnkDataBeginOffset; i < PbnkDataEndOffset; i++)
            {
                extCounter++;
                invTmp.Add(decrypted[i]);
                if (extCounter == 9)
                {
                    Console.WriteLine(string.Join(",", invTmp));
                    invItem iv = new invItem(invTmp, ih);
                    pbank.Add(iv);
                    debugInvData.Add(invTmp);
                    invTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            Console.WriteLine(safe.Count());

            int CoinDataBeginOffset = nameEndOffset + coinOffset;
            int CoinDataEndOffset = CoinDataBeginOffset + 80;

            extCounter = 0;

            List<int> coinTmp = new List<int> { };
            for (int i = CoinDataBeginOffset; i < CoinDataEndOffset; i++)
            {
                extCounter++;
                coinTmp.Add(decrypted[i]);
                if (extCounter == 10)
                {
                    invItem iv = new invItem(coinTmp, ih);
                    inventory.Add(iv);
                    debugInvData.Add(coinTmp);
                    coinTmp = new List<int> { };
                    extCounter = 0;
                }
            }

            int ColourDataBeginOffset = nameEndOffset + colOffset;
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

            var res = inventory.Where(invItem => invItem.item.name == "Unknown");
            if(res.Count() > 0)
            {
                MessageBox.Show("This player contains \"Unknown\" items. These are items which have a quantity or prefix but no ID. This may be caused by a game bug or (more likely) a mod. Be careful when editing these items.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    try
                    {
                        //reset variables
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
                        safe.Clear();
                        pbank.Clear();

                        lastReadPlrPath = dlg.FileName;
                        this.Text = "WinTerrEdit | [F1] About | [F2] Settings | (" + dlg.SafeFileName + ")";


                        loadData(dlg.FileName);
                        //gbInvHold.Enabled = true;
                        //gbColour.Enabled = true;
                        //gbPlayer.Enabled = true;
                        tcMain.Enabled = true;
                        gb_slot.Enabled = true;
                        gbItems.Enabled = true;
                        btnReload.Enabled = true;
                    }
                    catch
                    {
                        MessageBox.Show("There was an issue loading this player. It may be corrupted or invalid.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }

            btnSave.Enabled = true;
            invSelectedIndex = 0;
            updateInvDisplay();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
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
            safe.Clear();
            pbank.Clear();


            loadData(lastReadPlrPath);
            //gbInvHold.Enabled = true;
            //gbColour.Enabled = true;
            gb_slot.Enabled = true;
            gbItems.Enabled = true;
            tcMain.Enabled = true;
            btnSave.Enabled = true;
            invSelectedIndex = 0;
            updateInvDisplay();
        }   
        public void updateInvDisplay()
        {
            //hardcoded numbers EEEEEEEEVERYWHEREEEE

            try
            {
                if (invSelectedIndex <= 50)
                {
                    cbItem.SelectedItem = inventory[invSelectedIndex].item.name;
                    nudQuant.Value = inventory[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inventory[invSelectedIndex].prefix.name;
                    gb_slot.Text = "Inventory Slot " + (invSelectedIndex + 1) + " (" + inventory[invSelectedIndex].item.name + ")";
                }
                if (invSelectedIndex > 50 && invSelectedIndex < 91)
                {
                    cbItem.SelectedItem = pbank[invSelectedIndex - 51].item.name;
                    nudQuant.Value = pbank[invSelectedIndex - 51].quantity;
                    cbPrefixes.SelectedItem = pbank[invSelectedIndex - 51].prefix.name;
                    gb_slot.Text = "Piggybank Slot " + (invSelectedIndex + 1) + " (" + pbank[invSelectedIndex - 51].item.name + ")";
                }

                int slotNameCount = 1;
                int i = 0;
                foreach (PictureBox pb in pbCollection)
                {
                    if(slotNameCount <= 50)
                    {
                        pbCollection[i].Image = inventory[i].item.icon;

                        string processedNameData = " (" + inventory[slotNameCount - 1].item.name + " x" + inventory[slotNameCount - 1].quantity + ")";

                        if (slotNameCount <= 50)
                        {
                            baseTT.SetToolTip(pb, "Slot " + slotNameCount + processedNameData);
                        }
                    }
                    else if(slotNameCount > 50 && slotNameCount < 91)
                    {
                        pbCollection[i].Image = pbank[i-50].item.icon;
                        Console.WriteLine(i);

                        string processedNameData = " (" + pbank[slotNameCount - 51].item.name + " x" + pbank[slotNameCount - 51].quantity + ")";

                        if (slotNameCount <= 50)
                        {
                            baseTT.SetToolTip(pb, "Slot " + slotNameCount + processedNameData);
                        }
                    }
                    slotNameCount++;
                    i++;

                }
            }
            catch
            {
                cbItem.SelectedItem = ih.globalTerrariaItems[0].name;
                nudQuant.Value = 0;
                cbPrefixes.SelectedItem = ih.globalItemPrefixes[0].name;
                gb_slot.Text = "Slot 0";
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
            updateInvDisplay();
        }

        private void item_Click(object sender, EventArgs e)
        {
            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "b" }, StringSplitOptions.None);
            invSelectedIndex = Int32.Parse(npart[1]) - 1;
            updateInvDisplay();
        }

        public List<Byte> reEncode()
        {
            //i hate this method so much

            List<Byte> buffer = new List<Byte> { };

            List<Byte> save = rawDecrypted;
            save.RemoveRange(24, save[24] + 1);

            List<Byte> nn = new List<byte> { };

            byte[] nameConverted = Encoding.UTF8.GetBytes(playerName);

            nn.Add((byte)nameConverted.Length);
            nn.AddRange(nameConverted);

            save.InsertRange(24, nn);
            nameEndOffset = 25 + nameConverted.Length;

            foreach (invItem iv in inventory)
            {
                List<Byte> tmp = iv.recompile(ih);
                buffer.AddRange(tmp);
            }

            foreach(Color c in playerColours)
            {
                buffer.AddRange(new List<Byte> { c.R, c.G, c.B });
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

            int dataBeginOffset = nameEndOffset + inventoryOffset;
            int dataEndOffset = dataBeginOffset + 500;
            int extCount = 0;

            for (int i = dataBeginOffset; i < dataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int CoinDataBeginOffset = nameEndOffset + coinOffset;
            int CoinDataEndOffset = CoinDataBeginOffset + 80;

            for (int i = CoinDataBeginOffset; i < CoinDataEndOffset; i++)
            {
                save[i] = buffer[extCount];
                extCount++;
            }

            int ColourDataBeginOffset = nameEndOffset + colOffset;
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

            //insert padding if needed
            while(save.Count() % 16 != 0)
            {
                save.Add(0);
            }

            return save;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (useOverwriteFile)
            {
                cr.encryptAndSave(reEncode().ToArray(), lastReadPlrPath);
                isSaved = true;
                saveNotifier sn = new saveNotifier();
                sn.ShowDialog();
            }
            else
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
                        safe.Clear();
                        pbank.Clear();
                        lastReadPlrPath = dlg.FileName;

                        //reload the saved file
                        this.Text = "WinTerrEdit | [F1] About | [F2] Settings | (" + dlg.FileName.Split('\\')[dlg.FileName.Split('\\').Length-1] + ")";

                        loadData(dlg.FileName);
                        //gbInvHold.Enabled = true;
                        //gbColour.Enabled = true;
                        //gbPlayer.Enabled = true;
                        gb_slot.Enabled = true;
                        gbItems.Enabled = true;
                        btnReload.Enabled = true;
                    }
                }
            }
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "b" }, StringSplitOptions.None);
            int tmp = Int32.Parse(npart[1]) - 1;

            if(tmp == copyIndex)
            {
                e.Graphics.DrawRectangle(Pens.Blue, 0, 0, 31, 31);
            }
            else if (tmp == invSelectedIndex)
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
                updateInvDisplay();
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
                updateInvDisplay();
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
                for (int i = 0; i < 58; i++)
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

        private void btnHeal_Click(object sender, EventArgs e)
        {
            nudHealthCur.Value = nudHealthMax.Value;
        }

        private void gbFillMana_Click(object sender, EventArgs e)
        {
            nudManaCur.Value = nudManaMax.Value;
        }

        private void btnMaxHealth_Click(object sender, EventArgs e)
        {
            nudHealthMax.Value = 500;
            nudHealthCur.Value = 500;
        }

        private void gbMaximumMana_Click(object sender, EventArgs e)
        {
            nudManaMax.Value = 200;
            nudManaCur.Value = 200;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbItem.SelectedItem = "Empty";
            cbPrefixes.SelectedIndex = 0;
            nudQuant.Value = 0;
        }

        private void entry_kDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F1)
            {
                about ab = new about();
                ab.ShowDialog();
            }
            if(e.KeyCode == Keys.F2)
            {
                Settings st = new Settings(useOverwriteFile, useAutoReloadFile, useExtendedName);
                if(st.ShowDialog() == DialogResult.OK)
                {
                    useOverwriteFile = st.useOverwriteFile;
                    useAutoReloadFile = st.useAutoReloadFile;
                    useExtendedName = st.useExtendedName;

                    if (useAutoReloadFile)
                    {
                        if (lastReadPlrPath != "")
                        {
                            autoFunctionTimer.Start();
                        }
                    }
                    else
                    {
                        autoFunctionTimer.Stop();
                    }

                    if (useExtendedName)
                    {
                        tbName.MaxLength = 200;
                    }
                    else
                    {
                        tbName.MaxLength = 20;
                        if(tbName.Text.Length > 20)
                        {
                            tbName.Text = tbName.Text.Substring(0, 20);
                        }
                    }
                }
            }
            if(e.KeyCode == Keys.F3)
            {
                hexView hx = new hexView(debugInvData, rawDecrypted.ToArray(), nameEndOffset, versionCode);
                hx.ShowDialog();
            }
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                copyBuffer = inventory[invSelectedIndex];
                copyIndex = invSelectedIndex;
                updateInvDisplay();
            }
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                if(copyBuffer != null)
                {
                    cbItem.SelectedItem = copyBuffer.item.name;
                    cbPrefixes.SelectedItem = copyBuffer.prefix.name;
                    nudQuant.Value = copyBuffer.quantity;
                    updateInvDisplay();
                }
            }
            if(e.KeyCode == Keys.Escape)
            {
                copyIndex = -1;
                copyBuffer = null;
                updateInvDisplay();
            }
            if(e.KeyCode == Keys.Delete)
            {
                cbItem.SelectedItem = "Empty";
                cbPrefixes.SelectedIndex = 0;
                nudQuant.Value = 0;
            }
            if(invSelectedIndex < 50 && invSelectedIndex != -1)
            {
                if (e.KeyCode == Keys.NumPad8)
                {
                    if (invSelectedIndex > 10)
                    {
                        invSelectedIndex -= 10;
                        updateInvDisplay();
                    }
                }
                if (e.KeyCode == Keys.NumPad2)
                {
                    if (invSelectedIndex <= 39)
                    {
                        invSelectedIndex += 10;
                        updateInvDisplay();
                    }
                }
                if (e.KeyCode == Keys.NumPad4)
                {
                    if (invSelectedIndex > 0)
                    {
                        invSelectedIndex -= 1;
                        updateInvDisplay();
                    }
                }
                if (e.KeyCode == Keys.NumPad6)
                {
                    if (invSelectedIndex < 49)
                    {
                        invSelectedIndex += 1;
                        updateInvDisplay();
                    }
                }
            }
        }

        private void ndq_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control || e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad5)
            {
                e.SuppressKeyPress = true;
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

        public string calcMd5OfOpenFile()
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(lastReadPlrPath))
                    {
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }
            }
            catch
            {
                //rarely throws IO error if WinTerrEdit reading the file and Terraria writing to the file happens at the same time.
                return currentFileHash;
            }
        }

        //method for automatically reloading the latest file
        private void autoFunctionTimer_Tick(object sender, EventArgs e)
        {
            string tmp = calcMd5OfOpenFile();

            if (tmp != currentFileHash)
            {
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
                safe.Clear();
                pbank.Clear();

                loadData(lastReadPlrPath);
                //gbInvHold.Enabled = true;
                //gbColour.Enabled = true;
                gb_slot.Enabled = true;
                gbItems.Enabled = true;
                tcMain.Enabled = true;

                btnSave.Enabled = true;
                invSelectedIndex = 0;
                updateInvDisplay();

                currentFileHash = tmp;
            }
            else
            {
                //do shit all
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            playerName = tbName.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
