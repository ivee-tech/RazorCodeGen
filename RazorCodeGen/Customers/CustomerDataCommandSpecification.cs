using Iv.Data.GenericEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Customers
{
    public class CustomerDataCommandSpecification : DataCommandSpecificationBase<Customer>
    {
        public CustomerDataCommandSpecification(Action<Customer> action) : base(action)
        {
        }

    }
}
