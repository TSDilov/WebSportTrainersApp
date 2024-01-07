namespace SportApp.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data;
    using SportApp.Data.Models;
    using SportApp.Data.Repositories;
    using SportApp.Web.ViewModels;
    using SportApp.Web.ViewModels.Comment;
    using Xunit;

    public class VideoServiceTests
    {
        private IVideoSurvice service;
        private ApplicationDbContext applicationDbContext;

        [Fact]
        public async Task GetAllVideos()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var videoRepo = new EfDeletableEntityRepository<Video>(this.applicationDbContext);
            this.service = new VideoSurvice(videoRepo);

            var video = new Video
            {
                Name = "Test",
                Extension = "mp4",
                RemoteImageUrl = null,
            };

            await videoRepo.AddAsync(video);
            await videoRepo.SaveChangesAsync();

            var videos = await this.service.GetAllAsync();

            Assert.Equal(1, videos.Count());
            Assert.Contains(videoRepo.All().ToList(), x => x.Name == "Test");

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task CreateVideo()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var videoRepo = new EfDeletableEntityRepository<Video>(this.applicationDbContext);
            this.service = new VideoSurvice(videoRepo);
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");

            var video = new VideoModel
            {
                Name = "Test1",
                Video = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt"),
            };

            await this.service.CreateAsync(video, "C:\\Users\\cvdil\\Desktop\\CSharp-Web\\SportApp\\Web\\SportApp.Web\\wwwroot");

            var videos = await this.service.GetAllAsync();

            Assert.Equal(1, videos.Count());
            Assert.DoesNotContain(videos, x => x.Name == "NotTest");

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task DeleteVideo()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SportDb").Options;
            this.applicationDbContext = new ApplicationDbContext(contextOptions);
            var videoRepo = new EfDeletableEntityRepository<Video>(this.applicationDbContext);
            this.service = new VideoSurvice(videoRepo);

            var id = Guid.NewGuid().ToString();

            var video = new Video
            {
                Id = id,
                Name = "Test2",
                Extension = "mp4",
                RemoteImageUrl = null,
            };

            await videoRepo.AddAsync(video);
            await videoRepo.SaveChangesAsync();

            await this.service.DeleteAsync(id);

            var videos = await this.service.GetAllAsync();

            Assert.Equal(0, videos.Count());

            this.applicationDbContext.Database.EnsureDeleted();
            this.applicationDbContext.Database.EnsureCreated();
        }
    }
}
