// Goal: Find which product was bought by the most unique customers in a given day or period

using API.Models;

namespace API.Services
{
    // Core Business Logic
    public class SizzlingHotProductService : ISizzlingHotProductService
    {
        private readonly List<Order> orders;
        private readonly List<Product> products;

        public SizzlingHotProductService(List<Order> orders, List<Product> products)
        {
            this.orders = orders;
            this.products = products;
        }

        // A single day is just a period where from and to are the same date.
        public string? GetTopProductForDay(DateOnly date)
        {
            return GetTopProduct(date, date);
        }

        public string? GetTopProductForPeriod(DateOnly from, DateOnly to)
        {
            return GetTopProduct(from, to);
        }

        // Core logic shared by both methods. (avoid duplicating logic)
        private string? GetTopProduct(DateOnly from, DateOnly to)
        {
            // Step 1: Filter orders that fall within the date range
            var ordersInRange = orders
                .Where(o =>
                    {
                        // This parses the date string from the JSON into DateOnly
                        var orderDate = DateOnly.ParseExact(o.Date, "dd/MM/yyyy");
                        return orderDate >= from && orderDate <= to;
                    })
                .ToList();

            // Step 2:  Find cancelled order IDs and remove those completed orders
            // First collect the OrderIds of all cancelled orders into a HashSet (fast lookup)
            var cancelledOrderIds = ordersInRange
                .Where(o  => o.Status == "cancelled")
                .Select(o => o.OrderId)
                .ToHashSet();
            // Remove both the cancelled orders and their original completed orders
            var validOrders = ordersInRange
                // Then we filter out any order with those IDs
                .Where(o => !cancelledOrderIds.Contains(o.OrderId))
                .ToList();
            
            // Step 3: For each valid completed order, get unique (customerId, productId, date)
            // combinations — deduplication is automatic via HashSet (Rules 1 & 2)
            var uniqueSales = validOrders
                .Where(o => o.Status == "completed" && o.Entries != null)
                .SelectMany(o => o.Entries!.Select(item =>
                    (CustomerId: o.CustomerId!, ProductId: item.Id, Date: o.Date)))
                .ToHashSet();

            // Step 4: Count how many unique sales each product has
            var productSalesCounts = uniqueSales
                .GroupBy(sale => sale.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    SalesCount = group.Count()
                });

            // Step 5: Highest count wins — alphabetical product name as tie-breaker (Rule 4)
            var winnerId = productSalesCounts
                .OrderByDescending(p => p.SalesCount)
                .ThenBy(p => products.FirstOrDefault(pr => pr.Id == p.ProductId)?.Name)
                .FirstOrDefault()?.ProductId;

            // Step 6: Return the winning product name, or null if no sales found
            return products.FirstOrDefault(p => p.Id == winnerId)?.Name;
        }

    }
}