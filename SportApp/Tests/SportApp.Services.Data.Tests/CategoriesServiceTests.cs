namespace SportApp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Moq;
    using SportApp.Data;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using SportApp.Data.Repositories;
    using SportApp.Web.ViewModels.Administration.Categories;
    using Xunit;

    public class CategoriesServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Category>> categoryRepo = new Mock<IDeletableEntityRepository<Category>>();
        private readonly List<Category> categories;
        private ICategoriesService service;
        private ApplicationDbContext applicationDbContext;

        public CategoriesServiceTests()
        {
            this.categories = new List<Category>();
        }

        [Fact]
        public async Task CreateCategoryAndGetAllCategories()
        {
            this.categoryRepo.Setup(x => x.All()).Returns(this.categories.AsQueryable());
            this.categoryRepo.Setup(x => x.AddAsync(It.IsAny<Category>()))
                .Callback((Category category) => this.categories.Add(category));

            var category = new CategoryInputModel
            {
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            this.service = new CategoriesService(this.categoryRepo.Object);
            await this.service.CreateAsync(category);

            var categories = this.service.All();

            Assert.Equal(1, this.categories.Count);
            Assert.Contains(categories, x => x.Name == "Test");
            Assert.DoesNotContain(categories, x => x.Name == "NotTest");
        }

        [Fact]
        public async Task DeleteCategory()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var categoryRepo = new EfDeletableEntityRepository<Category>(this.applicationDbContext);
            this.service = new CategoriesService(categoryRepo);

            var category = new Category
            {
                Id = 2,
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveChangesAsync();

            await this.service.DeleteAsync(2);

            Assert.Equal(0, categoryRepo.All().ToList().Count);
            Assert.DoesNotContain(categoryRepo.All().ToList(), x => x.Name == "Test");
            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllCategoriesAsKeyValuePairs()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var categoryRepo = new EfDeletableEntityRepository<Category>(this.applicationDbContext);
            this.service = new CategoriesService(categoryRepo);

            var category = new Category
            {
                Id = 3,
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveChangesAsync();

            var category2 = new Category
            {
                Id = 4,
                Name = "A test",
                CreatedOn = DateTime.Now,
            };

            await categoryRepo.AddAsync(category2);
            await categoryRepo.SaveChangesAsync();

            var categoriesLikeKeyValuePairs = this.service.GetAllCategoriesAsKeyValuePairs();

            Assert.NotNull(categoriesLikeKeyValuePairs);

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCategoryByIdAsync()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var categoryRepo = new EfDeletableEntityRepository<Category>(this.applicationDbContext);
            this.service = new CategoriesService(categoryRepo);

            var category = new Category
            {
                Id = 5,
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveChangesAsync();

            var gettedCategory = await this.service.GetByIdAsync(5);

            Assert.Equal("Test", gettedCategory.Name);
            Assert.Contains(categoryRepo.All().ToList(), x => x.Name == "Test");
            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task UpdateCategory()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var categoryRepo = new EfDeletableEntityRepository<Category>(this.applicationDbContext);
            this.service = new CategoriesService(categoryRepo);

            var category = new Category
            {
                Id = 6,
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveChangesAsync();

            var editedCategory = new EditCategoryInputModel { Name = "TestUpdated" };

            await this.service.UpdateAsync(6, editedCategory);

            Assert.Equal("TestUpdated", categoryRepo.All().FirstOrDefault().Name);
            Assert.DoesNotContain(categoryRepo.All().ToList(), x => x.Name == "Test");
            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }
    }
}
