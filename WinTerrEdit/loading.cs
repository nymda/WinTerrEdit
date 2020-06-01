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
    public partial class loading : Form
    {
        public string str = "Loading assets";
        public string add = "";
        int count = 0;
        public loading(int itemCount)
        {
            InitializeComponent();
        }

        private void loading_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(count == 4)
            {
                count = 0;
            }
            StringBuilder tmp = new StringBuilder();
            tmp.Append(str);
            if(count > 0)
            {
                tmp.Append('.', count);
            }
            label1.Text = tmp.ToString();
            count++;
        }
    }
}
