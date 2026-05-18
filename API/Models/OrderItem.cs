
namespace API.Models
{
    // Represents a single product line within an order
    public class OrderItem
    {
        public required string Id { get; set; }
        // How many units of that product were purchased
        // Ignored by the business rules (rule 1)
        public int Quantity { get; set; }
    }
}