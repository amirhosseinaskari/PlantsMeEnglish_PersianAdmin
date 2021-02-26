using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
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
        [Route("En/Admin/ProductOption/AddProductOption")]
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

       
    }
}
