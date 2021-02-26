namespace Models
{
    public class HomeSliderImages
    {
        public HomeSliderImages() {
            ImageOrder = 0;
        }
        public int Id { get; set; }
        public int HomePageId { get; set; }
        public HomePage HomePage { get; set; }
        public string ImageName { get; set; }
        public string AlternativeText { get; set; }
        public string TargetLink { get; set; }
        public int ImageOrder { get; set; }
    }
}