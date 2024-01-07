namespace SportApp.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BookedUsersViewModel
    {
        public IEnumerable<BookedUserViewModel> BookedUsers { get; set; }

        public string TrainerName { get; set; }
    }
}
