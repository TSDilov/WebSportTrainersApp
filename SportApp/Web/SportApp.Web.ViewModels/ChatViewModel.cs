namespace SportApp.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;

    public class ChatViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
    }
}
