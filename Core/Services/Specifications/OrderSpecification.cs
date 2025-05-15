using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order;

namespace Services.Specifications
{
    public class OrderSpecification : BaseSpecifications<Order, Guid>
    {
        public OrderSpecification(Guid id) :base(O=>O.Id ==id)
        {
            AddInclude(O => O.deliveryMethod);
            AddInclude(O => O.orderItems);
        }
        public OrderSpecification(string userEmail) : base(O => O.UserEmail == userEmail)
        {
            AddInclude(O => O.deliveryMethod);
            AddInclude(O => O.orderItems);
            AddOrderBy(O => O.OrderDate);
        }
    }
}
