namespace SportApp.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using SportApp.Data.Models;
    using SportApp.Services.Data;
    using SportApp.Web.ViewModels;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task Send(string message)
        {
            var newMessage = new Message { User = this.Context.User.Identity.Name, Text = message, };
            await this.Clients.All.SendAsync(
                "NewMessage",
                newMessage);
            await this.messageService.CreateAsync(newMessage);
        }
    }
}
