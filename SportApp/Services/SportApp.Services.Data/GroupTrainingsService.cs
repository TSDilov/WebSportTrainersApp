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
    using SportApp.Web.ViewModels.GroupTrainings;
    using SportApp.Web.ViewModels.Users;

    public class GroupTrainingsService : IGroupTrainingsService
    {
        private readonly IDeletableEntityRepository<GroupTraining> groupTrainingRepository;
        private readonly IRepository<Trainer> trainerRepository;
        private readonly IRepository<ApplicationUserGroupTraining> userGroupTrainingService;

        public GroupTrainingsService(
            IDeletableEntityRepository<GroupTraining> groupTrainingRepository,
            IRepository<Trainer> trainerRepository,
            IRepository<ApplicationUserGroupTraining> userGroupTrainingService)
        {
            this.groupTrainingRepository = groupTrainingRepository;
            this.trainerRepository = trainerRepository;
            this.userGroupTrainingService = userGroupTrainingService;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            return await this.groupTrainingRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetSearchedTrainingsAsync<T>(string looking)
        {
            return await this.groupTrainingRepository.All()
                .Where(x => x.Name.Contains(looking))
                .To<T>()
                .ToListAsync();
        }

        public async Task CreateAsync(CreateGroupTrainingInputModel input)
        {
            var trainer = await this.trainerRepository.All()
                .FirstOrDefaultAsync(x => x.Name == input.TrainerName);

            if (trainer == null)
            {
                throw new InvalidOperationException("This trainer don't exist");
            }

            var groupTraining = new GroupTraining
            {
                Name = input.Name,
                Description = input.Description,
                Place = input.Place,
                DaysOfWeek = input.DaysOfWeek,
                StartHour = input.StartHour,
                Trainer = trainer,
                TrainerUserId = input.TrainerUserId,
            };

            await this.groupTrainingRepository.AddAsync(groupTraining);
            await this.trainerRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id)
        {
            return this.groupTrainingRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task UpdateAsync(int id, EditGroupTrainingInputModel input)
        {
            var training = await this.groupTrainingRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);

            training.Name = input.Name;
            training.Description = input.Description;
            training.DaysOfWeek = input.DaysOfWeek;
            training.Place = input.Place;
            training.StartHour = input.StartHour;

            await this.groupTrainingRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var training = await this.groupTrainingRepository.All().FirstOrDefaultAsync(t => t.Id == id);
            this.groupTrainingRepository.Delete(training);
            await this.trainerRepository.SaveChangesAsync();
        }

        public async Task<bool> SignInForTrainingAsync(int id, string userId)
        {
            var groupTrainingUser = await this.userGroupTrainingService.All()
                .FirstOrDefaultAsync(x => x.GroupTrainingId == id && x.ApplicationUserId == userId);

            if (groupTrainingUser == null)
            {
                groupTrainingUser = new ApplicationUserGroupTraining
                {
                    GroupTrainingId = id,
                    ApplicationUserId = userId,
                };

                var training = await this.groupTrainingRepository.All()
                    .FirstOrDefaultAsync(t => t.Id == id);
                if (training == null)
                {
                    return false;
                }

                training.ApplicationUserGroupTrainings.Add(groupTrainingUser);
                await this.trainerRepository.SaveChangesAsync();
            }

            return true;
        }

        public async Task<BookedUsersViewModel> SighnInUsersAsync(int id)
        {
            var training = await this.groupTrainingRepository.All()
                .Include(x => x.ApplicationUserGroupTrainings)
                .ThenInclude(y => y.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (training == null)
            {
                return null;
            }

            var bookedUsersModel = new BookedUsersViewModel();
            var bookedUsersList = new List<BookedUserViewModel>();

            foreach (var user in training.ApplicationUserGroupTrainings)
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
            bookedUsersModel.TrainerName = training.Name;

            return bookedUsersModel;
        }
    }
}
