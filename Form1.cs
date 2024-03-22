using CrudOperationUsingEF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudOperationUsingEF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //create object of contex and table model
        TutorialEntities db = new TutorialEntities();
        Employee Employee = new Employee();
        int EmpId = 0;
        /// <summary>
        /// form load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ClearData();
            SetDataInGridView();
        }

        /// <summary>
        /// reset all fields
        /// </summary>
        public void ClearData()
        {
            txtEmpAdd.Text = txtEmpAge.Text = txtEmpCity.Text = txtEmpName.Text = txtEmpSalary.Text = string.Empty;
            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            EmpId = 0;
        }
        /// <summary>
        /// set data in grid view
        /// </summary>
        public void SetDataInGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            var data = db.Employees.ToList<Employee>();
            dataGridView.DataSource = db.Employees.ToList<Employee>();
        }

        /// <summary>
        /// insert update data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Employee.EmployeeName = txtEmpName.Text.Trim();
            Employee.EmployeeAge = Convert.ToInt32(txtEmpAge.Text.Trim());
            Employee.EmployeeAddress = txtEmpAdd.Text.Trim();
            Employee.EmployeeCity = txtEmpCity.Text.Trim();
            Employee.EmployeeSalary = Convert.ToInt32(txtEmpSalary.Text.Trim());
            if (EmpId > 0)
                db.Entry(Employee).State = EntityState.Modified;
            else
            {
                db.Employees.Add(Employee);
            }
            db.SaveChanges();
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
            if (dataGridView.CurrentCell.RowIndex != -1)
            {
                EmpId = Convert.ToInt32(dataGridView.CurrentRow.Cells["EmployeeId"].Value);
                Employee = db.Employees.Where(x => x.EmployeeId == EmpId).FirstOrDefault();
                txtEmpName.Text = Employee.EmployeeName;
                txtEmpAdd.Text = Employee.EmployeeAddress;
                txtEmpAge.Text = Employee.EmployeeAge.ToString();
                txtEmpSalary.Text = Employee.EmployeeSalary.ToString();
                txtEmpAge.Text = Employee.EmployeeAge.ToString();
                txtEmpCity.Text = Employee.EmployeeCity;
            }
            btnSave.Text = "Update";
            btnDelete.Enabled = true;
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
                db.Employees.Remove(Employee);
                db.SaveChanges();
                ClearData();
                SetDataInGridView();
                MessageBox.Show("Record Deleted Successfully");
            }
        }
    }
}
