namespace SportApp.Services.Data
{
    using SportApp.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMessageService
    {
        IEnumerable<Message> GetAll();

        Message GetById(int id);

        Task CreateAsync(Message input);

        Task DeleteAsync(int id);
    }
}
