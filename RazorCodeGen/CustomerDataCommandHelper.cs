using System;
using RazorCodeGen.Customers;
using System.Linq;
using Iv.Data;
using Iv.Data.GenericEF;

namespace RazorCodeGen
{
    public class CustomerDataCommandHelper
    {
        private IDataContext ctx;
        private IDataCommand<Customer, int> cmd;
        private IDataQuery<Customer, int> qry;
        private IDataCommandSpecification<Customer> cmdSpec;

        public CustomerDataCommandHelper()
        {
            Initialize();
        }

        public void Initialize()
        {
            var modelBuilder = new CustomerModelBuilder();
            ctx = new CustomerDataContext(modelBuilder);
            cmd = new CustomerDataCommand(ctx);
            qry = new CustomerDataQuery(ctx);
            Action<Customer> action = delegate (Customer cust)
            {
                cust.DeliveryProducts.ToList().ForEach(prod =>
                {
                    cmd.SetEntityState<DeliveryProduct, int>(prod);
                });
            };
            cmdSpec = new CustomerDataCommandSpecification(action);
        }

        public Customer Create(string name)
        {
            var c = new Customer
            {
                Name = name,
                Description = $"Customer test {name}",
                CommencementDate = DateTime.Today
            };
            cmd.Create(c);
            return c;
        }

        public Customer CreateWithSpec(string name)
        {
            var c = new Customer
            {
                Name = name,
                Description = $"Customer test {name}",
                CommencementDate = DateTime.Today
            };
            var dp = new DeliveryProduct()
            {
                Name = $"DP {new Random().Next(1, 99)}",
                HasDate = false,
                Price = 10M,
                SortOrder = 10,
                IsNew = true
            };
            c.DeliveryProducts.Add(dp);
            Action<Customer> action = delegate (Customer cust)
            {
                cust.DeliveryProducts.ToList().ForEach(prod =>
                {
                    cmd.SetEntityState<DeliveryProduct, int>(prod);
                });
            };
            IDataCommandSpecification<Customer> spec = new CustomerDataCommandSpecification(action);
            cmd.Create(c);
            return c;
        }

        public Customer UpdateWithSpec(int id)
        {
            IDataQuerySpecification<Customer> qrySpec = new CustomerDataQuerySpecification(cust => cust.Id == id && !cust.IsInactive);
            qrySpec.Includes.Add(cust => cust.DeliveryProducts);
            var c = qry.Find(qrySpec);

            c.Description = $"Customer test {c.Name} - {DateTime.Now}";
            c.CommencementDate = DateTime.Today.AddMonths(-3);
            if (c.DeliveryProducts.Count > 0)
            {
                c.DeliveryProducts[0].Price = 11.50M;
                c.DeliveryProducts[0].SetDirty();
            }


            var dp = new DeliveryProduct()
            {
                Name = $"DP {new Random().Next(1, 99)}",
                HasDate = true,
                Price = 20M,
                SortOrder = 20,
                IsNew = true
            };
            c.DeliveryProducts.Add(dp);
            cmd.Update(c, cmdSpec);
            return c;
        }
    }
}
