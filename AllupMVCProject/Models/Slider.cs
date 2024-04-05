using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace AllupMVCProject.Models;

public class Slider:BaseEntity
{
	[Required]
	[StringLength(50)]
	public string Title { get; set; }
	[Required]
	[StringLength(250)]
	public string Desc { get; set; }
	public string RedirectUrl { get; set; }
	[StringLength(250)]
	public string? ImageUrl { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }

}
