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
    public class CustomerListTitle:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CustomerListTitle(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var customers = await _db.Users.AsNoTracking()
                 .Select(c => new CustomerTitleDropDown() { Id = c.Id, Name = c.Mobile })
                 .OrderBy(c=> c.Name)
                 .ToListAsync();
            return View(viewName: "CustomerListTitle", model: customers);
            
        }
    }

  
}
