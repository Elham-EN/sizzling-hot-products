
namespace API.Services
{
    // Defines what a service can do (Rules)
    // Nullable because there might be no orders at all for the date you query
    // So return null signals to no product found
    public interface ISizzlingHotProductService
    {
        string? GetTopProductForDay(DateOnly date);

        string? GetTopProductForPeriod(DateOnly from, DateOnly to);
    }
}