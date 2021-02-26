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
    public class CityList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CityList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        
        {
            if(id== -1)
            {
                var cities = await _db.Cities.Where(c => c.StateId.Equals(5))
              .OrderBy(c => c.Title).ToListAsync();
                return View(viewName: "CityList", model: cities);
            }
            else
            {
                var cities = await _db.Cities.Where(c => c.StateId.Equals(id))
             .OrderBy(c => c.Title).ToListAsync();
                return View(viewName: "CityList", model: cities);
            }
            
          
           

        }
    }


}
