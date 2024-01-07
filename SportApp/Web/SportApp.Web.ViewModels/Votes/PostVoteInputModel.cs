namespace SportApp.Web.ViewModels.Votes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PostVoteInputModel
    {
        public int TrainerId { get; set; }

        [Range(1, 5)]
        public byte Value { get; set; }
    }
}
