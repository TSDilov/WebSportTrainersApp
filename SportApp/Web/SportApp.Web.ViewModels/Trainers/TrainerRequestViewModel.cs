namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class TrainerRequestViewModel : IMapFrom<RequestTrainer>
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string InfoCard { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal PricePerTraining { get; set; }

        public string CategoryOfTraining { get; set; }
    }
}
