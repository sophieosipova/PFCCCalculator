using Moq;
using Xunit;
using PFCCCalculatorService.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using PFCCCalculator;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using PFCCCalculatorService.Models;
using System;
using SharedModels;
using Newtonsoft.Json;

namespace Tests.PFCCCalculatorServiceTests.ProductsController
{
    public class ProductControllerTests
    {
        public Mock<IProductsService> ProductsServiceMock = new Mock<IProductsService>();
        public Mock<IDishesService> DishesServiceMock = new Mock<IDishesService>();
        public Mock<ICommentsService> CommentsServiceMock = new Mock<ICommentsService>();
        //    public Mock<IGateway> CommentsServiceMock = new Mock<ICommentsService>();
        private TestServer _server;
        public HttpClient Client { get; set; }

        public ProductControllerTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>().ConfigureTestServices(services =>

            {
                services.AddSingleton<IProductsService>(ProductsServiceMock.Object);
                services.AddSingleton<IDishesService>(DishesServiceMock.Object);
                services.AddSingleton<ICommentsService>(CommentsServiceMock.Object);


            }));

            Client = _server.CreateClient();
        }

        [Fact]
        public async void GetProductsTest()
        {
            int pageSize = 1;
            int pageIndex = 0;
            ProductsServiceMock.Setup(c => c.Items(pageSize, pageIndex))
                .ReturnsAsync(GetTestPaginatedProducts);

            // Act
            var result = await Client.GetAsync($"/api/products?pageSize={pageSize}&pageIndex={pageIndex}");

            // Assert        
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var testmodel = GetTestPaginatedProducts();
            var model = Assert.IsType<PaginatedModel<ProductModel>>
                (JsonConvert.DeserializeObject<PaginatedModel<ProductModel>>(await result.Content.ReadAsStringAsync()));


            Assert.Equal(model.Count, testmodel.Count);
            Assert.Equal(model.Data, testmodel.Data);
            Assert.Equal(model.PageIndex, testmodel.PageIndex);
            Assert.Equal(model.PageSize, testmodel.PageSize);

        }


        [Fact]
        public async void NullGetProductsTest()
        {
            int pageSize = 1;
            int pageIndex = 0;
            ProductsServiceMock.Setup(c => c.Items(pageSize, pageIndex))
                .ReturnsAsync(GetTestNullPaginatedProducts);

            // Act
            var result = await Client.GetAsync($"/api/products?pageSize={pageSize}&pageIndex={pageIndex}");

            // Assert        
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        }

        [Fact]
        public async void GetProductsCategoriesTest()
        {

            ProductsServiceMock.Setup(c => c.GetProductsCategories())
                .ReturnsAsync(GetTestProductsCategories);

            // Act
            var result = await Client.GetAsync($"/api/products/categories");

            // Assert        
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var model = Assert.IsType<List<ProductsCategoryModel>>
                (JsonConvert.DeserializeObject<List<ProductsCategoryModel>>(await result.Content.ReadAsStringAsync()));

            Assert.Equal(model, GetTestProductsCategories());

        }

        [Fact]
        public async void GetProductsByCategoryIdTest()
        {
            int testCategoryId = 1;
            ProductsServiceMock.Setup(c => c.GetProductsByCategoryId(testCategoryId))
                .ReturnsAsync(GetTestProducts);

            // Act
            var result = await Client.GetAsync($"/api/products/categories/{testCategoryId}");

            // Assert        
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var model = Assert.IsType<List<ProductModel>>
                (JsonConvert.DeserializeObject<List<ProductModel>>(await result.Content.ReadAsStringAsync()));

            Assert.Equal(model, GetTestProducts());

        }

        private List<ProductModel> GetTestProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            products.Add(new ProductModel
            {
                ProductName = "Тест 1",
                ProductsCategoryId = 1,
                Protein = 1,
                Fat = 1,
                Carbohydrates = 1,
                Calories = 20,
                UserId = 2
            });
            products.Add(new ProductModel
            {
                ProductName = "Тест 2",
                ProductsCategoryId = 2,
                Protein = 0,
                Fat = 0,
                Carbohydrates = 1,
                Calories = 4,
                UserId = 1
            });

            return products;
        }

        private PaginatedModel<ProductModel> GetTestPaginatedProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            products.AddRange(GetTestProducts());
            


            PaginatedModel<ProductModel> paginatedModel = new PaginatedModel<ProductModel>
                (0, 1, 2, products);
            return paginatedModel;
        }


        private PaginatedModel<ProductModel> GetTestNullPaginatedProducts()
        {
            return null;
        }

        private List<ProductsCategoryModel> GetTestProductsCategories()
        {
            List<ProductsCategoryModel> productsCategory = new List<ProductsCategoryModel>();

            productsCategory.Add(new ProductsCategoryModel
            {
                ProductsCategoryId = 1,
                ProductsCategoryName = "Тест 1"

            });
            productsCategory.Add(new ProductsCategoryModel
            {
                ProductsCategoryId = 2,
                ProductsCategoryName = "Тест 2"

            });

            return productsCategory;
        }

    }
}
