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
        public readonly CrawlerService CrawlerService;
        public readonly AppDbContext AppDbContext;
        public readonly Repository<CategoryConfig> CategoryRepository;

        private Repository<SiteConfig> _siteRepository;
        private Repository<PostConfig> _postRepository;

        public Main(AppDbContext dbContext,
            Repository<SiteConfig> siteRepository,
            Repository<CategoryConfig> categoryRepository,
            Repository<PostConfig> postRepository
            )
        {
            AppWindow = this;
            InitializeComponent();
            AppDbContext = dbContext;
            this._siteRepository = siteRepository;
            this.CategoryRepository = categoryRepository;
            this._postRepository = postRepository;
        }

        /// <summary>
        /// form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ClearData();
            SetDataInGridView();
            SetPanelView(PageTypeEnum.ListSite);
        }

        public void SetPanelView(PageTypeEnum pageType, int siteId = 0)
        {
            var childMain = new UserControl();

            if (pageType == PageTypeEnum.ListSite)
            {
                var allSite = new AllSite(AppDbContext);
                allSite.SetCallback(ViewSiteDetail);
                childMain = allSite;
            }

            if (pageType == PageTypeEnum.AddNewSite || pageType == PageTypeEnum.EditSite)
            {
                var siteDetail = new SiteDetail(
                        AppDbContext,
                        this._siteRepository,
                        this.CategoryRepository,
                        this._postRepository
                    );
                if (pageType == PageTypeEnum.AddNewSite)
                {
                    siteDetail.setViewCreateSite();
                }
                if (pageType == PageTypeEnum.EditSite)
                {
                    siteDetail.setViewEditSite(siteId);
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

        /// <summary>
        /// reset all fields
        /// </summary>
        public void ClearData()
        {
            //txtEmpAdd.Text = txtEmpAge.Text = txtEmpCity.Text = txtEmpName.Text = txtEmpSalary.Text = string.Empty;
            //btnDelete.Enabled = false;
            //btnSave.Text = "Save";
            //EmpId = 0;
        }

        /// <summary>
        /// set data in grid view
        /// </summary>
        public void SetDataInGridView()
        {
            //dataGridView.AutoGenerateColumns = false;
            //var data = _dbContext.Sites.ToList<Site>();
            //dataGridView.DataSource = _dbContext.Sites.ToList<Site>();
        }

        /// <summary>
        /// insert update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Employee.EmployeeName = txtEmpName.Text.Trim();
            //Employee.EmployeeAge = Convert.ToInt32(txtEmpAge.Text.Trim());
            //Employee.EmployeeAddress = txtEmpAdd.Text.Trim();
            //Employee.EmployeeCity = txtEmpCity.Text.Trim();
            //Employee.EmployeeSalary = Convert.ToInt32(txtEmpSalary.Text.Trim());
            //if (EmpId > 0)
            //    _dbContext.Entry(Employee).State = EntityState.Modified;
            //else
            //{
            //    Employee.EmployeeId = 0;
            //    _dbContext.Sites.Add(Employee);
            //}
            AppDbContext.SaveChanges();
            ClearData();
            SetDataInGridView();
            MessageBox.Show("Record Save Successfully");
        }

        /// <summary>
        /// set data in textbox on grid view click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            //if (dataGridView.CurrentCell.RowIndex != -1)
            //{
            //    //EmpId = Convert.ToInt32(dataGridView.CurrentRow.Cells["EmployeeId"].Value);
            //    //Employee = _dbContext.Sites.Where(x => x.EmployeeId == EmpId).FirstOrDefault();
            //    //txtEmpName.Text = Employee.EmployeeName;
            //    //txtEmpAdd.Text = Employee.EmployeeAddress;
            //    //txtEmpAge.Text = Employee.EmployeeAge.ToString();
            //    //txtEmpSalary.Text = Employee.EmployeeSalary.ToString();
            //    //txtEmpAge.Text = Employee.EmployeeAge.ToString();
            //    //txtEmpCity.Text = Employee.EmployeeCity;
            //}
            ////btnSave.Text = "Update";
            ////btnDelete.Enabled = true;
        }

        /// <summary>
        /// clear data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record ?", "Delete ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //_dbContext.Sites.Remove(Employee);
                AppDbContext.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Record Deleted Successfully");
            }
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

        private void crawleNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(PageTypeEnum.Dashboard);
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