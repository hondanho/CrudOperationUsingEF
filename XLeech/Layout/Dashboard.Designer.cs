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
            this.StatusTb = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Sitelb = new System.Windows.Forms.Label();
            this.PostSuccessLb = new System.Windows.Forms.Label();
            this.PostFailedLb = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.ReCrawleBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StatusTb
            // 
            this.StatusTb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTb.Location = new System.Drawing.Point(-3, 31);
            this.StatusTb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StatusTb.Name = "StatusTb";
            this.StatusTb.Size = new System.Drawing.Size(905, 438);
            this.StatusTb.TabIndex = 2;
            this.StatusTb.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Site  Crawle:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Post Success:";
            // 
            // Sitelb
            // 
            this.Sitelb.AutoSize = true;
            this.Sitelb.Location = new System.Drawing.Point(71, 10);
            this.Sitelb.Name = "Sitelb";
            this.Sitelb.Size = new System.Drawing.Size(13, 15);
            this.Sitelb.TabIndex = 5;
            this.Sitelb.Text = "0";
            this.Sitelb.Click += new System.EventHandler(this.Sitelb_Click);
            // 
            // PostSuccessLb
            // 
            this.PostSuccessLb.AutoSize = true;
            this.PostSuccessLb.Location = new System.Drawing.Point(228, 10);
            this.PostSuccessLb.Name = "PostSuccessLb";
            this.PostSuccessLb.Size = new System.Drawing.Size(13, 15);
            this.PostSuccessLb.TabIndex = 6;
            this.PostSuccessLb.Text = "0";
            // 
            // PostFailedLb
            // 
            this.PostFailedLb.AutoSize = true;
            this.PostFailedLb.Location = new System.Drawing.Point(327, 10);
            this.PostFailedLb.Name = "PostFailedLb";
            this.PostFailedLb.Size = new System.Drawing.Size(13, 15);
            this.PostFailedLb.TabIndex = 8;
            this.PostFailedLb.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(262, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "Post Failed:";
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(3, 474);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 9;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(176, 474);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(75, 23);
            this.StopBtn.TabIndex = 10;
            this.StopBtn.Text = "Stop";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // ReCrawleBtn
            // 
            this.ReCrawleBtn.Location = new System.Drawing.Point(87, 474);
            this.ReCrawleBtn.Name = "ReCrawleBtn";
            this.ReCrawleBtn.Size = new System.Drawing.Size(83, 23);
            this.ReCrawleBtn.TabIndex = 11;
            this.ReCrawleBtn.Text = "ReCrawle All";
            this.ReCrawleBtn.UseVisualStyleBackColor = true;
            this.ReCrawleBtn.Click += new System.EventHandler(this.ReCrawleBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Site:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(125, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "0";
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ReCrawleBtn);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.PostFailedLb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.PostSuccessLb);
            this.Controls.Add(this.Sitelb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StatusTb);
            this.Name = "Dashboard";
            this.Size = new System.Drawing.Size(905, 500);
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox StatusTb;
        private Label label1;
        private Label label2;
        private Label Sitelb;
        private Label PostSuccessLb;
        private Label PostFailedLb;
        private Label label6;
        private Button StartBtn;
        private Button StopBtn;
        private Button ReCrawleBtn;
        private Label label3;
        private Label label4;
    }
}
