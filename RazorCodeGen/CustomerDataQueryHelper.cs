using System;
using System.Text;
using System.Collections.Generic;
using RazorCodeGen.Customers;
using System.Linq;
using System.Linq.Expressions;

namespace RazorCodeGen
{
    /// <summary>
    /// Summary description for CustomerDataQueryTests
    /// </summary>
    public class CustomerDataQueryHelper
    {
        private CustomerDataContext ctx;
        private CustomerDataQuery qry;

        public CustomerDataQueryHelper()
        {
            Initialize();
        }

        public void Initialize()
        {
            var modelBuilder = new CustomerModelBuilder();
            ctx = new CustomerDataContext(modelBuilder);
            qry = new CustomerDataQuery(ctx);
        }

        public IEnumerable<Customer> GetAll()
        {
            var list = qry.GetAll();
            return list;
        }

        public IEnumerable<Customer> Filter(string name)
        {
            var list = qry.Filter(c => c.Name == name);
            return list;
        }

        public IEnumerable<Customer> FilterSpec(string name)
        {
            Expression<Func<Customer, bool>> criteria = c => c.Name == name;
            var spec = new CustomerDataQuerySpecification(criteria);
            spec.Includes.Add(c => c.DeliveryProducts);
            var list = qry.Filter(spec);
            return list;
        }

        public Customer Find(int id)
        {
            var c = qry.Find(id);
            return c;
        }

        public Customer FindSpec(int id)
        {
            Expression<Func<Customer, bool>> criteria = c => c.Id == id;
            var spec = new CustomerDataQuerySpecification(criteria);
            spec.Includes.Add(c => c.DeliveryProducts);
            var cust = qry.Find(spec);
            return cust;
        }
    }
}
