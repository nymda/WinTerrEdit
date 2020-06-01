using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{
    public partial class hexView : Form
    {
        public byte[] data;
        public string output;
        public int NEO;
        public hexView(byte[] data, int NEO)
        {
            InitializeComponent();
            this.data = data;
            this.NEO = NEO;
        }
        private void hexView_Load(object sender, EventArgs e)
        {
            this.Text = "hexView | NEO @ " + NEO;
            update();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            update();
        }

        public void update()
        {
            if (rbInt.Checked)
            {
                cbConstSize.Enabled = true;
                if (!cbConstSize.Checked)
                {
                    output = string.Join(",", data);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach(byte b in data)
                    {
                        string tmp = ((int)b).ToString();
                        int neg = 3 - tmp.Length;
                        if(neg > 0)
                        {
                            sb.Append('0', neg);
                        }
                        sb.Append(tmp);
                        sb.Append(",");
                    }
                    output = sb.ToString();
                }
            }
            if (rbHex.Checked)
            {
                cbConstSize.Enabled = false;
                output = BitConverter.ToString(data);
            }
            if (rbStr.Checked)
            {
                cbConstSize.Enabled = false;
                StringBuilder sb = new StringBuilder();
                List<Char> tmpChar = new List<Char> { };
                foreach(byte b in data)
                {
                    char tmp = Encoding.UTF8.GetString(new byte[] { b })[0];
                    if (!char.IsControl(tmp))
                    {
                        tmpChar.Add(tmp);
                    }
                    else
                    {
                        tmpChar.Add('?');
                    }
                }
                output = string.Join("", tmpChar);
            }
            tbOut.Text = output;
        }
    }
}
