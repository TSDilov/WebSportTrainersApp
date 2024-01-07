namespace SportApp.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ListWithVideosViewModel
    {
        public IEnumerable<VideoViewModel> Videos { get; set; }
    }
}
