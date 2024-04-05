using AllupMVCProject.Business.Interfaces;
using AllupMVCProject.DAL;
using AllupMVCProject.Models;
using AllupMVCProject.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace AllupMVCProject.Business.Services;

public class ProductService : IProductService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;
    public ProductService(AllupDbContext context,IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task CreateAsync(Product product)
    {
        if (product.CoverImageFile is not null)
        {
            if (product.CoverImageFile.ContentType != "image/jpeg" && product.CoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
            }
            if (product.CoverImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string fileName = product.CoverImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                product.CoverImageFile.CopyTo(fileStream);
            }

            ProductImage productImage = new ProductImage()
            {
                Product = product,
                ImageUrl = fileName,
                IsCover = true,
            };
            await _context.ProductImages.AddAsync(productImage);
        }
        else
        {
            
            throw new RequiredPropertyException("CoverImageFile", "This area is required!");
            
        }


        if (product.HoverImageFile is not null)
        {
            if (product.HoverImageFile.ContentType != "image/jpeg" && product.HoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("HoverImageFile", "Please,You enter jpeg or png file");
            }
            if (product.HoverImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("HoverImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string fileName = product.HoverImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                product.HoverImageFile.CopyTo(fileStream);
            }

            ProductImage productImage = new ProductImage()
            {
                Product = product,
                ImageUrl = fileName,
                IsCover = false,
            };
            await _context.ProductImages.AddAsync(productImage);
        }
        else
        {
            throw new RequiredPropertyException("HoverImageFile", "This area is required!");
        }


        if (product.ImageFiles is not null)
        {
            foreach (var ImageFile in product.ImageFiles)
            {
                if (ImageFile.ContentType != "image/jpeg" && ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                }
                if (ImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                ProductImage productImage = new ProductImage()
                {
                    Product = product,
                    ImageUrl = fileName,
                    IsCover = null,
                };
                await _context.ProductImages.AddAsync(productImage);
            }

        }
        else
        {
            throw new RequiredPropertyException("ImageFiles", "This area is required!");
        }


        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        Product CurrentProduct = await _context.Products.FirstOrDefaultAsync(b => b.Id == product.Id);
        if (CurrentProduct == null) throw new NotFoundException("This product is not found!");
        if ((await _context.Products.FirstOrDefaultAsync(b => b.Name == product.Name) is not null) && (product.Name != CurrentProduct.Name))
        {
            throw new AlreadyExistException("name", "This Name is already exist!");
        }
        if (product.CoverImageFile is not null)
        {
            if (product.CoverImageFile.ContentType != "image/jpeg" && product.CoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
            }
            if (product.CoverImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string fileName = product.CoverImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;


            string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                product.CoverImageFile.CopyTo(fileStream);
            }

            ProductImage? coverImage = await _context.ProductImages.Where(bi => bi.IsCover == true).Where(bi => bi.ProductId == CurrentProduct.Id).FirstOrDefaultAsync();

            if (coverImage is not null)
            {
                string path2 = Path.Combine(_env.WebRootPath, "Uploads/Products", coverImage.ImageUrl);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
                _context.ProductImages.Remove(coverImage);

            }
            ProductImage productImage = new ProductImage()
            {
                Product = product,
                ImageUrl = fileName,
                IsCover = true,
                IsActivated = true,
                CreatedDate = DateTime.UtcNow.AddHours(4),
            };
            await _context.ProductImages.AddAsync(productImage);
        }
        if (product.HoverImageFile is not null)
        {
            if (product.HoverImageFile.ContentType != "image/jpeg" && product.HoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
            }
            if (product.HoverImageFile.Length > 2097152)
            {
                throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
            }
            string fileName = product.HoverImageFile.FileName;
            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;

            string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                product.HoverImageFile.CopyTo(fileStream);
            }

            ProductImage? hoverImage = await _context.ProductImages.Where(bi => bi.IsCover == false).Where(bi => bi.ProductId == CurrentProduct.Id).FirstOrDefaultAsync();
            if (hoverImage is not null)
            {
                string path2 = Path.Combine(_env.WebRootPath, "Uploads/Products", hoverImage.ImageUrl);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
                _context.ProductImages.Remove(hoverImage);
            }

            ProductImage productImage = new ProductImage()
            {
                Product = product,
                ImageUrl = fileName,
                IsCover = false,
                IsActivated = true,
                CreatedDate = DateTime.UtcNow.AddHours(4),
            };

            await _context.ProductImages.AddAsync(productImage);
        }

        foreach (var ImageFile in CurrentProduct.ProductImages.Where(bi => !product.ProductImageIds.Contains(bi.Id) && bi.IsCover == null))
        {
            string path2 = Path.Combine(_env.WebRootPath, "Uploads/Products", ImageFile.ImageUrl);
            if (File.Exists(path2))
            {
                File.Delete(path2);
            }
        }

        CurrentProduct.ProductImages.RemoveAll(bi => !product.ProductImageIds.Contains(bi.Id) && bi.IsCover == null);

        if (product.ImageFiles is not null)
        {
            foreach (var ImageFile in product.ImageFiles)
            {
                if (ImageFile.ContentType != "image/jpeg" && ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
                }
                if (ImageFile.Length > 2097152)
                {
                    throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
                }
                string fileName = ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                fileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "Uploads/Products", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    ImageFile.CopyTo(fileStream);
                }

                ProductImage productImage = new ProductImage()
                {
                    Product = product,
                    ImageUrl = fileName,
                    IsCover = null,
                    IsActivated = true,
                    CreatedDate = DateTime.UtcNow.AddHours(4),
                };

                _context.ProductImages.Add(productImage);
            }

        }
        CurrentProduct.Name = product.Name;
        CurrentProduct.UpdatedDate = DateTime.UtcNow.AddHours(4);
        CurrentProduct.Desc = product.Desc;
        CurrentProduct.CostPrice = product.CostPrice;
        CurrentProduct.SalePrice = product.SalePrice;
        CurrentProduct.DiscountPercent = product.DiscountPercent;
        CurrentProduct.CategoryId = product.CategoryId;
        CurrentProduct.IsFeatured = product.IsFeatured;
        CurrentProduct.IsBestSeller = product.IsBestSeller;
        CurrentProduct.ProductImages = product.ProductImages;   //
        CurrentProduct.BrandId = product.BrandId;
        CurrentProduct.IsNew = product.IsNew;
        CurrentProduct.IsActivated = product.IsActivated;
        CurrentProduct.StockCount = product.StockCount;
        CurrentProduct.ProductCode = product.ProductCode;

        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        Product? deletedProduct = await _context.Products.FirstOrDefaultAsync(b => b.Id == id);
        if (deletedProduct is null) throw new NotFoundException("Product is not found!");


            ProductImage? coverImage = await _context.ProductImages.Where(bi => bi.IsCover == true).Where(bi => bi.ProductId == deletedProduct.Id).FirstOrDefaultAsync();

            string url = coverImage.ImageUrl;

            string path2 = Path.Combine(_env.WebRootPath, "Uploads/Products", coverImage.ImageUrl);
            if (File.Exists(path2))
            {   
                File.Delete(path2);
            }
            _context.ProductImages.Remove(coverImage);

        
 
            ProductImage? hoverImage = await _context.ProductImages.Where(bi => bi.IsCover == false).Where(bi => bi.ProductId == deletedProduct.Id).FirstOrDefaultAsync();


            string path3 = Path.Combine(_env.WebRootPath, "Uploads/Products", hoverImage.ImageUrl);
            if (File.Exists(path3))
            {
                File.Delete(path3);
            }
            _context.ProductImages.Remove(hoverImage);

        
   
            List<ProductImage> bookImage1 = await _context.ProductImages.Where(bi => bi.IsCover == null).Where(bi => bi.ProductId == deletedProduct.Id).ToListAsync();
            foreach (var bookImage in bookImage1)
            {
                string path4 = Path.Combine(_env.WebRootPath, "Uploads/Products", bookImage.ImageUrl);
                if (File.Exists(path4))
                {
                    File.Delete(path4);
                }
                _context.ProductImages.Remove(bookImage);
            }
        
        _context.Products.Remove(deletedProduct);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Products.AsQueryable();
        query= _GetIncludes(query, includes);
        if (expression != null)
        {
            return await query.Where(expression).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        Product product = await _context.Products.FindAsync(id);
        return product;
    }

    public async Task<Product> GetSingleAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Products.AsQueryable();
        query=_GetIncludes(query, includes);
        return (expression is not null)
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }

    private IQueryable<Product> _GetIncludes(IQueryable<Product> query, params string[] includes)
    {
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }
}
