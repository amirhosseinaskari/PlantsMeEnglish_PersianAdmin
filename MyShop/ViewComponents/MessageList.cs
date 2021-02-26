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
    public class MessageList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        
        public MessageList(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1,int sort = 0, string searchText = null)

        {
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var messages = _db.Tickets.AsNoTracking()
                  .AsQueryable()
                  .Where(c=> c.FullName.Contains(searchText) || c.Subject.Contains(searchText))
                  .OrderByDescending(c => c.SubmitDate);
                var pageinatedList01 = await PaginatedList<Ticket>.CreateAsync(source: messages, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "MessageList", model: pageinatedList01);
            }
            switch (sort)
            {
                case 0:
                    var messages01 = _db.Tickets.AsNoTracking()
                   .AsQueryable()
                   .OrderByDescending(c => c.SubmitDate);
                    var pageinatedList01 = await PaginatedList<Ticket>.CreateAsync(source: messages01, pageIndex: pageIndex, pageSize: 10);
                    return View(viewName: "MessageList", model: pageinatedList01);
                case 1:
                    var messages02 = _db.Tickets.AsNoTracking()
                    .AsQueryable()
                    .OrderBy(c => c.SubmitDate);
                    var pageinatedList02 = await PaginatedList<Ticket>.CreateAsync(source: messages02, pageIndex: pageIndex, pageSize: 10);
                    return View(viewName: "MessageList", model: pageinatedList02);
                case 2:
                    var messages03 = _db.Tickets.AsNoTracking()
                   .AsQueryable()
                   .Where(c=> c.TicketStatus.Equals(TicketStatus.Replied))
                   .OrderByDescending(c => c.SubmitDate);
                    var pageinatedList03 = await PaginatedList<Ticket>.CreateAsync(source: messages03, pageIndex: pageIndex, pageSize: 10);
                    return View(viewName: "MessageList", model: pageinatedList03);
                case 3:
                    var messages04 = _db.Tickets.AsNoTracking()
                   .AsQueryable()
                   .Where(c => c.TicketStatus.Equals(TicketStatus.WaitForReply))
                   .OrderByDescending(c => c.SubmitDate);
                    var pageinatedList04 = await PaginatedList<Ticket>.CreateAsync(source: messages04, pageIndex: pageIndex, pageSize: 10);
                    return View(viewName: "MessageList", model: pageinatedList04);
                default:
                    var messages05 = _db.Tickets.AsNoTracking()
                 .AsQueryable()
                 .OrderByDescending(c => c.SubmitDate);
                    var pageinatedList05 = await PaginatedList<Ticket>.CreateAsync(source: messages05, pageIndex: pageIndex, pageSize: 10);
                    return View(viewName: "MessageList", model: pageinatedList05);
                   
            }
           


        }
    }


}
