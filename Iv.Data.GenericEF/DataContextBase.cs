using Iv.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iv.Data.GenericEF
{
    public class DataContextBase<T, TKey> : DbContext, IDataContext
        where TKey : IComparable
        where T : ObjectDefBase<TKey>
    {
        protected IModelBuilder<T, TKey> modelBuilder;

        public DbSet<T> DbSet { get; set; }

        public DataContextBase() : base("DataContext")
        {
        }

        public DataContextBase(string connectionName) : base(connectionName)
        {
        }

        public DataContextBase(IModelBuilder<T, TKey> modelBuilder) : this()
        {
            this.modelBuilder = modelBuilder;
        }

        public DataContextBase(IModelBuilder<T, TKey> modelBuilder, string connectionName) : this(connectionName)
        {
            this.modelBuilder = modelBuilder;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (this.modelBuilder != null)
            {
                this.modelBuilder.Map(modelBuilder);
            }
        }
    }
}