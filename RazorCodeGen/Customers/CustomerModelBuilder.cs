using Iv.Data.GenericEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e = Iv.Metadata; //.Data.Entities;

namespace RazorCodeGen.Customers
{
    class CustomerModelBuilder : ModelBuilderBase<Customer, int>
    {

        public override void Map(DbModelBuilder modelBuilder)
        {
            MapCustomer(modelBuilder);
            MapDeliveryProduct(modelBuilder);
        }

        private void MapCustomer(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToTable("Customers");

            modelBuilder.Entity<Customer>()
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Description)
                .HasColumnName("Description");

            modelBuilder.Entity<Customer>()
                .Property(p => p.CommencementDate)
                .HasColumnName("CommencementDate");

            modelBuilder.Entity<Customer>()
                .HasMany<DeliveryProduct>(p => p.DeliveryProducts)
                .WithRequired().HasForeignKey(p => p.CustomerId);

            //this.Ignore<Customer, int>(modelBuilder);
            modelBuilder.Entity<Customer>().Ignore(p => p.IsNew);
            modelBuilder.Entity<Customer>().Ignore(p => p.IsDirty);
            modelBuilder.Entity<Customer>().Ignore(p => p.IsDeleted);
            modelBuilder.Entity<Customer>().Ignore(p => p.Key);
        }

        private void MapDeliveryProduct(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryProduct>()
                .ToTable("DeliveryProducts");

            modelBuilder.Entity<DeliveryProduct>()
                .HasKey(p => p.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("Id");

            modelBuilder.Entity<DeliveryProduct>()
                .Property(p => p.Name)
                .HasColumnName("Name");

            modelBuilder.Entity<DeliveryProduct>()
                .Property(p => p.Price)
                .HasColumnName("Price");

            modelBuilder.Entity<DeliveryProduct>()
                .Property(p => p.HasDate)
                .HasColumnName("HasDate");

            modelBuilder.Entity<DeliveryProduct>()
                .Property(p => p.SortOrder)
                .HasColumnName("SortOrder");

            modelBuilder.Entity<DeliveryProduct>()
                .Property(p => p.CustomerId)
                .HasColumnName("CustomerId");

            /*
            modelBuilder.Entity<Customer>()
                .HasMany<Child>(p => p.Children)
                .WithRequired().HasForeignKey(p => p.CustomerId);
            */
            //this.Ignore<DeliveryProduct, int>(modelBuilder);
            modelBuilder.Entity<DeliveryProduct>().Ignore(p => p.IsNew);
            modelBuilder.Entity<DeliveryProduct>().Ignore(p => p.IsDirty);
            modelBuilder.Entity<DeliveryProduct>().Ignore(p => p.IsDeleted);
            modelBuilder.Entity<DeliveryProduct>().Ignore(p => p.Key);
        }

    }
}
