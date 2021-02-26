using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Blog:SEO
    {
        public Blog() {
            IsPublished = false;
            BlogOrder = 0;
            CreatedDate = System.DateTime.Now;
        }
        [Key]
        public int BlogId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public int BlogCategoryId { get; set; }
        public string CoverImage { get; set; }
        [Required(ErrorMessage = "وارد کردن عنوان الزامی است")]
        public string Title { get; set; }
        public string SummaryDescription { get; set; }
        public int BlogOrder { get; set; }
        public bool IsPublished { get; set; }
        public List<BlogContent> BlogContents { get; set; }
        public List<BlogTags> BlogTags { get; set; }
        public int ViewNumber { get; set; }
        public double RateNumber { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
