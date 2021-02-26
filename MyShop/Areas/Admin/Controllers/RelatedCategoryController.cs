using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class RelatedCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RelatedCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/RelatedCategory/AddToRelatedCategory")]
        [HttpPost]
        //Add to related category
        //by add a related category, the product will show in the category results
        public async Task<IActionResult> AddToRelatedCategory(int categoryId = -1, int productId = -1)
        {
            if(categoryId == -1 || productId == -1)
            {
                return Json(false);
            }
            try
            {
                RelatedCategory relatedCategory = new RelatedCategory();
                relatedCategory.ProductId = productId;
                relatedCategory.CategoryId = categoryId;
                await _db.RelatedCategories.AddAsync(relatedCategory);
                await _db.SaveChangesAsync();
                var addedCategory = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId.Equals(categoryId))
                    .Select(c => new CategoryTitleDropDown() { Id = c.CategoryId, Title = c.Title, Selected = false })
                    .FirstOrDefaultAsync();
                ViewData["ProductId"] = productId;
                return PartialView(viewName: "_AddedRelatedCategory", model: addedCategory);
            }
            catch (Exception)
            {
                return Json(false);
            }
            
        }
        [Route("Admin/RelatedCategory/DeleteRelatedCategory")]
        [HttpPost]
        //Delete a related category
        public async Task<IActionResult> DeleteRelatedCategory(int categoryId, int productId)
        {
            try
            {
                var relatedCategory = await _db.RelatedCategories
               .Where(c => c.CategoryId.Equals(categoryId) && c.ProductId.Equals(productId))
               .FirstOrDefaultAsync();
                _db.RelatedCategories.Remove(relatedCategory);
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
