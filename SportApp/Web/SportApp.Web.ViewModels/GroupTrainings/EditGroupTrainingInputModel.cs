namespace SportApp.Web.ViewModels.GroupTrainings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class EditGroupTrainingInputModel : IMapFrom<GroupTraining>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Place { get; set; }

        [Required]
        public string DaysOfWeek { get; set; }

        [Required]
        public TimeSpan StartHour { get; set; }

        public string TrainerUserId { get; set; }
    }
}
