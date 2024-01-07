namespace SportApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationUserGroupTraining
    {
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public int GroupTrainingId { get; set; }

        public GroupTraining GroupTraining { get; set; }
    }
}
