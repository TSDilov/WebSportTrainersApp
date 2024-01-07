namespace SportApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Common.Models;

    public class RequestTrainer : BaseDeletableModel<int>
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string InfoCard { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public decimal PricePerTraining { get; set; }

        [Required]
        public string CategoryOfTraining { get; set; }

        public bool IsApproved { get; set; }
    }
}
