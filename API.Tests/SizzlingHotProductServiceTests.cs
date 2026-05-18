using API.Models;
using API.Services;

namespace API.Tests;

public class SizzlingHotProductServiceTests
{
    static readonly DateOnly Day = new(2024, 1, 15);

    // Builds the service with the given orders and products
    static SizzlingHotProductService Svc(List<Order> orders, List<Product> products) => new(orders, products);

    // Shorthand for creating a product
    static Product Prod(string id, string name) => new() { Id = id, Name = name };

    // Shorthand for creating a completed order with one or more product entries
    static Order Done(string id, string customer, string date, params string[] items) => new()
    {
        OrderId = id, CustomerId = customer, Date = date, Status = "completed",
        Entries = [.. items.Select(i => new OrderItem { Id = i, Quantity = 1 })]
    };

    // Shorthand for creating a cancelled order (no customer or entries)
    static Order Cancelled(string id, string date) => new() { OrderId = id, Date = date, Status = "cancelled" };

    // Two customers buy Alpha, one buys Beta → expects Alpha as the winner
    [Fact]
    public void Returns_top_product()
    {
        var products = new List<Product> { Prod("p1", "Alpha"), Prod("p2", "Beta") };
        var orders = new List<Order>
        {
            Done("o1", "c1", "15/01/2024", "p1"),
            Done("o2", "c2", "15/01/2024", "p1"),
            Done("o3", "c3", "15/01/2024", "p2"),
        };

        Assert.Equal("Alpha", Svc(orders, products).GetTopProductForDay(Day));
    }

    // o1 is completed then cancelled — both records share the same OrderId so both are dropped
    // Only o3 (Beta) survives → expects Beta
    [Fact]
    public void Excludes_cancelled_orders()
    {
        var products = new List<Product> { Prod("p1", "Alpha"), Prod("p2", "Beta") };
        var orders = new List<Order>
        {
            Done("o1", "c1", "15/01/2024", "p1"),
            Cancelled("o1", "15/01/2024"),
            Done("o3", "c2", "15/01/2024", "p2"),
        };

        Assert.Equal("Beta", Svc(orders, products).GetTopProductForDay(Day));
    }

    // c1 buys Alpha twice but counts as one unique sale, c2 buys Beta once
    // Both products have 1 unique customer → tie broken alphabetically → expects Alpha
    [Fact]
    public void Deduplicates_repeat_customer_purchases()
    {
        var products = new List<Product> { Prod("p1", "Alpha"), Prod("p2", "Beta") };
        var orders = new List<Order>
        {
            Done("o1", "c1", "15/01/2024", "p1"),
            Done("o2", "c1", "15/01/2024", "p1"),
            Done("o3", "c2", "15/01/2024", "p2"),
        };

        Assert.Equal("Alpha", Svc(orders, products).GetTopProductForDay(Day));
    }

    // Zebra and Apple each bought by one unique customer → equal counts
    // Expects Apple because it comes first alphabetically
    [Fact]
    public void Breaks_tie_alphabetically()
    {
        var products = new List<Product> { Prod("p1", "Zebra"), Prod("p2", "Apple") };
        var orders = new List<Order>
        {
            Done("o1", "c1", "15/01/2024", "p1"),
            Done("o2", "c2", "15/01/2024", "p2"),
        };

        Assert.Equal("Apple", Svc(orders, products).GetTopProductForDay(Day));
    }

    // No orders exist for the day → expects null (no winner)
    [Fact]
    public void Returns_null_when_no_orders()
    {
        Assert.Null(Svc([], [Prod("p1", "Alpha")]).GetTopProductForDay(Day));
    }

    // Alpha bought on 14th and 15th (2 customers), Beta on 16th (1 customer)
    // All three days fall within the period → expects Alpha
    [Fact]
    public void Period_spans_multiple_days()
    {
        var products = new List<Product> { Prod("p1", "Alpha"), Prod("p2", "Beta") };
        var orders = new List<Order>
        {
            Done("o1", "c1", "14/01/2024", "p1"),
            Done("o2", "c2", "15/01/2024", "p1"),
            Done("o3", "c3", "16/01/2024", "p2"),
        };

        var result = Svc(orders, products).GetTopProductForPeriod(new(2024, 1, 14), new(2024, 1, 16));
        Assert.Equal("Alpha", result);
    }
}
