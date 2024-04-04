using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupMVCProject.Models;

public class Category:BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(200)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public ICollection<Product>? Products { get; set; }
}
