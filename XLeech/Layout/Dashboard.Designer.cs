namespace XLeech
{
    partial class Dashboard
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
            StatusTb = new RichTextBox();
            SuspendLayout();
            // 
            // StatusTb
            // 
            StatusTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            StatusTb.Location = new Point(3, 3);
            StatusTb.Name = "StatusTb";
            StatusTb.Size = new Size(960, 567);
            StatusTb.TabIndex = 2;
            StatusTb.Text = "";
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(StatusTb);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Dashboard";
            Size = new Size(966, 573);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox StatusTb;
    }
}
