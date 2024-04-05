using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Security;

namespace AllupMVCProject.Models;

public class FeaturesBanner:BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }
    [Required]
    [StringLength (250)]    
    public string Desc { get; set; }
    [StringLength (100)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
