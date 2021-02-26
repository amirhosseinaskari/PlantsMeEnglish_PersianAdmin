using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactUsController(ApplicationDbContext db)
        {
            _db = db;

        }

        
        [Route("ContactUs")]
        public async Task<IActionResult> Index()
        {
            var contactUs = await _db.ContactUs.AsNoTracking()
                .FirstOrDefaultAsync();
            return View(model:contactUs);
        }

        [Route("ContactUs/SendMessage")]
        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactMessage message)
        {
            if(message != null)
            {
                var objMessage = new ContactMessage();
                objMessage.Description = message.Description.Trim();
                objMessage.FullName = message.FullName.Trim();
                objMessage.Email = message.Email.Trim();
                objMessage.Mobile = message.Mobile.Trim();
                await _db.Messages.AddAsync(objMessage);
                await _db.SaveChangesAsync();
                objMessage.SendEmail(receiverEmail: "shopikarweb@gmail.com", password: "6507654a", 
                   smtpHost:"smtp.gmail.com",smtpPort: Convert.ToInt32(587));
                
                HttpContext.Session.SetInt32("Message", (int)Messages.MessageSentSuccessfully);
                return RedirectToAction("Index");

            }
            HttpContext.Session.SetInt32("Message", (int)Messages.ErrorSendingEmail);
            return RedirectToAction("Index");

        }
    }
}