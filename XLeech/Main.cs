using XLeech.Core.Service;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using XLeech.Data.Repository;
using XLeech.Model;

namespace XLeech
{
    public partial class Main : Form
    {
        public static Main AppWindow;
        public readonly ICrawlerService CrawlerService;
        public readonly IChatGPTService ChatGPTService;
        public readonly AppDbContext AppDbContext;
        public readonly Repository<SiteConfig> SiteConfigRepository;
        public readonly Repository<CategoryConfig> CategoryRepository;
        public readonly Repository<PostConfig> PostRepository;

        public Main(AppDbContext dbContext,
            Repository<CategoryConfig> categoryRepository,
            Repository<SiteConfig> siteConfigRepository,
            Repository<PostConfig> postRepository,
            ICrawlerService crawlerService,
            IChatGPTService chatGPTService
            )
        {
            AppWindow = this;
            InitializeComponent();
            this.AppDbContext = dbContext;
            this.CrawlerService = crawlerService;
            this.ChatGPTService = chatGPTService;
            this.CategoryRepository = categoryRepository;
            this.SiteConfigRepository = siteConfigRepository;
            this.PostRepository = postRepository;
        }

        /// <summary>
        /// form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.ListSite);
        }

        public void SetPanelView(PageTypeEnum pageType, int siteId = 0)
        {
            var childMain = new UserControl();

            if (pageType == PageTypeEnum.ListSite)
            {
                var allSite = new AllSite();
                allSite.SetCallback(ViewSiteDetail);
                childMain = allSite;
            }

            if (pageType == PageTypeEnum.AddNewSite || pageType == PageTypeEnum.EditSite)
            {
                var siteDetail = new SiteDetail();
                if (pageType == PageTypeEnum.AddNewSite)
                {
                    siteDetail.SetViewCreateSite();
                }
                if (pageType == PageTypeEnum.EditSite)
                {
                    siteDetail.SetViewEditSite(siteId);
                }

                siteDetail.SetCallback(BackToListSite);
                childMain = siteDetail;
            }

            if (pageType == PageTypeEnum.Dashboard)
            {
                childMain = new Dashboard();
            }

            if (pageType == PageTypeEnum.GeneralSettings)
            {
                childMain = new GeneralSettings();
            }

            AddUserControlToPanelView(childMain);
        }

        private void AddUserControlToPanelView(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            this.PanelMain.Controls.Clear();
            this.PanelMain.Controls.Add(userControl);
            userControl.Show();
        }

        private void allSitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.ListSite);
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.AddNewSite);
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.Dashboard);
        }

        private void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.GeneralSettings);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in ((ToolStrip)sender).Items)
            {
                if (item != e.ClickedItem)
                    item.BackColor = Color.WhiteSmoke;
                else
                    item.BackColor = Color.LightGray;
            }
        }

        private void ViewSiteDetail(int siteId)
        {
            SetPanelView(PageTypeEnum.EditSite, siteId);
        }

        private void BackToListSite()
        {
            SetPanelView(PageTypeEnum.ListSite);
        }
    }

    public delegate void ShowDetailDelegate(int siteId);

    public delegate void BackDelegate();
}