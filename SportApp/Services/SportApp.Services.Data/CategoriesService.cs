namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using SportApp.Services.Mapping;
    using SportApp.Web.ViewModels.Administration.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<CategoryViewModel> All()
        {
            var categories = this.categoryRepository.All().ToList();

            var listWithCategories = new List<CategoryViewModel>();

            foreach (var category in categories)
            {
                var categoryViewModel = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    CreatedOn = category.CreatedOn,
                };

                listWithCategories.Add(categoryViewModel);
            }

            return listWithCategories;
        }

        public async Task CreateAsync(CategoryInputModel input)
        {
            var category = new Category
            {
                Name = input.Name,
                CreatedOn = input.CreatedOn,
            };

            await this.categoryRepository.AddAsync(category);
            await this.categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await this.categoryRepository.All().FirstOrDefaultAsync(t => t.Id == id);
            this.categoryRepository.Delete(category);
            await this.categoryRepository.SaveChangesAsync();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs()
        {
            return this.categoryRepository.All().Select(x => new
            {
                x.Id,
                x.Name,
            })
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public async Task<EditCategoryInputModel> GetByIdAsync(int id)
        {
            var category = await this.categoryRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return null;
            }

            return new EditCategoryInputModel
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public async Task UpdateAsync(int id, EditCategoryInputModel input)
        {
            var category = await this.categoryRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);

            category.Name = input.Name;
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
