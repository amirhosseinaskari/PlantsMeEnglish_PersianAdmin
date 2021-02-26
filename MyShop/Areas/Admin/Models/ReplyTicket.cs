using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Models
{
    public class ReplyTicket
    {
        public ReplyTicket()
        {
            SubmitDate = System.DateTime.Now;
        }
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public DateTime SubmitDate { get; set; }
        public bool IsChecked { get; set; }
    }
}