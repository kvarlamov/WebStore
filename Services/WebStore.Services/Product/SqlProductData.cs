using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData //Unit of work
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections
           // .Include(section => section.Products)
           .AsEnumerable();

        public IEnumerable<Brand> GetBrands() => _db.Brands
           // .Include(brand => brand.Products)
           .AsEnumerable();

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Domain.Entities.Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            return query.AsEnumerable().Select(p => new ProductDto
            {
                Id = p.Id,
                Brand = p.Brand is null ? null : new BrandDto
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                },
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            });
        }

        public ProductDto GetProductById(int id)
        {
            var product = _db.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Section)
                    .FirstOrDefault(p => p.Id == id);
            return new ProductDto
            {
                Id = product.Id,
                Brand = product.Brand is null ? null : new BrandDto
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Name = product.Name,
                Price = product.Price,
                Order = product.Order,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
