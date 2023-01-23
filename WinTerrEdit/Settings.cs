using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class Settings : Form
    {
        public struct SettingsStruct {
            public bool useAutoReloadFile;
            public bool useExtendedName;
            public SettingsStruct(bool useARF, bool useEN) {
                useAutoReloadFile = useARF;
                useExtendedName = useEN;
            }
        }

        SettingsStruct settingsStruct;
        public SettingsStruct st { get => settingsStruct; }

        public Settings(bool useAutoReloadFile, bool useExtendedName) {
            InitializeComponent();
            settingsStruct.useAutoReloadFile = useAutoReloadFile;
            settingsStruct.useExtendedName = useExtendedName;
        }

        void Settings_Load(object sender, EventArgs e) {
            cbAutoReload.Checked = settingsStruct.useAutoReloadFile;
            cbExtendedName.Checked = settingsStruct.useExtendedName;
        }

        void onFormClose(object sender, FormClosedEventArgs e) {
            this.DialogResult = DialogResult.OK;
        }

        void cbAutoReload_CheckedChanged(object sender, EventArgs e) {
            settingsStruct.useAutoReloadFile = cbAutoReload.Checked;
        }

        void cbExtendedName_CheckedChanged(object sender, EventArgs e) {
            settingsStruct.useExtendedName = cbExtendedName.Checked;
        }
    }
}
