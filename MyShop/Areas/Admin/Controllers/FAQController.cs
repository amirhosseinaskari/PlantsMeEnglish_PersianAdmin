using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext _db;
        public FAQController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/FAQ")]
        public IActionResult Index()
        {
            return View();
        }

        //List of faq
        [Route("Admin/FAQ/FAQList")]
        [HttpPost]
        public IActionResult FAQList()
        {
            return ViewComponent(componentName: "FAQList");
        }

        //Add new faq
        [Route("Admin/FAQ/AddFAQ")]
        [HttpPost]
        public async Task<IActionResult> AddFAQ(string question, string answer)
        {
            var faq = new FAQ();
            faq.Answer = answer;
            faq.Question = question;
            faq.Order = _db.FAQs.AsNoTracking().ToList().Count();
            _db.FAQs.Add(faq);
            await _db.SaveChangesAsync();
            return ViewComponent(componentName: "FAQList");
        }

        //Delete the faq 
        //parameters: => id: faq id
        [Route("Admin/FAQ/DeleteFAQ")]
        [HttpPost]
        public async Task<IActionResult> DeleteFAQ(int id)
        {
            var faq = await _db.FAQs.FindAsync(id);
            if (faq != null)
            {
                _db.FAQs.Remove(faq);
                await _db.SaveChangesAsync();
                var faqs = await _db.FAQs.OrderBy(c => c.Order).ToListAsync();
                int Neworder = 1;
                foreach (var item in faqs)
                {
                    item.Order = Neworder;


                    Neworder++;
                }
                _db.FAQs.UpdateRange(faqs);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction(actionName:"Index");
            }
           
            return RedirectToAction(actionName: "Index");
        }

        [Route("Admin/FAQ/ChangeFAQOrder")]
        [HttpPost]
        public async Task<IActionResult> ChangeFAQOrder(IEnumerable<int?> myIds)
        {
            int order = 1;
            var myfaqs = new List<FAQ>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                FAQ objFaq = await _db.FAQs.FindAsync(item);
                objFaq.Order = order;
                myfaqs.Add(objFaq);

                order++;
            }
            _db.FAQs.UpdateRange(myfaqs);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }
    }
}