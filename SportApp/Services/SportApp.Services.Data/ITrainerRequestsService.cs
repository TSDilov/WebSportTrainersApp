namespace SportApp.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITrainerRequestsService
    {
        Task ApprovedAsync(int id);
    }
}
