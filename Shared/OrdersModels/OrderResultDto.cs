using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrdersModels
{
    public class OrderResultDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        
        public AddressDto ShippingAddress { get; set; }
        
        public ICollection<OrderItemDto> orderItems { get; set; } = new List<OrderItemDto>();//Navigation proprty
        
        public string deliveryMethod { get; set; }

        public string orderPaymentStatus { get; set; }
 
        public decimal SubTotal { get; set; }
        
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Total { get; set; }

    }
}
