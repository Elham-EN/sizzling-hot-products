
namespace API.Models
{
    // Represents a customer order (loaded from orders.json)
    // An order can be completed or cancelled, and contains one or more items.
    public class Order
    {
        public required string OrderId { get; set; }
        // Identifies who placed the order. Nullable because cancelled 
        // orders in the JSON don't always include it.
        public string? CustomerId { get; set; }
        // The list of products in the order. Nullable because cancelled orders
        // in the JSON have no entries field
        public List<OrderItem>? Entries { get; set; }
        // The date the order was placed
        public required string Date { get; set; }
        // Either the order is "completed" or "cancelled"
        public required string Status { get; set; }
    }
}