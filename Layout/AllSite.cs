using XAutoLeech.Database.EntityFramework;
using XAutoLeech.Database.Model;

namespace XAutoLeech
{
    public partial class AllSite : UserControl
    {
        private readonly AppDbContext _dbContext;

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
            dataGridView.DataSource = this._dbContext.Sites.ToList<Site>();
        }
    }
}