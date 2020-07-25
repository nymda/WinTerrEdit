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

        public int r = 255;
        public int g = 0;
        public int b = 0;
        float frequency = 0.1f;

        public Color current = Color.FromArgb(0, 0, 0);

        public Pen tblack = new Pen(Brushes.Black, 2);

        public Graphics tmp;
        public loading()
        {
            InitializeComponent();
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void loading_Load(object sender, EventArgs e)
        {
            tmp = this.CreateGraphics();
            timer1.Start();
            timer2.Start();
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

        //  ,adPPYba,    ,adPPYba,    8b,dPPYba,   8b,dPPYba,   
        // a8"     ""   a8"     "8a   88P'   "Y8   88P'   `"8a  
        // 8b           8b       d8   88           88       88  
        // "8a,   ,aa   "8a,   ,a8"   88           88       88  
        //  `"Ybbd8"'    `"YbbdP"'    88           88       88  

        public Bitmap getCorn()
        {
            Pen thblack = new Pen(current);
            Bitmap corn = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(corn);
            g.DrawRectangle(thblack, 3, 3, 1, 1);
            g.DrawLine(thblack, 2, 0, 2, 9);
            g.DrawLine(thblack, 0, 2, 9, 2);
            g.DrawLine(thblack, 3, 5, 3, 6);
            g.DrawLine(thblack, 5, 3, 6, 3);
            return corn;
        }

        public int i = -32;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (i == 32)
            {
                i = -32;
            }

            i++;

            r = (int)Math.Round(Math.Sin((frequency * i) + 0) * 127 + 128);
            g = (int)Math.Round(Math.Sin((frequency * i) + 2) * 127 + 128);
            b = (int)Math.Round(Math.Sin((frequency * i) + 4) * 127 + 128);
            current = Color.FromArgb(r, g, b);

            tblack = new Pen(current, 2);

            tmp.DrawRectangle(tblack, 1, 1, this.Width - 3, this.Height - 3);
            tmp.DrawRectangle(Pens.Black, 3, 3, 1, 1);
            Bitmap corn = getCorn();
            tmp.DrawImage(corn, 0, 0);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipX);
            tmp.DrawImage(corn, -1, 0);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipY);
            tmp.DrawImage(corn, -1, -1);
            corn.RotateFlip(RotateFlipType.RotateNoneFlipX);
            tmp.DrawImage(corn, 0, -1);
        }
    }
}
