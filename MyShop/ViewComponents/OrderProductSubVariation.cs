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
    public class OrderProductSubVariation : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public OrderProductSubVariation(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string subIds)
        
        {
            if (!string.IsNullOrWhiteSpace(subIds))
            {
                var stringList = subIds.Split(',').ToList();
                var ids = stringList.Select(int.Parse).ToList();
                var subProVars = await _db.SubProductVariations
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.SubProductVariationId))
                    .ToListAsync();

                return View(viewName: "OrderProductSubVariation", model: subProVars);
            }
            List<SubProductVariation> sp = null;
            return View(viewName: "OrderProductSubVariation",model: sp);

        }
    }


}
