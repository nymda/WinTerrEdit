using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class errorReporter : Form
    {
        public string stage { get; set; }
        public string error { get; set; }
        public string plr_b64 { get; set; }

        public errorReporter(string stage, string error, string plr_b64)
        {
            InitializeComponent();
            this.stage = stage;
            this.error = error;
            this.plr_b64 = plr_b64;
        }

        private void errorReporter_Load(object sender, EventArgs e)
        {
            string errorText = String.Format("STAGE: {0}\nERROR: {1}\n\nPLR FILE: {2}", stage, error, plr_b64);
            richTextBox1.Text = errorText;

            int stageStart = 0;
            int errorStart = richTextBox1.Find("ERROR:");
            int plrStart = richTextBox1.Find("PLR FILE:");

            richTextBox1.Select(stageStart, 6);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);

            richTextBox1.Select(errorStart, 6);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);

            richTextBox1.Select(plrStart, 9);
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();
            var reqparm = new System.Collections.Specialized.NameValueCollection();
            reqparm.Add("stage", stage);
            reqparm.Add("error", error);
            reqparm.Add("b64", plr_b64);
            byte[] response = wc.UploadValues("http://knedit.pw/WTE_Bugs/report.php", "POST", reqparm);
            string responsebody = Encoding.UTF8.GetString(response);

            if(responsebody == "OK")
            {
                button1.Enabled = false;
                label2.Text = "Report sent.";
            }
        }
    }
}
