using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbDemo.Configurations;
using MongoDbDemo.Entities;

namespace MongoDbDemo.Repositories
{
    public class ProductService :IProductService
    {
        private readonly IMongoCollection<ProductDetails> productCollection;

        public ProductService(
         IOptions<ProductDBSettings> productDatabaseSetting)
        {
            var mongoClient = new MongoClient(
                productDatabaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                productDatabaseSetting.Value.DatabaseName);

            productCollection = mongoDatabase.GetCollection<ProductDetails>(
                productDatabaseSetting.Value.ProductCollectionName);
        }

        public async Task<List<ProductDetails>> ProductListAsync()
        {
            return await productCollection.Find(_ => true).ToListAsync();
        }

        public async Task<ProductDetails> GetProductDetailByIdAsync(string productId)
        {
            return await productCollection.Find(x => x.Id == productId).FirstOrDefaultAsync();
        }

        public async Task AddProductAsync(ProductDetails productDetails)
        {
            await productCollection.InsertOneAsync(productDetails);
        }

        public async Task UpdateProductAsync(string productId, ProductDetails productDetails)
        {
             await productCollection.ReplaceOneAsync(x => x.Id == productId, productDetails);
        }

        public async Task DeleteProductAsync(string productId)
        {
            await productCollection.DeleteOneAsync(x => x.Id == productId);
        }
    }
}
