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
        public about()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.gnu.org/licenses/");
        }

        public string obtainContactInfoFromWeb()
        {
            using(WebClient w = new WebClient())
            {
                string contactInfo = Encoding.UTF8.GetString(w.DownloadData(@"http://knedit.pw/WTE_Contact_Data"));
                contactInfo = contactInfo.Split(new string[] { "##" }, StringSplitOptions.None)[0];
                return contactInfo;
            }
        }

        private void about_Load(object sender, EventArgs e)
        {
            try
            {
                label2.Text = obtainContactInfoFromWeb();
            }
            catch
            {
                label2.Text = "Could not contact server :(";
            }
        }
    }
}
