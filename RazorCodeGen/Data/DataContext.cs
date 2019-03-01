using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Data
{
    public class DataContext : DbContext
    {
        public DbSet<DataModel> DataModels { get; set; }

        public DataContext() : base("DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new DataModelBuilder().Map(modelBuilder);
        }

    }
}
