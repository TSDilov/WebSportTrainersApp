namespace SportApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels.Administration.Categories;

    [Area("Administration")]
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: Administration/Categories
        public IActionResult Index()
        {
            var model = this.categoryService.All();
            return this.View(model);
        }

        // GET: Administration/Categories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryInputModel input)
        {
            if (this.ModelState.IsValid)
            {
                await this.categoryService.CreateAsync(input);
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(input);
        }

        // GET: Administration/Categories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(category);
        }

        // POST: Administration/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCategoryInputModel input)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.categoryService.UpdateAsync(id, input);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return this.View(input);
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(input);
        }

        // GET: Administration/Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await this.categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/Categories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.categoryService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
