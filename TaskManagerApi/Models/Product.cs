using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public DateTime FechaCreacion { get; set; }
}
