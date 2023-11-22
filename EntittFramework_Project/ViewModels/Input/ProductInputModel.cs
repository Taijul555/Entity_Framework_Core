using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EntittFramework_Project.Models;

namespace EntittFramework_Project.ViewModels.Input
{
    public class ProductInputModel
    {
        public int ProductId { get; set; }
        [Required, StringLength(50)]
        public string ProductName { get; set; } = default!;
        [Required, EnumDataType(typeof(Size))]
        public Size Size { get; set; } = default!;
        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Required]
        public IFormFile Picture { get; set; } = default!;

        public bool OnSale { get; set; }
    }
}
