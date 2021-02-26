using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Ticket
    {
        public Ticket() {
            TicketStatus = TicketStatus.WaitForReply;
            SubmitDate = System.DateTime.Now;
        }
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string FullName { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Subject { get; set; }
        public int ProductId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string Description { get; set; }
        public List<ReplyTicket> ReplyTickets { get; set; }
        public bool IsChecked { get; set; }

    }

    public enum TicketStatus
    {
        WaitForReply = 0,
        Closed = 1,
        Replied = 2
    }
}
