

using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using XLeech.Data.Repository;

namespace XLeech
{
    public partial class AddNew : UserControl
    {
        private AppDbContext _dbContext;
        private Repository<Site> _siteRepository;
        private Repository<Category> _categoryRepository;
        private Repository<Post> _postRepository;

        public AddNew()
        {
            InitializeComponent();
        }

        public AddNew(AppDbContext dbContext,
            Repository<Site> siteRepository,
            Repository<Category> categoryRepository,
            Repository<Post> postRepository
            )
        {
            InitializeComponent();
            this._dbContext = dbContext;
            this._siteRepository = siteRepository;
            this._categoryRepository = categoryRepository;
            this._postRepository = postRepository;
            setShowTypeCrawler();
        }

        private void setShowTypeCrawler()
        {
            if (IsCrawleUrls())
            {
                showCrawlerUrls(true);
                showCategoryPage(false);
            }
            else
            {
                showCrawlerUrls(false);
                showCategoryPage(true);
            }
        }

        private bool IsCrawleUrls()
        {
            return this.ListUrlRb.Checked && !this.CategorypageRb.Checked;
        }

        private void showCrawlerUrls(bool isShow)
        {
            if (isShow)
            {
                this.CrawlerUrlsTb.Show();
                this.CrawlerUrlsLb.Show();
            }
            else
            {
                this.CrawlerUrlsTb.Hide();
                this.CrawlerUrlsLb.Hide();
            }

        }

        private void showCategoryPage(bool isShow)
        {
            if (isShow)
            {
                this.CategoryListPageURLLb.Show();
                this.CategoryListPageURLTb.Show();
                this.CategoryListURLSelectorLb.Show();
                this.CategoryListURLSelectorTb.Show();
                this.CategoryPostURLSelectorLb.Show();
                this.CategoryPostURLSelectorTb.Show();
                this.CategoryNextPageURLSelectorLb.Show();
                this.CategoryNextPageURLSelectorTb.Show();
            }
            else
            {
                this.CategoryListPageURLLb.Hide();
                this.CategoryListPageURLTb.Hide();
                this.CategoryListURLSelectorLb.Hide();
                this.CategoryListURLSelectorTb.Hide();
                this.CategoryPostURLSelectorLb.Hide();
                this.CategoryPostURLSelectorTb.Hide();
                this.CategoryNextPageURLSelectorLb.Hide();
                this.CategoryNextPageURLSelectorTb.Hide();
            }

        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            var site = new Site()
            {
                Title = this.SiteUrlTxt.Text,
                ActiveForScheduling = this.ActiveForSchedulingCb.Checked,
                CheckDuplicatePostViaContent = this.CheckDuplicatePostViaContentCb.Checked,
                CheckDuplicatePostViaTitle = this.CheckDuplicatePostViaTitleCb.Checked,
                CheckDuplicatePostViaUrl = this.CheckDuplicatePostViaUrlCb.Checked,
                MaximumPagesCrawlPerCategory = (int)this.MaximumPagesCrawlPerCategoryNumeric.Value,
                MaximumPagesCrawlPerPost = (int)this.MaximumPagesCrawlPerPostNumeric.Value,
                HTTPUserAgent = this.HTTPUserAgentTb.Text,
                ConnectionTimeout = (int)this.connectionTimeoutNumeric.Value,
                UseProxy = this.UseProxyCb.Checked,
                Proxies = this.ProxiesTb.Text,
                RandomizeProxies = this.RandomizeProxiesCb.Checked,
                TimeInterval = (int)this.TimeIntervalNumeric.Value,
                Notes = this.NoteTb.Text,
                Cookie = this.CookieCb.Text,
                ProxyRetryLimit = (int)this.ProxyRetryLimitNumeric.Value,
                UpdateTime = now,
                CreateTime = now
            };
            await this._siteRepository.AddAsync(site);

            var category = new Category()
            {
                SiteID = site.Id,
                
                CategoryMap = this.CategoryMapTb.Text,
                SaveFeaturedImages = this.SaveFeaturedImagesCb.Checked,
                FeaturedImageSelector = this.FeaturedImageSelectorTb.Text,
                FindAndReplaceRawHTML = this.FindAndReplaceRawHTMLTb.Text,
                RemoveElementAttributes = this.RemoveElementAttributesTb.Text,
                UnnecessaryElements = this.UnnecessaryElementsTb.Text,
                UpdateTime = now,
                CreateTime = now
            };
            if (IsCrawleUrls())
            {
                category.Urls = this.CrawlerUrlsTb.Text;
            }
            else
            {
                category.CategoryListPageURL = this.CategoryListPageURLTb.Text;
                category.CategoryListURLSelector = this.CategoryListURLSelectorTb.Text;
                category.CategoryPostURLSelector = this.CategoryPostURLSelectorTb.Text;
                category.CategoryNextPageURLSelector = this.CategoryNextPageURLSelectorTb.Text;
            }

            await this._categoryRepository.AddAsync(category);

            var post = new Post()
            {
                SiteID = site.Id,
                PostFormat = this.PostFormatCb.Text,
                PostType = this.PostTypeCb.Text,
                PostAuthor = this.PostAuthorTb.Text,
                PostStatus = this.PostStatusCb.Text,
                PostTitleSelector = this.PostTitleSelectorTb.Text,
                PostExcerptSelector = this.PostExcerptSelectorTb.Text,
                PostContentSelector = this.PostContentSelectorTb.Text,
                PostTagSelector = this.PostTagSelectorTb.Text,
                PostSlugSelector = this.PostSlugSelectorTb.Text,
                CategoryNameSelector = this.CategoryNameSelectorTb.Text,
                CategoryNameSeparatorSelector = this.CategoryNameSeparatorSelectorTb.Text,
                PostDateSelector = this.PostDateSelectorTb.Text,
                SaveMetaKeywords = this.SaveMetaKeywordsCb.Checked,
                AddMetaKeywordsAsTag = this.AddMetaKeywordsAsTagCb.Checked,
                SaveMetaDescription = this.SaveMetaDescriptionCb.Checked,
                SaveFeaturedImages = this.SaveFeaturedImagesCb.Checked,
                FeaturedImageSelector = this.FeaturedImageSelectorTb.Text,
                PaginatePosts = this.PaginatePostsCb.Checked,
                PostNextPageURLSelector = this.PostNextPageURLSelectorTb.Text,
                FindAndReplaceRawHTML = this.PostFindAndReplaceRawHTMLTb.Text,
                RemoveElementAttributes = this.PostRemoveElementAttributesTb.Text,
                UnnecessaryElements = this.PostUnnecessaryElementsTb.Text,
                UpdateTime = now,
                CreateTime = now
            };
            await this._postRepository.AddAsync(post);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void cancleBtn_Click(object sender, EventArgs e)
        {
            // Find the panel by name
            //Panel panel = this.Controls.Find("PanelMain", true).FirstOrDefault() as Panel;

            //if (panel != null)
            //{
            //    AllSite newChildForm = new AllSite(_dbContext);
            //    panel.Controls.Clear();
            //    newChildForm.Dock = DockStyle.Fill;
            //    panel.Controls.Add(newChildForm);
            //    newChildForm.Show();
            //}

            //AllSite newChildForm = new AllSite(new AppDbContext());
            //Panel panel = Main.instance.Controls.Find("PanelMain", true).FirstOrDefault() as Panel;
            //newChildForm.Dock = DockStyle.Fill;
            //panel.Controls.Add(newChildForm);
            //newChildForm.Show();
        }

        private void ListUrlRb_CheckedChanged(object sender, EventArgs e)
        {
            setShowTypeCrawler();
        }

        private void CategorypageRb_CheckedChanged(object sender, EventArgs e)
        {
            setShowTypeCrawler();
        }
    }
}