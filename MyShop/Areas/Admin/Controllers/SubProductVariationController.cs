using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SubProductVariationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SubProductVariationController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/SubProductVariation/Create")]
        [HttpPost]
        public async Task<IActionResult> Create(int productVariationId,string proVarTitle)
        {
            SubProductVariation subProVar = new SubProductVariation();
            ProductVariation proVar = await _db.ProductVariations
                .AsNoTracking()
                .Where(c => c.ProductVariationId.Equals(productVariationId))
                .FirstOrDefaultAsync();
            double price = await _db.Products.AsNoTracking()
                .Where(c => c.Id.Equals(proVar.ProductId))
                .Select(c => c.BasePrice)
                .FirstOrDefaultAsync();
            subProVar.Price = price;
            subProVar.Title = proVarTitle;
            subProVar.ProductVariationId = productVariationId;
            subProVar.productId = proVar.ProductId;
            subProVar.ProductVariationTitle = proVar.Title;
            await _db.SubProductVariations.AddAsync(subProVar);
            await _db.SaveChangesAsync();

            return ViewComponent(componentName: "SubProductVariationList",
                arguments: new { productVariationId = productVariationId });

        }

        [Route("Admin/SubProductVariation/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var subProductVariation = await _db.SubProductVariations.FindAsync(id);
                _db.SubProductVariations.Remove(subProductVariation);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
            
        }
        [Route("Admin/SubProductVariation/Titles")]
        [HttpPost]
        public IActionResult Titles(int productVariationId)
        {
            return ViewComponent(componentName: "SubProductVariationTitles",
                arguments: new { proVarId = productVariationId });
        }

        [Route("Admin/SubProductionVariation/DefinePrice")]
        [HttpPost]
        public async Task<IActionResult> DefinePrice(int id,double price,int productVariationId)
        {
            var subPro = await _db.SubProductVariations.FindAsync(id);
            subPro.HasDefinedDifferentPrice = true;
            subPro.Price = price;
            _db.SubProductVariations.Update(subPro);
            await _db.SaveChangesAsync();
            return ViewComponent(componentName: "SubProductVariationListPrice",
                arguments: new { productVariationId = productVariationId });
        }

        [Route("Admin/SubProductVariation/DeletePrice")]
        [HttpPost]
        public async Task<IActionResult> DeletePrice(int id)
        {
            try
            {
                var subPro = await _db.SubProductVariations.FindAsync(id);
                var proVar = await _db.ProductVariations.AsNoTracking()
                    .Where(c => c.ProductVariationId.Equals(subPro.ProductVariationId))
                    .FirstOrDefaultAsync();
                var productBasePrice = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(proVar.ProductId))
                    .Select(c => c.BasePrice)
                    .FirstOrDefaultAsync();
                subPro.Price = productBasePrice;
                subPro.HasDefinedDifferentPrice = false;
                _db.SubProductVariations.Update(subPro);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
           
        }
    }
}