using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crafts.Website.Models;
using Crafts.Website.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Crafts.Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public JsonFileProductService ProductService;
        
        // uses a private setter so users can't mess with data
        public IEnumerable<Product> Products { get; private set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            JsonFileProductService productService) // Adds the product service
        {
            _logger = logger;
            ProductService = productService;
        }
        // When someone "gets" this page, do the following...
        public void OnGet()
        {
            // Get the products from productService and sets it to a variable to be used in HTML
            Products = ProductService.GetProducts();
        }
    }
}
