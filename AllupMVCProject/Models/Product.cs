﻿

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AllupMVCProject.Models;

public class Product:BaseEntity
{
    [Required]
    [StringLength (50)]
    public string Name { get; set; }
    [Required]
    [StringLength(240)]
    public string Desc { get; set; }
    [Required]
    public double CostPrice { get; set; }
    [Required]
    public double SalePrice { get; set; }
    public int DiscountPercent { get; set; }
    public int StockCount {  get; set; }
    [Required]
    [StringLength (50)]
    public string ProductCode { get; set; }
    public bool IsNew { get; set; }
    public bool IsBestSeller { get; set; }
    public bool IsFeatured { get; set; }
    [NotMapped]
    public IFormFile? HoverImageFile { get; set; }
    [NotMapped]
    public IFormFile? CoverImageFile { get; set; }
    [NotMapped]
    public List<IFormFile>? ImageFiles { get; set; }
    public List<ProductImage>? ProductImages { get; set; }
    public List<int>? ProductImageIds { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }

}
