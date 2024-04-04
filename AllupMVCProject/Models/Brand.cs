using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupMVCProject.Models;

public class Brand:BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    [StringLength(200)]
    public string? LogoUrl { get; set; }
    [NotMapped]
    public IFormFile? LogoImageFile { get; set; }
    public ICollection<Product>? Products { get; set; }
}
