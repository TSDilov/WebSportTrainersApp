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
    using SportApp.Web.ViewModels.Comment;
    using Xunit;

    public class TrainerRequestsServiceTests
    {
        private ITrainerRequestsService service;
        private ApplicationDbContext applicationDbContext;

        [Fact]
        public async Task ApproveTrainer()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var trainerRequestsRepo = new EfDeletableEntityRepository<RequestTrainer>(this.applicationDbContext);
            this.service = new TrainerRequestsService(trainerRequestsRepo);

            var request = new RequestTrainer
            {
                Id = 1,
                UserId = "userId",
                Name = "Test",
                Username = "Test",
                Email = "test@abv.bg",
                InfoCard = "Test Subject",
                PhoneNumber = "0888888888",
                DateOfBirth = DateTime.Now,
                PricePerTraining = 10,
                CategoryOfTraining = "Test",
                IsApproved = false,
            };

            await trainerRequestsRepo.AddAsync(request);
            await trainerRequestsRepo.SaveChangesAsync();

            await this.service.ApprovedAsync(1);

            Assert.True(request.IsApproved);
        }
    }
}
