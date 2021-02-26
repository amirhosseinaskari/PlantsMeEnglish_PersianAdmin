using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public CategoryController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : SEO
        {
            [Required(ErrorMessage = "تعیین کردن نام الزامی است",
                 AllowEmptyStrings = false)]
            [Display(Name = "عنوان دسته بندی")]
            public string Title { get; set; }
            public string Description { get; set; }
            public IFormFile CategoryImage { get; set; }
            public int ParentId { get; set; }
            public bool CanAddProduct { get; set; }
            public bool PutOnHomePage { get; set; }
        }
        // GET: Admin/Category | Admin/Category/CreateCategory
        [Route("Admin/Category")]
        [Route("Admin/Category/CreateCategory")]
        [HttpGet]
        public ActionResult CreateCategory()
        {
            ViewData["parent-id"] = -1;
            return View();
        }

        //For Sub Categories
        [Route("Admin/Category/CreateCategory/{id}")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public IActionResult CreateCategory(int id)
        {

            ViewData["parent-id"] = id;
            return View();
        }
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> CreateCategory(string m)
        {

            if (ModelState.IsValid)
            {
                Category category = new Category();
                category.Title = Input.Title;
                category.TitlePage = Input.TitlePage;
                category.Description = Input.Description;
                category.MetaDescription = Input.MetaDescription;
                category.RedirectURL = Input.RedirectURL;
                category.ParentId = Input.ParentId;
                if (category.ParentId != -1)
                {
                    category.HasParent = true;
                }
                category.CanAddProduct = Input.CanAddProduct;
                category.PutOnHomePage = Input.PutOnHomePage;
                category.CatOrder = await _db.Categories.AsNoTracking()
                    .Where(c => c.ParentId == category.ParentId)
                    .CountAsync();
                category.IsPublished = false;
                //save category image
                if (Input.CategoryImage != null)
                {


                    //Create images/categories/medium/ Folder
                    string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories", "medium");
                    if (!Directory.Exists(mediumPath))
                    {
                        Directory.CreateDirectory(mediumPath);
                    }


                    //Create images/categories/small/ Folder
                    string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories", "small");
                    if (!Directory.Exists(smallPath))
                    {
                        Directory.CreateDirectory(smallPath);
                    }


                    category.CategoryImage = "_" + System.DateTime.Now.ToString("ddmmss") +
                        Path.GetFileName(Input.CategoryImage.FileName);
                 
                    //final medium path
                    var finalMediumPath = System.IO.Path.Combine(mediumPath, category.CategoryImage);
                    //final small path
                    var finalSmallPath = System.IO.Path.Combine(smallPath, category.CategoryImage);

                

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();

                    using (var webPFileStream = new FileStream(finalMediumPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {

                                imageFactory.Load(Input.CategoryImage.OpenReadStream())
                                      .Resize(new Size(400, 0))
                                      .Save(webPFileStream);

                        }
                    }

                    using (var webPFileStream = new FileStream(finalSmallPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                                imageFactory.Load(Input.CategoryImage.OpenReadStream())
                                     .Resize(new Size(250, 0))
                                     .Save(webPFileStream);

                        }
                    }



                   




                }
                //add category to category table
                await _db.Categories.AddAsync(category);
                await _db.SaveChangesAsync();
                if (category.HasParent)
                {
                    SubCategory subCategory = new SubCategory();
                    subCategory.MyIdInCatTable = category.CategoryId;
                    subCategory.CategoryId = Input.ParentId;
                    await _db.SubCategories.AddAsync(subCategory);
                    await _db.SaveChangesAsync();
                }
                ViewData["parent-id"] = category.ParentId;
                Input = null;
                HttpContext.Session.SetInt32("Message", (int)Messages.CategoryCreatedSuccessfully);
                return View();
            }
            return View();
        }
        [Route("Admin/Category/ChangeCategoryOrder")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> ChangeCategoryOrder(IEnumerable<int?> myIds)
        {
            int order = 1;
            var mycats = new List<Category>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                Category objCat = await _db.Categories.FindAsync(item);
                objCat.CatOrder = order;
                mycats.Add(objCat);

                order++;
            }
            _db.Categories.UpdateRange(mycats);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
        [HttpPost]
        [Route("Admin/Category/Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            category.IsPublished = true;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
        [HttpPost]
        [Route("Admin/Category/UnPublish")]
        public async Task<IActionResult> UnPublish(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            category.IsPublished = false;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
        [Route("Admin/Category/EditCategory/{id}")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            InputModel input = new InputModel();
            input.Title = category.Title;
            input.Description = category.Description;
            input.MetaDescription = category.MetaDescription;
            input.TitlePage = category.TitlePage;
            input.PutOnHomePage = category.PutOnHomePage;
            input.CanAddProduct = category.CanAddProduct;
            input.ParentId = category.ParentId;
            input.RedirectURL = category.RedirectURL;
            ViewData["ImageName"] = category.CategoryImage;
            ViewData["CategoryId"] = category.CategoryId;
            return View(model: input);
        }
        [Route("Admin/Category/EditCategory/{catId}")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> EditCategory(int catId, string m)
        {
            var category = await _db.Categories.FindAsync(catId);
            if (category != null)
            {
                category.Title = Input.Title;
                category.TitlePage = Input.TitlePage;
                category.MetaDescription = Input.MetaDescription;
                category.RedirectURL = Input.RedirectURL;
                category.ParentId = Input.ParentId;
                category.PutOnHomePage = Input.PutOnHomePage;
                category.Description = Input.Description;
                if (category.ParentId != -1)
                {
                    category.HasParent = true;
                }
                else
                {
                    category.HasParent = false;
                }

                category.CanAddProduct = Input.CanAddProduct;
                category.PutOnHomePage = Input.PutOnHomePage;
                if (Input.CategoryImage != null)
                {
                    //Delete old image and save new image in large folder
                    //Delete old large image:
                    if (category.CategoryImage != null)
                    {
                        var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/large");
                        var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(category.CategoryImage));
                        var finalSameImage = Path.Combine(oldLargePath, Path.GetFileName(Input.CategoryImage.FileName));
                        try
                        {
                            if (System.IO.File.Exists(finalOldLargePath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalOldLargePath);
                            }
                            if (System.IO.File.Exists(finalSameImage))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalSameImage);
                            }
                        }
                        catch (Exception e)
                        {
                        }

                        //Delete old image and save new image in medium folder
                        //Delete old medium image:
                        var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/medium");
                        var finalOldMediumPath = System.IO.Path.Combine(oldMediumPath, Path.GetFileName(category.CategoryImage));
                        var finalSameMediumPath = System.IO.Path.Combine(oldMediumPath, Path.GetFileName(Input.CategoryImage.FileName));
                        try
                        {
                            if (System.IO.File.Exists(finalOldMediumPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalOldMediumPath);
                            }
                            if (System.IO.File.Exists(finalSameMediumPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalSameMediumPath);
                            }
                        }
                        catch (Exception e)
                        {
                        }


                        //Delete old image and save new image in small folder
                        //Delete old small image:
                        var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/small");
                        var finalOldSmallPath = System.IO.Path.Combine(oldSmallPath, Path.GetFileName(category.CategoryImage));
                        var finalSameSmallPath = System.IO.Path.Combine(oldSmallPath, Path.GetFileName(Input.CategoryImage.FileName));
                        try
                        {
                            if (System.IO.File.Exists(finalOldSmallPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalOldSmallPath);
                            }
                            if (System.IO.File.Exists(finalSameSmallPath))
                            {
                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(finalSameSmallPath);
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }




                    category.CategoryImage = "_" + System.DateTime.Now.ToString("ddmmss") + Path.GetFileName(Input.CategoryImage.FileName);

                 
                    //path of medium image:
                    var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/medium");
                    var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, category.CategoryImage);
                    //path small image:
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/small");
                    var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, category.CategoryImage);

                  

                    using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(Input.CategoryImage.OpenReadStream())
                                        .Resize(new Size(400, 0))
                                        .Save(webPFileStream);
                        }
                        await webPFileStream.DisposeAsync();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }

                    using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(Input.CategoryImage.OpenReadStream())
                                        .Resize(new Size(250, 0))
                                        .Save(webPFileStream);
                        }
                        await webPFileStream.DisposeAsync();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }

                }

                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                if (category.HasParent)
                {
                    var subcat = await _db.SubCategories
                        .Where(c => c.MyIdInCatTable == catId)
                       .FirstOrDefaultAsync();
                    if (subcat == null)
                    {
                        SubCategory subCategory = new SubCategory();
                        subCategory.MyIdInCatTable = catId;
                        subCategory.CategoryId = Input.ParentId;
                        await _db.SubCategories.AddAsync(subCategory);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        subcat.CategoryId = Input.ParentId;
                        _db.SubCategories.Update(subcat);
                        await _db.SaveChangesAsync();
                    }

                }
                else
                {
                    var subcat = await _db.SubCategories
                         .Where(c => c.MyIdInCatTable == catId)
                        .FirstOrDefaultAsync();
                    if (subcat != null)
                    {
                        _db.SubCategories.Remove(subcat);
                        await _db.SaveChangesAsync();
                    }
                }
                ViewData["parent-id"] = category.ParentId;
                Input = null;
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction(actionName: "CreateCategory");
            }
            return View(model: Input);
        }


        [Route("Admin/Category/DeleteCategory")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            var subcategories = await _db.Categories
                .Where(c => c.ParentId == category.CategoryId)
                .ToListAsync();
            foreach (var item in subcategories)
            {
                //Delete category Images
                //Large images
                if (string.IsNullOrEmpty(item.CategoryImage))
                {
                    continue;
                }
                var oldLargePath01 = System.IO.Path.Combine(_host.WebRootPath, "images/categories/large");
                var finalOldLargePath01 = Path.Combine(oldLargePath01, Path.GetFileName(item.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldLargePath01))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldLargePath01);
                    }

                }
                catch (Exception e)
                {
                }

                //Medium Images
                var oldMediumPath01 = System.IO.Path.Combine(_host.WebRootPath, "images/categories/medium");
                var finalOldMediumPath01 = Path.Combine(oldMediumPath01, Path.GetFileName(item.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldMediumPath01))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldMediumPath01);
                    }

                }
                catch (Exception e)
                {
                }
                //Small Images
                var oldSmallPath01 = System.IO.Path.Combine(_host.WebRootPath, "images/categories/small");
                var finalOldSmallPath01 = Path.Combine(oldSmallPath01, Path.GetFileName(item.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldSmallPath01))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldSmallPath01);
                    }

                }
                catch (Exception e)
                {
                }
            }
            _db.Categories.RemoveRange(subcategories);
            await _db.SaveChangesAsync();
            var subcategory = await _db.SubCategories.Where(c => c.MyIdInCatTable.Equals(id)).FirstOrDefaultAsync();
            if (subcategory != null)
            {
                _db.SubCategories.Remove(subcategory);
                await _db.SaveChangesAsync();
            }
            //Delete category Images
            //Large images
            if (!string.IsNullOrEmpty(category.CategoryImage))
            {
                var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/large");
                var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(category.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldLargePath))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldLargePath);
                    }

                }
                catch (Exception e)
                {
                }

                //Medium Images
                var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/medium");
                var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(category.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldMediumPath))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldMediumPath);
                    }

                }
                catch (Exception e)
                {
                }
                //Small Images
                var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/categories/small");
                var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(category.CategoryImage));
                try
                {
                    if (System.IO.File.Exists(finalOldSmallPath))
                    {
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(finalOldSmallPath);
                    }

                }
                catch (Exception e)
                {
                }
            }
            
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            var categories = await _db.Categories.OrderBy(c => c.CatOrder).ToListAsync();
            int order = 1;
            foreach (var item in categories)
            {
                item.CatOrder = order;


                order++;
            }
            _db.Categories.UpdateRange(categories);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "CreateCategory");
        }
    }
}
