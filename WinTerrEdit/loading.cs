using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTerrEdit
{

    public partial class loading : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse );

        public string str = "Loading assets, please wait";
        public string add = "";
        int count = 0;

        public Pen tblack = new Pen(Brushes.Black, 2);
        public loading()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
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

        //  ,adPPYba,  ,adPPYba,  8b,dPPYba, 8b,dPPYba,   
        // a8"     "" a8"     "8a 88P'   "Y8 88P'   `"8a  
        // 8b         8b       d8 88         88       88  
        // "8a,   ,aa "8a,   ,a8" 88         88       88  
        //  `"Ybbd8"'  `"YbbdP"'  88         88       88  

        public Bitmap getCorn()
        {
            Bitmap corn = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(corn);
            g.DrawRectangle(Pens.Black, 3, 3, 1, 1);
            g.DrawLine(Pens.Black, 2, 0, 2, 9);
            g.DrawLine(Pens.Black, 0, 2, 9, 2);
            g.DrawLine(Pens.Black, 3, 5, 3, 6);
            g.DrawLine(Pens.Black, 5, 3, 6, 3);
            return corn;
        }

        private void onPaint(object sender, PaintEventArgs e)
        {
            //what... is this...
            e.Graphics.DrawRectangle(tblack, 1, 1, this.Width - 3 , this.Height - 3);
            e.Graphics.DrawRectangle(Pens.Black, 3, 3, 1, 1);
            Bitmap corn = getCorn();
            e.Graphics.DrawImage(corn, 0, 0);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipX);
            e.Graphics.DrawImage(corn, -1, 0);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipY);
            e.Graphics.DrawImage(corn, -1, -1);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipX);
            e.Graphics.DrawImage(corn, 0, -1);

        }
    }
}
