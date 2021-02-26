using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class FinalBill : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FinalBill(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int shoppingId)
        
        {
            var shopping = await _db.Shoppings.AsNoTracking()
                .Where(c => c.Id.Equals(shoppingId))
                .FirstOrDefaultAsync();

            return View(viewName: "FinalBill",model:shopping);

        }
    }


}
