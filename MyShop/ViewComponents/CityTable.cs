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
    public class CityTable : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CityTable(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int deliveryId)
        
        {
               var cities = await _db.Cities.Where(c => c.DeliveryId.Equals(deliveryId))
               .OrderBy(c => c.Title).ToListAsync();
               return View(viewName: "CityTable", model: cities);

        }
    }


}
