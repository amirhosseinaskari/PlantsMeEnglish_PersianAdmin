using MyShop.InfraStructure;

namespace Models
{
    public class BlogContent
    {
        public BlogContent() { }
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public ContentType ContentType { get; set; }
        public string Content { get; set; }
        public string AltTextForImage { get; set; }
        public int ContentOrder { get; set; }
    }
}