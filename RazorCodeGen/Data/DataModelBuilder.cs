using Iv.Data.GenericEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Data
{
    class DataModelBuilder : DbModelBuilder
    {

        public void Map(DbModelBuilder modelBuilder)
        {
            MapDataModel(modelBuilder);
        }

        private void MapDataModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataModel>()
                .ToTable("DataModels");

            modelBuilder.Entity<DataModel>()
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id");

            modelBuilder.Entity<DataModel>()
                .Property(p => p.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<DataModel>()
                .Property(p => p.Status)
                .HasColumnName("Status");

        }

    }
}
