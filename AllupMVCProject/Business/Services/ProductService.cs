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

    public Task UpdateAsync(Product product)
    {
        //product CurrentProduct = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
        //if (CurrentBook == null) throw new NotFoundException("This book is not found!");
        //if ((await _context.Books.FirstOrDefaultAsync(b => b.Name == book.Name) is not null) && (book.Name != CurrentBook.Name))
        //{
        //    throw new NameAlreadyExistException("name", "This Name is already exist!");
        //}
        //if (book.CoverImageFile is not null)
        //{
        //    if (book.CoverImageFile.ContentType != "image/jpeg" && book.CoverImageFile.ContentType != "image/png")
        //    {
        //        throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
        //    }
        //    if (book.CoverImageFile.Length > 2097152)
        //    {
        //        throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
        //    }
        //    string fileName = book.CoverImageFile.FileName;
        //    if (fileName.Length > 64)
        //    {
        //        fileName = fileName.Substring(fileName.Length - 64, 64);
        //    }
        //    fileName = Guid.NewGuid().ToString() + fileName;


        //    string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

        //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        book.CoverImageFile.CopyTo(fileStream);
        //    }

        //    BookImage? coverImage = await _context.BookImages.Where(bi => bi.IsCover == true).Where(bi => bi.BookId == CurrentBook.Id).FirstOrDefaultAsync();

        //    if (coverImage is not null)
        //    {
        //        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", coverImage.ImageUrl);
        //        if (File.Exists(path2))
        //        {
        //            File.Delete(path2);
        //        }
        //        _context.BookImages.Remove(coverImage);

        //    }
        //    BookImage bookImage = new BookImage()
        //    {
        //        Book = book,
        //        ImageUrl = fileName,
        //        IsCover = true,
        //        IsActivated = true,
        //        CreatedDate = DateTime.UtcNow.AddHours(4),
        //    };
        //    await _context.BookImages.AddAsync(bookImage);
        //}
        //if (book.HoverImageFile is not null)
        //{
        //    if (book.HoverImageFile.ContentType != "image/jpeg" && book.HoverImageFile.ContentType != "image/png")
        //    {
        //        throw new InvalidContentTypeException("CoverImageFile", "Please,You enter jpeg or png file");
        //    }
        //    if (book.HoverImageFile.Length > 2097152)
        //    {
        //        throw new SizeOfFileException("CoverImageFile", "Please,You just can send low size file from 2 mb!");
        //    }
        //    string fileName = book.HoverImageFile.FileName;
        //    if (fileName.Length > 64)
        //    {
        //        fileName = fileName.Substring(fileName.Length - 64, 64);
        //    }
        //    fileName = Guid.NewGuid().ToString() + fileName;

        //    string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

        //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        book.HoverImageFile.CopyTo(fileStream);
        //    }

        //    BookImage? hoverImage = await _context.BookImages.Where(bi => bi.IsCover == false).Where(bi => bi.BookId == CurrentBook.Id).FirstOrDefaultAsync();
        //    if (hoverImage is not null)
        //    {
        //        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", hoverImage.ImageUrl);
        //        if (File.Exists(path2))
        //        {
        //            File.Delete(path2);
        //        }
        //        _context.BookImages.Remove(hoverImage);
        //    }

        //    BookImage bookImage = new BookImage()
        //    {
        //        Book = book,
        //        ImageUrl = fileName,
        //        IsCover = false,
        //        IsActivated = true,
        //        CreatedDate = DateTime.UtcNow.AddHours(4),
        //    };

        //    await _context.BookImages.AddAsync(bookImage);
        //    }

        //    foreach (var ImageFile in CurrentBook.BookImages.Where(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsCover == null))
        //    {
        //        string path2 = Path.Combine(_env.WebRootPath, "uploads/sliders", ImageFile.ImageUrl);
        //        if (File.Exists(path2))
        //        {
        //            File.Delete(path2);
        //        }
        //    }

        //    CurrentBook.BookImages.RemoveAll(bi => !book.BookImageIds.Contains(bi.Id) && bi.IsCover == null);

        //    if (book.ImageFiles is not null)
        //    {
        //        foreach (var ImageFile in book.ImageFiles)
        //        {
        //            if (ImageFile.ContentType != "image/jpeg" && ImageFile.ContentType != "image/png")
        //            {
        //                throw new InvalidContentTypeException("ImageFile", "Please,You enter jpeg or png file");
        //            }
        //            if (ImageFile.Length > 2097152)
        //            {
        //                throw new SizeOfFileException("ImageFile", "Please,You just can send low size file from 2 mb!");
        //            }
        //            string fileName = ImageFile.FileName;
        //            if (fileName.Length > 64)
        //            {
        //                fileName = fileName.Substring(fileName.Length - 64, 64);
        //            }
        //            fileName = Guid.NewGuid().ToString() + fileName;

        //            string path = Path.Combine(_env.WebRootPath, "uploads/Books", fileName);

        //            using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //            {
        //                ImageFile.CopyTo(fileStream);
        //            }

        //            BookImage bookImage = new BookImage()
        //            {
        //                Book = book,
        //                ImageUrl = fileName,
        //                IsCover = null,
        //                IsActivated = true,
        //                CreatedDate = DateTime.UtcNow.AddHours(4),
        //            };

        //            _context.BookImages.Add(bookImage);
        //        }

        //    }
        //    CurrentBook.Name = book.Name;
        //    CurrentBook.UpdatedDate = DateTime.UtcNow.AddHours(4);
        //    CurrentBook.Desc = book.Desc;
        //    CurrentBook.CostPrice = book.CostPrice;
        //    CurrentBook.SellPrice = book.SellPrice;
        //    CurrentBook.Discount = book.Discount;
        //    CurrentBook.AuthorId = book.AuthorId;
        //    CurrentBook.IsFeatured = book.IsFeatured;
        //    CurrentBook.MostView = book.MostView;
        //    CurrentBook.BookImages = book.BookImages;
        //    CurrentBook.GenreId = book.GenreId;
        //    CurrentBook.IsNew = book.IsNew;
        //    CurrentBook.IsActivated = book.IsActivated;
        //    CurrentBook.StockCount = book.StockCount;
        //    CurrentBook.ProductCode = book.ProductCode;

        //    await _context.SaveChangesAsync();
        throw new NotImplementedException();
    }
    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Products.AsQueryable();
        query= _GetIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).ToListAsync()
            : await query.ToListAsync();
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
        return expression is not null
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
