namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVoteService
    {
        Task SetVoteAsync(int trainerId, string userId, byte value);

        double GetAverageVotes(int trainerId);
    }
}
