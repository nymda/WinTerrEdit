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
        public int setCount = 0;
        public int itemCount = 0;
        public loading(int itemCount)
        {
            InitializeComponent();
            progressBar1.Maximum = itemCount;
            this.itemCount = itemCount;
        }

        public void increase()
        {
            setCount++;
            progressBar1.Value = setCount;
        }
    }
}
