namespace SportApp.Web.ViewModels.Trainers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class EditTrainerInputModel : BaseTrainerInputModel, IMapFrom<Trainer>
    {
        public int Id { get; set; }
    }
}
