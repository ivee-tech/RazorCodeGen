using Iv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorCodeGen.Customers
{
    public class Customer : ObjectDefBase<int>
    {

        public Customer()
        {
            DeliveryProducts = new List<DeliveryProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CommencementDate { get; set; }
        public bool HasDeliveryFees { get; set; }
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

        public IList<DeliveryProduct> DeliveryProducts { get; private set; }

        public override void SetRefRelationship()
        {
            foreach(var dp in DeliveryProducts)
            {
                dp.CustomerId = Id;
            }
        }
    }
}
