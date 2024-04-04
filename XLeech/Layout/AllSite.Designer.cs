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
            components = new System.ComponentModel.Container();
            dataGridView = new DataGridView();
            contextMenuStripGrid = new ContextMenuStrip(components);
            deleteToolStripMenuItem1 = new ToolStripMenuItem();
            editToolStripMenuItem1 = new ToolStripMenuItem();
            Id = new DataGridViewTextBoxColumn();
            SiteTitle = new DataGridViewTextBoxColumn();
            ActiveForScheduling1 = new DataGridViewTextBoxColumn();
            IsDone = new DataGridViewTextBoxColumn();
            CategoryNextPageURL = new DataGridViewTextBoxColumn();
            IsCrawlePageUrlShow = new DataGridViewTextBoxColumn();
            UseProxy = new DataGridViewTextBoxColumn();
            TimeInterval = new DataGridViewTextBoxColumn();
            ActiveForScheduling = new DataGridViewTextBoxColumn();
            IsPageUrl = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            contextMenuStripGrid.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Id, SiteTitle, ActiveForScheduling1, IsDone, CategoryNextPageURL, IsCrawlePageUrlShow, UseProxy, TimeInterval, ActiveForScheduling, IsPageUrl });
            dataGridView.ContextMenuStrip = contextMenuStripGrid;
            dataGridView.Location = new Point(0, 0);
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.RowTemplate.Height = 24;
            dataGridView.Size = new Size(1170, 568);
            dataGridView.TabIndex = 12;
            dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.CellMouseUp += dataGridView_CellMouseUp;
            // 
            // contextMenuStripGrid
            // 
            contextMenuStripGrid.ImageScalingSize = new Size(20, 20);
            contextMenuStripGrid.Items.AddRange(new ToolStripItem[] { deleteToolStripMenuItem1, editToolStripMenuItem1 });
            contextMenuStripGrid.Name = "contextMenuStrip1";
            contextMenuStripGrid.Size = new Size(123, 52);
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.Size = new Size(122, 24);
            deleteToolStripMenuItem1.Text = "Delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // editToolStripMenuItem1
            // 
            editToolStripMenuItem1.Name = "editToolStripMenuItem1";
            editToolStripMenuItem1.Size = new Size(122, 24);
            editToolStripMenuItem1.Text = "Edit";
            editToolStripMenuItem1.Click += editToolStripMenuItem1_Click;
            // 
            // Id
            // 
            Id.DataPropertyName = "Id";
            Id.HeaderText = "Id";
            Id.MinimumWidth = 50;
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Width = 50;
            // 
            // SiteTitle
            // 
            SiteTitle.DataPropertyName = "Name";
            SiteTitle.HeaderText = "Name";
            SiteTitle.MinimumWidth = 6;
            SiteTitle.Name = "SiteTitle";
            SiteTitle.ReadOnly = true;
            SiteTitle.Width = 125;
            // 
            // ActiveForScheduling1
            // 
            ActiveForScheduling1.DataPropertyName = "ActiveForScheduling1";
            ActiveForScheduling1.HeaderText = "Active For Scheduling";
            ActiveForScheduling1.MinimumWidth = 135;
            ActiveForScheduling1.Name = "ActiveForScheduling1";
            ActiveForScheduling1.ReadOnly = true;
            ActiveForScheduling1.Width = 135;
            // 
            // IsDone
            // 
            IsDone.DataPropertyName = "IsDone";
            IsDone.HeaderText = "Is Done";
            IsDone.MinimumWidth = 6;
            IsDone.Name = "IsDone";
            IsDone.ReadOnly = true;
            IsDone.Width = 125;
            // 
            // CategoryNextPageURL
            // 
            CategoryNextPageURL.DataPropertyName = "CategoryNextPageURL";
            CategoryNextPageURL.HeaderText = "Category Next Page URL";
            CategoryNextPageURL.MinimumWidth = 250;
            CategoryNextPageURL.Name = "CategoryNextPageURL";
            CategoryNextPageURL.ReadOnly = true;
            CategoryNextPageURL.Width = 250;
            // 
            // IsCrawlePageUrlShow
            // 
            IsCrawlePageUrlShow.HeaderText = "Crawle Type";
            IsCrawlePageUrlShow.MinimumWidth = 125;
            IsCrawlePageUrlShow.Name = "IsCrawlePageUrlShow";
            IsCrawlePageUrlShow.ReadOnly = true;
            IsCrawlePageUrlShow.Width = 125;
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
            // TimeInterval
            // 
            TimeInterval.DataPropertyName = "TimeInterval";
            TimeInterval.HeaderText = "Time Interval";
            TimeInterval.MinimumWidth = 50;
            TimeInterval.Name = "TimeInterval";
            TimeInterval.ReadOnly = true;
            TimeInterval.Width = 125;
            // 
            // ActiveForScheduling
            // 
            ActiveForScheduling.DataPropertyName = "ActiveForScheduling";
            ActiveForScheduling.HeaderText = "ActiveForScheduling Hide";
            ActiveForScheduling.MinimumWidth = 6;
            ActiveForScheduling.Name = "ActiveForScheduling";
            ActiveForScheduling.ReadOnly = true;
            ActiveForScheduling.Visible = false;
            ActiveForScheduling.Width = 125;
            // 
            // IsPageUrl
            // 
            IsPageUrl.DataPropertyName = "IsPageUrl";
            IsPageUrl.HeaderText = "Is Crawle Page Url Hide";
            IsPageUrl.MinimumWidth = 6;
            IsPageUrl.Name = "IsPageUrl";
            IsPageUrl.ReadOnly = true;
            IsPageUrl.Visible = false;
            IsPageUrl.Width = 125;
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
            contextMenuStripGrid.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
        private ContextMenuStrip contextMenuStripGrid;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem editToolStripMenuItem1;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn SiteTitle;
        private DataGridViewTextBoxColumn ActiveForScheduling1;
        private DataGridViewTextBoxColumn IsDone;
        private DataGridViewTextBoxColumn CategoryNextPageURL;
        private DataGridViewTextBoxColumn IsCrawlePageUrlShow;
        private DataGridViewTextBoxColumn UseProxy;
        private DataGridViewTextBoxColumn TimeInterval;
        private DataGridViewTextBoxColumn ActiveForScheduling;
        private DataGridViewTextBoxColumn IsPageUrl;
    }
}
