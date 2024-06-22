using Catalog.Core.Entities;
using Catalog.Core.Repositories;
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
    public async Task<IEnumerable<Product>> GetProducts() 
    {
        return await _catalogContext
            .Products
            .Find(p => true)
            .ToListAsync();
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
        FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Brand.Name, name);
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
}
