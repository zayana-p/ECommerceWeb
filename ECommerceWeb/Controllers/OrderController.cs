using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq;
using Dapper;
using ECommerceWeb.Model;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly string _connectionString = "your_database_connection_string_here";

    [HttpPost("getRecentOrder")]
    public IActionResult GetRecentOrder([FromBody] OrderRequest request)
    {
        // Validate the request
        if (string.IsNullOrEmpty(request.User) || string.IsNullOrEmpty(request.CustomerId))
        {
            return BadRequest("Invalid request. User and CustomerId are required.");
        }

        // Retrieve data from the database using Dapper
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Get the customer
            var customer = connection.QueryFirstOrDefault<Customer>(
                "SELECT * FROM CUSTOMERS WHERE EMAIL = @Email AND CUSTOMERID = @CustomerId",
                new { Email = request.User, CustomerId = request.CustomerId });

            if (customer == null)
            {
                return BadRequest("Invalid request. User's email address does not match the customer number.");
            }

            // Get the most recent order for the customer
            var order = connection.QueryFirstOrDefault<OrderDetails>(
                "SELECT TOP 1 * FROM ORDERS WHERE CUSTOMERID = @CustomerId ORDER BY ORDERDATE DESC",
                new { CustomerId = request.CustomerId });

            if (order == null)
            {
                return Ok(new OrderResponse { Customer = customer });
            }

            // Get order items
            var orderItems = connection.Query<OrderItem>(
                "SELECT * FROM ORDERITEMS WHERE ORDERID = @OrderId",
                new { OrderId = order.OrderNumber }).ToList();

            // Replace product name with "Gift" if it contains a gift
            orderItems.ForEach(item =>
            {
                if (item.ContainsGift)
                {
                    item.ProductName = "Gift";
                }
            });

            // Build the response
            var response = new OrderResponse
            {
                Customer = customer,
                Order = new OrderDetails
                {
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    DeliveryAddress = $"{customer.HouseNo} {customer.Street}, {customer.Town}, {customer.PostCode}",
                    OrderItems = orderItems,
                    DeliveryExpected = order.DeliveryExpected
                }
            };

            return Ok(response);

        }
    }
}
