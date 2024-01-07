using SportApp.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportApp.Data.Models
{
    public class Message : BaseDeletableModel<int>
    {
        public string User { get; set; }

        public string Text { get; set; }
    }
}
