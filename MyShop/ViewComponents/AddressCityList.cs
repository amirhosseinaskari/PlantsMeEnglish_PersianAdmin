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
    public class AddressCityList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public AddressCityList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id = -1,int selectedCityId = -1)

        {
            ViewData["SelectedCityId"] = selectedCityId;
            var deliveryId = await _db.Deliveries
                .AsNoTracking()
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
            if (id.Equals(-1))
            {
                var states = await _db.States
             .Where(c => c.DeliveryId.Equals(deliveryId))
              .OrderBy(c => c.StateName).ToListAsync();
                if (states.Count() > 0)
                {
                    var cities01 = await _db.Cities.Where(c => c.StateId.Equals(states.FirstOrDefault().Id) &&
                  c.DeliveryId.Equals(deliveryId))
               .OrderBy(c => c.Title).ToListAsync();
                    return View(viewName: "AddressCityList", model: cities01);
                }
            }


            var cities = await _db.Cities.Where(c => c.StateId.Equals(id) &&
             c.DeliveryId.Equals(deliveryId))
          .OrderBy(c => c.Title).ToListAsync();
            return View(viewName: "AddressCityList", model: cities);


        }
    }


}
