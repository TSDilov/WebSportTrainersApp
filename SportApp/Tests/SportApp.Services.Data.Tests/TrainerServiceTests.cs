namespace SportApp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using SportApp.Data;
    using SportApp.Data.Models;
    using SportApp.Data.Repositories;
    using SportApp.Web.ViewModels.Comment;
    using Xunit;

    public class TrainerServiceTests
    {
        private ITrainerService service;
        private ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        [Fact]
        public async Task BookedTrainerReturnFalseIfTrainerDoesNotExist()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var trainerRepo = new EfDeletableEntityRepository<Trainer>(this.applicationDbContext);
            var userTrainerRepo = new EfRepository<ApplicationUserTrainer>(this.applicationDbContext);
            var requestTrainerRepo = new EfDeletableEntityRepository<RequestTrainer>(this.applicationDbContext);
            this.service = new TrainerService(trainerRepo, userTrainerRepo, requestTrainerRepo, this.userManager, this.applicationDbContext);

            var result = await this.service.BookTrainerAsync(2, "user1");

            Assert.False(result);
        }

        [Fact]
        public async Task BookedTrainerReturnTrueIfTrainerExist()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var trainerRepo = new EfDeletableEntityRepository<Trainer>(this.applicationDbContext);
            var userTrainerRepo = new EfRepository<ApplicationUserTrainer>(this.applicationDbContext);
            var requestTrainerRepo = new EfDeletableEntityRepository<RequestTrainer>(this.applicationDbContext);
            this.service = new TrainerService(trainerRepo, userTrainerRepo, requestTrainerRepo, this.userManager, this.applicationDbContext);

            var trainer = new Trainer
            {
                Id = 1,
                Name = "Test",
                InfoCard = "Test",
                Email = "Test",
                PhoneNumber = "0888888888",
                DateOfBirth = DateTime.Now,
                PricePerTraining = 10,
            };

            await trainerRepo.AddAsync(trainer);
            await trainerRepo.SaveChangesAsync();

            var result = await this.service.BookTrainerAsync(1, "user1");

            Assert.True(result);
        }

        [Fact]
        public async Task CreateTrainer()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var trainerRepo = new EfDeletableEntityRepository<Trainer>(this.applicationDbContext);
            var userTrainerRepo = new EfRepository<ApplicationUserTrainer>(this.applicationDbContext);
            var requestTrainerRepo = new EfDeletableEntityRepository<RequestTrainer>(this.applicationDbContext);
            this.service = new TrainerService(trainerRepo, userTrainerRepo, requestTrainerRepo, this.userManager, this.applicationDbContext);

            var trainer = new Trainer
            {
                Id = 1,
                Name = "Test",
                InfoCard = "Test",
                Email = "Test",
                PhoneNumber = "0888888888",
                DateOfBirth = DateTime.Now,
                PricePerTraining = 10,
            };

            await trainerRepo.AddAsync(trainer);
            await trainerRepo.SaveChangesAsync();

            var result = await this.service.BookTrainerAsync(1, "user1");

            Assert.True(result);
        }
    }
}
