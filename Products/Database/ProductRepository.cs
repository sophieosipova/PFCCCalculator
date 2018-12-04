using ProductsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProductsService.Models;
using System.Net;
using System.Threading.Tasks;
using SharedModels;


namespace ProductsService.Database
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext db;

        public ProductRepository(ProductsContext context)
        {
            this.db = context;
            if (!db.ProductsCategories.Any())
            {
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Овощи" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Фрукты" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Молочные продукты" });
                db.ProductsCategories.Add(new ProductsCategory { ProductsCategoryName = "Выпечка" });
                db.SaveChanges();
            }
            if (!db.Products.Any())
            {
                db.Products.Add(new Product { ProductName = "Яблоко", ProductsCategoryId = 2, Protein = 0, Fat = 0, Carbohydrates = 12, Calories = 60 });
                db.Products.Add(new Product { ProductName = "Сыр", ProductsCategoryId = 3, Protein = 15, Fat = 30, Carbohydrates = 1, Calories = 280 });
                db.Products.Add(new Product { ProductName = "Батон", ProductsCategoryId = 4, Protein = 1, Fat = 2, Carbohydrates = 50, Calories = 290 });
                db.SaveChanges();
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                return await db.Products.Where(p => p.UserId == 0).ToListAsync();
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<List<Product>> GetUsersProducts(int userId)
        {
            try
            {
                return await db.Products.Where(p => p.UserId == userId).ToListAsync();
            }
            catch
            {
                return null;
            }      
        }

        public async Task<List<ProductsCategory>> GetProductsCategories()
        {          
            try
            {
                return await db.ProductsCategories.ToListAsync();
            }
            catch
            {
                return null;
            }

        }
        public async Task<List<Product>> GetProductsByCategoryId(int productCategoryId)
        {
            // var product = await db.Products.Where(p => p.ProductsCategoryId == productCategoryId).ToListAsync();
            try
            {
                return await db.Products.Where(p => p.ProductsCategoryId == productCategoryId).ToListAsync();
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                return await db.Products.SingleOrDefaultAsync(ci => ci.ProductId == productId);
            }
            catch
            {
                return null;
            }
            
        }
        public async Task<Product> CreateProduct(int userId, [FromBody]Product product)
        {
            var item = new Product
            {
                UserId = userId,
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates,
                Calories = product.Calories
            };

            try
            {
                db.Products.Add(item);
                db.SaveChanges();
                return await db.Products.LastAsync();

            }
            catch
            {
                return null;
            }


            
        }

        public async Task<bool> DeleteProduct(int userId, int productId)
        {
            var product = db.Products.SingleOrDefault(x => x.ProductId == productId && x.UserId == userId);

            if (product == null)
                return false;

            try
            {
                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }


        public async Task<Product> UpdateProduct(int userId, Product productToUpdate)
        {
            var product = await db.Products
                .SingleOrDefaultAsync(i => i.ProductId == productToUpdate.ProductId);

            if (product == null)
                return null;

            try
            {
                product = productToUpdate;
                db.Products.Update(product);

                await db.SaveChangesAsync();

                return product;
            }
            catch
            {
                return null;
            }
        }


        public async Task<PaginatedModel<Product>> Items(int pageSize = 10, int pageIndex = 0)
        {

            try
            {
                var totalItems = await db.Products
                    .LongCountAsync();

                var itemsOnPage = await db.Products
                    .OrderBy(c => c.ProductName)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync();


                var model = new PaginatedModel<Product>(pageIndex, pageSize, totalItems, itemsOnPage);

                return model;
            }
            catch
            {
                return null;
            }
        }


    }
}
