using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Create(CreateProductDto dto);
    bool Update(int id, UpdateProductDto dto);
    bool Delete(int id);
}
