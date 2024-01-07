namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Web.ViewModels.Administration.Categories;
    using SportApp.Web.ViewModels.Trainers;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllCategoriesAsKeyValuePairs();

        IEnumerable<CategoryViewModel> All();

        Task CreateAsync(CategoryInputModel input);

        Task<EditCategoryInputModel> GetByIdAsync(int id);

        Task UpdateAsync(int id, EditCategoryInputModel input);

        Task DeleteAsync(int id);
    }
}
