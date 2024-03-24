using Microsoft.EntityFrameworkCore;
using System.Data;
using XAutoLeech.Database.Model;
using XAutoLeech.Database.EntityFramework;
using XAutoLeech.Model;
using System.Windows.Forms;

namespace XAutoLeech
{
    public partial class Main : Form
    {
        //create object of contex and table model
        private readonly AppDbContext _dbContext;
        private Site Employee = new Site();
        private int EmpId = 0;

        public Main(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
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
            SetPanelView(TypeSiteEnum.AllSite);
        }

        private void SetPanelView(TypeSiteEnum typeSiteEnum)
        {
            var childMain = new UserControl();
            switch (typeSiteEnum)
            {
                case TypeSiteEnum.AllSite:
                    childMain = new AllSite(_dbContext);
                    break;

                case TypeSiteEnum.AddNew:
                    childMain = new AddNew();
                    break;

                case TypeSiteEnum.Dashboard:
                    childMain = new Dashboard();
                    break;

                case TypeSiteEnum.GeneralSettings:
                    childMain = new GeneralSettings();
                    break;

                default:
                    break;
            }

            childMain.Dock = DockStyle.Fill;
            this.PanelMain.Controls.Clear();
            this.PanelMain.Controls.Add(childMain);
            childMain.Show();
        }

        /// <summary>
        /// reset all fields
        /// </summary>
        public void ClearData()
        {
            //txtEmpAdd.Text = txtEmpAge.Text = txtEmpCity.Text = txtEmpName.Text = txtEmpSalary.Text = string.Empty;
            //btnDelete.Enabled = false;
            //btnSave.Text = "Save";
            EmpId = 0;
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
            _dbContext.SaveChanges();
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
                _dbContext.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Record Deleted Successfully");
            }
        }

        private void allSitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(TypeSiteEnum.AllSite);
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(TypeSiteEnum.AddNew);
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(TypeSiteEnum.Dashboard);
        }

        private void generalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPanelView(TypeSiteEnum.GeneralSettings);
        }
    }
}