using ApiTest.Contracts.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiTest.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IEnumerable<Product> Products { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? UpdatedAfter { get; set; }
        public bool HasMorePages { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10, DateTime? updatedAfter = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            UpdatedAfter = updatedAfter;

            var productsQuery = await _productService.GetAllProductsAsync(updatedAfter);

            var filteredProducts = productsQuery
                .OrderBy(p => p.Name);

            var totalProducts = filteredProducts.Count(); // Total count for pagination

            var productsList = filteredProducts
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize + 1) // Fetch one more item to check for more pages
                .ToList();

            Products = productsList.Take(PageSize).ToList();
            HasMorePages = productsList.Count > PageSize;
        }


    }
}
