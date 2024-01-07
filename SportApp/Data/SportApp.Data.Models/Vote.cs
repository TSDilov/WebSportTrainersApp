namespace SportApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SportApp.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        public int TrainerId { get; set; }

        public virtual Trainer Trainer { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
