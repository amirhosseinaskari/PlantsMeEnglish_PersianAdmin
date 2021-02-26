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
    public class AllSpecialDiscountList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AllSpecialDiscountList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1)
        {
            IQueryable<Product> products =
                 _db.Products
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.IsPublished)
                .Include(c => c.SpecialDiscount)
                .Include(c => c.Images)
                .Where(c => c.SpecialDiscount.Where(c => c.ExpirationDate > System.DateTime.Now).Count() > 0)
                .OrderBy(c=> c.CreatedDate);
                var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 16);
            //detect saved products in favorits
            if (User.Identity.IsAuthenticated)
            {
                var userId = await _userManager.Users
                .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
                var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                    .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

            }
            return View(viewName: "AllSpecialDiscountList", model: pageinatedList);

        }
    }

  
}
