namespace AllupMVCProject.Models;

public class ProductImage:BaseEntity
{
    public int ProductId { get; set; }
    public bool? IsCover { get; set; } //true = uz qabigi, false = Hower , null = detail sekilleri
    public string ImageUrl { get; set; }
    public Product Product { get; set; }
}
