namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class RequestTrainerInputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MinLength(30)]
        [MaxLength(500)]
        public string InfoCard { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public decimal PricePerTraining { get; set; }

        [Required]
        public string CategoryOfTraining { get; set; }
    }
}
