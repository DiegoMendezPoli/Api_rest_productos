using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = [];
    private readonly object _lock = new();
    private int _nextId = 1;

    public IEnumerable<Product> GetAll()
    {
        lock (_lock)
        {
            return _products
                .Select(product => new Product
                {
                    Id = product.Id,
                    Nombre = product.Nombre,
                    Precio = product.Precio,
                    Stock = product.Stock,
                    FechaCreacion = product.FechaCreacion
                })
                .ToList();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is null
                ? null
                : new Product
                {
                    Id = product.Id,
                    Nombre = product.Nombre,
                    Precio = product.Precio,
                    Stock = product.Stock,
                    FechaCreacion = product.FechaCreacion
                };
        }
    }

    public Product Create(CreateProductDto dto)
    {
        lock (_lock)
        {
            var product = new Product
            {
                Id = _nextId++,
                Nombre = dto.Nombre.Trim(),
                Precio = dto.Precio,
                Stock = dto.Stock,
                FechaCreacion = DateTime.UtcNow
            };

            _products.Add(product);
            return product;
        }
    }

    public bool Update(int id, UpdateProductDto dto)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                return false;
            }

            product.Nombre = dto.Nombre.Trim();
            product.Precio = dto.Precio;
            product.Stock = dto.Stock;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product is null)
            {
                return false;
            }

            _products.Remove(product);
            return true;
        }
    }
}
