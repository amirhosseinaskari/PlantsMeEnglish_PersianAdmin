using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;


namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public HomePageController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        [Route("Admin/HomePage")]
        public async Task<IActionResult> HomePage()
        {
            var homePage = await _db.HomePage.AsNoTracking().FirstOrDefaultAsync();

            return View(model: homePage);
        }
        //Edit logo 
        [Route("Admin/HomePage/EditLogo")]
        [HttpPost]
        public IActionResult EditLogo(IFormFile siteLogo)
        {
            //Create images/products/medium/product_ProductId Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/base", "logo.png" );
           
            if (System.IO.File.Exists(mediumPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(mediumPath);
            }

            using (var webPFileStream = new FileStream(mediumPath, FileMode.Create))
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(siteLogo.OpenReadStream())
                                .Resize(new Size(270, 0))
                                .Save(webPFileStream);
                }
            }
            return Json(true);
        }
        //Edit homepage totalInformation
        [Route("Admin/HomePage/EditTotalInformation")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(string title)
        {
            var homepage = await _db.HomePage.FirstOrDefaultAsync();
            if (homepage != null)
            {
                homepage.Title = title;
                _db.HomePage.Update(homepage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Edit hompage InstagramAPI
        [Route("Admin/HomePage/EditInstagramAPI")]
        [HttpPost]
        public async Task<IActionResult> EditInstagramAPI(string instagramAPI)
        {
            var homepage = await _db.HomePage.FirstOrDefaultAsync();
            if (homepage != null)
            {
                homepage.InstagramAPI = instagramAPI;
                _db.HomePage.Update(homepage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Edit homepage features
        [Route("Admin/HomePage/EditFeatures")]
        [HttpPost]
        public async Task<IActionResult> EditFeatures(bool hasFastDelivery,
            bool hasOriginalWarranty, bool hasLocalPaymentOption, bool has24Support)
        {
            var homepage = await _db.HomePage.FirstOrDefaultAsync();
            if (homepage != null)
            {
                homepage.Has24Support = has24Support;
                homepage.HasFastDeliveryOption = hasFastDelivery;
                homepage.HasOriginalWarranty = hasOriginalWarranty;
                homepage.HasLocalPaymentOption = hasLocalPaymentOption;
                _db.HomePage.Update(homepage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Edit Home Description
        [Route("Admin/HomePage/EditHomeDescription")]
        [HttpPost]
        public async Task<IActionResult> EditHomeDescription(string description)
        {
            var homePage = await _db.HomePage.FirstOrDefaultAsync();
            if(homePage != null)
            {
                homePage.Description = description;
                _db.HomePage.Update(homePage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);

        }

        //Edit Footer Description
        [Route("Admin/HomePage/EditFooterDescription")]
        [HttpPost]
        public async Task<IActionResult> EditFooterDescription(string description)
        {
            var homePage = await _db.HomePage.FirstOrDefaultAsync();
            if (homePage != null)
            {
                homePage.FooterDescription = description;
                _db.HomePage.Update(homePage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);

        }
        //SEO Setting
        [Route("Admin/HomePage/SEO")]
        [HttpPost]
        public async Task<IActionResult> SEO(string titlePage, string metaDescription, string redirectURL)
        {
            var homepage = await _db.HomePage.FirstOrDefaultAsync();
            if (homepage != null)
            {
                homepage.TitlePage = titlePage;
                homepage.MetaDescription = metaDescription;
                homepage.RedirectURL = redirectURL;
                _db.HomePage.Update(homepage);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }



    }
}