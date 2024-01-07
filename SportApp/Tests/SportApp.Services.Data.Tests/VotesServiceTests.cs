namespace SportApp.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using Xunit;

    public class VotesServiceTests
    {
        [Fact]
        public async Task WhenUserVoteTwoTimesOnlyOneVoteShouldCounted()
        {
            var list = new List<Vote>();
            var mockRepo = new Mock<IRepository<Vote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));
            var service = new VoteService(mockRepo.Object);

            await service.SetVoteAsync(1, "1", 1);
            await service.SetVoteAsync(1, "1", 5);
            await service.SetVoteAsync(1, "1", 5);

            Assert.Equal(1, list.Count);
            Assert.Equal(5, list.First().Value);
        }

        [Fact]
        public async Task WhenTwoUsersVoteForOneTrainerTheAverageVoteShouldBeCorrect()
        {
            var list = new List<Vote>();
            var mockRepo = new Mock<IRepository<Vote>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));
            var service = new VoteService(mockRepo.Object);

            await service.SetVoteAsync(3, "Ceco", 5);
            await service.SetVoteAsync(3, "Pesho", 1);
            await service.SetVoteAsync(3, "Ceco", 2);

            Assert.Equal(2, list.Count);
            Assert.Equal(1.5, service.GetAverageVotes(3));
        }
    }
}
