using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Controllers
{
    [Authorize]
    public class FavoritProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FavoritProductController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        //list of saved products
        [HttpGet]
        [Route("/FavoritProduct/Favorites")]
        public async Task<IActionResult> Index()
        {
         
            return View();
        }
        //add product to user favorits
        [HttpPost]
        [Route("FavoritProduct/AddToMyFavorit")]
        public async Task<IActionResult> AddToMyFavorit(int productId)
        {
            try
            {
                var favorit = new FavoritProduct();
                favorit.ProductId = productId;

                favorit.UserId = await _userManager.Users
                    .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
                await _db.FavoritProduct.AddAsync(favorit);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
           
        }

        //remove from my favorit product
        [HttpPost]
        [Route("FavoritProduct/RemoveFromMyFavorit")]
        public async Task<IActionResult> RemoveFromMyFavorit(int productId)
        {
            try
            {
                var userId = await _userManager.Users
                    .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
                var favorit = await _db.FavoritProduct
                    .Where(c => c.UserId.Equals(userId) && c.ProductId.Equals(productId)).FirstOrDefaultAsync();
                _db.FavoritProduct.Remove(favorit);
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
