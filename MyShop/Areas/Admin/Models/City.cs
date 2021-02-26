namespace Models
{
    public class City
    {
        public City() {
           
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public State State { get; set; }
        public int StateId { get; set; }
        public int DeliveryId { get; set; }
        public string StateName { get; set; }
        public double DeliveryPrice { get; set; }
        public bool IsSetDeliveryPrice { get; set; }
    }
}