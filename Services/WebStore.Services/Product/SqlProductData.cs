﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData //Unit of work
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections
           // .Include(section => section.Products)
           .AsEnumerable();

        public Section GetSectionById(int id) => _db.Sections.FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => _db.Brands
           // .Include(brand => brand.Products)
           .AsEnumerable();

        public Brand GetBrandById(int id) => _db.Brands.FirstOrDefault(b => b.Id == id);

        public PagedProductDto GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Domain.Entities.Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            var totalCount = query.Count();

            if (Filter?.PageSize != null)
            {
                query = query
                    .Skip((Filter.Page - 1) * (int) Filter.PageSize)
                    .Take((int) Filter.PageSize);
            }

            return new PagedProductDto
            {
                Products = query.AsEnumerable().Select(ProductMapper.ToDto),
                TotalCount = totalCount
            };
            // return query.AsEnumerable().Select(ProductMapper.ToDto);
        }

        public ProductDto GetProductById(int id)
        {
            return _db.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Section)
                    .FirstOrDefault(p => p.Id == id).ToDto();
        }
    }
}
