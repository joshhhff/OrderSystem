using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystemApp.Data;

namespace OrderSystemApp.Controllers;

[Route("api/products/{id}")]
[ApiController]
public class GetProductDetailsController : ControllerBase
{
    private readonly SystemContext _context;

    public GetProductDetailsController(SystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProductDetails(int id)
    {
        var product = _context.Product
            .Where(p => p.ID == id)
            .Select(p => new 
            {
                p.Description,
                p.Rate,
                p.TaxRateString
            })
            .FirstOrDefault();

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}
