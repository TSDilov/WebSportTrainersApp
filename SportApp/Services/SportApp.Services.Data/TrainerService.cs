namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using SportApp.Services.Mapping;
    using SportApp.Web.ViewModels.Trainers;
    using SportApp.Web.ViewModels.Users;

    using static System.Net.Mime.MediaTypeNames;

    public class TrainerService : ITrainerService
    {
        private readonly IDeletableEntityRepository<Trainer> trainerRepository;
        private readonly IRepository<ApplicationUserTrainer> aplicationUserTrainer;
        private readonly IDeletableEntityRepository<RequestTrainer> requestTrainerRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public TrainerService(
            IDeletableEntityRepository<Trainer> trainerRepository,
            IRepository<ApplicationUserTrainer> aplicationUserTrainer,
            IDeletableEntityRepository<RequestTrainer> requestTrainerRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            this.trainerRepository = trainerRepository;
            this.aplicationUserTrainer = aplicationUserTrainer;
            this.requestTrainerRepository = requestTrainerRepository;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<bool> BookTrainerAsync(int id, string userId)
        {
            var trainerUser = await this.aplicationUserTrainer.All()
                .FirstOrDefaultAsync(x => x.TrainerId == id && x.ApplicationUserId == userId);

            if (trainerUser == null)
            {
                trainerUser = new ApplicationUserTrainer
                {
                    TrainerId = id,
                    ApplicationUserId = userId,
                };

                var trainer = await this.trainerRepository.All()
                    .FirstOrDefaultAsync(t => t.Id == id);
                if (trainer == null)
                {
                    return false;
                }

                trainer.Rating += 0.5M;
                trainer.ApplicationUsersTrainers.Add(trainerUser);
                await this.trainerRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task CreateAsync(CreateTrainerInputModel input, string imagePath)
        {
            var trainer = new Trainer
            {
                Name = input.Name,
                InfoCard = input.InfoCard,
                DateOfBirth = input.DateOfBirth,
                PricePerTraining = input.PricePerTraining,
                CategotyId = input.CategoryId,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
            };

            var allowedExtensions = new[] { "jpg", "png", "gif", "jfif" };
            Directory.CreateDirectory($"{imagePath}/trainerss/");
            foreach (var image in input.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!allowedExtensions.Contains(extension))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new SportApp.Data.Models.Image
                {
                    Extension = extension,
                };

                trainer.Images.Add(dbImage);
                var physicalPath = $"{imagePath}/trainerss/{dbImage.Id}.{extension}";
                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            await this.trainerRepository.AddAsync(trainer);
            await this.trainerRepository.SaveChangesAsync();

            var user = await this.userManager.FindByNameAsync(input.Username);
            user.OwnTrainerId = trainer.Id.ToString();
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trainer = await this.trainerRepository.All().FirstOrDefaultAsync(t => t.Id == id);
            this.trainerRepository.Delete(trainer);
            await this.trainerRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12)
        {
            return await this.trainerRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetSearchedTrainersAsync<T>(string looking, int page, int itemsPerPage = 12)
        {
            return await this.trainerRepository.AllAsNoTracking()
                .Where(x => x.Name.Contains(looking))
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public T GetById<T>(int id)
        {
            return this.trainerRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<BookedUsersViewModel> BookedUsersAsync(int id)
        {
            var trainer = await this.trainerRepository.All()
                .Include(x => x.ApplicationUsersTrainers)
                .ThenInclude(y => y.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (trainer == null)
            {
                return null;
            }

            var bookedUsersModel = new BookedUsersViewModel();
            var bookedUsersList = new List<BookedUserViewModel>();

            foreach (var user in trainer.ApplicationUsersTrainers)
            {
                var currentUser = new BookedUserViewModel
                {
                    Username = user.ApplicationUser.UserName,
                    Email = user.ApplicationUser?.Email,
                    PhoneNumber = user.ApplicationUser?.PhoneNumber,
                };

                bookedUsersList.Add(currentUser);
            }

            bookedUsersModel.BookedUsers = bookedUsersList;
            bookedUsersModel.TrainerName = trainer.Name;

            return bookedUsersModel;
        }

        public int GetCount()
        {
            return this.trainerRepository.All().Count();
        }

        public async Task UpdateAsync(int id, EditTrainerInputModel input)
        {
            var trainer = await this.trainerRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);

            trainer.Name = input.Name;
            trainer.InfoCard = input.InfoCard;
            trainer.Email = input.Email;
            trainer.PhoneNumber = input.PhoneNumber;
            trainer.DateOfBirth = input.DateOfBirth;
            trainer.PricePerTraining = input.PricePerTraining;
            trainer.CategotyId = input.CategoryId;

            await this.trainerRepository.SaveChangesAsync();
        }

        public async Task RequestTrainerAsync(RequestTrainerInputModel input, string email, string phone)
        {
            var requestTrainer = new RequestTrainer
            {
                UserId = input.UserId,
                Username = input.Username,
                Name = input.Name,
                InfoCard = input.InfoCard,
                DateOfBirth = input.DateOfBirth,
                PricePerTraining = input.PricePerTraining,
                CategoryOfTraining = input.CategoryOfTraining,
            };

            requestTrainer.IsApproved = false;
            requestTrainer.Email = email;
            requestTrainer.PhoneNumber = phone;

            await this.requestTrainerRepository.AddAsync(requestTrainer);
            await this.requestTrainerRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrainerRequestViewModel>> GetAllTrainersRequestsAsync()
        {
            var trainerRequests = await this.requestTrainerRepository.All()
                .Where(x => x.IsApproved == false)
                .To<TrainerRequestViewModel>()
                .ToListAsync();

            return trainerRequests;
        }

        public async Task<IEnumerable<T>> GetTopTrainersAsync<T>()
        {
            return await this.trainerRepository.AllAsNoTracking()
                .Where(x => x.Rating > 0)
                .To<T>()
                .ToListAsync();
        }
    }
}
