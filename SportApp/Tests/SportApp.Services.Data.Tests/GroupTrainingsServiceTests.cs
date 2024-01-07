namespace SportApp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data;
    using SportApp.Data.Models;
    using SportApp.Data.Repositories;
    using SportApp.Web.ViewModels.Administration.Categories;
    using SportApp.Web.ViewModels.GroupTrainings;
    using Xunit;

    public class GroupTrainingsServiceTests
    {
        private IGroupTrainingsService service;
        private ApplicationDbContext applicationDbContext;

        [Fact]
        public async Task CreateGroupTrainingThrowsExceptionIfTrainerDoesNotExist()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var groupTrainingRepo = new EfDeletableEntityRepository<GroupTraining>(this.applicationDbContext);
            var trainerRepo = new EfRepository<Trainer>(this.applicationDbContext);
            var userGroupTrainingsRepo = new EfRepository<ApplicationUserGroupTraining>(this.applicationDbContext);
            this.service = new GroupTrainingsService(groupTrainingRepo, trainerRepo, userGroupTrainingsRepo);

            var groupTraining = new CreateGroupTrainingInputModel
            {
                Name = "Test",
                Description = "Test description",
                Place = "Test place",
                DaysOfWeek = "Test days",
                TrainerUserId = "trainerId",
                TrainerName = "Test Trainer",
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.service.CreateAsync(groupTraining));
        }
    }
}
