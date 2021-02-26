using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Controllers
{

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductsController(ApplicationDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Route("AllProducts")]
        public async Task<IActionResult> AllProducts(int sort)
        {
            var maxPriceValue = (await _db.Products.MaxAsync(item =>(double?) item.BasePrice)) ?? 1000000;
            ViewData["MaxPrice"] = (int)maxPriceValue;
            return View(model: sort);
        }

        [Route("All-Products/{id}/{title}")]
        public async Task<IActionResult> ProductDetail(int id, string title = null)
        {

            var product = await _db.Products
                .Where(c => c.Id.Equals(id))
                .Include(c => c.Images)
                .Include(c => c.SpecialDiscount)
                .FirstOrDefaultAsync();
            var session = HttpContext.Session.GetString("Viewer");
            if (session != title + id)
            {
                HttpContext.Session.SetString("Viewer", title + id);
                product.ViewNumber++;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                var specialDiscount = product.SpecialDiscount.Where(c => c.ProductId.Equals(product.Id)
                       && c.ExpirationDate > System.DateTime.Now &&
                                 c.ActivationDate <= System.DateTime.Now).FirstOrDefault();
                if (specialDiscount != null)
                {
                    specialDiscount.ViewNumber++;
                    _db.SpecialDiscount.Update(specialDiscount);
                    await _db.SaveChangesAsync();
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _userManager.Users
               .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
               .Select(c => c.Id)
               .FirstOrDefaultAsync();
                var fovoritProducts = await _db.FavoritProduct
                .AsNoTracking().Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                if (fovoritProducts.Contains(id))
                {
                    product.IsProductSaved = true;
                }
            }
            return View(model: product);
        }
        
        public async Task<IActionResult> ProductComments()
        {
            return PartialView(viewName: "_ProductComments");
        }
        public async Task<IActionResult> Products()
        {
            return View();
        }

        //Product option list
        //when click on product-options tab in product detail page
        [Route("Products/ProductOptions")]
        [HttpPost]
        public IActionResult ProductOptions(int productId)
        {
            return ViewComponent(componentName: "ProductOptions",
                arguments: new { productId = productId});
        }

        //Product comment list
        //when click on comment tab in product detail page
        [Route("Products/ProductCommentList")]
        [HttpPost]
        public IActionResult ProductCommentList(int productId, int pageIndex = 1)
        {
            return ViewComponent(componentName: "ProductCommentList",
                arguments: new { productId = productId, pageIndex = pageIndex });
        }
        //Product description
        //when click on description tab in product detail page
        [Route("Products/ProductDescription")]
        [HttpPost]
        public IActionResult ProductDescription(int productId)
        {
            return ViewComponent(componentName: "ProductTechnicalContent",
                arguments: new { productId = productId });
        }
    }
}