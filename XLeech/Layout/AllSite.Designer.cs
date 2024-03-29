namespace XLeech
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
            dataGridView = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            SiteTitle = new DataGridViewTextBoxColumn();
            ActiveForScheduling = new DataGridViewTextBoxColumn();
            UseProxy = new DataGridViewTextBoxColumn();
            LatestRun = new DataGridViewTextBoxColumn();
            TimeInterval = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, SiteTitle, ActiveForScheduling, UseProxy, LatestRun, TimeInterval });
            dataGridView.Location = new Point(0, 0);
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.RowTemplate.Height = 24;
            dataGridView.Size = new Size(749, 568);
            dataGridView.TabIndex = 12;
            dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "Id";
            Id.MinimumWidth = 6;
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Width = 125;
            // 
            // SiteTitle
            // 
            SiteTitle.DataPropertyName = "Title";
            SiteTitle.HeaderText = "Title";
            SiteTitle.MinimumWidth = 6;
            SiteTitle.Name = "SiteTitle";
            SiteTitle.ReadOnly = true;
            SiteTitle.Width = 125;
            // 
            // ActiveForScheduling
            // 
            ActiveForScheduling.DataPropertyName = "ActiveForScheduling";
            ActiveForScheduling.HeaderText = "Active For Scheduling";
            ActiveForScheduling.MinimumWidth = 200;
            ActiveForScheduling.Name = "ActiveForScheduling";
            ActiveForScheduling.ReadOnly = true;
            ActiveForScheduling.Width = 200;
            // 
            // UseProxy
            // 
            UseProxy.DataPropertyName = "UseProxy";
            UseProxy.HeaderText = "Use Proxy";
            UseProxy.MinimumWidth = 6;
            UseProxy.Name = "UseProxy";
            UseProxy.ReadOnly = true;
            UseProxy.Width = 125;
            // 
            // LatestRun
            // 
            LatestRun.DataPropertyName = "LatestRun";
            LatestRun.HeaderText = "Latest Run";
            LatestRun.MinimumWidth = 6;
            LatestRun.Name = "LatestRun";
            LatestRun.ReadOnly = true;
            LatestRun.Width = 125;
            // 
            // TimeInterval
            // 
            TimeInterval.DataPropertyName = "TimeInterval";
            TimeInterval.HeaderText = "Time Interval";
            TimeInterval.MinimumWidth = 6;
            TimeInterval.Name = "TimeInterval";
            TimeInterval.ReadOnly = true;
            TimeInterval.Width = 125;
            // 
            // AllSite
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGridView);
            Margin = new Padding(3, 4, 3, 4);
            Name = "AllSite";
            Size = new Size(749, 568);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn SiteTitle;
        private DataGridViewTextBoxColumn ActiveForScheduling;
        private DataGridViewTextBoxColumn UseProxy;
        private DataGridViewTextBoxColumn LatestRun;
        private DataGridViewTextBoxColumn TimeInterval;
    }
}
