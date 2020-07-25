namespace WinTerrEdit
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.cbUseOverwrite = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAutoReload = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbExtendedName = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbUseOverwrite
            // 
            this.cbUseOverwrite.AutoSize = true;
            this.cbUseOverwrite.Location = new System.Drawing.Point(12, 12);
            this.cbUseOverwrite.Name = "cbUseOverwrite";
            this.cbUseOverwrite.Size = new System.Drawing.Size(102, 17);
            this.cbUseOverwrite.TabIndex = 0;
            this.cbUseOverwrite.Text = "Overwrite player";
            this.cbUseOverwrite.UseVisualStyleBackColor = true;
            this.cbUseOverwrite.CheckedChanged += new System.EventHandler(this.cbUseOverwrite_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "Automatically overwrites the origional .PLR file\r\nwhen the \"Save\" button is click" +
    "ed. Does not show\r\nthe save file window.\r\n";
            // 
            // cbAutoReload
            // 
            this.cbAutoReload.AutoSize = true;
            this.cbAutoReload.Location = new System.Drawing.Point(12, 88);
            this.cbAutoReload.Name = "cbAutoReload";
            this.cbAutoReload.Size = new System.Drawing.Size(111, 17);
            this.cbAutoReload.TabIndex = 2;
            this.cbAutoReload.Text = "Auto reload player";
            this.cbAutoReload.UseVisualStyleBackColor = true;
            this.cbAutoReload.CheckedChanged += new System.EventHandler(this.cbAutoReload_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 39);
            this.label2.TabIndex = 3;
            this.label2.Text = "Automatically reloads the player file if it is changed\r\nby terraria. \r\n\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Increases the players name length limit to 200\r\n(terraria accepts 20 as default)";
            // 
            // cbExtendedName
            // 
            this.cbExtendedName.AutoSize = true;
            this.cbExtendedName.Location = new System.Drawing.Point(12, 150);
            this.cbExtendedName.Name = "cbExtendedName";
            this.cbExtendedName.Size = new System.Drawing.Size(121, 17);
            this.cbExtendedName.TabIndex = 4;
            this.cbExtendedName.Text = "Unlock name length";
            this.cbExtendedName.UseVisualStyleBackColor = true;
            this.cbExtendedName.CheckedChanged += new System.EventHandler(this.cbExtendedName_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 213);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbExtendedName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbAutoReload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbUseOverwrite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onFormClose);
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbUseOverwrite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbAutoReload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbExtendedName;
    }
}