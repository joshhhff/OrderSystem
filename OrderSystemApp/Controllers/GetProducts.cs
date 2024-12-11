using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystemApp.Data;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly SystemContext _context;

    public ProductsController(SystemContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = new SelectList(_context.Product, "ID", "Name");
        return Ok(products);
    }
}