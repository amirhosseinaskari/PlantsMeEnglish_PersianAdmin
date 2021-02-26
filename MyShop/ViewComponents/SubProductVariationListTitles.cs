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
    public class SubProductVariationListTitles : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public SubProductVariationListTitles(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productVarId)

        {

            var subProductVariations = await _db.SubProductVariations
              .AsNoTracking()
              .Where(c => c.ProductVariationId.Equals(productVarId))
              .Select(c => new SubProductVariationTitlesDropDown() { Id = c.SubProductVariationId, Title = c.Title })
              .ToListAsync();


            return View(viewName: "SubProductVariationListTitles", model: subProductVariations);

        }
    }


}
