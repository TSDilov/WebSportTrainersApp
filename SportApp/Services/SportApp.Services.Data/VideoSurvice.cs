namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SportApp.Data.Common.Repositories;
    using SportApp.Data.Models;
    using SportApp.Services.Mapping;
    using SportApp.Web.ViewModels;

    public class VideoSurvice : IVideoSurvice
    {
        private readonly IDeletableEntityRepository<Video> videoRepository;

        public VideoSurvice(IDeletableEntityRepository<Video> videoRepository)
        {
            this.videoRepository = videoRepository;
        }

        public async Task CreateAsync(VideoModel model, string imagePath)
        {
            var video = new Video
            {
                Name = model.Name,
            };

            Directory.CreateDirectory($"{imagePath}/videos");
            var extension = Path.GetExtension(model.Video.FileName).TrimStart('.');

            video.Extension = extension;
            var physicalPath = $"{imagePath}/videos/{video.Id}.{extension}";
            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await model.Video.CopyToAsync(fileStream);
            }

            await this.videoRepository.AddAsync(video);
            await this.videoRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var video = await this.videoRepository.All().FirstOrDefaultAsync(t => t.Id == id);
            this.videoRepository.Delete(video);
            await this.videoRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<VideoViewModel>> GetAllAsync()
        {
            return await this.videoRepository.AllAsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .Select(x => new VideoViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Extension = x.Extension,
                    })
                    .ToListAsync();
        }
    }
}
