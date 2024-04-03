using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;

namespace XLeech
{
    public partial class AllSite : UserControl
    {
        private readonly AppDbContext _dbContext;
        public ShowDetailDelegate _showDetailDelegate;

        public AllSite(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            SetDataInGridView();
        }

        public void SetDataInGridView()
        {
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = this._dbContext.Sites.ToList<SiteConfig>();
        }

        public void SetCallback(ShowDetailDelegate showDetailDelegate)
        {
            _showDetailDelegate = showDetailDelegate;
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var siteId = Convert.ToInt32(dataGridView.CurrentRow.Cells["Id"].Value);
            _showDetailDelegate(siteId);
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("delete");
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            MessageBox.Show("delete");
        }
    }

}

