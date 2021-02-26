using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Notification
    {
        public Notification() { }
        public int Id { get; set; }
        public string LinkText { get; set; }
        public string MainText { get; set; }
        public bool IsPublished { get; set; }
    }
}
