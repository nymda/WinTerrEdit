using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinTerrEdit.Properties;

namespace WinTerrEdit {
    public partial class entry : Form {
        #region Summary
        /// <summary>
        ///
        /// WinTerrEdit - an open source terraria character editor that i never considered people might use.
        ///
        /// This is quickly becoming a massive, mostly unmanagable mess. This whole project desperatly needs a full recode.
        /// Lasciate ogne speranza, voi ch'intrate
        ///
        /// </summary>

        /// <note>
        /// the combobox "cbItem" is no longer nessicary on the UI, however, its unctionality is integeral to the program.
        /// dont delete cbItem, it is the glue holding everything together.
        /// </note>
        #endregion

        #region Fields
        #region General
        //general stuff
        itemHandler ih = new itemHandler(true);
        public List<Byte> rawDecrypted = new List<Byte> { };
        public readonly string playerfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Terraria\\Players";
        public string lastReadPlrPath = "";
        public string currentFileHash = "";
        #endregion
        #region Settings
        //bools controling settings
        public bool useAutoReloadFile = false;
        public bool useExtendedName = false;
        #endregion
        #region Player
        #region Static
        //static player variables
        public string playerName = "";
        public gamemodes.gamemode playerMode;
        public int versionCode;
        #endregion
        #region Modifiable
        //modifiable player variables
        public List<invItem> inv_main = new List<invItem> { };
        public List<invItem> inv_piggybank = new List<invItem> { };
        public List<invItem> inv_safe = new List<invItem> { };
        public List<invItem> inv_ammocoins = new List<invItem> { };
        public List<playerBuff> playerBuffs = new List<playerBuff> { };

        public List<int> playerHealth = new List<int> { };
        public List<int> playerMana = new List<int> { };
        public List<Color> playerColours = new List<Color> { }; //hair, skin, eyes, shirt, undershirt, pants, shoes
        public int playerHS = 0;

        string fileName = "-1";
        bool doSaveAss = false;
        #endregion
        #endregion
        #region Displays
        //displays
        public List<PictureBox> pbs_inventory = new List<PictureBox> { };
        public List<PictureBox> pbs_piggybank = new List<PictureBox> { };
        public List<PictureBox> pbs_safe = new List<PictureBox> { };
        public List<PictureBox> pbs_ammocoins = new List<PictureBox> { };
        public List<PictureBox> pbs_buffs = new List<PictureBox> { };
        #endregion
        #region Other
        //version
        public const string WTEversion = "1.10.4-B";
        public invItem copyBuffer;
        public int copyIndex = -1;

        //only populated if unlock all is used
        public List<Byte> unlockAllData = new List<Byte> { };

        //other
        public int inventoryOffset;
        public int ammoOffset;
        public int coinOffset;
        public int colOffset;
        public int pigOffset;
        public int safeOffset;
        public int buffOffset;
        public List<List<int>> debugInvData = new List<List<int>> { };
        public List<ListViewItem> lvis = new List<ListViewItem> { };
        public List<ListViewItem> lvis_buff = new List<ListViewItem> { };
        public List<Panel> pnCollection = new List<Panel> { };
        public int nameEndOffset = 0;
        public int invSelectedIndex = -1;
        public int trueSelectedIndex = -1;
        crypto cr = new crypto();
        public bool isSaved = true;
        public loading ld;
        public ToolTip baseTT = new ToolTip();
        public int selectedTab = 0;
        Stopwatch st = new Stopwatch();
        public string aboutBoxContactData;
        public registryHandler rh = new registryHandler();

        //conext menus
        public ContextMenuStrip cm = new ContextMenuStrip();
        #endregion
        #region Debug and error handling
        //debug and error handling
        public int stage = 0;
        public string error = "";
        public bool doBuffs = true;
        public bool loading_complete = false;
        #endregion
        #endregion

        #region Accessors
        public string Status {
            get => statusMsg.Text;
            set {
                statusMsg.Text = value;
            }
        }
        #endregion

        #region Methods
        public entry()
        {
            st.Start();
            InitializeComponent();
            new Thread(new ThreadStart(delegate
            {
                Application.Run(ld = new loading(WTEversion));
            })).Start();
        }
        void Entry_Load(object sender, EventArgs e)
        {
            string settings = "000";
            cm.Items.Add("Copy");
            cm.Items.Add("Paste");
            cm.Items.Add("Delete");
            cm.Items.Add("Toggle favourite");
            cm.ItemClicked += new ToolStripItemClickedEventHandler(cm_ItemClicked);

            try { settings = rh.loadRegData(); }
            catch { }

            if (settings[0] == '1')
            {
                useAutoReloadFile = true;
            }
            if (settings[1] == '1')
            {
                useExtendedName = true;
            }

            btnReload.Image = Resources.crappyreload;

            baseTT.ShowAlways = true;
            baseTT.SetToolTip(btnLoad, "Open a .PLR file");
            baseTT.SetToolTip(btnReload, "Reload the last opened .PLR file");
            baseTT.SetToolTip(btnSave, "Save the currently open .PLR file");

            btnReload.UseCompatibleTextRendering = true;

            //display content
            pbs_inventory.AddRange(new List<PictureBox> { Pb1, Pb2, Pb3, Pb4, Pb5, Pb6, Pb7, Pb8, Pb9, Pb10, Pb11, Pb12, Pb13, Pb14, Pb15, Pb16, Pb17, Pb18, Pb19, Pb20, Pb21, Pb22, Pb23, Pb24, Pb25, Pb26, Pb27, Pb28, Pb29, Pb30, Pb31, Pb32, Pb33, Pb34, Pb35, Pb36, Pb37, Pb38, Pb39, Pb40, Pb41, Pb42, Pb43, Pb44, Pb45, Pb46, Pb47, Pb48, Pb49, Pb50 });
            pbs_piggybank.AddRange(new List<PictureBox> { Pb51, Pb52, Pb53, Pb54, Pb55, Pb56, Pb57, Pb58, Pb59, Pb60, Pb61, Pb62, Pb63, Pb64, Pb65, Pb66, Pb67, Pb68, Pb69, Pb70, Pb71, Pb72, Pb73, Pb74, Pb75, Pb76, Pb77, Pb78, Pb79, Pb80, Pb81, Pb82, Pb83, Pb84, Pb85, Pb86, Pb87, Pb88, Pb89, Pb90 });
            pbs_safe.AddRange(new List<PictureBox> { Pb91, Pb92, Pb93, Pb94, Pb95, Pb96, Pb97, Pb98, Pb99, Pb100, Pb101, Pb102, Pb103, Pb104, Pb105, Pb106, Pb107, Pb108, Pb109, Pb110, Pb111, Pb112, Pb113, Pb114, Pb115, Pb116, Pb117, Pb118, Pb119, Pb120, Pb121, Pb122, Pb123, Pb124, Pb125, Pb126, Pb127, Pb128, Pb129, Pb130 });
            pbs_ammocoins.AddRange(new List<PictureBox> { Pb131, Pb132, Pb133, Pb134, Pb135, Pb136, Pb137, Pb138 });
            pbs_buffs.AddRange(new List<PictureBox> { Pb179, Pb180, Pb181, Pb182, Pb183, Pb184, Pb185, Pb186, Pb187, Pb188, Pb189, Pb190, Pb191, Pb192, Pb193, Pb194, Pb195, Pb196, Pb197, Pb198, Pb199, Pb200 });

            pnCollection.AddRange(new List<Panel> { hairPnl, skinPnl, eyesPnl, shirtPnl, undershirtPnl, pantsPnl, shoesPnl });

            int cnt = 0;
            itemLV.View = View.Details;
            itemLV.Columns[0].Width = itemLV.Width - 25;
            imgl_items.ImageSize = new Size(32, 32);
            itemLV.SmallImageList = imgl_items;
            itemLV.BeginUpdate();
            foreach (baseItem itm in ih.globalTerrariaItems)
            {
                cbItem.Items.Add(itm.name);
                imgl_items.Images.Add(itm.icon);
                if (itm.ID != -1)
                {
                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = itm.name;
                    tmp.ImageIndex = cnt;
                    itemLV.Items.Add(tmp);
                    lvis.Add(tmp);
                }
                cnt++;
            }

            cnt = 0;
            buffLV.View = View.Details;
            buffLV.Columns[0].Width = buffLV.Width - 25;
            imgl_buffs.ImageSize = new Size(32, 32);
            buffLV.SmallImageList = imgl_buffs;
            buffLV.BeginUpdate();
            foreach (buff bff in ih.globalBuffs)
            {
                cbBuffs.Items.Add(bff.name);
                imgl_buffs.Images.Add(bff.icon);
                if (bff.ID != -1)
                {
                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = bff.name;
                    tmp.ImageIndex = cnt;
                    if (bff.buffStatus == buffStatus.Debuff)
                    {
                        tmp.ForeColor = Color.Red;
                    }
                    buffLV.Items.Add(tmp);
                    lvis_buff.Add(tmp);
                }
                cnt++;
            }

            st.Stop();
            Debug.WriteLine($"Load took {st.ElapsedMilliseconds} ms ({st.ElapsedTicks})");
            // Consider using Debug.Write(Line) to write output directly to VS debug output
            // (since its no console app)

            itemLV.Sorting = SortOrder.Ascending;
            itemLV.Sort();
            itemLV.EndUpdate();
            buffLV.Sorting = SortOrder.Ascending;
            buffLV.Sort();
            buffLV.EndUpdate();

            this.Invoke(new MethodInvoker(delegate ()
            {
                //LOADING FINISHED
                ld.Close();
            }));

            Thread getContact = new Thread(() => aboutBoxContactData = getContactInfoFromExtServer());
            getContact.Start();

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
            cbBuffs.SelectedIndex = 0;
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

            #region Stages
            #region Stage 1
            //decryption complete
            stage = 1;

            rawDecrypted = decrypted.ToList();
            versionCode = ih.resolveEncodedData(decrypted[0], decrypted[1]);

            if (versionCode > 512)
            {
                throw new Exception("Invalid PLR header data");
            }

            int startpos = 25;
            int nameLen = decrypted[startpos - 1];
            byte[] namebytes = new byte[nameLen];
            Array.Copy(decrypted, startpos, namebytes, 0, nameLen);
            nameEndOffset = startpos + nameLen;
            playerMode = (gamemodes.gamemode)decrypted[nameEndOffset];
            playerName = Encoding.UTF8.GetString(namebytes);
            tbName.Text = playerName;

            if (versionCode < 230)
            {
                inventoryOffset = 211;
                coinOffset = 711;
                colOffset = 40;
                pigOffset = 841;
                safeOffset = 1201;

                doBuffs = false;
                foreach (PictureBox p in pbs_buffs)
                {
                    p.Enabled = false;
                }
                nudDur.Enabled = false;
                cbBuffs.Enabled = false;
                BuffClearBtn.Enabled = false;
                buffLV.Enabled = false;
            }
            else
            {
                inventoryOffset = 213;
                coinOffset = 713;
                colOffset = 42;
                pigOffset = 843;
                safeOffset = 1203;
                buffOffset = 2284; //2284
            }
            #endregion
            #region Stage 2
            //mana and name has been found, NEO has been set
            stage = 2;

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
                    inv_main.Add(iv);
                    debugInvData.Add(invTmp);
                    invTmp = new List<int> { };
                    extCounter = 0;
                }
            }
            #endregion
            #region Stage 3
            //inventory data has been found
            stage = 3;

            int PbnkDataBeginOffset = nameEndOffset + pigOffset;
            int PbnkDataEndOffset = PbnkDataBeginOffset + 360;

            extCounter = 0;

            List<int> PbnkTmp = new List<int> { };
            for (int i = PbnkDataBeginOffset; i < PbnkDataEndOffset; i++)
            {
                extCounter++;
                PbnkTmp.Add(decrypted[i]);
                if (extCounter == 9)
                {
                    invItem iv = new invItem(PbnkTmp, ih);
                    inv_piggybank.Add(iv);
                    debugInvData.Add(PbnkTmp);
                    PbnkTmp = new List<int> { };
                    extCounter = 0;
                }
            }
            #endregion
            #region Stage 4
            //piggybank data has been found
            stage = 4;

            int SafeDataBeginOffset = nameEndOffset + safeOffset;
            int SafeDataEndOffset = SafeDataBeginOffset + 360;

            extCounter = 0;

            List<int> SafeTmp = new List<int> { };
            for (int i = SafeDataBeginOffset; i < SafeDataEndOffset; i++)
            {
                extCounter++;
                SafeTmp.Add(decrypted[i]);
                if (extCounter == 9)
                {
                    invItem iv = new invItem(SafeTmp, ih);
                    inv_safe.Add(iv);
                    debugInvData.Add(SafeTmp);
                    SafeTmp = new List<int> { };
                    extCounter = 0;
                }
            }
            #endregion
            #region Stage 5
            //safe data has been found
            stage = 5;

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
                    inv_ammocoins.Add(iv);
                    debugInvData.Add(coinTmp);
                    coinTmp = new List<int> { };
                    extCounter = 0;
                }
            }
            #endregion
            #region Stage 6
            //coins data has been found
            stage = 6;

            if (doBuffs)
            {
                int BuffDataBeginOffset = nameEndOffset + buffOffset;
                int BuffDataEndOffset = BuffDataBeginOffset + 176;

                extCounter = 0;

                List<int> buffBtm = new List<int> { };
                for (int i = BuffDataBeginOffset; i < BuffDataEndOffset; i++)
                {
                    extCounter++;
                    buffBtm.Add(decrypted[i]);
                    if (extCounter == 8)
                    {
                        playerBuff iv = new playerBuff(buffBtm, ih);
                        playerBuffs.Add(iv);
                        debugInvData.Add(buffBtm);
                        buffBtm = new List<int> { };
                        extCounter = 0;
                    }
                }
                nudDur.Value = playerBuffs[0].duration;
            }
            #endregion
            #region Stage 7
            //buff data has been found
            stage = 7;

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
            #endregion
            #region Stage 8
            //colour data has been found and shown in UI
            stage = 8;

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

            int phc = playerHealth[0];
            int phm = playerHealth[1];

            if (phc > nudHealthCur.Maximum)
            {
                nudHealthCur.Maximum = phc;
            }
            if (phm > nudHealthMax.Maximum)
            {
                nudHealthMax.Maximum = phm;
            }

            nudHealthCur.Value = phc;
            nudHealthMax.Value = phm;
            #endregion
            #region Stage 9
            //health data has been set and shown in UI
            stage = 9;

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

            int pmc = playerMana[0];
            int pmm = playerMana[1];

            if (pmc > nudManaCur.Maximum)
            {
                nudManaCur.Maximum = pmc;
            }
            if (pmm > nudManaMax.Maximum)
            {
                nudManaMax.Maximum = pmm;
            }

            nudManaCur.Value = playerMana[0];
            nudManaMax.Value = playerMana[1];
            #endregion
            #region Stage 10
            //mana data has been set and shown in UI
            stage = 10;

            int hs = decrypted[nameEndOffset + 9];

            if (hs > nudHair.Maximum)
            {
                nudHair.Maximum = hs;
            }

            playerHS = hs;
            nudHair.Value = hs;
            cbGamemode.SelectedIndex = (int)playerMode;
            #endregion
            #region Stage 11
            //hairstyle and gamemode has been shown in UI
            stage = 11;

            switch (selectedTab)
            {
                case 0:
                    nudQuant.Value = inv_main[invSelectedIndex].quantity;
                    break;

                case 1:
                    nudQuant.Value = inv_piggybank[invSelectedIndex].quantity;
                    break;

                case 2:
                    nudQuant.Value = inv_safe[invSelectedIndex].quantity;
                    break;

                case 3:
                    nudQuant.Value = inv_ammocoins[invSelectedIndex].quantity;
                    break;

                case 4:
                    cbBuffs.SelectedItem = playerBuffs[invSelectedIndex].buff.name;
                    nudDur.Value = playerBuffs[invSelectedIndex].duration;
                    break;
            }
            #endregion
            #region Stage 12
            //other UI data has been set
            stage = 12;

            var res = inv_main.Where(invItem => invItem.item.name == "Unknown");
            if (res.Count() > 0)
            {
                MessageBox.Show("This player contains \"Unknown\" items. These are items which have a quantity or prefix but no ID. This may be caused by a game bug or (more likely) a mod. Be careful when editing these items.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            #endregion
            #endregion
        }

        #region Player file
        void btnLoad_Click(object sender, EventArgs e) => loadPlayer();
        void btnReload_Click(object sender, EventArgs e) => reloadPlayer();
        void btnSave_Click(object sender, EventArgs e) => savePlayer();

        public void loadPlayer()
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
                        #region Reset
                        //reset variables
                        rawDecrypted.Clear();
                        playerName = "";
                        inv_main.Clear();
                        playerHealth.Clear();
                        playerMana.Clear();
                        playerColours.Clear();
                        nameEndOffset = 0;
                        invSelectedIndex = 0;
                        trueSelectedIndex = 0;
                        isSaved = true;
                        unlockAllData.Clear();
                        debugInvData.Clear();
                        inv_main.Clear();
                        inv_piggybank.Clear();
                        inv_safe.Clear();
                        inv_ammocoins.Clear();
                        playerBuffs.Clear();
                        #endregion

                        lastReadPlrPath = dlg.FileName;
                        this.Text = "WinTerrEdit | (" + dlg.SafeFileName + ")";

                        loadData(dlg.FileName);
                        Status = "Loading file...";
                        #region Enable all elements
                        //gbInvHold.Enabled = true;
                        //gbColour.Enabled = true;
                        //gbPlayer.Enabled = true;
                        tcMain.Enabled = true;
                        gb_slot_items.Enabled = true;
                        gb_slot_buff.Enabled = true;
                        gbItems.Enabled = true;
                        gbBuffs.Enabled = true;
                        btnReload.Enabled = true;
                        #endregion
                        updateInvDisplay();
                        if(tcMain.SelectedIndex == 0)
                            item_Click(Pb1, null);
                        fileName = dlg.FileName;
                    }
                    catch (Exception ex) {
                        errorReporter er = new errorReporter(stage.ToString(), ex.Message, Convert.ToBase64String(File.ReadAllBytes(lastReadPlrPath)));
                        er.ShowDialog();
                        //MessageBox.Show(String.Format("There was an issue loading this player. It may be corrupted or invalid. \n\nSTAGE: {0} \nERROR: {1}", stage, ex.Message), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            btnSave.Enabled = true;
            invSelectedIndex = 0;
            //updateInvDisplay(); WHY?
            Status = "File loaded!";
        }
        public void reloadPlayer()
        {
            rawDecrypted.Clear();
            playerName = "";
            inv_main.Clear();
            playerHealth.Clear();
            playerMana.Clear();
            playerColours.Clear();
            nameEndOffset = 0;
            isSaved = true;
            unlockAllData.Clear();
            debugInvData.Clear();
            inv_main.Clear();
            inv_piggybank.Clear();
            inv_safe.Clear();
            inv_ammocoins.Clear();
            playerBuffs.Clear();

            loadData(lastReadPlrPath);
            //gbInvHold.Enabled = true;
            //gbColour.Enabled = true;
            gb_slot_items.Enabled = true;
            gb_slot_buff.Enabled = true;
            gbItems.Enabled = true;
            gbBuffs.Enabled = true;
            tcMain.Enabled = true;
            btnSave.Enabled = true;
            updateInvDisplay();
        }
        public void savePlayer() {
            if (fileName == "-1" && doSaveAss) {
                Native.MessageBoxInterface.Show("Meh", "", 0);
                return;
            }
            if (fileName != "-1")
            {
                cr.encryptAndSave(reEncode().ToArray(), lastReadPlrPath);
                isSaved = true;
                Native.MessageBoxInterface.Show("Meh", "", 0);
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
                        Native.MessageBoxInterface.Show("Meh", "", 0);

                        rawDecrypted.Clear();
                        playerName = "";
                        inv_main.Clear();
                        playerHealth.Clear();
                        playerMana.Clear();
                        playerColours.Clear();
                        nameEndOffset = 0;
                        invSelectedIndex = 0;
                        isSaved = true;
                        unlockAllData.Clear();
                        debugInvData.Clear();
                        inv_main.Clear();
                        inv_piggybank.Clear();
                        inv_safe.Clear();
                        inv_ammocoins.Clear();
                        playerBuffs.Clear();

                        lastReadPlrPath = dlg.FileName;

                        //reload the saved file
                        this.Text = "WinTerrEdit | (" + dlg.FileName.Split('\\')[dlg.FileName.Split('\\').Length - 1] + ")";

                        loadData(dlg.FileName);
                        //gbInvHold.Enabled = true;
                        //gbColour.Enabled = true;
                        //gbPlayer.Enabled = true;
                        gb_slot_items.Enabled = true;
                        gb_slot_buff.Enabled = true;
                        gbItems.Enabled = true;
                        gbBuffs.Enabled = true;
                        btnReload.Enabled = true;
                    }
                }
            }
        }

        //method for automatically reloading the latest file
        void autoFunctionTimer_Tick(object sender, EventArgs e)
        {
            string tmp = calcMd5OfOpenFile();

            if (tmp != currentFileHash)
            {
                rawDecrypted.Clear();
                playerName = "";
                inv_main.Clear();
                playerHealth.Clear();
                playerMana.Clear();
                playerColours.Clear();
                nameEndOffset = 0;
                isSaved = true;
                unlockAllData.Clear();
                debugInvData.Clear();
                inv_main.Clear();
                inv_piggybank.Clear();
                inv_safe.Clear();
                inv_ammocoins.Clear();
                playerBuffs.Clear();

                loadData(lastReadPlrPath);
                gb_slot_items.Enabled = true;
                gb_slot_buff.Enabled = true;
                gbItems.Enabled = true;
                gbBuffs.Enabled = true;
                tcMain.Enabled = true;

                btnSave.Enabled = true;
                updateInvDisplay();

                currentFileHash = tmp;
            }
            else
            {
                //do shit all
            }
        }
        #endregion
        #region Uncathegorized events
        public void updateInvDisplay()
        {
            //hardcoded numbers EEEEEEEEVERYWHEREEEE

            try
            {
                int slotNameCount = 1;
                int i = 0;

                switch (selectedTab)
                {
                    case 0:
                        slotNameCount = 1;
                        i = 0;
                        foreach (PictureBox pb in pbs_inventory)
                        {
                            pb.ContextMenuStrip = cm;
                            pbs_inventory[i].Image = inv_main[i].item.icon;
                            string processedNameData = " (" + inv_main[slotNameCount - 1].item.name + " x" + inv_main[slotNameCount - 1].quantity + ")";
                            baseTT.SetToolTip(pb, "Slot " + slotNameCount + processedNameData);
                            slotNameCount++;
                            i++;
                        }
                        break;

                    case 1:
                        slotNameCount = 1;
                        i = 0;
                        foreach (PictureBox pb in pbs_piggybank)
                        {
                            pb.ContextMenuStrip = cm;
                            pbs_piggybank[i].Image = inv_piggybank[i].item.icon;
                            string processedNameData = " (" + inv_piggybank[slotNameCount - 1].item.name + " x" + inv_piggybank[slotNameCount - 1].quantity + ")";
                            baseTT.SetToolTip(pb, "Piggybank slot " + slotNameCount + processedNameData);
                            slotNameCount++;
                            i++;
                        }
                        break;

                    case 2:
                        slotNameCount = 1;
                        i = 0;
                        foreach (PictureBox pb in pbs_safe)
                        {
                            pb.ContextMenuStrip = cm;
                            pbs_safe[i].Image = inv_safe[i].item.icon;
                            string processedNameData = " (" + inv_safe[slotNameCount - 1].item.name + " x" + inv_safe[slotNameCount - 1].quantity + ")";
                            baseTT.SetToolTip(pb, "Safe slot " + slotNameCount + processedNameData);
                            slotNameCount++;
                            i++;
                        }
                        break;

                    case 3:
                        slotNameCount = 1;
                        i = 0;
                        foreach (PictureBox pb in pbs_ammocoins)
                        {
                            pb.ContextMenuStrip = cm;
                            pbs_ammocoins[i].Image = inv_ammocoins[i].item.icon;
                            string processedNameData = " (" + inv_ammocoins[slotNameCount - 1].item.name + " x" + inv_ammocoins[slotNameCount - 1].quantity + ")";
                            baseTT.SetToolTip(pb, "Safe slot " + slotNameCount + processedNameData);
                            slotNameCount++;
                            i++;
                        }
                        break;

                    case 4:
                        if (doBuffs)
                        {
                            slotNameCount = 1;
                            i = 0;
                            foreach (PictureBox pb in pbs_buffs)
                            {
                                pb.ContextMenuStrip = cm;
                                pbs_buffs[i].Image = playerBuffs[i].buff.icon;
                                string processedNameData = " (" + playerBuffs[slotNameCount - 1].buff.name + " for " + playerBuffs[slotNameCount - 1].duration + " seconds)";
                                baseTT.SetToolTip(pb, "Buff slot " + slotNameCount + processedNameData);
                                slotNameCount++;
                                i++;
                            }
                        }
                        break;
                }

            }
            catch
            {
                cbItem.SelectedItem = ih.globalTerrariaItems[0].name;
                nudQuant.Value = 0;
                cbPrefixes.SelectedItem = ih.globalItemPrefixes[0].name;
                gb_slot_items.Text = "Slot 0";
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            try
            {
                switch (selectedTab)
                {
                    case 0:
                        inv_main[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 1:
                        inv_piggybank[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 2:
                        inv_safe[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 3:
                        inv_ammocoins[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 4:
                        playerBuffs[invSelectedIndex].duration = (int)nudDur.Value;
                        break;

                }
            }
            catch
            {
                ///
                /// This block allows me to assign a manually edited quantity to
                /// an item when the users clicks off of it, however it can
                /// also cause an outOfBounds error.
                ///
            }

            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "b" }, StringSplitOptions.None);
            invSelectedIndex = Int32.Parse(npart[1]) - 1;
            trueSelectedIndex = invSelectedIndex;
            string titleText;
            switch (selectedTab)
            {
                case 0:
                    cbItem.SelectedItem = inv_main[invSelectedIndex].item.name;
                    nudQuant.Value = inv_main[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_main[invSelectedIndex].prefix.name;
                    titleText = "Inventory slot " + (invSelectedIndex + 1) + " (" + inv_main[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 1:
                    invSelectedIndex -= 50;
                    cbItem.SelectedItem = inv_piggybank[invSelectedIndex].item.name;
                    nudQuant.Value = inv_piggybank[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_piggybank[invSelectedIndex].prefix.name;
                    titleText = "Piggybank slot " + (invSelectedIndex + 1) + " (" + inv_piggybank[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 2:
                    invSelectedIndex -= 90;
                    cbItem.SelectedItem = inv_safe[invSelectedIndex].item.name;
                    nudQuant.Value = inv_safe[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_safe[invSelectedIndex].prefix.name;
                    titleText = "Safe slot " + (invSelectedIndex + 1) + " (" + inv_safe[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 3:
                    invSelectedIndex -= 130;
                    cbItem.SelectedItem = inv_ammocoins[invSelectedIndex].item.name;
                    nudQuant.Value = inv_ammocoins[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_ammocoins[invSelectedIndex].prefix.name;
                    titleText = "Coin / ammo slot " + (invSelectedIndex + 1) + " (" + inv_ammocoins[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 4:
                    invSelectedIndex -= 178;
                    nudDur.Value = playerBuffs[invSelectedIndex].duration;
                    cbBuffs.SelectedItem = playerBuffs[invSelectedIndex].buff.name;
                    gb_slot_buff.Text = "Buff slot " + (invSelectedIndex + 1);
                    break;

            }

            updateInvDisplay();
        }

        public List<Byte> reEncode()
        {
            //define each data block for re-encoding
            List<Byte> encodedInvData = new List<Byte> { };
            List<Byte> encodedBankData = new List<Byte> { };
            List<Byte> encodedSafeData = new List<Byte> { };
            List<Byte> encodedAmmoData = new List<Byte> { };
            List<Byte> encodedColourData = new List<Byte> { };
            List<Byte> encodedHealthData = new List<Byte> { };
            List<Byte> encodedManaData = new List<Byte> { };
            List<Byte> encodedBuffData = new List<Byte> { };

            //create the template
            List<Byte> save = rawDecrypted;

            //insert the name and recalculate NEO
            save.RemoveRange(24, save[24] + 1);
            List<Byte> nn = new List<byte> { };
            byte[] nameConverted = Encoding.UTF8.GetBytes(playerName);
            nn.Add((byte)nameConverted.Length);
            nn.AddRange(nameConverted);
            save.InsertRange(24, nn);
            nameEndOffset = 25 + nameConverted.Length;

            //populate encoded inventory data
            foreach (invItem iv in inv_main)
            {
                encodedInvData.AddRange(iv.recompile(ih, encodeMethod.Long));
            }

            //populate encoded ammo / coin data
            foreach (invItem iv in inv_ammocoins)
            {
                encodedAmmoData.AddRange(iv.recompile(ih, encodeMethod.Long));
            }

            //populate encoded piggybank data
            foreach (invItem iv in inv_piggybank)
            {
                encodedBankData.AddRange(iv.recompile(ih, encodeMethod.Short));
            }

            //populate encoded safe data
            foreach (invItem iv in inv_safe)
            {
                encodedSafeData.AddRange(iv.recompile(ih, encodeMethod.Short));
            }

            //populate encoded colour data
            foreach (Color c in playerColours)
            {
                encodedColourData.AddRange(new List<Byte> { c.R, c.G, c.B });
            }

            if (doBuffs)
            {
                foreach (playerBuff pb in playerBuffs)
                {
                    encodedBuffData.AddRange(pb.recompile(ih));
                }
            }

            //populate encoded health data
            encodedHealthData.InsertRange(0, new List<Byte> { (byte)ih.encodeData(playerHealth[0])[0], (byte)ih.encodeData(playerHealth[0])[1], 0x00, 0x00, (byte)ih.encodeData(playerHealth[1])[0], (byte)ih.encodeData(playerHealth[1])[1], 0x00, 0x00 });

            //populate encoded mana data
            encodedManaData.InsertRange(0, new List<Byte> { (byte)ih.encodeData(playerMana[0])[0], (byte)ih.encodeData(playerMana[0])[1], 0x00, 0x00, (byte)ih.encodeData(playerMana[1])[0], (byte)ih.encodeData(playerMana[1])[1], 0x00, 0x00 });

            //insert inventory data
            int dataBeginOffset = nameEndOffset + inventoryOffset;
            save.RemoveRange(dataBeginOffset, 500);
            save.InsertRange(dataBeginOffset, encodedInvData);
            Debug.WriteLine("Inventory data: Removed 500 bytes, Inserted " + encodedInvData.Count() + " bytes");

            int AmmmoDataBeginOffset = nameEndOffset + coinOffset;
            save.RemoveRange(AmmmoDataBeginOffset, 80);
            save.InsertRange(AmmmoDataBeginOffset, encodedAmmoData);
            Debug.WriteLine("Ammo data: Removed 80 bytes, Inserted " + encodedAmmoData.Count() + " bytes");

            int PbnkDataBeginOffset = nameEndOffset + pigOffset;
            save.RemoveRange(PbnkDataBeginOffset, 360);
            save.InsertRange(PbnkDataBeginOffset, encodedBankData);
            Debug.WriteLine("Ammo data: Removed 360 bytes, Inserted " + encodedBankData.Count() + " bytes");

            int SafeDataBeginOffset = nameEndOffset + safeOffset;
            save.RemoveRange(SafeDataBeginOffset, 360);
            save.InsertRange(SafeDataBeginOffset, encodedSafeData);
            Debug.WriteLine("Ammo data: Removed 360 bytes, Inserted " + encodedSafeData.Count() + " bytes");

            int ColourDataBeginOffset = nameEndOffset + colOffset;
            save.RemoveRange(ColourDataBeginOffset, 21);
            save.InsertRange(ColourDataBeginOffset, encodedColourData);
            Debug.WriteLine("Colour data: Removed 21 bytes, Inserted " + encodedColourData.Count() + " bytes");

            int HealthDataBeginOffset = nameEndOffset + 18;
            save.RemoveRange(HealthDataBeginOffset, 8);
            save.InsertRange(HealthDataBeginOffset, encodedHealthData);
            Debug.WriteLine("Health data: Removed 8 bytes, Inserted " + encodedHealthData.Count() + " bytes");

            int ManaDataBeginOffset = nameEndOffset + 26;
            save.RemoveRange(ManaDataBeginOffset, 8);
            save.InsertRange(ManaDataBeginOffset, encodedManaData);
            Debug.WriteLine("Mana data: Removed 8 bytes, Inserted " + encodedManaData.Count() + " bytes");

            if (doBuffs)
            {
                int BuffDataBeginOffset = nameEndOffset + buffOffset;
                save.RemoveRange(BuffDataBeginOffset, 176);
                save.InsertRange(BuffDataBeginOffset, encodedBuffData);
                Debug.WriteLine("Buff data: Removed 176 bytes, Inserted " + encodedBuffData.Count() + " bytes");
            }

            save[nameEndOffset + 9] = (byte)playerHS;
            save[nameEndOffset] = (byte)playerMode;

            //insert padding if needed
            while (save.Count() % 16 != 0)
            {
                save.Add(0);
            }

            return save;
        }

        void item_Paint(object sender, PaintEventArgs e)
        {
            string elementName = (sender as PictureBox).Name;
            string[] npart = elementName.Split(new string[] { "b" }, StringSplitOptions.None);
            int tmp = Int32.Parse(npart[1]) - 1;

            if (tcMain.SelectedIndex == 0 && inv_main.Count() > 0 && inv_main[tmp].isFavorite)
            {
                e.Graphics.FillRectangle(Brushes.Orange, 2, 2, 5, 5);
            }

            if (tmp == copyIndex && tcMain.SelectedIndex != 4)
            {
                e.Graphics.DrawRectangle(Pens.Blue, 0, 0, 31, 31);
            }
            else if (tmp == trueSelectedIndex)
            {
                e.Graphics.DrawRectangle(Pens.Red, 0, 0, 31, 31);
            }
        }

        void gb_slot_Enter(object sender, EventArgs e)
        {

        }

        void cbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(invSelectedIndex >= 0)
            {
                try
                {
                    switch (selectedTab)
                    {
                        case 0:
                            if (cbItem.SelectedIndex.ToString() != "")
                            {
                                inv_main[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
                            }
                            isSaved = false;
                            if (inv_main[invSelectedIndex].quantity == 0 && inv_main[invSelectedIndex].item.name != "Empty")
                            {
                                inv_main[invSelectedIndex].quantity += 1;
                                nudQuant.Value += 1;
                            }
                            if (inv_main[invSelectedIndex].item.name == "Empty")
                            {
                                inv_main[invSelectedIndex].quantity = 0;
                                nudQuant.Value = 0;
                                inv_main[invSelectedIndex].isFavorite = false;
                            }
                            break;

                        case 1:
                            if (cbItem.SelectedIndex.ToString() != "")
                            {
                                inv_piggybank[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
                            }
                            isSaved = false;
                            if (inv_piggybank[invSelectedIndex].quantity == 0 && inv_piggybank[invSelectedIndex].item.name != "Empty")
                            {
                                inv_piggybank[invSelectedIndex].quantity += 1;
                                nudQuant.Value += 1;
                            }
                            if (inv_piggybank[invSelectedIndex].item.name == "Empty")
                            {
                                inv_piggybank[invSelectedIndex].quantity = 0;
                                nudQuant.Value = 0;
                            }
                            break;

                        case 2:
                            if (cbItem.SelectedIndex.ToString() != "")
                            {
                                inv_safe[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
                            }
                            isSaved = false;
                            if (inv_safe[invSelectedIndex].quantity == 0 && inv_safe[invSelectedIndex].item.name != "Empty")
                            {
                                inv_safe[invSelectedIndex].quantity += 1;
                                nudQuant.Value += 1;
                            }
                            if (inv_safe[invSelectedIndex].item.name == "Empty")
                            {
                                inv_safe[invSelectedIndex].quantity = 0;
                                nudQuant.Value = 0;
                            }
                            break;

                        case 3:
                            if (cbItem.SelectedIndex.ToString() != "")
                            {
                                inv_ammocoins[invSelectedIndex].item = ih.searchItemByName(cbItem.SelectedItem.ToString());
                            }
                            isSaved = false;
                            if (inv_ammocoins[invSelectedIndex].quantity == 0 && inv_ammocoins[invSelectedIndex].item.name != "Empty")
                            {
                                inv_ammocoins[invSelectedIndex].quantity += 1;
                                nudQuant.Value += 1;
                            }
                            if (inv_ammocoins[invSelectedIndex].item.name == "Empty")
                            {
                                inv_ammocoins[invSelectedIndex].quantity = 0;
                                nudQuant.Value = 0;
                            }
                            break;

                        case 4:
                            if (cbBuffs.SelectedIndex.ToString() != "")
                            {
                                playerBuffs[invSelectedIndex].buff = ih.searchBuffByName(cbBuffs.SelectedItem.ToString());
                            }
                            isSaved = false;
                            if (playerBuffs[invSelectedIndex].duration == 0 && playerBuffs[invSelectedIndex].buff.name != "None")
                            {
                                playerBuffs[invSelectedIndex].duration += 1;
                                nudQuant.Value += 1;
                            }
                            if (playerBuffs[invSelectedIndex].buff.name == "None")
                            {
                                playerBuffs[invSelectedIndex].duration = 0;
                                nudQuant.Value = 0;
                            }
                            break;
                    }

                    updateInvDisplay();
                }
                catch
                {

                }
            }
        }
        void cbBuffs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(invSelectedIndex >= 0)
            {
                if (doBuffs)
                {
                    try
                    {
                        if (cbBuffs.SelectedIndex.ToString() != "")
                        {
                            playerBuffs[invSelectedIndex].buff = ih.searchBuffByName(cbBuffs.SelectedItem.ToString());
                        }
                        isSaved = false;
                        if (playerBuffs[invSelectedIndex].duration == 0 && playerBuffs[invSelectedIndex].buff.name != "None")
                        {
                            playerBuffs[invSelectedIndex].duration += 60;
                            nudDur.Value += 60;
                        }
                        if (playerBuffs[invSelectedIndex].buff.name == "None")
                        {
                            playerBuffs[invSelectedIndex].duration = 0;
                            nudDur.Value = 0;
                        }
                        updateInvDisplay();
                    }
                    catch
                    {

                    }
                }
            }
        }
        void cbPrefixes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(invSelectedIndex >= 0)
            {
                try
                {
                    switch (selectedTab)
                    {
                        case 0:
                            if (cbPrefixes.SelectedItem.ToString() != "")
                            {
                                inv_main[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
                            }
                            break;

                        case 1:
                            if (cbPrefixes.SelectedItem.ToString() != "")
                            {
                                inv_piggybank[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
                            }
                            break;

                        case 2:
                            if (cbPrefixes.SelectedItem.ToString() != "")
                            {
                                inv_safe[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
                            }
                            break;

                        case 3:
                            if (cbPrefixes.SelectedItem.ToString() != "")
                            {
                                inv_ammocoins[invSelectedIndex].prefix = ih.searchPrefixByName(cbPrefixes.SelectedItem.ToString());
                            }
                            break;
                    }

                    updateInvDisplay();
                    isSaved = false;
                }
                catch
                {

                }
            }
        }

        void nudQuant_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (selectedTab)
                {
                    case 0:
                        inv_main[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 1:
                        inv_piggybank[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 2:
                        inv_safe[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;

                    case 3:
                        inv_ammocoins[invSelectedIndex].quantity = (int)nudQuant.Value;
                        break;
                }

                isSaved = false;
            }
            catch
            {

            }
        }
        void nudDur_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                playerBuffs[invSelectedIndex].duration = (int)nudDur.Value;

                isSaved = false;
            }
            catch
            {

            }
        }

        void ScrollHandlerFunction(object sender, MouseEventArgs e)
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

        void onClose(object sender, FormClosingEventArgs e)
        {
            if (isSaved)
            {
                rh.saveRegData(useAutoReloadFile, useExtendedName);
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
                var dialogResult = Native.MessageBoxInterface.Show("Changes wont be applied if you dont save.\nWould you like to exit?\n", "Warning", 0);
                if (dialogResult == (int) DialogResult.OK)
                {
                    rh.saveRegData(useAutoReloadFile, useExtendedName);
                    Environment.Exit(0);
                }
            }
        }

        void colourSelecter_Click(object sender, EventArgs e)
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

        void nudHealthCur_ValueChanged(object sender, EventArgs e)
        {
            playerHealth[0] = (int)nudHealthCur.Value;
        }
        void nudHealthMax_ValueChanged(object sender, EventArgs e)
        {
            playerHealth[1] = (int)nudHealthMax.Value;
        }
        void nudManaCur_ValueChanged(object sender, EventArgs e)
        {
            playerMana[0] = (int)nudManaCur.Value;
            playerMana[0] = (int)nudManaCur.Value;
        }
        void nudManaMax_ValueChanged(object sender, EventArgs e)
        {
            playerMana[1] = (int)nudManaMax.Value;
        }
        #endregion
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

        #region Button click events
        void btnHeal_Click(object sender, EventArgs e)
        {
            nudHealthCur.Value = nudHealthMax.Value;
        }
        void gbFillMana_Click(object sender, EventArgs e)
        {
            nudManaCur.Value = nudManaMax.Value;
        }
        void btnMaxHealth_Click(object sender, EventArgs e)
        {
            nudHealthMax.Value = 500;
            nudHealthCur.Value = 500;
        }
        void gbMaximumMana_Click(object sender, EventArgs e)
        {
            nudManaMax.Value = 200;
            nudManaCur.Value = 200;
        }
        void btnClear_Click(object sender, EventArgs e)
        {
            cbItem.SelectedItem = "Empty";
            cbPrefixes.SelectedIndex = 0;
            nudQuant.Value = 0;
        }
        void BuffClearBtn_Click(object sender, EventArgs e)
        {
            cbBuffs.SelectedItem = "None";
            nudDur.Value = 0;
        }
        #endregion
        #region Other events
        void entry_kDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                about ab = new about(aboutBoxContactData, WTEversion);
                ab.ShowDialog();
            }
            if (e.KeyCode == Keys.F2)
            {
                Settings st = new Settings(useAutoReloadFile, useExtendedName);
                if (st.ShowDialog() == DialogResult.OK)
                {
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
                        if (tbName.Text.Length > 20)
                        {
                            tbName.Text = tbName.Text.Substring(0, 20);
                        }
                    }
                }
            }
            if (e.KeyCode == Keys.F3)
            {
                hexView hx = new hexView(debugInvData, rawDecrypted.ToArray(), nameEndOffset, versionCode);
                hx.ShowDialog();
            }
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                switch (selectedTab)
                {
                    case 0:
                        copyBuffer = inv_main[invSelectedIndex];
                        break;

                    case 1:
                        copyBuffer = inv_piggybank[invSelectedIndex];
                        break;

                    case 2:
                        copyBuffer = inv_safe[invSelectedIndex];
                        break;

                    case 3:
                        copyBuffer = inv_ammocoins[invSelectedIndex];
                        break;
                }
                copyIndex = trueSelectedIndex;
                updateInvDisplay();
            }
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                if (copyBuffer != null)
                {
                    cbItem.SelectedItem = copyBuffer.item.name;
                    cbPrefixes.SelectedItem = copyBuffer.prefix.name;
                    nudQuant.Value = copyBuffer.quantity;
                    updateInvDisplay();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                copyIndex = -1;
                copyBuffer = null;
                updateInvDisplay();
            }
            if (e.KeyCode == Keys.Delete)
            {
                cbItem.SelectedItem = "Empty";
                cbPrefixes.SelectedIndex = 0;
                nudQuant.Value = 0;
            }
            if (invSelectedIndex < 50 && invSelectedIndex != -1)
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
            if (e.KeyCode == Keys.Menu && selectedTab == 0)
            {
                inv_main[invSelectedIndex].isFavorite = !inv_main[invSelectedIndex].isFavorite;
                updateInvDisplay();
            }
        }

        void ndq_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control || e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.Alt)
            {
                e.SuppressKeyPress = true;
            }
            switch (selectedTab)
            {
                case 0:
                    inv_main[invSelectedIndex].quantity = (int)nudQuant.Value;
                    break;

                case 1:
                    inv_piggybank[invSelectedIndex].quantity = (int)nudQuant.Value;
                    break;

                case 2:
                    inv_safe[invSelectedIndex].quantity = (int)nudQuant.Value;
                    break;

                case 3:
                    inv_ammocoins[invSelectedIndex].quantity = (int)nudQuant.Value;
                    break;

                case 4:
                    playerBuffs[invSelectedIndex].duration = (int)nudDur.Value;
                    break;

            }

        }

        void lb_activ(object sender, EventArgs e)
        {
            cbItem.SelectedItem = itemLV.SelectedItems[0].Text;
            string titleText;

            switch (selectedTab)
            {
                case 0:
                    inv_main[invSelectedIndex].quantity = (int)nudQuant.Value;
                    cbItem.SelectedItem = inv_main[invSelectedIndex].item.name;
                    nudQuant.Value = inv_main[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_main[invSelectedIndex].prefix.name;
                    titleText = "Inventory slot " + (invSelectedIndex + 1) + " (" + inv_main[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 1:
                    inv_piggybank[invSelectedIndex].quantity = (int)nudQuant.Value;
                    cbItem.SelectedItem = inv_piggybank[invSelectedIndex].item.name;
                    nudQuant.Value = inv_piggybank[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_piggybank[invSelectedIndex].prefix.name;
                    titleText = "Piggybank slot " + (invSelectedIndex + 1) + " (" + inv_main[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 2:
                    inv_safe[invSelectedIndex].quantity = (int)nudQuant.Value;
                    cbItem.SelectedItem = inv_safe[invSelectedIndex].item.name;
                    nudQuant.Value = inv_safe[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_safe[invSelectedIndex].prefix.name;
                    titleText = "Safe slot " + (invSelectedIndex + 1) + " (" + inv_main[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 3:
                    inv_ammocoins[invSelectedIndex].quantity = (int)nudQuant.Value;
                    cbItem.SelectedItem = inv_ammocoins[invSelectedIndex].item.name;
                    nudQuant.Value = inv_ammocoins[invSelectedIndex].quantity;
                    cbPrefixes.SelectedItem = inv_ammocoins[invSelectedIndex].prefix.name;
                    titleText = "Coin / Ammo slot " + (invSelectedIndex + 1) + " (" + inv_main[invSelectedIndex].item.name + ")";
                    if (titleText.Length > 40)
                    {
                        titleText = titleText.Substring(0, 40) + "...";
                    }
                    gb_slot_items.Text = titleText;
                    break;

                case 4:
                    playerBuffs[invSelectedIndex].duration = (int)nudDur.Value;
                    nudDur.Value = playerBuffs[invSelectedIndex].duration;
                    cbBuffs.SelectedItem = playerBuffs[invSelectedIndex].buff.name;
                    gb_slot_buff.Text = "Buff slot " + (invSelectedIndex + 1);
                    break;

            }
        }

        void blb_activ(object sender, EventArgs e)
        {
            cbBuffs.SelectedItem = buffLV.SelectedItems[0].Text;
        }
        #endregion
        #region Fields value changed events
        void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 2)
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

        void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 2)
            {
                var results = lvis_buff.Where(x => x.Text.ToLower().Contains(textBox2.Text)).ToList();
                buffLV.Items.Clear();
                foreach (var i in results)
                {
                    buffLV.Items.Add(i);
                }
            }
            else
            {
                buffLV.Items.Clear();
                buffLV.Items.AddRange(lvis_buff.ToArray());
            }
        }

        void nudHair_ValueChanged(object sender, EventArgs e)
        {
            playerHS = (int)nudHair.Value;
        }

        void tbName_TextChanged(object sender, EventArgs e)
        {
            playerName = tbName.Text;
        }
        #endregion

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

        void tabs_selectedChanged(object sender, EventArgs e)
        {
            selectedTab = tcMain.SelectedIndex;
            updateInvDisplay();

            if (tcMain.SelectedIndex == 0)
            {
                toggleFavoriteToolStripMenuItem.Enabled = true;
            }
            else
            {
                toggleFavoriteToolStripMenuItem.Enabled = false;
            }

            if (tcMain.SelectedIndex == 4)
            {
                gbBuffs.BringToFront();
                gb_slot_buff.BringToFront();
            }
            else
            {
                gbItems.BringToFront();
                gb_slot_items.BringToFront();
            }
        }

        public string getContactInfoFromExtServer()
        {
            try
            {
                WebClient w = new WebClient();
                w.Headers.Add("user-agent", "Internal WTE request | " + WTEversion);
                string _tmp = w.DownloadString(@"http://knedit.pw/WTE_Contact_Data_Tmp/");
                if (_tmp[0] == 'W')
                {
                    return _tmp;
                }
                else
                {
                    throw new Exception("corrupt data");
                }
            }
            catch
            {
                return "Unknown server error :(";
            }
        }

        void quant_leaveFocus(object sender, EventArgs e) => Debug.WriteLine("Broke focus");

        void cbGamemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbGamemode.SelectedIndex)
            {
                case 0:
                    playerMode = gamemodes.gamemode.Classic;
                    break;
                case 1:
                    playerMode = gamemodes.gamemode.MediumCore;
                    break;
                case 2:
                    playerMode = gamemodes.gamemode.HardCore;
                    break;
                case 3:
                    playerMode = gamemodes.gamemode.Journey;
                    break;
            }
        }

        #region Menu click events
        void cm_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            Debug.WriteLine((sender as ContextMenuStrip).SourceControl.Name + ":" + item.Text);

            string elementName = (sender as ContextMenuStrip).SourceControl.Name;
            string[] npart = elementName.Split(new string[] { "b" }, StringSplitOptions.None);
            int tmp_invSelectedIndex = Int32.Parse(npart[1]) - 1;
            int tmp_true = tmp_invSelectedIndex;
            if (item.Text == "Copy")
            {
                switch (selectedTab)
                {
                    case 0:
                        //no modification needed, just perform action
                        //main inv
                        copyBuffer = inv_main[tmp_invSelectedIndex];
                        break;

                    case 1:
                        tmp_invSelectedIndex -= 50;
                        //piggybank
                        copyBuffer = inv_piggybank[tmp_invSelectedIndex];
                        break;

                    case 2:
                        tmp_invSelectedIndex -= 90;
                        //safe
                        copyBuffer = inv_safe[tmp_invSelectedIndex];
                        break;

                    case 3:
                        tmp_invSelectedIndex -= 130;
                        //ammo and coins
                        copyBuffer = inv_ammocoins[tmp_invSelectedIndex];
                        break;

                    case 4:
                        //buffs
                        break;
                }
                copyIndex = tmp_true;
                updateInvDisplay();
            }
            if (item.Text == "Paste")
            {
                if (copyBuffer != null)
                {
                    cbItem.SelectedItem = copyBuffer.item.name;
                    cbPrefixes.SelectedItem = copyBuffer.prefix.name;
                    nudQuant.Value = copyBuffer.quantity;
                    updateInvDisplay();
                }
            }
            if (item.Text == "Delete")
            {
                cbItem.SelectedItem = "Empty";
                cbPrefixes.SelectedIndex = 0;
                nudQuant.Value = 0;
            }
            if (item.Text == "Toggle favourite" && tcMain.SelectedIndex == 0)
            {
                inv_main[invSelectedIndex].isFavorite = !inv_main[invSelectedIndex].isFavorite;
                updateInvDisplay();
            }
        }

        void aaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadPlayer();
        }
        void bbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savePlayer();
        }
        void ccToolStripMenuItem_Click(object sender, EventArgs e) => reloadPlayer();
        void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int tmp_invSelectedIndex = invSelectedIndex;
            int tmp_true = tmp_invSelectedIndex;

            switch (selectedTab)
            {
                case 0:
                    //no modification needed, just perform action
                    //main inv
                    copyBuffer = inv_main[invSelectedIndex];
                    break;

                case 1:
                    copyBuffer = inv_piggybank[invSelectedIndex];
                    break;

                case 2:
                    copyBuffer = inv_safe[invSelectedIndex];
                    break;

                case 3:
                    copyBuffer = inv_ammocoins[invSelectedIndex];
                    break;

                case 4:
                    //buffs
                    break;
            }
            copyIndex = trueSelectedIndex;
            updateInvDisplay();
        }
        void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (copyBuffer != null)
            {
                cbItem.SelectedItem = copyBuffer.item.name;
                cbPrefixes.SelectedItem = copyBuffer.prefix.name;
                nudQuant.Value = copyBuffer.quantity;
                updateInvDisplay();
            }
        }
        void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cbItem.SelectedItem = "Empty";
            cbPrefixes.SelectedIndex = 0;
            nudQuant.Value = 0;
        }
        void toggleFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedIndex == 0)
            {
                inv_main[invSelectedIndex].isFavorite = !inv_main[invSelectedIndex].isFavorite;
                updateInvDisplay();
            }
        }
        void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings st = new Settings(useAutoReloadFile, useExtendedName);
            if (st.ShowDialog() == DialogResult.OK)
            {
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
                    if (tbName.Text.Length > 20)
                    {
                        tbName.Text = tbName.Text.Substring(0, 20);
                    }
                }
            }
        }
        void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about ab = new about(aboutBoxContactData, WTEversion);
            ab.ShowDialog();
        }
        void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hexView hx = new hexView(debugInvData, rawDecrypted.ToArray(), nameEndOffset, versionCode);
            hx.ShowDialog();
        }
        #endregion

        #endregion

        void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            doSaveAss = true;
            savePlayer();
        }
    }
}
