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

    public class GroupTrainingViewModel : IMapFrom<GroupTraining>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Place { get; set; }

        public string DaysOfWeek { get; set; }

        public TimeSpan StartHour { get; set; }

        public int TrainerId { get; set; }

        public string TrainerName { get; set; }

        public string TrainerUserId { get; set; }
    }
}
