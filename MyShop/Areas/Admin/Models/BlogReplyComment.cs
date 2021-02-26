using System;

namespace Models
{
    public class BlogReplyComment
    {
        public BlogReplyComment() {
            SubmitedDate = System.DateTime.Now;
            IsPublished = false;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public DateTime SubmitedDate { get; set; }
        public bool IsPublished { get; set; }
        public int BlogCommentId { get; set; }
        public BlogComment BlogComment { get; set; }
        public bool IsFromAdmin { get; set; }
        public bool IsChecked { get; set; }
    }
}