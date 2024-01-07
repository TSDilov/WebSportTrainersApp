namespace SportApp.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels;

    public class ChatController : BaseController
    {
        private readonly IMessageService messageService;

        public ChatController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [Authorize]
        public IActionResult Chat()
        {
            var model = new ChatViewModel
            {
                Messages = this.messageService.GetAll(),
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var message = this.messageService.GetById(id);
            if (message == null)
            {
                return this.RedirectToAction(nameof(this.Chat));
            }

            if (this.User.Identity.Name == message.User)
            {
                await this.messageService.DeleteAsync(id);
            }

            return this.RedirectToAction(nameof(this.Chat));
        }
    }
}
