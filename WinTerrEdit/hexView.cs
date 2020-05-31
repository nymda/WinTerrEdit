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
        public hexView(byte[] data)
        {
            InitializeComponent();
            this.data = data;
        }
        private void hexView_Load(object sender, EventArgs e)
        {
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
                output = BitConverter.ToString(data);
            }
            if (rbStr.Checked)
            {
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
