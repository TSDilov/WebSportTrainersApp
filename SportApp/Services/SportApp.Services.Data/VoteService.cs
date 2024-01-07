namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;

    public class VoteService : IVoteService
    {
        private readonly IRepository<Vote> votesRepository;

        public VoteService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public double GetAverageVotes(int trainerId)
        {
            return this.votesRepository.All()
                .Where(x => x.TrainerId == trainerId)
                .Average(x => x.Value);
        }

        public async Task SetVoteAsync(int trainerId, string userId, byte value)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.TrainerId == trainerId && x.UserId == userId);
            if (vote == null)
            {
                vote = new Vote
                {
                    TrainerId = trainerId,
                    UserId = userId,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = value;
            await this.votesRepository.SaveChangesAsync();
        }
    }
}
