﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MyShop.InfraStructure;
using Microsoft.AspNetCore.Authorization;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SpecialDiscountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialDiscountController(ApplicationDbContext db)
        {
            _db = db;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "وارد کردن مبلغ الزامی است")]
            public double DiscountPrice { get; set; }
        
            [Required(ErrorMessage = "انتخاب محصول الزامی است")]
            public int ProductId { get; set; }
            [Required(ErrorMessage = "وارد کردن تاربخ فعالسازی الزامی است")]
            public string ActivationDate { get; set; }
            [Required(ErrorMessage = "وارد کردن تاریخ انقضا الزامی است")]
            public string ExpiredDate { get; set; }
            public bool Publishment { get; set; }
        }
        [Route("Admin/SpecialDiscount")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Admin/SpecialDiscount/CreateSpecialDiscount")]
        public IActionResult CreateSpecialDiscount()
        {
            return View();
        }
        [Route("Admin/SpecialDiscount/CreateSpecialDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateSpecialDiscount(string m)
        {
            if (ModelState.IsValid)
            {
                var specialDiscount = new SpecialDiscount();
                try
                {
                    var splitActiveDate = Input.ActivationDate.Split("/");
                    var splitExpireDate = Input.ExpiredDate.Split("/");
                    PersianCalendar persianCalendar = new PersianCalendar();
                    specialDiscount.ActivationDate = persianCalendar
                        .ToDateTime(Int32.Parse(splitActiveDate[0]), Int32.Parse(splitActiveDate[1]),
                        Int32.Parse(splitActiveDate[2]), 0, 0, 0, 0);
                    specialDiscount.ExpirationDate = persianCalendar
                        .ToDateTime(Int32.Parse(splitExpireDate[0]), Int32.Parse(splitExpireDate[1]),
                        Int32.Parse(splitExpireDate[2]), 0, 0, 0, 0);
                    if (specialDiscount.ExpirationDate <= specialDiscount.ActivationDate)
                    {
                        ModelState.AddModelError("ExpiredDate", "تاریخ انقضا باید بعد از تاریخ فعالسازی باشد");
                        return View();
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError("ExpiredDate", "فرمت تاریخ های وارد شده صحیح نیست");
                    ModelState.AddModelError("ActivationDate", "فرمت تاریخ های وارد شده صحیح نیست");
                    return View();
                }
                if (Input.ProductId < 1)
                {
                    ModelState.AddModelError("ProductId", "انتخاب محصول الزامی است");
                    return View();
                }
                specialDiscount.DiscountPrice = Input.DiscountPrice;
                specialDiscount.IsPublished = Input.Publishment;
                var oldSpecialDiscount = await _db.SpecialDiscount.AsNoTracking()
                    .Where(c => c.ProductId.Equals(Input.ProductId))
                    .FirstOrDefaultAsync();
                if (oldSpecialDiscount != null)
                {
                    ModelState.AddModelError("ProductId", "قبلا یک پیشنهاد شگفت انگیز برای این محصول ساخته شده است");
                    return View();
                }
                specialDiscount.ProductId = Input.ProductId;
                specialDiscount.ProductName = await _db.Products.AsNoTracking()
                .Where(c => c.Id.Equals(Input.ProductId))
                .Select(c => c.Title).FirstOrDefaultAsync();
                specialDiscount.BasePrice = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(Input.ProductId))
                    .Select(c => c.BasePrice)
                    .FirstOrDefaultAsync();
                await _db.SpecialDiscount.AddAsync(specialDiscount);
                await _db.SaveChangesAsync();

               
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.SpecialDiscountCreatedSuccessfully);
                return RedirectToAction(actionName: "Index");
            }

            return View();
        }


        //Edit special discount
        [Route("Admin/SpecialDiscount/EditSpecialDiscount/{id}")]
        public async Task<IActionResult> EditSpecialDiscount(int id)
        {
            var discount = await _db.SpecialDiscount.FindAsync(id);
            var input = new InputModel();
            var persianDate = new PersianCalendar();
            input.ActivationDate = string.Format("{0}/{1}/{2}", persianDate.GetYear(discount.ActivationDate),
                persianDate.GetMonth(discount.ActivationDate),
                persianDate.GetDayOfMonth(discount.ActivationDate));
            input.ExpiredDate = string.Format("{0}/{1}/{2}", persianDate.GetYear(discount.ExpirationDate),
               persianDate.GetMonth(discount.ExpirationDate),
               persianDate.GetDayOfMonth(discount.ExpirationDate));
            input.DiscountPrice = discount.DiscountPrice;
            input.ProductId = discount.ProductId;
            input.Publishment = discount.IsPublished;
            ViewData["DiscountId"] = discount.Id;
            return View(model: input);
        }
        [Route("Admin/SpecialDiscount/EditSpecialDiscount")]
        public async Task<IActionResult> EditSpecialDiscount(int id, string m)
        {
            var specialDiscount = await _db.SpecialDiscount.FindAsync(id);
            ViewData["DiscountId"] = id;
            if (ModelState.IsValid)
            {
                try
                {
                    var splitActiveDate = Input.ActivationDate.Split("/");
                    var splitExpireDate = Input.ExpiredDate.Split("/");
                    PersianCalendar persianCalendar = new PersianCalendar();
                    specialDiscount.ActivationDate = persianCalendar
                        .ToDateTime(Int32.Parse(splitActiveDate[0]), Int32.Parse(splitActiveDate[1]),
                        Int32.Parse(splitActiveDate[2]), 0, 0, 0, 0);
                    specialDiscount.ExpirationDate = persianCalendar
                        .ToDateTime(Int32.Parse(splitExpireDate[0]), Int32.Parse(splitExpireDate[1]),
                        Int32.Parse(splitExpireDate[2]), 0, 0, 0, 0);
                    if (specialDiscount.ExpirationDate <= specialDiscount.ActivationDate)
                    {
                        ModelState.AddModelError("ExpiredDate", "تاریخ انقضا باید بعد از تاریخ فعالسازی باشد");
                        return View();
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError("ExpiredDate", "فرمت تاریخ های وارد شده صحیح نیست");
                    ModelState.AddModelError("ActivationDate", "فرمت تاریخ های وارد شده صحیح نیست");
                    return View();
                }
                specialDiscount.DiscountPrice = Input.DiscountPrice;
                specialDiscount.IsPublished = Input.Publishment;
                var oldSpecialDiscount = await _db.SpecialDiscount.AsNoTracking()
                    .Where(c => c.ProductId.Equals(Input.ProductId) && !c.Id.Equals(id))
                    .FirstOrDefaultAsync();
                if (oldSpecialDiscount != null)
                {
                    ModelState.AddModelError("ProductId", "قبلا یک پیشنهاد شگفت انگیز برای این محصول ساخته شده است");
                    return View();
                }
                specialDiscount.ProductId = Input.ProductId;
                specialDiscount.ProductName = await _db.Products.AsNoTracking()
                .Where(c => c.Id.Equals(Input.ProductId))
                .Select(c => c.Title).FirstOrDefaultAsync();
                specialDiscount.BasePrice = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(Input.ProductId))
                    .Select(c => c.BasePrice)
                    .FirstOrDefaultAsync();
                _db.SpecialDiscount.Update(specialDiscount);
                await _db.SaveChangesAsync();
               
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction(actionName: "Index");
            }
            return View();
        }
        //Special-Discount List when click on pagination button 
        //TODO: go to the target page of special discount list
        //Parameteres: pageIndex: index of page 
        [Route("Admin/SpecialDiscount/SpecialDiscountList")]
        [HttpPost]
        public IActionResult SpecialDiscountList(int pageIndex = 1, int sortType = 0, string text = null)
        {
            return ViewComponent(componentName: "SpecialDiscountList",
                arguments: new { pageIndex = pageIndex, sortType = sortType, text = text });
        }

        //Publish
        [Route("Admin/SpecialDiscount/Publish")]
        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            try
            {
                var discount = await _db.SpecialDiscount.FindAsync(id);
                discount.IsPublished = true;
                _db.SpecialDiscount.Update(discount);
                await _db.SaveChangesAsync();
               
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //UnPublish
        [Route("Admin/SpecialDiscount/UnPublish")]
        [HttpPost]
        public async Task<IActionResult> UnPublish(int id)
        {
            try
            {
                var discount = await _db.SpecialDiscount.FindAsync(id);
                discount.IsPublished = false;
                _db.SpecialDiscount.Update(discount);
                await _db.SaveChangesAsync();
              
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Publish a group of special discounts
        //parameters: => ids:special discount ids
        [Route("Admin/SpecialDiscount/PublishGroupSpecialDiscount")]
        [HttpPost]
        public async Task<IActionResult> PublishGroupSpecialDiscount(List<int?> ids)
        {
            try
            {
                var discounts = await _db.SpecialDiscount.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in discounts)
                {
                    item.IsPublished = true;
                 
                }
                _db.SpecialDiscount.UpdateRange(discounts);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }


        }

        //UnPublish a group of special discounts
        //parameters: => ids:special discount ids
        [Route("Admin/SpecialDiscount/UnPublishGroupSpecialDiscount")]
        [HttpPost]
        public async Task<IActionResult> UnPublishGroupSpecialDiscount(List<int?> ids)
        {
            try
            {
                var discounts = await _db.SpecialDiscount.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in discounts)
                {
                    item.IsPublished = false;
                
                }
                _db.SpecialDiscount.UpdateRange(discounts);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

        //Delete a group of special discounts
        //parameters: => ids: product ids
        [Route("Admin/SpecialDiscount/DeleteGroupSpecialDiscount")]
        [HttpPost]
        public async Task<IActionResult> DeleteGroupSpecialDiscount(List<int?> ids)
        {
            var discounts = await _db.SpecialDiscount.Where(c => ids.Contains(c.Id)).ToListAsync();

            _db.SpecialDiscount.RemoveRange(discounts);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "SpecialDiscount");
        }

        //Delete Special Discount
        [Route("Admin/SpecialDiscount/DeleteSpecialDiscount")]
        [HttpPost]
        public async Task<IActionResult> DeleteSpecialDiscount(int id)
        {
            var discount = await _db.SpecialDiscount.FindAsync(id);

            _db.SpecialDiscount.Remove(discount);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "SpecialDiscount");
        }
    }
}