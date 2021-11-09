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
        public bool useAutoReloadFile { get; set; }
        public bool useExtendedName { get; set; }

        public Settings(bool useAutoReloadFile, bool useExtendedName)
        {
            InitializeComponent();
            this.useAutoReloadFile = useAutoReloadFile;
            this.useExtendedName = useExtendedName;
        }

        void Settings_Load(object sender, EventArgs e)
        {
            cbAutoReload.Checked = useAutoReloadFile;
            cbExtendedName.Checked = useExtendedName;
        }

        void onFormClose(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        void cbAutoReload_CheckedChanged(object sender, EventArgs e)
        {
            useAutoReloadFile = cbAutoReload.Checked;
        }

        void cbExtendedName_CheckedChanged(object sender, EventArgs e)
        {
            useExtendedName = cbExtendedName.Checked;
        }
    }
}
