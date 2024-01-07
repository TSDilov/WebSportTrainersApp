namespace SportApp.Services.Data
{
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MessageService : IMessageService
    {
        private readonly IDeletableEntityRepository<Message> messageRepository;

        public MessageService(IDeletableEntityRepository<Message> messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public async Task CreateAsync(Message input)
        {
            await this.messageRepository.AddAsync(input);
            await this.messageRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await this.messageRepository.All().FirstOrDefaultAsync(m => m.Id == id);
            this.messageRepository.Delete(message);
            await this.messageRepository.SaveChangesAsync();
        }

        public IEnumerable<Message> GetAll()
        {
            return this.messageRepository.All().ToList();
        }

        public Message GetById(int id)
        {
            return this.messageRepository.All().FirstOrDefault(x => x.Id == id);
        }
    }
}
