using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupMVCProject.Models;

public class Banner:BaseEntity
{
    [Required]
    [StringLength(200)]
    public string ImageUrl { get; set; }
    [Required]
    [StringLength(200)]
    public string RedirectUrl { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }    
}
