using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class FavoritProductList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FavoritProductList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var userId = await _userManager.Users
                 .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                 .Select(c => c.Id)
                 .FirstOrDefaultAsync();
            var fovoritProducts = await _db.FavoritProduct
                .AsNoTracking().Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
            var products = await _db.Products.AsNoTracking()
                .Where(c => fovoritProducts.Contains(c.Id))
                .Include(c=> c.Images)
                .Include(c=> c.SpecialDiscount)
                .ToListAsync();
            return View(viewName: "FavoritProductList", model: products);
            
        }
    }

  
}
