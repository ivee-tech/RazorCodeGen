using Iv.Data.GenericEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Customers
{
    public class CustomerDataCommand : GenericDataCommand<Customer, int>
    {
        public CustomerDataCommand(IDataContext ctx) : base(ctx)
        {

        }
    }
}
