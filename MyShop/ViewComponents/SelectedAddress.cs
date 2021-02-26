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
    public class SelectedAddress : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public SelectedAddress(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int addressId)

        {
            var address = await _db.Addresses
                .AsNoTracking()
                .Where(c => c.AddressId.Equals(addressId))
                .FirstOrDefaultAsync();
            return View(viewName: "SelectedAddress", model: address);


        }
    }


}
