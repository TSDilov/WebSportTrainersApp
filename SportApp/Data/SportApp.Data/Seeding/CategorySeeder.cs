namespace SportApp.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;

    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Boxing Training" });
            await dbContext.Categories.AddAsync(new Category { Name = "Fitness Training" });
            await dbContext.Categories.AddAsync(new Category { Name = "Conditional Training" });

            await dbContext.SaveChangesAsync();
        }
    }
}
