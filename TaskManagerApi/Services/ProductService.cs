using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services;

public class ProductService(AppDbContext db) : IProductService
{
    public async Task<IEnumerable<Product>> GetAllAsync()
        => await db.Products.AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Nombre = dto.Nombre.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            FechaCreacion = DateTime.UtcNow
        };

        db.Products.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return false;

        product.Nombre = dto.Nombre.Trim();
        product.Precio = dto.Precio;
        product.Stock = dto.Stock;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return false;

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return true;
    }
}
