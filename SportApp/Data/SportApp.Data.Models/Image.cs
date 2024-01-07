namespace SportApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using SportApp.Data.Common.Models;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [ForeignKey("Trainer")]
        public int TrainerId { get; set; }

        public virtual Trainer Trainer { get; set; }

        public string Extension { get; set; }

        public string RemoteImageUrl { get; set; }
    }
}