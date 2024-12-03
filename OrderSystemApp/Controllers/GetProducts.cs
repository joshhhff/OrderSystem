using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystemApp.Data;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly SystemContext _context;

    // Constructor injection for SystemContext
    public ProductsController(SystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        // Use the injected context to fetch products
        var products = new SelectList(_context.Product, "ID", "Name");
        return Ok(products);
    }
}