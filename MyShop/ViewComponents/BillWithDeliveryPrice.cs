using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class BillWithDeliveryPrice : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public BillWithDeliveryPrice(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(int selectedAddressId = -1)
        
        {
            var bill = new Bill();
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = await _db.Orders.AsNoTracking()
                .Where(c => c.UserId.Equals(user.Id) &&
                c.Status.Equals(Status.WaitForRegister))
                .ToListAsync();
           
            bill.TotalPrice = 0;
            bill.OrdersCount = 0;
            bill.DeliveryPrice = 0;
            bool isFreeForTheseProducts = true;
            foreach (var item in orders)
            {
                bill.TotalPrice += item.TotalPrice * item.Number;
                bill.OrdersCount += item.Number;
                var hasFreeDelivery = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(item.ProductId))
                    .Select(c => c.HasFreeDelivery).FirstOrDefaultAsync();
                if (!hasFreeDelivery)
                {
                    isFreeForTheseProducts = false;
                }
            }
            if (isFreeForTheseProducts)
            {
                bill.DeliveryPrice = 0;
                return View(viewName: "BillWithDeliveryPrice",model:bill);
            }
            var delivery = await _db.Deliveries.AsNoTracking().FirstOrDefaultAsync();
            if (delivery.CityPriceStatus.Equals(CityPriceStatus.FreeForAllCities))
            {
                bill.DeliveryPrice = 0;
                return View(viewName: "BillWithDeliveryPrice", model: bill);
            }
            if (delivery.CityPriceStatus.Equals(CityPriceStatus.LocalPayment))
            {
                bill.DeliveryPrice = -2;
                return View(viewName: "BillWithDeliveryPrice", model: bill);
            }
            if (delivery.HasMinAmountForFreeDelivery && 
                bill.TotalPrice > delivery.MinAmountForFreeDelivery)
            {
                bill.DeliveryPrice = 0;
                return View(viewName: "BillWithDeliveryPrice", model: bill);
            }
           if(selectedAddressId.Equals(-1))
            {
                bill.DeliveryPrice = -1;
                return View(viewName: "BillWithDeliveryPrice", model: bill);
            }
            if (delivery.CityPriceStatus.Equals(CityPriceStatus.DeifferentForEachCity))
            {
                var address = await _db.Addresses
               .AsNoTracking()
               .Where(c => c.AddressId.Equals(selectedAddressId))
               .FirstOrDefaultAsync();
                var city = await _db.Cities
                    .AsNoTracking()
                    .Where(c => c.Id.Equals(address.CityId) && 
                    c.IsSetDeliveryPrice && 
                    c.DeliveryId.Equals(delivery.Id))
                    .FirstOrDefaultAsync();
                if (city == null)
                {
                    bill.DeliveryPrice = -3;
                    return View(viewName: "BillWithDeliveryPrice", model: bill);
                }

                bill.DeliveryPrice = city.DeliveryPrice;
                bill.TotalPrice += bill.DeliveryPrice;
                return View(viewName: "BillWithDeliveryPrice", model: bill);
            }
           
            return View(viewName: "BillWithDeliveryPrice",model:bill);

        }
    }


}
