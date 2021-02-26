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
    public class StateList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public StateList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        
        {

            var states = await _db.States.OrderBy(c => c.StateName).ToListAsync();
            
          
            return View(viewName:"StateList",model:states);

        }
    }


}
