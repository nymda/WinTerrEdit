﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        List<List<int>> invDat = new List<List<int>> { };
        public int versionCode;
        public hexView(List<List<int>> invDat, byte[] data, int NEO, int versionCode)
        {
            InitializeComponent();
            this.data = data;
            this.NEO = NEO;
            this.invDat = invDat;
            this.versionCode = versionCode;
        }
        void hexView_Load(object sender, EventArgs e)
        {
            this.Text = "| NEO " + NEO + " | VERS " + versionCode + " |";
            update();
        }

        void rb_CheckedChanged(object sender, EventArgs e)
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
                        var chars = "\\x" + Convert.ToByte(tmp).ToString("x2");
                        foreach (var itm in chars)
                            tmpChar.Add(itm);
                    }
                }
                output = string.Join("", tmpChar);
            }
            if (rbInv.Checked)
            {
                if (cbConstSize.Checked)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (List<int> i in invDat)
                    {
                        foreach (int b in i)
                        {
                            string tmp = b.ToString();
                            int neg = 3 - tmp.Length;
                            if (neg > 0){ sb.Append('0', neg); }
                            sb.Append(tmp);
                            sb.Append(",");
                        }
                        sb.Append(Environment.NewLine);
                    }
                    output = sb.ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (List<int> i in invDat)
                    {
                        sb.AppendLine(string.Join(",", i));
                    }
                    output = sb.ToString();
                }
            }
            if (rbHyb.Checked)
            {
                byte[] printables = new byte[] { (byte)'a', (byte)'b', (byte)'c', (byte)'d', (byte)'e', (byte)'f', (byte)'g', (byte)'h', (byte)'i', (byte)'j', (byte)'k', (byte)'l', (byte)'m', (byte)'n', (byte)'o', (byte)'p', (byte)'q', (byte)'r', (byte)'s', (byte)'t', (byte)'u', (byte)'v', (byte)'w', (byte)'x', (byte)'y', (byte)'z', (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H', (byte)'I', (byte)'J', (byte)'K', (byte)'L', (byte)'M', (byte)'N', (byte)'O', (byte)'P', (byte)'Q', (byte)'R', (byte)'S', (byte)'T', (byte)'U', (byte)'V', (byte)'W', (byte)'X', (byte)'Y', (byte)'Z' };
                if (cbConstSize.Checked)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in data)
                    {
                        if (printables.Contains(b))
                        {
                            sb.Append("_" + (char)b + "_,");
                        }
                        else
                        {
                            string tmpStr = b.ToString();
                            int neg = 3 - tmpStr.Length;
                            if (neg > 0) { sb.Append('0', neg); }
                            sb.Append(tmpStr);
                            sb.Append(",");
                        }
                    }
                    output = sb.ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in data)
                    {
                        if (printables.Contains(b))
                        {
                            sb.Append((char)b);
                        }
                        else
                        {
                            sb.Append(b);
                        }
                        sb.Append(',');
                    }
                    output = sb.ToString();
                }
            }
            tbOut.Text = output;
        }

        public void colourSections()
        {
            string[] data = tbOut.Text.Split(',');
            int point = 0;
            foreach(string s in data)
            {
                try
                {
                    tbOut.Select(point, 4);
                    tbOut.SelectionColor = Color.FromArgb(0, Int32.Parse(s), 0, 0);
                }
                catch
                {

                }
                point += 4;
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            colourSections();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Native.UserInterface.SetClipboardText(tbOut.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(saveFileDialog1.FileName, tbOut.Text);
            }
        }
    }
}
