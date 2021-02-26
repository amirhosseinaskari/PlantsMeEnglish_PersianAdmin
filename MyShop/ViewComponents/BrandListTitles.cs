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
    public class BrandListTitles : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BrandListTitles(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int myId = -1)
        {
            if (myId == -1)
            {
                var brands = await _db.Brands.AsNoTracking()
                .Select(a => new BrandTitleDropDown() { Title = a.Title, Id = a.BrandId, Selected = false })
                .ToListAsync();
                return View(viewName: "BrandListTitles", model: brands);
            }
            else
            {
                var brands = await _db.Brands.AsNoTracking()
               .Select(a => new BrandTitleDropDown() { Title = a.Title, Id = a.BrandId, Selected = false })
               .ToListAsync();
                brands.Where(c => c.Id == myId).FirstOrDefault().Selected = true;
                return View(viewName: "BrandListTitles", model: brands);
            }
           

        }
    }


}
