namespace Models
{
    public class BlogTags
    {
        public BlogTags() { }
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public int BlogId { get; set; }
        public string Text { get; set; }
    }
}