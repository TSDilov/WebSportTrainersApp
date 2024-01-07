using System.ComponentModel.DataAnnotations;

namespace SportApp.Web.ViewModels.Comment
{
    public class CommentInputModel
    {
        public int TrainerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Message { get; set; }
    }
}
