using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using Models;

namespace MyShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;

        }

        //Show category list when click on categories link on top menu
        //Handled by ajax
        [Route("Category/MenuCategoryList")]
        public IActionResult MenuCategoryList()
        {

            return ViewComponent(componentName: "MenuCategoryList");
        }

        //Show category list when click on categories link on mobile menu
        //Handled by ajax
        [Route("Category/MobileMenuCategoryList")]
        public IActionResult MobileMenuCategoryList()
        {

            return ViewComponent(componentName: "MobileMenuCategoryList");
        }

        //Show sub category list when click on categories link on top menu
        //Handled by ajax
        [Route("Category/MenuSubCategoryList")]
        [HttpPost]
        public async Task<IActionResult> MenuSubCategoryList(int id)
        {

            var category = await _db.Categories.AsNoTracking()
                .Where(c => c.CategoryId.Equals(id)).FirstOrDefaultAsync();
            if (category == null)
            {
                return RedirectToAction(actionName: "MenuCategoryList");
            }
            return ViewComponent(componentName: "MenuSubCategoryList",arguments:new {parentId = id});
        }

        //Show sub category list when click on categories link on mobile menu
        //Handled by ajax
        [Route("Category/MobileMenuSubCategoryList")]
        [HttpPost]
        public async Task<IActionResult> MobileMenuSubCategoryList(int id)
        {

            var category = await _db.Categories.AsNoTracking()
                .Where(c => c.CategoryId.Equals(id)).FirstOrDefaultAsync();
            if (category == null)
            {
                return RedirectToAction(actionName: "MobileMenuCategoryList");
            }
            return ViewComponent(componentName: "MobileMenuSubCategoryList", arguments: new { parentId = id });
        }

        //Back to main mobile menu when click on back button
        //Handled by ajax
        [Route("Category/BackToMainMobileMenu")]
        public IActionResult BackToMainMobileMenu()
        {
            return PartialView("_MainMobileMenuList");
        }

        //Products of this category
        [Route("Category/{categoryTitle}/{id}")]
        [Route("Category/CategoryDetail/{id}")]
        
        //[Route("Category/CategoryDetail/{id}")]
        public async Task<IActionResult> CategoryDetail(int? id, string categoryTitle)
        {
            var maxPriceValue = (await _db.Products.MaxAsync(item => (double?)item.BasePrice)) ?? 1000000;
            ViewData["MaxPrice"] = (int)maxPriceValue;
            if (id == null)
            {
                categoryTitle = categoryTitle.Replace("-", " ");
                var category = await _db.Categories.AsNoTracking()
               .Where(c => c.Title.Contains(categoryTitle))
               .FirstOrDefaultAsync();
                return View(model: category);
            }
            else
            {
                var category = await _db.Categories.AsNoTracking()
               .Where(c => c.CategoryId.Equals(id))
               .FirstOrDefaultAsync();
                return View(model: category);
            }
           
        }

        //filter by categories selected
        [Route("Category/CategoryProductList")]
        [HttpPost]
        public IActionResult CategoryProductList(List<int?> catIds, 
            List<int?> brandIds,
            int defaultCategoryId, 
            int defaultBrandId, 
            int sort, 
            int pageIndex = 1,
            string searchText = null,
            int minValuePrice = 0,
            int maxValuePrice = 0,
            bool hasFreeDelivery = false,
            bool hasSellingStock = false)
        {
            
            return ViewComponent(componentName: "CategoryProductList", arguments: new {
                pageIndex = pageIndex,
                defaultCategoryId = defaultCategoryId,
                catIds = catIds,
                brandIds=brandIds,
                defaultBrandId = defaultBrandId,
                sort = sort,
                searchText = searchText,
                minValuePrice = minValuePrice,
                maxValuePrice = maxValuePrice,
                hasFreeDelivery = hasFreeDelivery,
                hasSellingStock = hasSellingStock
            });
        }
    }
}