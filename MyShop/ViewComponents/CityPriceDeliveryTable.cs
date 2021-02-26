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
    public class CityPriceDeliveryTable : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CityPriceDeliveryTable(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()

        {
            var cityDeliveryPrices = await _db.Cities
                .Where(c => c.IsSetDeliveryPrice && c.DeliveryPrice != -1)
                .OrderBy(c => c.Title)
                .ToListAsync();
            return View(viewName: "CityPriceDeliveryTable", model: cityDeliveryPrices);

        }
    }


}
