
namespace XLeech
{
    partial class Main
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
            PanelMain = new Panel();
            menuStrip1 = new MenuStrip();
            allSitesToolStripMenuItem = new ToolStripMenuItem();
            addNewToolStripMenuItem = new ToolStripMenuItem();
            dashboardToolStripMenuItem = new ToolStripMenuItem();
            generalSettingsToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PanelMain
            // 
            PanelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PanelMain.Location = new Point(14, 56);
            PanelMain.Margin = new Padding(3, 4, 3, 4);
            PanelMain.Name = "PanelMain";
            PanelMain.Size = new Size(1157, 675);
            PanelMain.TabIndex = 1;
            // 
            // menuStrip1
            // 
            menuStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            menuStrip1.BackColor = Color.WhiteSmoke;
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { allSitesToolStripMenuItem, addNewToolStripMenuItem, dashboardToolStripMenuItem, generalSettingsToolStripMenuItem });
            menuStrip1.Location = new Point(14, 7);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(0, 3, 0, 3);
            menuStrip1.Size = new Size(388, 30);
            menuStrip1.TabIndex = 3;
            menuStrip1.TabStop = true;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += toolStrip1_ItemClicked;
            // 
            // allSitesToolStripMenuItem
            // 
            allSitesToolStripMenuItem.Name = "allSitesToolStripMenuItem";
            allSitesToolStripMenuItem.Size = new Size(74, 24);
            allSitesToolStripMenuItem.Text = "All sites";
            allSitesToolStripMenuItem.Click += allSitesToolStripMenuItem_Click;
            // 
            // addNewToolStripMenuItem
            // 
            addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            addNewToolStripMenuItem.Size = new Size(85, 24);
            addNewToolStripMenuItem.Text = "Add New";
            addNewToolStripMenuItem.Click += addNewToolStripMenuItem_Click;
            // 
            // dashboardToolStripMenuItem
            // 
            dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            dashboardToolStripMenuItem.Size = new Size(96, 24);
            dashboardToolStripMenuItem.Text = "Dashboard";
            dashboardToolStripMenuItem.Click += dashboardToolStripMenuItem_Click;
            // 
            // generalSettingsToolStripMenuItem
            // 
            generalSettingsToolStripMenuItem.Name = "generalSettingsToolStripMenuItem";
            generalSettingsToolStripMenuItem.Size = new Size(131, 24);
            generalSettingsToolStripMenuItem.Text = "General Settings";
            generalSettingsToolStripMenuItem.Click += generalSettingsToolStripMenuItem_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1184, 748);
            Controls.Add(PanelMain);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Main";
            Padding = new Padding(0, 7, 0, 7);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "X Auto Leech";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel PanelMain;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem allSitesToolStripMenuItem;
        private ToolStripMenuItem addNewToolStripMenuItem;
        private ToolStripMenuItem dashboardToolStripMenuItem;
        private ToolStripMenuItem generalSettingsToolStripMenuItem;
    }

    public class MyRenderer : ToolStripProfessionalRenderer
    {
        public MyRenderer() : base(new MyColorTable())
        {
        }
    }

    public class MyColorTable : ProfessionalColorTable
    {
        public override Color ButtonCheckedGradientBegin
        {
            get { return ButtonPressedGradientBegin; }
        }
        public override Color ButtonCheckedGradientEnd
        {
            get { return ButtonPressedGradientEnd; }
        }
        public override Color ButtonCheckedGradientMiddle
        {
            get { return ButtonPressedGradientMiddle; }
        }
    }
}

