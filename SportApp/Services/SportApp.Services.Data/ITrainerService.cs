namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Web.ViewModels.Trainers;
    using SportApp.Web.ViewModels.Users;

    public interface ITrainerService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12);

        Task<IEnumerable<T>> GetTopTrainersAsync<T>();

        Task<IEnumerable<T>> GetSearchedTrainersAsync<T>(string looing, int page, int itemsPerPage = 12);

        Task CreateAsync(CreateTrainerInputModel input, string imagePath);

        T GetById<T>(int id);

        Task UpdateAsync(int id, EditTrainerInputModel input);

        Task DeleteAsync(int id);

        int GetCount();

        Task<bool> BookTrainerAsync(int id, string userId);

        Task<BookedUsersViewModel> BookedUsersAsync(int id);

        Task RequestTrainerAsync(RequestTrainerInputModel input, string emali, string phone);

        Task<IEnumerable<TrainerRequestViewModel>> GetAllTrainersRequestsAsync();
    }
}
