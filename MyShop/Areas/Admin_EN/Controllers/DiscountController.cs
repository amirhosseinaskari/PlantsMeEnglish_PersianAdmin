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

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
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
        [Route("En/Admin/Discount")]
        public IActionResult Index()
        {
            return View();
        }
        
        //Create new discount with two parameters (code and number of discount)
        //modal form in index
        [Route("En/Admin/Discount/CreateDiscount")]
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


        //Edit discount
        [Route("En/Admin/Discount/EditDiscount/{id}")]
        public async Task<IActionResult> EditDiscount(int id)
        {
            var discount = await _db.Discounts.FindAsync(id);

            return View(model:discount);
        }

       
        //Add select products when choose discount target for some products
        //Handled by ajax
        [Route("En/Admin/Discount/ChooseProductsForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseProductsForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseProductsForDiscountTarget", model: id);
        }

        //Add select categories when choose discount target for some category
        //Handled by ajax
        [Route("En/Admin/Discount/ChooseCategoriesForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseCategoriesForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseCategoriesForDiscountTarget", model: id);
        }

        //Add select brands when choose discount target for some brand
        //Handled by ajax
        [Route("En/Admin/Discount/ChooseBrandsForDiscountTarget")]
        [HttpPost]
        public IActionResult ChooseBrandsForDiscountTarget(int id)
        {
            return PartialView(viewName: "_ChooseBrandsForDiscountTarget", model: id);
        }

        //Add product for discount target
        //Handled by ajax
        [Route("En/Admin/Discount/AddProductDiscountTarget")]
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
        [Route("En/Admin/Discount/AddCategoryDiscountTarget")]
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
        [Route("En/Admin/Discount/AddBrandDiscountTarget")]
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
        [Route("En/Admin/Discount/AddDiscountCodeForCustomer")]
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

     

        //Discount List when click on pagination button 
        //TODO: go to the target page of discount list
        //Parameteres: pageIndex: index of page 
        [Route("En/Admin/Discount/DiscountList")]
        [HttpPost]
        public IActionResult DiscountList(int pageIndex = 1, string text = null, int sortType = 0)
        {
            return ViewComponent(componentName: "DiscountList",
                arguments: new { pageIndex = pageIndex, text = text, sortType = sortType });
        }



        //Delete a group of discounts
        //parameters: => ids: product ids
        [Route("En/Admin/Discount/DeleteGroupDiscount")]
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
        [Route("En/Admin/Discount/DeleteDiscount")]
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