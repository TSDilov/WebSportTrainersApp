namespace SportApp.Web.ViewModels.Comment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CommentsViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public int TrainerId { get; set; }
    }
}
