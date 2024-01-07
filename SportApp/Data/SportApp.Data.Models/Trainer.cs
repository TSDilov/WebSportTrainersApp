namespace SportApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using SportApp.Data.Common.Models;

    public class Trainer : BaseDeletableModel<int>
    {
        public Trainer()
        {
            this.ApplicationUsersTrainers = new HashSet<ApplicationUserTrainer>();
            this.Images = new HashSet<Image>();
            this.Votes = new HashSet<Vote>();
            this.Comments = new HashSet<Comment>();
            this.GroupTrainings = new HashSet<GroupTraining>();
        }

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

        public decimal Rating { get; set; }

        [ForeignKey("Category")]
        public int CategotyId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<ApplicationUserTrainer> ApplicationUsersTrainers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<GroupTraining> GroupTrainings { get; set; }
    }
}
