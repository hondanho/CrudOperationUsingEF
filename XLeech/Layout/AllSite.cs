using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
using XLeech.Data.Entity;
using XLeech.Data.EntityFramework;
using XLeech.Data.Repository;

namespace XLeech
{
    public partial class AllSite : UserControl
    {
        private readonly AppDbContext _dbContext;
        public ShowDetailDelegate _showDetailDelegate;
        private readonly Repository<SiteConfig> _siteConfigRepository;

        public AllSite()
        {
            InitializeComponent();
            if (Main.AppWindow?.SiteConfigRepository != null)
            {
                _siteConfigRepository = Main.AppWindow?.SiteConfigRepository;
            }
            if (Main.AppWindow?.AppDbContext != null)
            {
                _dbContext = Main.AppWindow?.AppDbContext;
            }
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

        private async void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                var siteId = Convert.ToInt32(dataGridView.CurrentRow.Cells["Id"].Value);
                if (siteId > 0)
                {
                    var site = _dbContext.Sites
                                            .Where(x => x.Id == siteId)
                                            .Include(x => x.Category)
                                            .Include(x => x.Post)
                                            .FirstOrDefault();
                    DialogResult dialogResult = MessageBox.Show(string.Format("Bạn có muốn xóa site [{0}]?", site.Name), "Sure", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        await _siteConfigRepository.DeleteAsync(site);
                        SetDataInGridView();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        this.contextMenuStripGrid.Hide();
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var siteId = Convert.ToInt32(dataGridView.CurrentRow.Cells["Id"].Value);
            _showDetailDelegate(siteId);
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.dataGridView.Rows[e.RowIndex].Selected = true;
                this.dataGridView.CurrentCell = this.dataGridView.Rows[e.RowIndex].Cells[1];
                this.contextMenuStripGrid.Show(this.dataGridView, e.Location);
                this.contextMenuStripGrid.Show(Cursor.Position);
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ActiveForScheduling"].Value))
                {
                    row.Cells[2].Style.ForeColor = Color.Green;
                }
                else
                {
                    row.Cells[2].Style.ForeColor = Color.Yellow;
                }

                row.Cells[2].Value = "⚫";

                if (Convert.ToBoolean(row.Cells["IsPageUrl"].Value))
                {
                    row.Cells[5].Value = "List Post Link";
                }
                else
                {
                    row.Cells[5].Value = "Category Page Link";
                }
            }
        }
    }

}

