using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class AddressStateList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public AddressStateList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int selectedStateId = -1)
        
        {
            var deliveryId = await _db.Deliveries.AsNoTracking()
              .Select(c => c.Id)
              .FirstOrDefaultAsync();
            var states = await _db.States
               .Where(c => c.DeliveryId.Equals(deliveryId))
                .OrderBy(c => c.StateName).ToListAsync();
            ViewData["SelectedStateId"] = selectedStateId;
            return View(viewName:"AddressStateList",model:states);

        }
    }


}
