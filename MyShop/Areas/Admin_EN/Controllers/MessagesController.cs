using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.En.Controllers { 
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class MessagesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public MessagesController(UserManager<ApplicationUser> userManager,
              ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [Route("En/Admin/Messages")]
        public IActionResult Index()
        {
            return View();
        }

        //Ticket detail and reply to ticket
        [Route("En/Admin/Messages/TicketDetail")]
        public async Task<IActionResult> TicketDetail(int id)
        {
            var ticket = await _db.Tickets.AsNoTracking()
                .Where(c => c.Id.Equals(id)).Include(c=> c.ReplyTickets).FirstOrDefaultAsync();
            if (ticket != null)
            {
                ticket.IsChecked = true;
                _db.Tickets.Update(ticket);
                await _db.SaveChangesAsync();
            }
            return View(model:ticket);
        }
        //Add reply to a ticket
        //handled by ajax
        [Route("En/Admin/Messages/AddReplyTicket")]
        [HttpPost]
        public async Task<IActionResult> AddReplyTicket(int ticketId, string replyDescription)
        {
            
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var replyTicket = new ReplyTicket();
                replyTicket.UserId = user.Id;
                replyTicket.FullName = user.FullName;
                replyTicket.Description = replyDescription;
                replyTicket.TicketId = ticketId;
                await _db.ReplyTickets.AddAsync(replyTicket);
                await _db.SaveChangesAsync();
                var ticket = await _db.Tickets.FindAsync(ticketId);
                ticket.TicketStatus = TicketStatus.Replied;
                _db.Tickets.Update(ticket);
                await _db.SaveChangesAsync();
                return PartialView(viewName: "_AddedReplyTicket", model: replyTicket);

           
           
        }

        //Delete a reply ticket
        [Route("En/Admin/Messages/DeleteReplyTicket")]
        [HttpPost]
        public async Task<IActionResult> DeleteReplyTicket(int id)
        {
            var replyTicket = await _db.ReplyTickets.FindAsync(id);
            try
            {
                var ticketId = replyTicket.TicketId;
                _db.ReplyTickets.Remove(replyTicket);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction(actionName: "TicketDetail", routeValues: new { id = ticketId });
            }
            catch (Exception)
            {

                return RedirectToAction(actionName: "Index");
            }
        }
        //Delete a ticket
        [Authorize(Roles = "Manager")]
        [Route("En/Admin/Messages/DeleteTicket")]
        [HttpPost]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _db.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _db.Tickets.Remove(ticket);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction(actionName: "Index");
            }
            return RedirectToAction(actionName: "Index");
            
        }

        //Message List (Ticket List)
        [Route("En/Admin/Messages/TicketList")]
        [HttpPost]
        public IActionResult TicketList(int sort = 0, int pageIndex =1,string searchText = null)
        {
            return ViewComponent(componentName: "MessageList",
                arguments: new { sort = sort, pageIndex = pageIndex, searchText = searchText });
        }
    }
}