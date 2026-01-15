using System.ComponentModel.DataAnnotations;

namespace GestionStock.Client.Models
{
    public class ProductForm
    {
        // validation client
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public IEnumerable<int> Categories { get; set; } = [4];
    }
}
