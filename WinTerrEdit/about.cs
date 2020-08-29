using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class about : Form
    {
        public string cont;

        public about(string cont)
        {
            InitializeComponent();
            this.cont = cont;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.gnu.org/licenses/");
        }

        private void about_Load(object sender, EventArgs e)
        {
            label2.Text = cont;
        }
    }
}
