namespace WinTerrEdit
{
    partial class hexView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(hexView));
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbStr = new System.Windows.Forms.RadioButton();
            this.cbConstSize = new System.Windows.Forms.CheckBox();
            this.rbInv = new System.Windows.Forms.RadioButton();
            this.rbHyb = new System.Windows.Forms.RadioButton();
            this.tbOut = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // rbInt
            // 
            this.rbInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbInt.AutoSize = true;
            this.rbInt.Checked = true;
            this.rbInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbInt.Location = new System.Drawing.Point(12, 554);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(43, 17);
            this.rbInt.TabIndex = 2;
            this.rbInt.TabStop = true;
            this.rbInt.Text = "INT";
            this.rbInt.UseVisualStyleBackColor = true;
            this.rbInt.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbHex
            // 
            this.rbHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbHex.AutoSize = true;
            this.rbHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHex.Location = new System.Drawing.Point(62, 554);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(47, 17);
            this.rbHex.TabIndex = 3;
            this.rbHex.Text = "HEX";
            this.rbHex.UseVisualStyleBackColor = true;
            this.rbHex.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbStr
            // 
            this.rbStr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbStr.AutoSize = true;
            this.rbStr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbStr.Location = new System.Drawing.Point(112, 554);
            this.rbStr.Name = "rbStr";
            this.rbStr.Size = new System.Drawing.Size(47, 17);
            this.rbStr.TabIndex = 4;
            this.rbStr.Text = "STR";
            this.rbStr.UseVisualStyleBackColor = true;
            this.rbStr.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // cbConstSize
            // 
            this.cbConstSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConstSize.AutoSize = true;
            this.cbConstSize.Checked = true;
            this.cbConstSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConstSize.Location = new System.Drawing.Point(432, 554);
            this.cbConstSize.Name = "cbConstSize";
            this.cbConstSize.Size = new System.Drawing.Size(80, 17);
            this.cbConstSize.TabIndex = 5;
            this.cbConstSize.Text = "Explicit size";
            this.cbConstSize.UseVisualStyleBackColor = true;
            this.cbConstSize.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbInv
            // 
            this.rbInv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbInv.AutoSize = true;
            this.rbInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbInv.Location = new System.Drawing.Point(212, 554);
            this.rbInv.Name = "rbInv";
            this.rbInv.Size = new System.Drawing.Size(43, 17);
            this.rbInv.TabIndex = 7;
            this.rbInv.Text = "INV";
            this.rbInv.UseVisualStyleBackColor = true;
            this.rbInv.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbHyb
            // 
            this.rbHyb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbHyb.AutoSize = true;
            this.rbHyb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHyb.Location = new System.Drawing.Point(162, 554);
            this.rbHyb.Name = "rbHyb";
            this.rbHyb.Size = new System.Drawing.Size(47, 17);
            this.rbHyb.TabIndex = 8;
            this.rbHyb.Text = "HYB";
            this.rbHyb.UseVisualStyleBackColor = true;
            this.rbHyb.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // tbOut
            // 
            this.tbOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOut.Location = new System.Drawing.Point(12, 12);
            this.tbOut.Name = "tbOut";
            this.tbOut.Size = new System.Drawing.Size(500, 538);
            this.tbOut.TabIndex = 9;
            this.tbOut.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(351, 554);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 17);
            this.button1.TabIndex = 10;
            this.button1.Text = "highlight";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(261, 556);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(31, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Copy";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.linkLabel2.Location = new System.Drawing.Point(290, 556);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(55, 13);
            this.linkLabel2.TabIndex = 12;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Save as...";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // hexView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 580);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbOut);
            this.Controls.Add(this.rbHyb);
            this.Controls.Add(this.rbInv);
            this.Controls.Add(this.cbConstSize);
            this.Controls.Add(this.rbStr);
            this.Controls.Add(this.rbHex);
            this.Controls.Add(this.rbInt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "hexView";
            this.Text = "Debug";
            this.Load += new System.EventHandler(this.hexView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        System.Windows.Forms.RadioButton rbInt;
        System.Windows.Forms.RadioButton rbHex;
        System.Windows.Forms.RadioButton rbStr;
        System.Windows.Forms.CheckBox cbConstSize;
        System.Windows.Forms.RadioButton rbInv;
        System.Windows.Forms.RadioButton rbHyb;
        System.Windows.Forms.RichTextBox tbOut;
        System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}