using System;
using System.Collections.Generic;
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

namespace MyShop.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AddressController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [Authorize]
        [Route("Address/AddressInformation")]
        public async Task<IActionResult> AddressInformation(double totalPrice = -1)
        {
            string userId = await _userManager.Users
                 .Where(c => c.UserName.Equals(User.Identity.Name))
                 .Select(c => c.Id).FirstOrDefaultAsync();
            var orders = await _db.Orders
                 .AsNoTracking()
                 .Where(c => c.UserId.Equals(userId) &&
                 c.Status.Equals(Status.WaitForRegister)).ToListAsync();
            ViewData["TotalPrice"] = totalPrice;
          
            if (orders.Count < 1)
            {
                return RedirectToAction(actionName:"ShoppingBasket",controllerName:"Shopping");
            }
            //Clear out of stock products
            var isThereOutOfProduct = await OutOfStockProductsExist(orders);
            //if there is an out of stock product, will redirect to shopping basket page
            if (isThereOutOfProduct)
            {
                return RedirectToAction(actionName: "ShoppingBasket", controllerName: "Shopping");
            }
            var addresses = await _db.Addresses
                .AsNoTracking()
                .Where(c => c.MyUserId.Equals(userId))
                .ToListAsync();
      
            if (addresses.Count() < 1)
            {
                ViewData["SelectedAddressId"] = -1;
            }
            else
            {
                var selectedAddressId = addresses.Where(c => c.IsSelected)
                  .Select(c => c.AddressId)
                  .FirstOrDefault();
                ViewData["SelectedAddressId"] = selectedAddressId;
            }
            
            return View(model:addresses);
        }
        //Clear out of stock products
        private async Task<bool> OutOfStockProductsExist(List<Order> orders)
        {
            List<Order> shouldBeDelete = new List<Order>();
            foreach (var item in orders)
            {
                var outofStockProduct = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(item.ProductId) && c.Stock < item.Number)
                    .CountAsync();
                if(outofStockProduct > 0)
                {
                    shouldBeDelete.Add(item);
                }
            }
            _db.Orders.RemoveRange(shouldBeDelete);
            await _db.SaveChangesAsync();
            if(shouldBeDelete.Count() > 0)
            {
                return true;
            }
            return false;
        }

        [Authorize]
       [Route("Address/AddNewAddress")]
       [HttpPost]
       public async Task<IActionResult> AddNewAddress(Address address)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var objAddress = new Address();
            objAddress.CityName = address.CityName;
            objAddress.CityId = address.CityId;
            objAddress.StateName = address.StateName;
            objAddress.StateId = address.StateId;
            objAddress.Details = address.Details.Trim();
            objAddress.MyUserId = user.Id;
            objAddress.UserFullName = user.FullName;
            objAddress.PostalCode = address.PostalCode;
            objAddress.IsSelected = true;
            objAddress.ReceiverName = address.ReceiverName.Trim();
            objAddress.ReceiverMobileNumber = address.ReceiverMobileNumber;
            try
            {
                await _db.Addresses.AddAsync(objAddress);
                await _db.SaveChangesAsync();

                var otherAddresses = await _db.Addresses
                    .Where(c => c.MyUserId.Equals(user.Id) &&
                    !c.AddressId.Equals(objAddress.AddressId))
                    .ToListAsync();
                foreach (var item in otherAddresses)
                {
                    item.IsSelected = false;
                }
                _db.Addresses.UpdateRange(otherAddresses);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.AddedAddressSuccessfully);
                return RedirectToAction("AddressInformation");
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Return cities of selected state
        //Handled with ajax
        [Route("Address/CityList")]
        [HttpPost]
        public IActionResult CityList(int stateId)
        {
            return ViewComponent(componentName: "AddressCityList", arguments: new { id = stateId });
        }

        //Delete an Address
        [Authorize]
        [Route("Address/DeleteAddress")]
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var address = await _db.Addresses
                .FindAsync(addressId);
            if (address != null)
            {
                _db.Addresses.Remove(address);
                await _db.SaveChangesAsync();
                var selectedAddresses = await _db.Addresses
                    .Where(c => c.MyUserId.Equals(address.MyUserId))
                    .FirstOrDefaultAsync();
                if (selectedAddresses != null)
                {
                    selectedAddresses.IsSelected = true;
                    _db.Addresses.Update(selectedAddresses);
                    await _db.SaveChangesAsync();
                }
                return RedirectToAction("AddressInformation");
            }
            return RedirectToAction("AddressInformation");
        }

        //Change selected address
        //handled by ajax
        [Authorize]
        [Route("Address/ChangeSelectedAddress")]
        [HttpPost]
        public async Task<IActionResult> ChangeSelectedAddress(int selectedAddressId)
        {
            var address = await _db.Addresses.FindAsync(selectedAddressId);
            if (address != null)
            {
                address.IsSelected = true;
                _db.Addresses.Update(address);
                await _db.SaveChangesAsync();
                var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var otherAddresses = await _db.Addresses
                    .Where(c => c.MyUserId.Equals(user.Id) &&
                    !c.AddressId.Equals(address.AddressId))
                    .ToListAsync();
                foreach (var item in otherAddresses)
                {
                    item.IsSelected = false;
                }
                _db.Addresses.UpdateRange(address);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }
        //Bill with delivery price
        [Route("Address/BillWithDeliveryPrice")]
        [HttpPost]
        public IActionResult BillWithDeliveryPrice(int selectedAddressId)
        {
            return ViewComponent(componentName: "BillWithDeliveryPrice",
                arguments: new { selectedAddressId = selectedAddressId });
        }

        //Total price
        [Route("Address/TotalPrice")]
        [HttpPost]
        public IActionResult TotalPrice(int selectedAddressId)
        {
            return ViewComponent(componentName: "TotalPrice",
               arguments: new { selectedAddressId = selectedAddressId });
        }

        //Edit address
        [Authorize]
        [Route("Address/EditAddress")]
        [HttpPost]
        public async Task<IActionResult> EditAddress(Address address)
        {
            var myAddress = await _db.Addresses.FindAsync(address.AddressId);
            if (myAddress != null)
            {
                myAddress.CityId = address.CityId;
                myAddress.CityName = address.CityName;
                myAddress.StateId = address.StateId;
                myAddress.StateName = address.StateName;
                myAddress.ReceiverMobileNumber = address.ReceiverMobileNumber;
                myAddress.ReceiverName = address.ReceiverName.Trim();
                myAddress.Details = address.Details.Trim();
                myAddress.PostalCode = address.PostalCode;
                _db.Addresses.Update(myAddress);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction("AddressInformation");
            }
            return RedirectToAction("AddressInformation");
        }
    }
}