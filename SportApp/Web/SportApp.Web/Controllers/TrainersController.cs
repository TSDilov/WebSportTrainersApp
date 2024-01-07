namespace SportApp.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SportApp.Common;
    using SportApp.Data.Models;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels.Comment;
    using SportApp.Web.ViewModels.GroupTrainings;
    using SportApp.Web.ViewModels.Trainers;

    public class TrainersController : Controller
    {
        private readonly ITrainerService trainerService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly ICommentsService commentsService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ITrainerRequestsService trainerRequestService;

        public TrainersController(
            ITrainerService trainerService,
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            ICommentsService commentsService,
            RoleManager<ApplicationRole> roleManager,
            ITrainerRequestsService trainerRequestService)
        {
            this.trainerService = trainerService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.environment = environment;
            this.commentsService = commentsService;
            this.roleManager = roleManager;
            this.trainerRequestService = trainerRequestService;
        }

        public async Task<IActionResult> All(int id = 1)
        {
            const int ItemsPerPage = 12;
            var viewModel = new TrainersListViewModel
            {
                PageNumber = id,
                Trainers = await this.trainerService.GetAllAsync<TrainerInListViewModel>(id, ItemsPerPage),
                TrainersCount = this.trainerService.GetCount(),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> All(string looking, int id = 1)
        {
            const int ItemsPerPage = 12;
            var viewModel = new TrainersListViewModel
            {
                PageNumber = id,
                Trainers = await this.trainerService.GetSearchedTrainersAsync<TrainerInListViewModel>(looking, id, ItemsPerPage),
                TrainersCount = this.trainerService.GetCount(),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            var viewModel = new CreateTrainerInputModel();
            viewModel.CategoryItems = this.categoriesService.GetAllCategoriesAsKeyValuePairs();
            viewModel.Rating = 0;
            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Create(CreateTrainerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoryItems = this.categoriesService.GetAllCategoriesAsKeyValuePairs();
                return this.View(input);
            }

            input.Rating = 0;
            var user = await this.userManager.FindByNameAsync(input.Username);
            try
            {
                await this.trainerService.CreateAsync(input, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult ById(int id)
        {
            var trainer = this.trainerService.GetById<SingleTrainerViewModel>(id);
            if (trainer == null)
            {
                return this.RedirectToAction("All");
            }

            return this.View(trainer);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var model = this.trainerService.GetById<EditTrainerInputModel>(id);
            if (model == null)
            {
                return this.RedirectToAction(nameof(this.All));
            }

            model.CategoryItems = this.categoriesService.GetAllCategoriesAsKeyValuePairs();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, EditTrainerInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.CategoryItems = this.categoriesService.GetAllCategoriesAsKeyValuePairs();
                return this.View(model);
            }

            await this.trainerService.UpdateAsync(id, model);
            return this.RedirectToAction(nameof(this.ById), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.trainerService.DeleteAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> Book(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var result = await this.trainerService.BookTrainerAsync(id, user.Id);
            if (!result)
            {
                return this.RedirectToAction(nameof(this.All));
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult ShowComments(int id)
        {
            var model = new CommentsViewModel();
            if (this.User.Identity.IsAuthenticated)
            {
                model.Comments = this.commentsService.GetTrainerComments(id);
                model.TrainerId = id;
                return this.View(model);
            }

            return this.RedirectToAction("Login", "User");
        }

        [Authorize(Roles = GlobalConstants.TrainerRoleName)]
        public async Task<IActionResult> BookedUsers(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var bookedUsers = await this.trainerService.BookedUsersAsync(id);
            if (bookedUsers != null && user.OwnTrainerId == id.ToString())
            {
                return this.View(bookedUsers);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [Authorize]
        public async Task<IActionResult> RequestTrainerForm()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!user.RequestTrainer)
            {
                var model = new RequestTrainerInputModel();
                return this.View(model);
            }

            return this.Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RequestTrainerForm(RequestTrainerInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            try
            {
                user.RequestTrainer = true;
                await this.trainerService.RequestTrainerAsync(input, user.Email, user.PhoneNumber);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(string.Empty, ex.Message);
                return this.View(input);
            }

            return this.RedirectToAction("All", "Trainers");
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> AllRequestsForTrainer()
        {
            var viewModel = new TrainerRequestsViewModel
            {
                TrainersRequests = await this.trainerService.GetAllTrainersRequestsAsync(),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> ApprovedTrainer(string userId, int requestTrainerId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            await this.userManager.AddToRoleAsync(user, GlobalConstants.TrainerRoleName);
            await this.trainerRequestService.ApprovedAsync(requestTrainerId);
            return this.RedirectToAction("AllRequestsForTrainer", "Trainers");
        }
    }
}
