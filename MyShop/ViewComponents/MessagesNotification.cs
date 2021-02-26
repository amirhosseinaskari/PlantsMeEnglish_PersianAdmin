using Microsoft.AspNetCore.Hosting;
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
    public class MessagesNotification : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public MessagesNotification(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {

            var count = await _db.Tickets.AsNoTracking()
               .Where(c => !c.IsChecked)
               .CountAsync();
           
          
            return View(viewName: "MessagesNotification", model:count);

        }
    }


}
