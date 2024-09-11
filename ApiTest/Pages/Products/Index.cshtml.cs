using ApiTest.Contracts.Models;
using ApiTest.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiTest.Pages.Products
{
    /// <summary>
    /// Represents the model for the product index page, handling pagination and filtering.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class with the specified product service.
        /// </summary>
        /// <param name="productService">The product service to retrieve product data.</param>
        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets or sets the list of products displayed on the page.
        /// </summary>
        public IEnumerable<Product> Products { get; set; } = [];

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of products to display per page.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the date to filter products that were updated after this value.
        /// </summary>
        public DateTime? UpdatedAfter { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages based on the current page size.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are more pages available for pagination.
        /// </summary>
        public bool HasMorePages { get; set; }

        /// <summary>
        /// Handles the HTTP GET request for the products page, applying pagination and optional filtering.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination (default is 1).</param>
        /// <param name="pageSize">The number of products to display per page (default is 10).</param>
        /// <param name="updatedAfter">Filters products updated after this date (optional).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// The method retrieves products from the service, applies pagination, and determines if there are more pages.
        /// Example usage:
        /// 
        ///     GET /Products/Index?pageNumber=1&amp;pageSize=10&amp;updatedAfter=2023-01-01
        /// </remarks>
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10, DateTime? updatedAfter = null)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            UpdatedAfter = updatedAfter;

            var productsQuery = await _productService.GetAllProductsAsync(updatedAfter);
            var filteredProducts = productsQuery.OrderBy(p => p.Name);

            TotalItems = filteredProducts.Count(); // Total count for pagination

            // Calculate total pages and ensure at least 1 page if there are items
            TotalPages = TotalItems > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 1;

            // Ensure page number does not exceed total pages
            if (PageNumber > TotalPages)
            {
                PageNumber = TotalPages > 0 ? TotalPages : 1; // Default to page 1 if there are no products
            }

            var productsList = filteredProducts
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize + 1) // Fetch one more item to check for more pages
                .ToList();

            Products = productsList.Take(PageSize).ToList();
            HasMorePages = productsList.Count > PageSize;
        }




    }
}
