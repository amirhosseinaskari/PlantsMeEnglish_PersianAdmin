using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class State
    {
        public State() { }
        public int Id { get; set; }
        public string StateName { get; set; }
        public List<City> Cities { get; set; }
        public int DeliveryId { get; set; }
    }
}
