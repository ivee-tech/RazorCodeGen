using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Customers
{
    public class DeliveryProduct : ObjectDefBase<int>
    {

        public DeliveryProduct()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool HasDate { get; set; }
        public int SortOrder { get; set; }
        public int CustomerId { get; set; }
        public override int Key 
        {
            get
            {
                return Id;
            }

            set
            {
                Id = value;
            }
        }

    }
}
