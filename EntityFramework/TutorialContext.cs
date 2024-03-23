//using CrudOperationUsingEF.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CrudOperationUsingEF.EntityFramework
//{
//    public class TutorialContext : DbContext
//    {
//        public TutorialContext()
//            : base("name=TutorialEntities")
//        {
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            throw new UnintentionalCodeFirstException();
//        }

//        public virtual DbSet<Employee> Employees { get; set; }
//    }
//}
