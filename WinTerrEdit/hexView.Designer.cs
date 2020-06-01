namespace WinTerrEdit
{
    partial class hexView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(hexView));
            this.tbOut = new System.Windows.Forms.TextBox();
            this.rbInt = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbStr = new System.Windows.Forms.RadioButton();
            this.cbConstSize = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbOut
            // 
            this.tbOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOut.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOut.Location = new System.Drawing.Point(12, 12);
            this.tbOut.Multiline = true;
            this.tbOut.Name = "tbOut";
            this.tbOut.ReadOnly = true;
            this.tbOut.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOut.Size = new System.Drawing.Size(500, 541);
            this.tbOut.TabIndex = 1;
            // 
            // rbInt
            // 
            this.rbInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbInt.AutoSize = true;
            this.rbInt.Checked = true;
            this.rbInt.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbInt.Location = new System.Drawing.Point(12, 559);
            this.rbInt.Name = "rbInt";
            this.rbInt.Size = new System.Drawing.Size(44, 15);
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
            this.rbHex.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbHex.Location = new System.Drawing.Point(62, 559);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(44, 15);
            this.rbHex.TabIndex = 3;
            this.rbHex.Text = "HEX";
            this.rbHex.UseVisualStyleBackColor = true;
            this.rbHex.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbStr
            // 
            this.rbStr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbStr.AutoSize = true;
            this.rbStr.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbStr.Location = new System.Drawing.Point(112, 559);
            this.rbStr.Name = "rbStr";
            this.rbStr.Size = new System.Drawing.Size(44, 15);
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
            this.cbConstSize.Location = new System.Drawing.Point(432, 557);
            this.cbConstSize.Name = "cbConstSize";
            this.cbConstSize.Size = new System.Drawing.Size(80, 17);
            this.cbConstSize.TabIndex = 5;
            this.cbConstSize.Text = "Explicit size";
            this.cbConstSize.UseVisualStyleBackColor = true;
            this.cbConstSize.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // hexView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 583);
            this.Controls.Add(this.cbConstSize);
            this.Controls.Add(this.rbStr);
            this.Controls.Add(this.rbHex);
            this.Controls.Add(this.rbInt);
            this.Controls.Add(this.tbOut);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "hexView";
            this.Text = "hexView";
            this.Load += new System.EventHandler(this.hexView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbOut;
        private System.Windows.Forms.RadioButton rbInt;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbStr;
        private System.Windows.Forms.CheckBox cbConstSize;
    }
}