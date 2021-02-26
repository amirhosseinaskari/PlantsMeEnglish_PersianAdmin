using System;

namespace Models
{
    public class ReplyComment
    {
        public ReplyComment() {
            SubmitedDate = System.DateTime.Now;
            IsPublished = false;
            IsFromAdmin = false;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public DateTime SubmitedDate { get; set; }
        public bool IsPublished { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public bool IsFromAdmin { get; set; }
        public bool IsChecked { get; set; }
    }
}