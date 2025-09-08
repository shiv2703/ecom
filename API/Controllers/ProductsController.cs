using System;
using System.Runtime.InteropServices;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext context;

    public ProductsController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Products>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        return product != null ? product : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Products>> CreateProduct(Products product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Products product)
    {
        if (product.Id != id || !await productExists(id))
        {
            return BadRequest("Cannot update the product");
        }

        context.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }
      [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return NoContent();
    }
    
    public async Task<bool> productExists(int id)
    {
        return await context.Products.AnyAsync(x => x.Id == id);
    }

  
}

