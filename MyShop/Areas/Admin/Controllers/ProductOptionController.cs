using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductOptionController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductOptionController(ApplicationDbContext db)
        {
            _db = db;
        }
        //add new product option (handled by ajax)
        [HttpPost]
        [Route("Admin/ProductOption/AddProductOption")]
        public async Task<IActionResult> AddProductOption(int productId, string title, string value)
        {
            try
            {
                var productOption = new ProductOption();
                productOption.OptionTitle = title;
                productOption.OptionValue = value;
                productOption.ProductId = productId;
                await _db.ProductOptions.AddAsync(productOption);
                await _db.SaveChangesAsync();
                return PartialView(viewName: "_AddedProductOption", model: productOption);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //delete a product option
        [HttpPost]
        [Route("Admin/ProductOption/DeleteProductOption")]
        public async Task<IActionResult> DeleteProductOption(int productOptionId)
        {
            try
            {
                var productOption = await _db.ProductOptions.FindAsync(productOptionId);
                _db.ProductOptions.Remove(productOption);
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
