using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities.Order
{
    public class Order:BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            this.orderItems = orderItems;
            this.deliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        //User Email
        public string UserEmail { get; set; }
        //Shipping Address
        public Address ShippingAddress { get; set; }
        //Order Items
        public ICollection<OrderItem> orderItems { get; set; } = new List<OrderItem>();//Navigation proprty
        //Delivery Method
        public DeliveryMethod deliveryMethod { get; set; } //Navigation Proprty
        public int? DeliveryMethodId { get; set; } //fk

        // Payment Status
        public OrderPaymentStatus orderPaymentStatus { get; set; } = OrderPaymentStatus.pending;

        //SubTotal
        public decimal SubTotal { get; set; }
        //Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public string PaymentIntentId { get; set; }

    }
}
