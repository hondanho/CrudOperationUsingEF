namespace XAutoLeech
{
    partial class AddNew
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.siteNamTxb = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.cancleBtn = new System.Windows.Forms.Button();
            this.noteTab = new System.Windows.Forms.TabPage();
            this.postTab = new System.Windows.Forms.TabPage();
            this.categoryTab = new System.Windows.Forms.TabPage();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.mainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // siteNamTxb
            // 
            this.siteNamTxb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.siteNamTxb.Location = new System.Drawing.Point(0, 0);
            this.siteNamTxb.Name = "siteNamTxb";
            this.siteNamTxb.PlaceholderText = "Enter site name here";
            this.siteNamTxb.Size = new System.Drawing.Size(917, 23);
            this.siteNamTxb.TabIndex = 2;
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveBtn.Location = new System.Drawing.Point(761, 483);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 3;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            // 
            // cancleBtn
            // 
            this.cancleBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancleBtn.Location = new System.Drawing.Point(842, 483);
            this.cancleBtn.Name = "cancleBtn";
            this.cancleBtn.Size = new System.Drawing.Size(75, 23);
            this.cancleBtn.TabIndex = 4;
            this.cancleBtn.Text = "Cancle";
            this.cancleBtn.UseVisualStyleBackColor = true;
            // 
            // noteTab
            // 
            this.noteTab.Location = new System.Drawing.Point(4, 24);
            this.noteTab.Name = "noteTab";
            this.noteTab.Padding = new System.Windows.Forms.Padding(3);
            this.noteTab.Size = new System.Drawing.Size(909, 416);
            this.noteTab.TabIndex = 3;
            this.noteTab.Text = "Note";
            this.noteTab.UseVisualStyleBackColor = true;
            // 
            // postTab
            // 
            this.postTab.Location = new System.Drawing.Point(4, 24);
            this.postTab.Name = "postTab";
            this.postTab.Padding = new System.Windows.Forms.Padding(3);
            this.postTab.Size = new System.Drawing.Size(909, 416);
            this.postTab.TabIndex = 2;
            this.postTab.Text = "Post";
            this.postTab.UseVisualStyleBackColor = true;
            // 
            // categoryTab
            // 
            this.categoryTab.Location = new System.Drawing.Point(4, 24);
            this.categoryTab.Name = "categoryTab";
            this.categoryTab.Padding = new System.Windows.Forms.Padding(3);
            this.categoryTab.Size = new System.Drawing.Size(909, 416);
            this.categoryTab.TabIndex = 1;
            this.categoryTab.Text = "Category";
            this.categoryTab.UseVisualStyleBackColor = true;
            // 
            // mainTab
            // 
            this.mainTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTab.Controls.Add(this.categoryTab);
            this.mainTab.Controls.Add(this.postTab);
            this.mainTab.Controls.Add(this.noteTab);
            this.mainTab.Location = new System.Drawing.Point(0, 32);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(917, 444);
            this.mainTab.TabIndex = 1;
            // 
            // AddNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancleBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.siteNamTxb);
            this.Controls.Add(this.mainTab);
            this.Name = "AddNew";
            this.Size = new System.Drawing.Size(917, 506);
            this.mainTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox siteNamTxb;
        private Button saveBtn;
        private Button cancleBtn;
        private TabPage noteTab;
        private TabPage postTab;
        private TabPage categoryTab;
        private TabControl mainTab;
    }
}
