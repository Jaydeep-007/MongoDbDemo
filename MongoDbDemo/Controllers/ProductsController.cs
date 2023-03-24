using Microsoft.AspNetCore.Mvc;
using MongoDbDemo.Entities;
using MongoDbDemo.Repositories;

namespace MongoDbDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService) =>
            this.productService = productService;

        [HttpGet]
        public async Task<List<ProductDetails>> Get() =>
            await productService.ProductListAsync();

        [HttpGet("{productId:length(24)}")]
        public async Task<ActionResult<ProductDetails>> Get(string productId)
        {
            var productDetails = await productService.GetProductDetailByIdAsync(productId);

            if (productDetails is null)
            {
                return NotFound();
            }

            return productDetails;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDetails productDetails)
        {
            await productService.AddProductAsync(productDetails);

            return CreatedAtAction(nameof(Get), new { id = productDetails.Id }, productDetails);
        }

        [HttpPut("{productId:length(24)}")]
        public async Task<IActionResult> Update(string productId, ProductDetails productDetails)
        {
            var productDetail = await productService.GetProductDetailByIdAsync(productId);

            if (productDetail is null)
            {
                return NotFound();
            }

            productDetails.Id = productDetail.Id;

            await productService.UpdateProductAsync(productId, productDetails);

            return NoContent();
        }

        [HttpDelete("{productId:length(24)}")]
        public async Task<IActionResult> Delete(string productId)
        {
            var productDetails = await productService.GetProductDetailByIdAsync(productId);

            if (productDetails is null)
            {
                return NotFound();
            }

            await productService.DeleteProductAsync(productId);

            return NoContent();
        }
    }
}
