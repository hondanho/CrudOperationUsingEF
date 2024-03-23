using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudOperationUsingEF.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> EmployeeAge { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeCity { get; set; }
        public Nullable<decimal> EmployeeSalary { get; set; }
    }
}
