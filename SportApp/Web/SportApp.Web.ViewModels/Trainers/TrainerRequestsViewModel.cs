namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrainerRequestsViewModel
    {
        public IEnumerable<TrainerRequestViewModel> TrainersRequests { get; set; }
    }
}
