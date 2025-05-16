using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order;

namespace Services.Specifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecifications<Order,Guid>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId):base(O=>O.PaymentIntentId == paymentIntentId)
        {
            
        }

    }
}
