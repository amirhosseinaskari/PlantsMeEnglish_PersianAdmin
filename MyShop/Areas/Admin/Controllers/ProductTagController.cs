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
    public class ProductTagController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Add new product tag
        //parameters: => id: product id of tag / text: text of tag
        [Route("Admin/ProductTag/CreateTag")]
        [HttpPost]
        public async Task<IActionResult> CreateTag(int id,string text)
        {
            try
            {
                var tag = new ProductTag();
                tag.ProductId = id;
                tag.Text = text;
                await _db.ProductTags.AddAsync(tag);
                await _db.SaveChangesAsync();
                return ViewComponent(componentName: "ProductTagList", arguments: new { id = id });
            }
            catch (Exception)
            {

                return Json(false);
            }
           
        }

        //Delete a product tag
        //parameters: => id: product tag id
        [Route("Admin/ProductTag/DeleteTag")]
        [HttpPost]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _db.ProductTags.FindAsync(id);
            try
            {
                _db.ProductTags.Remove(tag);
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