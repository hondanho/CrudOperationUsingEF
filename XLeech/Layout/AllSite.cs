﻿using Microsoft.EntityFrameworkCore;
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
        private int _rowIndex = 0;

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
                        MessageBox.Show("Xóa site thành công");
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
                this._rowIndex = e.RowIndex;
                this.dataGridView.CurrentCell = this.dataGridView.Rows[e.RowIndex].Cells[1];
                this.contextMenuStripGrid.Show(this.dataGridView, e.Location);
                this.contextMenuStripGrid.Show(Cursor.Position);
            }
        }
    }

}

