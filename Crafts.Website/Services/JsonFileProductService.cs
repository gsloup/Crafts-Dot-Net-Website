using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Crafts.Website.Models;
using Microsoft.AspNetCore.Hosting;

namespace Crafts.Website.Services
{
    public class JsonFileProductService
    {
        // This is the constructor
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        // Used to route to the specific data file in the webHostEnvironment
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }
        
        // Deserializing the JSON data into something that can be actually used
        public IEnumerable<Product> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Returns a list of Products
                return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public void AddRating(string productId, int rating)
        {
            var products = GetProducts();

            // Retrieving the product by its productId
            var query = products.First(x => x.Id == productId);

            // If you don't find a rating, create a new list
            if(query.Ratings == null)
            {
                query.Ratings = new int[] { rating };
            }
            // If you find some ratings, convert them to a list
            else
            {
                var ratings = query.Ratings.ToList();
                ratings.Add(rating);
                query.Ratings = ratings.ToArray();
            }

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<Product>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                ); 
            }
        }
    }
}