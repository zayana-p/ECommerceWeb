using System.Drawing;
using System.Security.Cryptography.Xml;

namespace ECommerceWeb.Model
{
    public class OrderRequest
    {
        public string User { get; set; }
        public string CustomerId { get; set; }
    }

    public class OrderResponse
    {
        public Customer Customer { get; set; }
        public OrderDetails Order { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
    }

    public class OrderDetails
    {
        public int OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime? DeliveryExpected { get; set; }
    }


    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool ContainsGift { get; set; }
        public string ProductName { get; set; }
    }

}
