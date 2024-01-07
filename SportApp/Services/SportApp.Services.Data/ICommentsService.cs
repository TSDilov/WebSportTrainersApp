namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Web.ViewModels.Comment;

    public interface ICommentsService
    {
        Task CreateAsync(CommentInputModel input, string userId);

        IEnumerable<CommentViewModel> GetTrainerComments(int id);

        CommentViewModel GetById(int id);

        Task DeleteAsync(int id);
    }
}
