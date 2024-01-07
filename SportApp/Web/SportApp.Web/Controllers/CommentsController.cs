namespace SportApp.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels.Comment;
    using SportApp.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post(CommentInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.commentsService.CreateAsync(model, userId);
            return this.Ok();
        }

        [Authorize]
        [Route("Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var comment = this.commentsService.GetById(id);
            if (comment == null)
            {
                return this.RedirectToAction("All", "Trainers");
            }

            if (comment.ApplicationUserId == userId)
            {
                await this.commentsService.DeleteAsync(id);
            }

            return this.RedirectToAction("All", "Trainers");
        }
    }
}
