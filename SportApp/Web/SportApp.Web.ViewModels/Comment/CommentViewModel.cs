namespace SportApp.Web.ViewModels.Comment
{

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string ApplicationUserId { get; set; }

        public int TrainerId { get; set; }
    }
}
