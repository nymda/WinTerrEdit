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
    public partial class saveNotifier : Form
    {
        public saveNotifier()
        {
            InitializeComponent();
        }

        void saveNotifier_Load(object sender, EventArgs e)
        {

        }

        void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
