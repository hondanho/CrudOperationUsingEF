namespace XAutoLeech
{
    partial class AllSite
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.SiteTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActiveForScheduling = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UseProxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LatestRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SiteTitle,
            this.ActiveForScheduling,
            this.UseProxy,
            this.LatestRun,
            this.TimeInterval});
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(655, 426);
            this.dataGridView.TabIndex = 12;
            // 
            // SiteTitle
            // 
            this.SiteTitle.DataPropertyName = "SiteTitle";
            this.SiteTitle.HeaderText = "Title";
            this.SiteTitle.MinimumWidth = 6;
            this.SiteTitle.Name = "SiteTitle";
            this.SiteTitle.ReadOnly = true;
            this.SiteTitle.Width = 125;
            // 
            // ActiveForScheduling
            // 
            this.ActiveForScheduling.DataPropertyName = "ActiveForScheduling";
            this.ActiveForScheduling.HeaderText = "Active For Scheduling";
            this.ActiveForScheduling.MinimumWidth = 8;
            this.ActiveForScheduling.Name = "ActiveForScheduling";
            this.ActiveForScheduling.ReadOnly = true;
            this.ActiveForScheduling.Width = 125;
            // 
            // UseProxy
            // 
            this.UseProxy.DataPropertyName = "UseProxy";
            this.UseProxy.HeaderText = "Use Proxy";
            this.UseProxy.MinimumWidth = 6;
            this.UseProxy.Name = "UseProxy";
            this.UseProxy.ReadOnly = true;
            this.UseProxy.Width = 125;
            // 
            // LatestRun
            // 
            this.LatestRun.DataPropertyName = "LatestRun";
            this.LatestRun.HeaderText = "Latest Run";
            this.LatestRun.MinimumWidth = 6;
            this.LatestRun.Name = "LatestRun";
            this.LatestRun.ReadOnly = true;
            this.LatestRun.Width = 125;
            // 
            // TimeInterval
            // 
            this.TimeInterval.HeaderText = "Time Interval";
            this.TimeInterval.Name = "TimeInterval";
            this.TimeInterval.ReadOnly = true;
            // 
            // AllSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView);
            this.Name = "AllSite";
            this.Size = new System.Drawing.Size(655, 426);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn SiteTitle;
        private DataGridViewTextBoxColumn ActiveForScheduling;
        private DataGridViewTextBoxColumn UseProxy;
        private DataGridViewTextBoxColumn LatestRun;
        private DataGridViewTextBoxColumn TimeInterval;
    }
}
