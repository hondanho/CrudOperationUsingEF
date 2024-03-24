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

namespace XAutoLeech
{
    public partial class AddNew : UserControl
    {
        private AppDbContext _dbContext;

        public AddNew()
        {
            InitializeComponent();
        }

        public AddNew(AppDbContext dbContext)
        {
            InitializeComponent();
            this._dbContext = dbContext;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {

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
