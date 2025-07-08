using Microsoft.AspNetCore.Mvc;

namespace ControllersAndRouting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Product 1", Price = 10.99m },
        new Product { Id = 2, Name = "Product 2", Price = 20.50m }
    };

    // GET: api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(Products);
    }

    // GET api/products/5
    [HttpGet("{id}")]
    public ActionResult<Product> Get(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    // POST api/products
    [HttpPost]
    public ActionResult<Product> Post([FromBody] Product product)
    {
        product.Id = Products.Max(p => p.Id) + 1;
        Products.Add(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    // DELETE api/products/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        Products.Remove(product);
        return NoContent();
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}