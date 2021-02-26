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
    public class FAQList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public FAQList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            List<FAQ> faqs =await _db.FAQs
                .AsNoTracking()
                .OrderBy(c => c.Order).ToListAsync();
                            
           
            return View(viewName: "FAQList", model: faqs);
            
        }
    }

  
}
