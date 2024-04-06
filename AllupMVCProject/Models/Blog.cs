using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupMVCProject.Models;

public class Blog:BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }
    [Required]
    [StringLength(250)]
    public string Desc { get; set; }
    [Required]
    [StringLength(200)]
    public string RedirectUrl { get; set; }
    [StringLength(100)]
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? Imagefile { get; set; }

}
