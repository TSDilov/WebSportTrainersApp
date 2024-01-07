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
    using SportApp.Web.ViewModels.Comment;

    public class CommentsServise : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentRepository;

        public CommentsServise(
            IDeletableEntityRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task CreateAsync(CommentInputModel input, string userId)
        {
            var comment = new Comment
            {
                TrainerId = input.TrainerId,
                Name = input.Name,
                Email = input.Email,
                Subject = input.Subject,
                Message = input.Message,
                ApplicationUserId = userId,
            };

            await this.commentRepository.AddAsync(comment);
            await this.commentRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await this.commentRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.commentRepository.Delete(comment);
            await this.commentRepository.SaveChangesAsync();
        }

        public CommentViewModel GetById(int id)
        {
            return this.commentRepository.All()
                .Where(x => x.Id == id)
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Subject = x.Subject,
                    Message = x.Message,
                    ApplicationUserId = x.ApplicationUserId,
                    TrainerId = x.TrainerId,
                })
                .FirstOrDefault();
        }

        public IEnumerable<CommentViewModel> GetTrainerComments(int id)
        {
            var comments = this.commentRepository.All()
                .Where(c => c.TrainerId == id)
                .ToList();

            var commentsView = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                var commentViewModel = new CommentViewModel
                {
                    Id = comment.Id,
                    Name = comment.Name,
                    Email = comment.Email,
                    Subject = comment.Subject,
                    Message = comment.Message,
                    ApplicationUserId = comment.ApplicationUserId,
                    TrainerId = comment.TrainerId,
                };

                commentsView.Add(commentViewModel);
            }

            return commentsView;
        }
    }
}
