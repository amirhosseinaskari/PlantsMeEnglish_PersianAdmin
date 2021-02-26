using MyShop.InfraStructure;

namespace Models
{
    public class AboutUsContent
    {
        public AboutUsContent() { }
        public int Id { get; set; }
        public int AboutId { get; set; }
        public About About { get; set; }
        public ContentType ContentType { get; set; }
        public string Content { get; set; }
        public string AltTextForImage { get; set; }
        public int ContentOrder { get; set; }
    }
}