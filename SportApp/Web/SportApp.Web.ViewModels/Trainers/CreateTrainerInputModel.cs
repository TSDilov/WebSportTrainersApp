namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class CreateTrainerInputModel : BaseTrainerInputModel
    {
        [Required]
        public IEnumerable<IFormFile> Images { get; set; }

        [Required]
        public string Username { get; set; }

        public decimal Rating { get; set; }
    }
}
