namespace Domain.Entities.Order
{
    public class OrderItem:BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductInOrderItem productInOrderItem, int quantity, decimal price)
        {
            this.productInOrderItem = productInOrderItem;
            Quantity = quantity;
            Price = price;
        }

        public ProductInOrderItem productInOrderItem { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}