using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class BlogComment
    {
        public BlogComment()
        {
            SubmitedDate = System.DateTime.Now;
            IsPublished = false;
        }
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime SubmitedDate { get; set; }
        public bool IsPublished { get; set; }
        public int Rate { get; set; }
        public List<ReplyComment> ReplyComment { get; set; }
        public bool IsChecked { get; set; }
    }
}
