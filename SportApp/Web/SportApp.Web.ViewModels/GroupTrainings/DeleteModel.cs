namespace SportApp.Web.ViewModels.GroupTrainings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class DeleteModel : IMapFrom<GroupTraining>
    {
        public string TrainerUserId { get; set; }
    }
}
