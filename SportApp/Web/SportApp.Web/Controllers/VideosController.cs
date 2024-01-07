namespace SportApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using SportApp.Common;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels;

    public class VideosController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IVideoSurvice videoSurvice;

        public VideosController(IWebHostEnvironment environment, IVideoSurvice videoSurvice)
        {
            this.environment = environment;
            this.videoSurvice = videoSurvice;
        }

        [Authorize]
        public async Task<IActionResult> All()
        {
            var videos = new ListWithVideosViewModel
            {
                Videos = await this.videoSurvice.GetAllAsync(),
            };

            return this.View(videos);
        }

        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public IActionResult Add()
        {
            var videoModel = new VideoModel();
            return this.View(videoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public async Task<IActionResult> Add(VideoModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                await this.videoSurvice.CreateAsync(model, $"{this.environment.WebRootPath}");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(model);
            }

            return this.RedirectToAction("All");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(string id)
        {
            await this.videoSurvice.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
