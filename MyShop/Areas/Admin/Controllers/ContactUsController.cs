using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ContactUsController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/ContactUs")]
        public async Task<IActionResult> Index()
        {
            var contactUs = await _db.ContactUs.AsNoTracking().FirstOrDefaultAsync();
         
            return View(model:contactUs);
        }

        //Edit total information
        [Route("Admin/ContactUs/EditTotalInformation")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(string email, string phone,
            string mobile, string address, string whatsapp, string googleMap,string googleMapLink)
        {
            var contactUs = await _db.ContactUs.FirstOrDefaultAsync();
            if (contactUs != null)
            {
                if (email != null)
                {
                    contactUs.Email = email.Trim();
                }
               if(phone != null)
                {
                    contactUs.Phone = phone.Trim();
                }
                if (mobile != null)
                {
                    contactUs.Mobile = mobile.Trim();
                }
                if (address != null)
                {
                    contactUs.Address = address.Trim();
                }
                if (whatsapp != null)
                {
                    contactUs.WhatsAppNumber = whatsapp.Trim();
                }
                if (googleMap != null)
                {
                    contactUs.GoogleMap = googleMap.Trim();
                }
                if (googleMapLink != null)
                {
                    contactUs.GoogleMapLink = googleMapLink.Trim();
                }
                _db.ContactUs.Update(contactUs);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Edit social networks
        [Route("Admin/ContactUs/SocialNetworks")]
        [HttpPost]
        public async Task<IActionResult> SocialNetworks(string instagram, string twitter,
            string telegram, string facebook,string youtube,string aparat)
        {
            var contactus = await _db.ContactUs.FirstOrDefaultAsync();
            if (contactus != null)
            {
                if (instagram != null)
                {
                    contactus.Instagram = instagram.Trim();
                }
                if (twitter != null)
                {
                    contactus.Twitter = twitter.Trim();
                }
                if (telegram != null)
                {
                    contactus.Telegram = telegram.Trim();
                }
               
                if (facebook != null)
                {
                    contactus.Facebook = facebook.Trim();
                }
                if (aparat != null)
                {
                    contactus.Aparat = aparat.Trim();
                }
                if (youtube != null)
                {
                    contactus.YouTube = youtube.Trim();
                }
                _db.ContactUs.Update(contactus);
                await _db.SaveChangesAsync();
                return Json(true);

            }
            return Json(false);
        }

        //SEO setting
        [Route("Admin/ContactUs/SEO")]
        [HttpPost]
        public async Task<IActionResult> SEO(string titlePage,string metaDescription, string redirectURL)
        {
            var contactUs = await _db.ContactUs.FirstOrDefaultAsync();
            if (contactUs != null)
            {
                if (titlePage != null)
                {
                    contactUs.TitlePage = titlePage.Trim();
                }
                if (metaDescription != null)
                {
                    contactUs.MetaDescription = metaDescription.Trim();
                }
                if (redirectURL != null)
                {
                    contactUs.RedirectURL = redirectURL.Trim();
                }
                _db.ContactUs.Update(contactUs);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

    }
}