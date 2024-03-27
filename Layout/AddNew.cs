using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XAutoLeech.Database.EntityFramework;
using XAutoLeech.Database.Model;
using XAutoLeech.Repository;

namespace XAutoLeech
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
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
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
                LastModified = DateTime.Now,
                CreateTime = DateTime.Now
            };
            await this._siteRepository.AddAsync(site);

            var category = new Category()
            {
                SiteID = site.Id,
                CategoryListPageURL = this.CategoryListPageURLTb.Text,
                CategoryListURLSelector = this.CategoryListURLSelectorTb.Text,
                CategoryMap = this.CategoryMapTb.Text,
                CategoryPostURLSelector = this.CategoryPostURLSelectorTb.Text,
                CategoryNextPageURLSelector = this.CategoryNextPageURLSelectorTb.Text,
                SaveFeaturedImages = this.SaveFeaturedImagesCb.Checked,
                FeaturedImageSelector = this.FeaturedImageSelectorTb.Text,
                FindAndReplaceRawHTML = this.FindAndReplaceRawHTMLTb.Text,
                RemoveElementAttributes = this.RemoveElementAttributesTb.Text,
                UnnecessaryElements = this.UnnecessaryElementsTb.Text
            };
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
                UnnecessaryElements = this.PostUnnecessaryElementsTb.Text
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
    }
}
