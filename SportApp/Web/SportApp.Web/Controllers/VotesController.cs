namespace SportApp.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : BaseController
    {
        private readonly IVoteService voteservice;

        public VotesController(IVoteService voteservice)
        {
            this.voteservice = voteservice;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostVoteResponceModel>> Post(PostVoteInputModel model)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.voteservice.SetVoteAsync(model.TrainerId, userId, model.Value);
            var averageVotes = this.voteservice.GetAverageVotes(model.TrainerId);
            return new PostVoteResponceModel { AverageVote = averageVotes };
        }
    }
}
