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
        public string version;

        public about(string cont, string version)
        {
            InitializeComponent();
            this.cont = cont;
            this.version = version;
        }

        ToolTip tt = new ToolTip();
        Bitmap discord = new Bitmap(1, 1);

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.gnu.org/licenses/");
        }

        public int r = 255;
        public int g = 0;
        public int b = 0;
        float frequency = 0.1f;

        public Color current = Color.FromArgb(0, 0, 0);

        public Pen tblack = new Pen(Brushes.Black, 2);

        public Graphics tmp;


        private void about_Load(object sender, EventArgs e)
        {
            label2.Text = cont;
            this.Text = "about | Release " + version;
            tt.SetToolTip(pictureBox1, "Join my discord server!");
            discord = (Bitmap)pictureBox1.Image.Clone();
            tmp = Graphics.FromImage(discord);
            discordRainbow.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.com/invite/2xVTgwH");
        }

        public int i = 0;

        private void discordRainbow_Tick(object sender, EventArgs e)
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

            tblack = new Pen(current, 1);
            pictureBox1.Image = discord;
        }

        private void discord_paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(tblack, 0, 0, 44, 44);
        }
    }
}
