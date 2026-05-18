using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizzlingHotProductsController(ISizzlingHotProductService service) : ControllerBase
{
    // GET /api/sizzlinghotproducts/daily?date=2026-04-21
    // Date must be in yyyy-MM-dd format (e.g. 2026-04-21) as required by DateOnly query binding
    // Returns the product bought by the most unique customers on a single day
    [HttpGet("daily")]
    public IActionResult GetDaily([FromQuery] DateOnly date)
    {
        var result = service.GetTopProductForDay(date);
        if (result is null)
            return NotFound("No sales found for that date.");
        return Ok(new { product = result });
    }

    // GET /api/sizzlinghotproducts/period?from=2026-04-19&to=2026-04-21
    // Returns the product bought by the most unique customers over a 3-day (or any) range
    [HttpGet("period")]
    public IActionResult GetPeriod([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        if (from > to)
            return BadRequest("'from' must be on or before 'to'.");

        var result = service.GetTopProductForPeriod(from, to);
        if (result is null)
            return NotFound("No sales found for that period.");
        return Ok(new { product = result });
    }
}
