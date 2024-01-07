namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrainersListViewModel : PagingViewModel
    {
        public IEnumerable<TrainerInListViewModel> Trainers { get; set; }
    }
}
