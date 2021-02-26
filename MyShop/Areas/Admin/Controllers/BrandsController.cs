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
using LazZiya.ImageResize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public BrandsController(ApplicationDbContext db, IWebHostEnvironment host)
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
            public IFormFile Image { get; set; }
            public bool PutOnHomePage { get; set; }
            public string Description { get; set; }
        }
        [Route("Admin/Brands")]
        [Route("Admin/Brands/CreateBrand")]
        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }

        [Route("Admin/Brands/CreateBrand")]
        [HttpPost]
        public async Task<IActionResult> CreateBrand(string m)
        {
            if (ModelState.IsValid)
            {
                var brand = new Brand();
                brand.BrandOrder =await _db.Brands.AsNoTracking().CountAsync();
                brand.Title = Input.Title.Trim();
                brand.TitlePage = Input.TitlePage;
                brand.MetaDescription = Input.MetaDescription;
                brand.RedirectURL = Input.RedirectURL;
                brand.IsPublished = false;
                brand.PutOnHomePage = Input.PutOnHomePage;
                brand.Description = Input.Description;
                if (Input.Image != null)
                {
                    //Create images/products/large/product_ProductId Folder
                    string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "large");
                    if (!Directory.Exists(largePath))
                    {
                        Directory.CreateDirectory(largePath);
                    }


                    //Create images/products/medium/product_ProductId Folder
                    string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "medium");
                    if (!Directory.Exists(mediumPath))
                    {
                        Directory.CreateDirectory(mediumPath);
                    }


                    //Create images/products/small/product_ProductId Folder
                    string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "small");
                    if (!Directory.Exists(smallPath))
                    {
                        Directory.CreateDirectory(smallPath);
                    }

                    brand.Image = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmss") + Path.GetFileName(Input.Image.FileName);
                    //path of large image
                    var finalLargePath = Path.Combine(largePath, brand.Image);
                    //path of medium image
                    var finalMediumPath = System.IO.Path.Combine(mediumPath, brand.Image);
                    //path of small image
                    var finalSmallPath = System.IO.Path.Combine(smallPath, brand.Image);
                    
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                  
                        using (var webPFileStream = new FileStream(finalLargePath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(600, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(400, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(200, 0))
                                            .Save(webPFileStream);
                            }
                        }


                
                    

                }
                else
                {
                    brand.Image = null;
                }
                //Save new brand in database
                await _db.Brands.AddAsync(brand);
                await _db.SaveChangesAsync();
            }
            return View();
        }
        [Route("Admin/Brands/EditBrand/{id}")]
        [HttpGet]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> EditBrand(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            InputModel input = new InputModel();
            input.Title = brand.Title;
            input.MetaDescription = brand.MetaDescription;
            input.TitlePage = brand.TitlePage;
            input.RedirectURL = brand.RedirectURL;
            input.PutOnHomePage = brand.PutOnHomePage;
            input.Description = brand.Description;
            ViewData["ImageName"] = brand.Image;
            ViewData["BrandId"] = brand.BrandId;
            return View(model: input);
        }

        [Route("Admin/Brands/EditBrand/{brandId}")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> EditBrand(int brandId, string m)
        {
            var brand = await _db.Brands.FindAsync(brandId);
            if (ModelState.IsValid)
            {
                brand.Title = Input.Title.Trim();
                brand.TitlePage = Input.TitlePage;
                brand.MetaDescription = Input.MetaDescription;
                brand.RedirectURL = Input.RedirectURL;
                brand.PutOnHomePage = Input.PutOnHomePage;
                brand.Description = Input.Description;
                if (Input.Image != null)
                {
                    //Delete old image and save new image in large folder
                    //Delete old large image:
                    if (brand.Image != null)
                    {
                        var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/large");
                        var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(brand.Image));
                        var finalSameImage = Path.Combine(oldLargePath, Path.GetFileName(Input.Image.FileName));
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
                        var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/medium");
                        var finalOldMediumPath = System.IO.Path.Combine(oldMediumPath, Path.GetFileName(brand.Image));
                        var finalSameMediumPath = System.IO.Path.Combine(oldMediumPath, Path.GetFileName(Input.Image.FileName));
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
                        var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/small");
                        var finalOldSmallPath = System.IO.Path.Combine(oldSmallPath, Path.GetFileName(brand.Image));
                        var finalSameSmallPath = System.IO.Path.Combine(oldSmallPath, Path.GetFileName(Input.Image.FileName));
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

                    brand.Image = "_" + System.DateTime.Now.ToString("YYYYMMDDhhmmss") + Path.GetFileName(Input.Image.FileName);
                    //Create images/products/large/product_ProductId Folder
                    string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "large");
                    if (!Directory.Exists(largePath))
                    {
                        Directory.CreateDirectory(largePath);
                    }


                    //Create images/products/medium/product_ProductId Folder
                    string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "medium");
                    if (!Directory.Exists(mediumPath))
                    {
                        Directory.CreateDirectory(mediumPath);
                    }


                    //Create images/products/small/product_ProductId Folder
                    string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands", "small");
                    if (!Directory.Exists(smallPath))
                    {
                        Directory.CreateDirectory(smallPath);
                    }
                    //Save new large image:

                   // var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/large");
                    var finalNewLargePath = Path.Combine(largePath, brand.Image);
                    using (var stream = System.IO.File.Create(finalNewLargePath))
                    {
                        await Input.Image.CopyToAsync(stream);
                        await stream.DisposeAsync();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                    //path of medium image:
                   // var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/medium");
                    var finalNewMediumPath = System.IO.Path.Combine(mediumPath, brand.Image);
                    //path of small image:
                    //var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/small");
                    var finalNewSmallPath = System.IO.Path.Combine(smallPath, brand.Image);

                   
                        using (var webPFileStream = new FileStream(finalNewLargePath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(600, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(400, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(Input.Image.OpenReadStream())
                                            .Resize(new Size(200, 0))
                                            .Save(webPFileStream);
                            }
                        }
                    
                   

                    
               

                }


                //Update and save the brand in database
                _db.Brands.Update(brand);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction(actionName: "CreateBrand");
            }
            return View(model: Input);
        }
        [Route("Admin/Brands/ChangeBrandOrder")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> ChangeBrandOrder(IEnumerable<int?> myIds)
        {
            int order = 1;
            var mybrands = new List<Brand>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                Brand objBrand = await _db.Brands.FindAsync(item);
                objBrand.BrandOrder = order;
                mybrands.Add(objBrand);
              
                order++;
            }
            _db.Brands.UpdateRange(mybrands);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [HttpPost]
        [Route("Admin/Brands/Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            brand.IsPublished = true;
            _db.Brands.Update(brand);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
        [HttpPost]
        [Route("Admin/Brands/UnPublish")]
        public async Task<IActionResult> UnPublish(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            brand.IsPublished = false;
            _db.Brands.Update(brand);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [Route("Admin/Brands/DeleteBrand")]
        [HttpPost]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            //Delete Brand Images
            //Large images
            if(brand.Image != null)
            {
                var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/large");
                var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(brand.Image));
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
                var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/medium");
                var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(brand.Image));
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
                var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/brands/small");
                var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(brand.Image));
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
          
            var products = await _db.Products
                .Where(c => c.BrandId.Equals(id))
                .ToListAsync();
            foreach (var item in products)
            {
                item.BrandId = -1;
               
            }
            _db.Products.UpdateRange(products);
            await _db.SaveChangesAsync();
            _db.Brands.Remove(brand);
            await _db.SaveChangesAsync();
            var brands = await _db.Brands.OrderBy(c => c.BrandOrder).ToListAsync();
            int order = 1;
            foreach (var item in brands)
            {
                item.BrandOrder = order;
              
                order++;
            }
            _db.Brands.UpdateRange(brands);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "CreateBrand");
        }
    }
}