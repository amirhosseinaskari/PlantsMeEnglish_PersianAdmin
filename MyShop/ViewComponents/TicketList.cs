using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class TicketList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TicketList(UserManager<ApplicationUser> userManager,ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var tickets = _db.Tickets.AsNoTracking().AsQueryable()
                .Where(c => c.UserId.Equals(user.Id))
                .OrderByDescending(c => c.SubmitDate);
            var pageinatedList = await PaginatedList<Ticket>.CreateAsync(source: tickets, pageIndex: pageIndex, pageSize: 12);
            return View(viewName: "TicketList", model: pageinatedList);
            
        }
    }

  
}
