namespace SportApp.Web.ViewModels.GroupTrainings
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateGroupTrainingInputModel
    {
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

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string TrainerName { get; set; }

        public string TrainerUserId { get; set; }
    }
}
