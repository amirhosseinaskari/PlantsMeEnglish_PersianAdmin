using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DiscountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public DiscountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Code { get; set; }
            public byte Percent { get; set; }
            public int Number { get; set; }
            public DateTime ActivationDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public bool IsPublished { get; set; }
            public bool IsForAllCustomers { get; set; }
            public bool IsForAllProducts { get; set; }
            public bool IsForMinBuyValue { get; set; }
            public bool IsForMinBuyNumber { get; set; }
            public double MinBuyValue { get; set; }
            public int MinBuyNumber { get; set; }
        }
        [Route("Admin/Discount")]
        public IActionResult Index()
        {
            return View();
        }
        
        //Create new discount with two parameters (code and number of discount)
        //modal form in index
        [Route("Admin/Discount/CreateDiscount")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscount()
        {
            if (ModelState.IsValid)
            {
                var sameDiscount = await _db.Discounts.AsNoTracking()
                    .Where(c => c.Code.Equals(Input.Code))
                    .FirstOrDefaultAsync();
                if(sameDiscount != null)
                {
                    HttpContext.Session.SetInt32("Message", (int)Messages.ErrorCreateProduct);
                    return RedirectToAction(actionName: "Index");
                }
                var discount = new Discount();
                discount.Code = Input.Code;
                discount.Number = Input.Number;
                discount.RemainingNumber = Input.Number;
                discount.Percent = Input.Percent;
                await _db.Discounts.AddAsync(discount);
                await _db.SaveChangesAsync();

                var products = await _db.Products.ToListAsync();
                foreach (var item in products)
                {
                    item.DiscountCode = discount.Code;
                }
                _db.Products.UpdateRange(products);
                return View(viewName: "EditDiscount", model: discount);
            }
            HttpContext.Session.SetInt32("Message", (int)Messages.ErrorCreateProduct);
            return RedirectToAction(actionName: "Index");
        }


        //Check unique discount code
        //Handled by ajax
        [Route("Admin/Discount/CheckUniqueDiscountCode")]
        [HttpPost]
        public async Task<IActionResult> CheckUniqueDiscountCode(string code)
        {
            var discounts = await _db.Discounts.AsNoTracking()
                .Where(c => c.Code.Equals(code))
                .ToListAsync();
            if(discounts.Count() > 0)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }

        //Edit discount
        [Route("Admin/Discount/EditDiscount/{id}")]
        public async Task<IActionResult> EditDiscount(int id)
        {
            var discount = await _db.Discounts.FindAsync(id);

            return View(model:discount);
        }

        //Edit Total Information
        //Handled by ajax
        [Route("Admin/Discount/EditTotalInformation")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(string code, byte percent, int number, int id)
        {
            var discount =await _db.Discounts.FindAsync(id);
            if (discount != null)
            {
                discount.Code = code;
                discount.Percent = percent;
                discount.Number = number;
                discount.RemainingNumber = number;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Edit discount target
        //Handled by ajax
        [Route("Admin/Discount/DiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> EditDiscountTarget(int id, DiscountTarget discountTarget)
        {
            var discount = await _db.Discounts.FindAsync(id);
            if (discount != null)
            {
                discount.DiscountTarget = discountTarget;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                if (discountTarget.Equals(DiscountTarget.AllProducts))
                {
                    var products = await _db.Products.ToListAsync();
                    foreach (var item in products)
                    {
                        item.DiscountCode = discount.Code;
                    }
                    _db.Products.UpdateRange(products);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var otherCategories = await _db.Categories.ToListAsync();
                    foreach (var item in otherCategories)
                    {
                        item.DiscountCode = null;
                    }
                    _db.Categories.UpdateRange(otherCategories);
                    await _db.SaveChangesAsync();

                    var otherBrands = await _db.Brands.ToListAsync();
                    foreach (var item in otherBrands)
                    {
                        item.DiscountCode = null;
                    }
                    _db.Brands.UpdateRange(otherBrands);
                    await _db.SaveChangesAsync();

                    var Otherproducts = await _db.Products.ToListAsync();
                    foreach (var item in Otherproducts)
                    {
                        item.DiscountCode = null;
                    }
                    _db.Products.UpdateRange(Otherproducts);
                    await _db.SaveChangesAsync();
                }
              
                return Json(true);
            }
            return Json(false);
        }

        //Add select products when choose discount target for some products
        //Handled by ajax
        [Route("Admin/Discount/ChooseProductsForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseProductsForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseProductsForDiscountTarget", model: id);
        }

        //Add select categories when choose discount target for some category
        //Handled by ajax
        [Route("Admin/Discount/ChooseCategoriesForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseCategoriesForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseCategoriesForDiscountTarget", model: id);
        }

        //Add select brands when choose discount target for some brand
        //Handled by ajax
        [Route("Admin/Discount/ChooseBrandsForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseBrandsForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseBrandsForDiscountTarget", model: id);
        }

        //Add product for discount target
        //Handled by ajax
        [Route("Admin/Discount/AddProductDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> AddProductDiscountTarget(int productId,int discountId)
        {

            var discount = await _db.Discounts
                .AsNoTracking()
                .Where(c => c.Id.Equals(discountId))
                .Select(c=> c.Code)
                .FirstOrDefaultAsync();
            var product = await _db.Products.FindAsync(productId);
            product.DiscountCode = discount;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return ViewComponent(componentName: "ProductDiscountTarget", new {myId = discountId });
        }

        //Add category for discount target
        //Handled by ajax
        [Route("Admin/Discount/AddCategoryDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> AddCategoryDiscountTarget(int categoryId, int discountId)
        {
            var discount = await _db.Discounts
                .AsNoTracking()
                .Where(c => c.Id.Equals(discountId))
                .Select(c => c.Code)
                .FirstOrDefaultAsync();
            var category = await _db.Categories.FindAsync(categoryId);
            category.DiscountCode = discount;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            var products = await _db.Products.Where(c => c.CategoryId.Equals(categoryId))
                .ToListAsync();
            foreach (var item in products)
            {
                item.DiscountCode = discount;
            }
            _db.Products.UpdateRange(products);
            await _db.SaveChangesAsync();
            var subCategories = await _db.SubCategories.Where(c => c.CategoryId.Equals(categoryId))
                .ToListAsync();
            foreach (var item in subCategories)
            {
                var cat = await _db.Categories.FindAsync(item.MyIdInCatTable);
                cat.DiscountCode = discount;
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                var myproducts = await _db.Products.Where(c => c.CategoryId.Equals(categoryId))
                .ToListAsync();
                foreach (var n in myproducts)
                {
                    n.DiscountCode = discount;
                }
                _db.Products.UpdateRange(myproducts);
                await _db.SaveChangesAsync();
            }
            return ViewComponent(componentName: "CategoryDiscountTarget", new { myId = discountId });
        }

        //Add category for discount target
        //Handled by ajax
        [Route("Admin/Discount/AddBrandDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> AddBrandDiscountTarget(int brandId, int discountId)
        {
            var discount = await _db.Discounts
                .AsNoTracking()
                .Where(c => c.Id.Equals(discountId))
                .Select(c => c.Code)
                .FirstOrDefaultAsync();
            var brand = await _db.Brands.FindAsync(brandId);
            brand.DiscountCode = discount;
            _db.Brands.Update(brand);
            await _db.SaveChangesAsync();
            var products = await _db.Products.Where(c => c.BrandId.Equals(brandId))
                .ToListAsync();
            foreach (var item in products)
            {
                item.DiscountCode = discount;
            }
            _db.Products.UpdateRange(products);
            await _db.SaveChangesAsync();
            return ViewComponent(componentName: "BrandDiscountTarget", new { myId = discountId });
        }

        //Delete product from discount target
        //Handled by ajax
        [Route("Admin/Discount/DeleteProductDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> DeleteProductDiscountTarget(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.DiscountCode = null;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }


        //Delete category from discount target
        //Handled by ajax
        [Route("Admin/Discount/DeleteCategoryDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> DeleteCategoryDiscountTarget(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                category.DiscountCode = null;
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();

                var products = await _db.Products.Where(c => c.CategoryId.Equals(id))
                 .ToListAsync();
                foreach (var item in products)
                {
                    item.DiscountCode = null;
                }
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
                var subCategories = await _db.SubCategories.Where(c => c.CategoryId.Equals(id))
               .ToListAsync();
                foreach (var item in subCategories)
                {
                    var cat = await _db.Categories.FindAsync(item.MyIdInCatTable);
                    cat.DiscountCode = null;
                    _db.Categories.Update(category);
                    await _db.SaveChangesAsync();
                    var myproducts = await _db.Products.Where(c => c.CategoryId.Equals(id))
                    .ToListAsync();
                    foreach (var n in myproducts)
                    {
                        n.DiscountCode = null;
                    }
                    _db.Products.UpdateRange(myproducts);
                    await _db.SaveChangesAsync();
                }
                return Json(true);
            }
            return Json(false);
        }

        //Delete brand from discount target
        //Handled by ajax
        [Route("Admin/Discount/DeleteBrandDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> DeleteBrandDiscountTarget(int id)
        {
            var brand = await _db.Brands.FindAsync(id);
            if (brand != null)
            {
                brand.DiscountCode = null;
                _db.Brands.Update(brand);
                await _db.SaveChangesAsync();

                var products = await _db.Products.Where(c => c.BrandId.Equals(id))
                 .ToListAsync();
                foreach (var item in products)
                {
                    item.DiscountCode = null;
                }
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Change is for min value (discount condition)
        //Handled by ajax
        [Route("Admin/Discount/ChangeIsForMinValueCondition")]
        [HttpPost]
        public async Task<IActionResult> ChangeIsForMinValueCondition(int discountId, bool isForMinBuyValue)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            if (discount != null)
            {
                discount.IsForMinBuyValue = isForMinBuyValue;
                discount.MinBuyValue = 0;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Change is for min number (discount condition)
        //Handled by ajax
        [Route("Admin/Discount/ChangeIsForMinBuyNumberCondition")]
        [HttpPost]
        public async Task<IActionResult> ChangeIsForMinBuyNumberCondition(int discountId, bool isForMinBuyNumber)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            if (discount != null)
            {
                discount.IsForMinBuyNumber = isForMinBuyNumber;
                discount.MinBuyNumber = 0;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Edit min buy value / min buy number
        //Handled by ajax
        [Route("Admin/Discount/EditMinValue")]
        [HttpPost]
        public async Task<IActionResult> EditMinValue(int discountId,
            double minBuyValue = 0, int minBuyNumber = 0)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            if (discount != null)
            {
                discount.MinBuyNumber = minBuyNumber;
                discount.MinBuyValue = minBuyValue;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Change is for all customer 
        //Handled by ajax
        [Route("Admin/Discount/ChangeIsForAllCustomer")]
        [HttpPost]
        public async Task<IActionResult> ChangeIsForAllCustomer(int id, bool isForAllCustomer)
        {
            var discount = await _db.Discounts.AsNoTracking()
                .Where(c => c.Id.Equals(id))
                .Select(c => c.Code)
                .FirstOrDefaultAsync();
            if(discount != null)
            {
                if (isForAllCustomer)
                {
                    var customers = await _db.Users.ToListAsync();
                    foreach (var item in customers)
                    {
                        item.DiscountCode = discount;
                    }
                }
                else
                {
                    var customers = await _db.Users.Where(c => c.DiscountCode.Equals(discount)).ToListAsync();
                    foreach (var item in customers)
                    {
                        item.DiscountCode = null;
                    }
                }
                return Json(true);
            }
            return Json(false);
        }

        //Edit Customer Discount
        //Handled by ajax
        public async Task<IActionResult> EditCustomerDiscount(int discountId, bool isForAllCustomer)
        {
            var discount = await _db.Discounts
                .Where(c => c.Id.Equals(discountId))
                .FirstOrDefaultAsync();
            discount.IsForAllCustomers = isForAllCustomer;
            _db.Discounts.Update(discount);
            await _db.SaveChangesAsync();
            if (discount != null)
            {
                if (isForAllCustomer)
                {
                    var customers = await _db.Users.ToListAsync();
                    foreach (var item in customers)
                    {
                        item.DiscountCode = discount.Code;
                    }
                    _db.Users.UpdateRange(customers);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var customers = await _db.Users.Where(c => c.DiscountCode.Equals(discount.Code)).ToListAsync();
                    foreach (var item in customers)
                    {
                        item.DiscountCode = null;
                    }
                    _db.Users.UpdateRange(customers);
                    await _db.SaveChangesAsync();
                }
               
            }
            return PartialView("_ChoosenCustomerForDiscount",discountId);
        }


        //Add discount code for a customer
        //Handled by ajax
        [Route("Admin/Discount/AddDiscountCodeForCustomer")]
        [HttpPost]
        public async Task<IActionResult> AddDiscountCodeForCustomer(int discountId,string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            var discount = await _db.Discounts.AsNoTracking()
                .Where(c => c.Id.Equals(discountId))
                .Select(c => c.Code)
                .FirstOrDefaultAsync();
            if (user != null && discount!=null)
            {
                user.DiscountCode = discount;
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();
               
            }
            return ViewComponent(componentName: "CustomerDiscountTarget", new { myId = discountId });
        }

        //Delete CustomerDiscountTarget
        //Handled by ajax
        [Route ("Admin/Discount/DeleteCustomerDiscountTarget")]
        [HttpPost]
        public async Task<IActionResult> DeleteCustomerDiscountTarget(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                user.DiscountCode = null;
                await _userManager.UpdateAsync(user);
                return Json(true);
            }
            return Json(false);
        }

        //Set Discount Date
        //Handled by ajax
        [Route("Admin/Discount/SetDiscountDate")]
        [HttpPost]
        public async Task<IActionResult> SetDiscountDate(string activationDate, string expirationDate, int discountId)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            if (discount != null)
            {
                var splitActiveDate = activationDate.Split("/");
                var splitExpireDate = expirationDate.Split("/");
                PersianCalendar persianCalendar = new PersianCalendar();
                discount.ActivationDate = persianCalendar
                    .ToDateTime(Int32.Parse(splitActiveDate[0]), Int32.Parse(splitActiveDate[1]),
                    Int32.Parse(splitActiveDate[2]),0,0,0,0);
                discount.ExpirationDate = persianCalendar
                    .ToDateTime(Int32.Parse(splitExpireDate[0]), Int32.Parse(splitExpireDate[1]),
                    Int32.Parse(splitExpireDate[2]), 0, 0, 0, 0);
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Discount Publishement
        //Handled by ajax
        [Route("Admin/Discount/DiscountPublishment")]
        [HttpPost]
        public async Task<IActionResult> DiscountPublishment(int discountId,bool isPublished)
        {
            var discount = await _db.Discounts.FindAsync(discountId);
            if (discount != null)
            {
                discount.IsPublished = isPublished;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Publish
        [Route("Admin/Discount/Publish")]
        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            try
            {
                var discount = await _db.Discounts.FindAsync(id);
                discount.IsPublished = true;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //UnPublish
        [Route("Admin/Discount/UnPublish")]
        [HttpPost]
        public async Task<IActionResult> UnPublish(int id)
        {
            try
            {
                var discount = await _db.Discounts.FindAsync(id);
                discount.IsPublished = false;
                _db.Discounts.Update(discount);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Discount List when click on pagination button 
        //TODO: go to the target page of discount list
        //Parameteres: pageIndex: index of page 
        [Route("Admin/Discount/DiscountList")]
        [HttpPost]
        public IActionResult DiscountList(int pageIndex = 1, string text = null, int sortType = 0)
        {
            return ViewComponent(componentName: "DiscountList",
                arguments: new { pageIndex = pageIndex, text = text, sortType = sortType });
        }

        //Publish a group of discounts
        //parameters: => ids: discount ids
        [Route("Admin/Discount/PublishGroupDiscount")]
        [HttpPost]
        public async Task<IActionResult> PublishGroupDiscount(List<int?> ids)
        {
            try
            {
                var discounts = await _db.Discounts.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in discounts)
                {
                    item.IsPublished = true;
                }
                _db.Discounts.UpdateRange(discounts);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }


        }

        //UnPublish a group of discounts
        //parameters: => ids: discount ids
        [Route("Admin/Discount/UnPublishGroupDiscount")]
        [HttpPost]
        public async Task<IActionResult> UnPublishGroupDiscount(List<int?> ids)
        {
            try
            {
                var discounts = await _db.Discounts.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in discounts)
                {
                    item.IsPublished = false;
                }
                _db.Discounts.UpdateRange(discounts);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }

        //Delete a group of discounts
        //parameters: => ids: product ids
        [Route("Admin/Discount/DeleteGroupDiscount")]
        [HttpPost]
        public async Task<IActionResult> DeleteGroupDiscount(List<int?> ids)
        {
            var discounts = await _db.Discounts.Where(c => ids.Contains(c.Id)).ToListAsync();
            foreach (var item in discounts)
            {
                var products = await _db.Products.Where(c => c.DiscountCode.Equals(item.Code))
                    .ToListAsync();
                foreach (var p in products)
                {
                    p.DiscountCode = null;
                }
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
            }
            _db.Discounts.RemoveRange(discounts);
            await _db.SaveChangesAsync();
            
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "Discount");
        }

        //Delete Discount
        [Route("Admin/Discount/DeleteDiscount")]
        [HttpPost]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _db.Discounts.FindAsync(id);
            var products = await _db.Products.Where(c => c.DiscountCode.Equals(discount.Code))
                  .ToListAsync();
            foreach (var p in products)
            {
                p.DiscountCode = null;
            }
            _db.Products.UpdateRange(products);
            await _db.SaveChangesAsync();
            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "Discount");
        }
    }



}