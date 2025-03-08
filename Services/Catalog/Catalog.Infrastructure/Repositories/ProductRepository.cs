using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _catalogContext;
    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }
    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams) 
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if(!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            var searchFilter = builder.Regex(x=>x.Name, new MongoDB.Bson.BsonRegularExpression(catalogSpecParams.Search));
            filter &= searchFilter;
        }
        if(!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            var brandFilter = builder.Eq(x => x.Brands.Id,catalogSpecParams.BrandId);
            filter &= brandFilter;
        }
        if(!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            var typeFilter = builder.Eq(x => x.Types.Id, catalogSpecParams.TypeId);
            filter &= typeFilter;
        }
        if(!string.IsNullOrEmpty(catalogSpecParams.Sort))
        {
            return new Pagination<Product>
            {
                PageSize = catalogSpecParams.PageSize,
                PageIndex = catalogSpecParams.PageIndex,
                Data = await DataFilter(catalogSpecParams, filter),
                Count = await _catalogContext.Products.CountDocumentsAsync(p =>
                    true) //TODO: Need to check while applying with UI
            };
        }
        return new Pagination<Product>() 
        {
            PageSize = catalogSpecParams.PageSize,
            PageIndex = catalogSpecParams.PageIndex,
            Data = await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex-1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync(),
            Count=await _catalogContext.Products.CountDocumentsAsync(p=>true)
        };
    }
    public async Task<Product> CreateProduct(Product product)
    {
        await _catalogContext
            .Products
            .InsertOneAsync(product);
        return product;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult result = await _catalogContext.Products.DeleteOneAsync(filterDefinition);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _catalogContext
            .Brands
            .Find(b => true)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return await _catalogContext
            .Types
            .Find(t => true)
            .ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _catalogContext
            .Products
            .Find(p=> p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrand(string name)
    {
        FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _catalogContext
            .Products
            .Find(filterDefinition)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p=>p.Name, name);
        return await _catalogContext
            .Products
            .Find(filterDefinition)
            .ToListAsync();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var result = await _catalogContext
           .Products
           .ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        switch (catalogSpecParams.Sort)
        {
            case "priceAsc":
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            case "priceDesc":
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending("Price"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
            default:
                return await _catalogContext
                    .Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending("Name"))
                    .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
                    .Limit(catalogSpecParams.PageSize)
                    .ToListAsync();
        }
    }
}
