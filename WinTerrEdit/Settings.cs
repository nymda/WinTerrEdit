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
        public bool useOverwriteFile { get; set; }
        public bool useAutoReloadFile { get; set; }
        public bool useExtendedName { get; set; }

        public Settings(bool useOverwriteFile, bool useAutoReloadFile, bool useExtendedName)
        {
            InitializeComponent();
            this.useOverwriteFile = useOverwriteFile;
            this.useAutoReloadFile = useAutoReloadFile;
            this.useExtendedName = useExtendedName;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            cbUseOverwrite.Checked = useOverwriteFile;
            cbAutoReload.Checked = useAutoReloadFile;
            cbExtendedName.Checked = useExtendedName;
        }

        private void onFormClose(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cbUseOverwrite_CheckedChanged(object sender, EventArgs e)
        {
            useOverwriteFile = cbUseOverwrite.Checked;
        }

        private void cbAutoReload_CheckedChanged(object sender, EventArgs e)
        {
            useAutoReloadFile = cbAutoReload.Checked;
        }

        private void cbExtendedName_CheckedChanged(object sender, EventArgs e)
        {
            useExtendedName = cbExtendedName.Checked;
        }
    }
}
