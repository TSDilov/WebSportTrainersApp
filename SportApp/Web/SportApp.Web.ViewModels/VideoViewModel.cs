namespace SportApp.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Models;
    using SportApp.Services.Mapping;

    public class VideoViewModel 
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }
    }
}
