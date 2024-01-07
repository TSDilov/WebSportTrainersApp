namespace SportApp.Web.ViewModels.GroupTrainings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Web.ViewModels.Trainers;

    public class GroupTrainingsListViewModel
    {
        public IEnumerable<GroupTrainingViewModel> GroupTrainings { get; set; }
    }
}
