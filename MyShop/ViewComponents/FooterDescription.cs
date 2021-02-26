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
    public class FooterDescription:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public FooterDescription(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            string footerDescription = await _db.HomePage.AsNoTracking()
                 .Select(c => c.FooterDescription).FirstOrDefaultAsync();

            
            return View(viewName: "FooterDescription", model: footerDescription);
            
        }
    }

  
}
