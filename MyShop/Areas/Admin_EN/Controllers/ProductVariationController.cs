using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductVariationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductVariationController(ApplicationDbContext db)
        {
            _db = db;
           
        }
        [Route("En/Admin/ProductVariation/CreateProductVariation")]
        [HttpPost]
        public async Task<IActionResult> CreateProductVariation(int productId, string varTitle, bool hasDifPrice)
        {
            ProductVariation productVariation = new ProductVariation();
            try
            {
                productVariation.ProductId = productId;
                productVariation.Title = varTitle;
                productVariation.HasDifferentPrice = hasDifPrice;
                
                await _db.ProductVariations.AddAsync(productVariation);
                await _db.SaveChangesAsync();
                var product =await _db.Products.FindAsync(productId);
                product.VarNumber=await _db.ProductVariations.AsNoTracking()
                    .Where(c => c.ProductId.Equals(productId))
                    .CountAsync();
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return ViewComponent(componentName: "ProductVariationList", arguments: new { productId = productId });
            }
            catch (Exception)
            {
                return Json("False");
            }
        }

        [Route("En/Admin/ProductVariation/DeleteProductVariation")]
        [HttpPost]
        public async Task<IActionResult> DeleteProductVariation(int id)
        {
            ProductVariation productVariation = await _db.ProductVariations.FindAsync(id);
            var product = await _db.Products.FindAsync(productVariation.ProductId);
            _db.ProductVariations.Remove(productVariation);
            await _db.SaveChangesAsync();
            product.VarNumber = await _db.ProductVariations.AsNoTracking()
                .Where(c => c.ProductId.Equals(product.Id))
                .CountAsync();
            _db.Update(product);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(controllerName: "Products", 
                actionName: "EditProduct",routeValues:new { id = productVariation.ProductId});
        }
    }
}