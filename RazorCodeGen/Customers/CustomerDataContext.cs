using Iv.Data.GenericEF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Customers
{
    public class CustomerDataContext : DataContextBase<Customer, int>
    {
        public DbSet<DeliveryProduct> DeliveryProducts { get; set; }

        public CustomerDataContext()
        {
            this.modelBuilder = new CustomerModelBuilder();
        }

        public CustomerDataContext(IModelBuilder<Customer, int> modelBuilder) : base(modelBuilder)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
