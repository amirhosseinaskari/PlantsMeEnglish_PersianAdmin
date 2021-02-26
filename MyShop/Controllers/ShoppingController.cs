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
    public class ShoppingController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
      
        public ShoppingController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            
        }
        //Async Methods in PaymentGateWay and SuccessfullPayment actions:
        //Start: 
        //Update Product Async Method
        private async Task<bool> UpdateProductsAsync(IEnumerable<Product> products )
        {
            try
            {
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //Update Order Async Method
        private async Task<bool> UpdateOrdersAsync(IEnumerable<Order> orders)
        {
            try
            {
                _db.Orders.UpdateRange(orders);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //Update Discounts Async Method
        private async Task<bool> UpdateSpeicalDiscountsAsync(IEnumerable<SpecialDiscount> specialDiscounts)
        {
            try
            {
                _db.SpecialDiscount.UpdateRange(specialDiscounts);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //Decrease discount numbr
        private async Task<bool> DecreaseDiscountNumberAsync(Shopping shopping)
        {
            try
            {
                var discount = await _db.Discounts.FindAsync(shopping.DiscountId);
                if (discount != null)
                {
                    discount.RemainingNumber--;
                    discount.UsedNumber++;
                    _db.Discounts.Update(discount);
                    await _db.SaveChangesAsync();

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
        //Update Shopping Async
        private async Task<bool> UpdateShoppingAsync(Shopping shopping = null, string refId = null, Status status = Status.WaitForRegister)
        {
            try
            {
                shopping.TracingCode = refId;
                shopping.Status = status;
                shopping.PaymentDateTime = System.DateTime.Now;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        //End

        //Actions of this controller:

        //Add new order
        /// <summary>
        /// After the customer has selected her product, if she has logged in to her account, 
        /// she has entered the shopping basket to finalize her purchase.
        /// </summary>
        /// <param name="productId">id of product selected</param>
        /// <param name="ids">product variation ids (like color, size and etc)</param>
        /// <returns></returns>
        [Authorize]
        [Route("Shopping/AddNewOrder")]
        public async Task<IActionResult> AddNewOrder(int productId, List<int?> ids)
        {
            //add new order 
            var product = await _db.Products
               .AsNoTracking()
               .Include(c => c.SpecialDiscount)
               .Where(c => c.Id.Equals(productId))
               .FirstOrDefaultAsync();

            //in the first total price equals with base price
            double totalPrice = product.BasePrice;

            //check the prices of product variation and select max price as total price
            List<double?> prices = new List<double?>();
            foreach (var item in ids)
            {

                var subProVarPrice = await _db.SubProductVariations
                    .AsNoTracking()
                    .Where(c => c.SubProductVariationId.Equals(item) &&
                    c.productId.Equals(productId) && c.HasDefinedDifferentPrice)
                    .Select(c => c.Price)
                    .FirstOrDefaultAsync();
                if (subProVarPrice > 0)
                {
                    prices.Add(subProVarPrice);
                }

            }

            if (prices.Count() > 0)
            {
                double? max = prices[0];
                foreach (var item in prices)
                {
                    if (item > max)
                    {
                        max = item;
                    }
                }
                totalPrice = (double)max;
            }

            //check special discount price and date if is active for this product
            if (product.SpecialDiscount.Where(c => c.ExpirationDate > System.DateTime.Now &&
            c.ActivationDate <= System.DateTime.Now).Count() > 0)
            {
                totalPrice = totalPrice - product.SpecialDiscount.First().DiscountPrice;
            }

            //submit the order for this customer
            Order order = new Order();
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            order.UserId = user.Id;
            order.FullName = user.FullName;
            order.ProductId = productId;
            order.ProductTitle = product.Title;
            order.TotalPrice = totalPrice;
            order.Status = Status.WaitForRegister;
            order.Number = 1;
            string mySubProductVariationIds = string.Join(',', ids);
            order.SubProductVariationIds = mySubProductVariationIds;
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return RedirectToAction("ShoppingBasket");
        }
        /// <summary>
        /// The first step to buying a product, after select an order is entering the shopping basket page page.
        /// On this page, the customer It can select the number of orders, 
        /// delete its order and view the first invoice.After everything is OK, 
        /// he clicks on the Continue button to register his address
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("ShoppingBasket")]
        public async Task<IActionResult> ShoppingBasket()
        {


            string userId = await _userManager.Users
              .Where(c => c.UserName.Equals(User.Identity.Name))
              .Select(c => c.Id).FirstOrDefaultAsync();
            var orders = await _db.Orders
                 .AsNoTracking()
                 .Where(c => c.UserId.Equals(userId) &&
                 c.Status.Equals(Status.WaitForRegister))
                 .ToListAsync();
            return View(model: orders);

        }



        //Delete Order
        /// <summary>
        /// In shopping basket page, the customer can delete his/her order/orders
        /// </summary>
        /// <param name="orderId">id of his/her order that he/her wants to delete it</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Shopping/DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction("ShoppingBasket");
        }

        //Increase Order
        /// <summary>
        /// Customer can increase his/her order number
        /// </summary>
        /// <param name="orderId">order id that customer wants increase it</param>
        /// <returns></returns>
        [Authorize]
        [Route("Shopping/IncreaseOrder")]
        [HttpPost]
        public async Task<IActionResult> IncreaseOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                var productStock = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(order.ProductId))
                    .Select(c => c.Stock).FirstOrDefaultAsync();
                if(productStock < (order.Number + 1))
                {
                    return Json(-2);
                }
                order.Number++;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                var totalPrice = order.TotalPrice * order.Number;
                return Json(totalPrice);
            }
            return Json(-1);
        }

        //Decrease Order
        /// <summary>
        /// Customer can decrease his/her order number
        /// </summary>
        /// <param name="orderId">order id that customer wants decrease it</param>
        /// <returns></returns>
        [Authorize]
        [Route("Shopping/DecreaseOrder")]
        [HttpPost]
        public async Task<IActionResult> DecreaseOrder(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Number--;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                var totalPrice = order.TotalPrice * order.Number;
                return Json(totalPrice);
            }
            return Json(-1);
        }
        /// <summary>
        /// Change order number (manual)
        /// </summary>
        /// <param name="orderId">id of this order</param>
        /// <param name="number">the number that user typed</param>
        /// <returns></returns>
        [Authorize]
        [Route("Shopping/ChangeOrderNumber")]
        [HttpPost]
        public async Task<IActionResult> ChangeOrderNumber(int orderId,int number)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                var productStock = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(order.ProductId))
                    .Select(c => c.Stock).FirstOrDefaultAsync();
                if (productStock < number)
                {
                    return Json(-2);
                }
                order.Number = number;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                var totalPrice = order.TotalPrice * order.Number;
                return Json(totalPrice);
            }
            return Json(-1);
        }

        //Confirm and payment step
        /// <summary>
        /// After the customer registers his mailing address, he enters this step. At this stage,
        /// a summary of the purchase 
        /// invoice is displayed to the user and if approved, 
        /// he enters the final purchase stage (payment gateway or registered for local payment)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("Shopping/ConfirmAndPayment")]
        public async Task<IActionResult> ConfirmAndPayment()
        {
           
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var orders = await _db.Orders
                 .AsNoTracking()
                 .Where(c => c.UserId.Equals(user.Id) &&
                 c.Status.Equals(Status.WaitForRegister))
                 .ToListAsync();

            if (orders.Count() < 1)
            {
                return RedirectToAction(actionName: "ShoppingBasket", controllerName: "Shopping");
            }

            //Check address of users and add it to shopping
            var address = await _db.Addresses
                .AsNoTracking()
                .Where(c => c.MyUserId.Equals(user.Id) && c.IsSelected)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                return RedirectToAction(actionName: "AddressInformation", controllerName: "Address");
            }
            //Calculate total price and delivery price
            double totalPrice = 0;
            int orderCount = 0;
            double deliveryPrice = 0;
            bool isFreeForTheseProducts = true;
            bool hasLocalPaymentOptions = true;
            foreach (var item in orders)
            {
                totalPrice += item.TotalPrice * item.Number;
                orderCount += item.Number;
                var hasFreeDelivery = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(item.ProductId))
                    .Select(c => c.HasFreeDelivery).FirstOrDefaultAsync();
                if (!hasFreeDelivery)
                {
                    isFreeForTheseProducts = false;
                }

                var hasLocalPayment = await _db.Products.AsNoTracking()
                    .Where(c => c.Id.Equals(item.ProductId))
                    .Select(c => c.LocalPayment).FirstOrDefaultAsync();
                if (!hasLocalPayment)
                {
                    hasLocalPaymentOptions = false;
                }
            }

            var delivery = await _db.Deliveries.AsNoTracking().FirstOrDefaultAsync();
            if (isFreeForTheseProducts)
            {
                deliveryPrice = 0;

            }
            else if (delivery.CityPriceStatus.Equals(CityPriceStatus.FreeForAllCities))
            {
                deliveryPrice = 0;

            }
            else if (delivery.CityPriceStatus.Equals(CityPriceStatus.LocalPayment))
            {
                deliveryPrice = -2;

            }
            else if (delivery.HasMinAmountForFreeDelivery &&
                totalPrice > delivery.MinAmountForFreeDelivery)
            {
                deliveryPrice = 0;

            }


            else if (delivery.CityPriceStatus.Equals(CityPriceStatus.DeifferentForEachCity))
            {

                var city = await _db.Cities
                    .AsNoTracking()
                    .Where(c => c.Id.Equals(address.CityId) &&
                    c.IsSetDeliveryPrice &&
                    c.DeliveryId.Equals(delivery.Id))
                    .FirstOrDefaultAsync();
                if (city == null)
                {
                    deliveryPrice = 0;
                    return RedirectToAction(actionName: "AddressInformation", controllerName: "Address");
                }

                deliveryPrice = city.DeliveryPrice;


            }
            if (deliveryPrice > 0)
            {
                totalPrice += deliveryPrice;
            }
            var shopping = await _db.Shoppings.Where(c => c.UserId.Equals(user.Id) &&
            c.Status.Equals(Status.WaitForRegister)).FirstOrDefaultAsync();
            if (shopping == null)
            {
                shopping = new Shopping();
                shopping.Status = Status.WaitForRegister;
                shopping.UserId = user.Id;
                shopping.FullName = user.FullName;
                shopping.OrderIds = string.Join(',', orders.Select(c => c.Id).ToList());
                shopping.OrdersCount = orderCount;
                shopping.HasLocalPaymentOption = hasLocalPaymentOptions;
                shopping.IsLocalPayment = false;
                shopping.BasePrice = totalPrice - deliveryPrice;
                shopping.DeliveryPrice = deliveryPrice;
                shopping.AddressInformation =
                    string.Format($"{address.StateName}، {address.CityName}، {address.Details}");
                shopping.PostalCode = address.PostalCode;
                shopping.ReceiverName = address.ReceiverName;
                shopping.ReceiverMobileNumber = address.ReceiverMobileNumber;
                shopping.TotalPrice = totalPrice;
                await _db.Shoppings.AddAsync(shopping);
                await _db.SaveChangesAsync();
                return View(model: shopping);
            }

            shopping.OrderIds = string.Join(',', orders.Select(c => c.Id).ToList());
            shopping.BasePrice = totalPrice - deliveryPrice;
            shopping.DeliveryPrice = deliveryPrice;
            shopping.AddressInformation =
                   string.Format($"{address.StateName}، {address.CityName}، {address.Details}");
            shopping.PostalCode = address.PostalCode;
            shopping.ReceiverName = address.ReceiverName;
            shopping.ReceiverMobileNumber = address.ReceiverMobileNumber;
            shopping.OrdersCount = orderCount;
            shopping.IsLocalPayment = false;
            shopping.DiscountCode = null;
            shopping.DiscountId = 0;
            shopping.DiscountPrice = 0;
            shopping.HasLocalPaymentOption = hasLocalPaymentOptions;
            shopping.TotalPrice = totalPrice;
            _db.Shoppings.Update(shopping);
            await _db.SaveChangesAsync();
            return View(model: shopping);
        }

        //change is local payment
        //Handled by ajax
        [Authorize]
        [Route("Shopping/ChangeIsLocalPayment")]
        [HttpPost]
        public async Task<IActionResult> ChangeIsLocalPayment(int shoppingId, bool isLocalPayment)
        {
            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            if (shopping != null)
            {
                shopping.IsLocalPayment = isLocalPayment;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);

        }

        //using discount code
        //Handled by ajax
        [Authorize]
        [Route("Shopping/UsingDiscountCode")]
        [HttpPost]
        public async Task<IActionResult> UsingDiscountCode(string discountCode, int shoppingId)
        {
            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            var discount = await _db.Discounts.AsNoTracking()
                .Where(c => c.Code.Equals(discountCode) &&
                c.ExpirationDate > System.DateTime.Now &&
                c.ActivationDate <= System.DateTime.Now &&
                c.RemainingNumber > 0).FirstOrDefaultAsync();
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (discount == null)
            {
                if (shopping != null)
                {
                    shopping.DiscountPrice = 0;
                    shopping.DiscountCode = null;
                    shopping.DiscountId = 0;
                    shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                    _db.Shoppings.Update(shopping);
                    await _db.SaveChangesAsync();
                }
                return Json(false);
            }
            if (!discount.IsForAllCustomers && !user.DiscountCode.Equals(discountCode))
            {
                if (shopping != null)
                {
                    shopping.DiscountPrice = 0;
                    shopping.DiscountCode = null;
                    shopping.DiscountId = 0;
                    shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                    _db.Shoppings.Update(shopping);
                    await _db.SaveChangesAsync();
                }
                return Json(false);
            }

            if (discount.DiscountTarget.Equals(DiscountTarget.SomeProducts)
                || discount.DiscountTarget.Equals(DiscountTarget.SomeCategories)
                || discount.DiscountTarget.Equals(DiscountTarget.SomeBrands))
            {
                bool isForTheseProduct = false;
                double discountPrice = 0;
                var orderIds = shopping.OrderIds.Split(',').ToList();
                foreach (var item in orderIds)
                {
                    var order = await _db.Orders.AsNoTracking()
                        .Where(c => c.Id.Equals(Int32.Parse(item))).FirstOrDefaultAsync();
                    var product = await _db.Products.AsNoTracking()
                        .Where(c => c.Id.Equals(order.ProductId)).FirstOrDefaultAsync();
                    var productDiscount = await _db.Products.AsNoTracking()
                        .Where(c => c.Id.Equals(product.Id))
                        .Select(c => c.DiscountCode).FirstOrDefaultAsync();
                    if (productDiscount != null)
                    {
                        if (productDiscount.Equals(discountCode))
                        {
                            isForTheseProduct = true;
                            discountPrice += order.TotalPrice *
                                ((double)discount.Percent / 100) * order.Number;
                        }

                    }
                }
                if (isForTheseProduct)
                {
                    if (discount.IsForMinBuyValue && shopping.TotalPrice < discount.MinBuyValue)
                    {
                        shopping.DiscountPrice = 0;
                        shopping.DiscountCode = null;
                        shopping.DiscountId = 0;
                        shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                        _db.Shoppings.Update(shopping);
                        await _db.SaveChangesAsync();
                        return Json(false);
                    }
                    else if (discount.IsForMinBuyNumber && shopping.OrdersCount < discount.MinBuyNumber)
                    {
                        shopping.DiscountPrice = 0;
                        shopping.DiscountCode = null;
                        shopping.DiscountId = 0;
                        shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                        _db.Shoppings.Update(shopping);
                        await _db.SaveChangesAsync();
                        return Json(false);
                    }
                    shopping.DiscountCode = discountCode;
                    shopping.DiscountId = discount.Id;
                    shopping.DiscountPrice = discountPrice;
                    shopping.TotalPrice = (shopping.BasePrice + shopping.DeliveryPrice) - discountPrice;
                    _db.Shoppings.Update(shopping);
                    await _db.SaveChangesAsync();
                    return Json(true);
                }

                shopping.DiscountPrice = 0;
                shopping.DiscountCode = null;
                shopping.DiscountId = 0;
                shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return Json(false);

            }
            if (discount.IsForMinBuyValue && shopping.TotalPrice < discount.MinBuyValue)
            {
                shopping.DiscountPrice = 0;
                shopping.DiscountCode = null;
                shopping.DiscountId = 0;
                shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return Json(false);
            }
            else if (discount.IsForMinBuyNumber && shopping.OrdersCount < discount.MinBuyNumber)
            {
                shopping.DiscountPrice = 0;
                shopping.DiscountCode = null;
                shopping.DiscountId = 0;
                shopping.TotalPrice = shopping.BasePrice + shopping.DeliveryPrice;
                _db.Shoppings.Update(shopping);
                await _db.SaveChangesAsync();
                return Json(false);
            }
            shopping.DiscountCode = discountCode;
            shopping.DiscountId = discount.Id;
            shopping.DiscountPrice = shopping.BasePrice * ((double)discount.Percent / 100);
            shopping.TotalPrice = (shopping.BasePrice + shopping.DeliveryPrice) - (shopping.BasePrice * ((double)discount.Percent / 100));
            _db.Shoppings.Update(shopping);
            await _db.SaveChangesAsync();
            return Json(true);
        }

        //shopping final bill
        //handled by ajax
        [Authorize]
        [Route("Shopping/FinalBill")]
        [HttpPost]
        public IActionResult FinalBill(int shoppingId)
        {
            return ViewComponent(componentName: "FinalBill", arguments: new { shoppingId = shoppingId });
        }

        [Authorize]
        [Route("Shopping/TotalPrice")]
        [HttpPost]
        public async Task<IActionResult> TotalPrice(int shoppingId)
        {
            var shoppingTotalPrice = await _db.Shoppings.AsNoTracking()
                .Where(c => c.Id.Equals(shoppingId))
                .Select(c => c.TotalPrice)
                .FirstOrDefaultAsync();
            return Json(shoppingTotalPrice);

        }

        //Payment GateWay
        [Authorize]
        [Route("Shopping/PaymentGateway")]
        public async Task<IActionResult> PaymentGateway(int shoppingId = -1)
        {

            var shopping = await _db.Shoppings.FindAsync(shoppingId);
            if (shopping == null)
            {
                return RedirectToAction(actionName: "ShoppingBasket");
            }
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            //if local payment is selected
            if (shopping.IsLocalPayment)
            {
                //update shopping
                await UpdateShoppingAsync(shopping, System.DateTime.Now.ToString("yyyyMMddmmss"), Status.Registered);
                //update user: update buy number and total buy value of this user
                user.BuyNumber++;
                user.TotalBuyValue += shopping.TotalPrice;
                await _userManager.UpdateAsync(user);
                //update discount remaining number and discount used number
                await DecreaseDiscountNumberAsync(shopping);
               
                var orders = await _db.Orders
                     .Where(c => c.UserId.Equals(user.Id) &&
                     c.Status.Equals(Status.WaitForRegister))
                     .ToListAsync();
                var products = new List<Product>();
                var specialDiscounts = new List<SpecialDiscount>();
                foreach (var item in orders)
                {
                    item.Status = Status.Registered;
                    item.ShoppingId = shopping.Id;

                    var product = await _db.Products.Where(c => c.Id.Equals(item.ProductId))
                        .Include(c => c.SpecialDiscount).FirstOrDefaultAsync();
                    product.SellNumber++;
                    product.Stock--;
                    products.Add(product);
                    var specialDiscount = product.SpecialDiscount.Where(c => c.ProductId.Equals(product.Id)
                    && c.ExpirationDate > System.DateTime.Now &&
                              c.ActivationDate <= System.DateTime.Now).FirstOrDefault();
                    if (specialDiscount != null)
                    {
                        specialDiscount.SellNumber++;
                        specialDiscounts.Add(specialDiscount);
                    }
                }
               
                await UpdateOrdersAsync(orders);
                await UpdateProductsAsync(products);
                await UpdateSpeicalDiscountsAsync(specialDiscounts);
              
                //Send SMS to customer 
                try
                {
                    Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("677045782B79456B7242656C377A372B514A667157487031756D2B536E687954");
                    var verifysms = api.VerifyLookup(user.Mobile, user.FullName, shopping.TracingCode, shopping.TotalPrice.ToString(), "success");

                }
                catch (Kavenegar.Exceptions.ApiException ex)
                {
                    // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.


                }

                //Send SMS to Admin and Manager
                try
                {
                    var webmasters = await _userManager.Users.Where(c => c.ClientRole.Equals(ClientRole.Admin) ||
               c.ClientRole.Equals(ClientRole.Manager)).ToListAsync();
                    foreach (var item in webmasters)
                    {
                        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("677045782B79456B7242656C377A372B514A667157487031756D2B536E687954");
                        var verifysms = api.VerifyLookup(item.Mobile, shopping.TracingCode, shopping.TotalPrice.ToString(), shopping.ReceiverName, "order");
                    }
                }
                catch (Exception)
                {


                }


                return RedirectToAction(actionName: "SuccessfulPayment",
                    routeValues: new { shoppingId = shopping.Id });
            }
            else
            {

                //if online payment is selected send user to zarin pal gateway
                int amount = (int)shopping.TotalPrice;
                ZarinPalGateWay.PaymentGatewayImplementationServicePortTypeClient zp =
                    new ZarinPalGateWay.PaymentGatewayImplementationServicePortTypeClient();
                var paymentRequest = await zp.PaymentRequestAsync(MerchantID: "1e608052-d5c6-11e8-bfd3-005056a205be",
                    Amount: amount, Description: "خرید محصول", Email: "",
                    CallbackURL: "http://www.shopikardemo.ir/Shopping/SuccessfulPayment?shoppingId=" + shopping.Id
                    , Mobile: user.Mobile);
                int status = paymentRequest.Body.Status;
                string authority = paymentRequest.Body.Authority;
                if (status.Equals(100) && authority.Length.Equals(36))
                {
                    
                    return Redirect("https://www.zarinpal.com/pg/StartPay/" + authority + "/ZarinGate ");
                }
            }

            return RedirectToAction("ConfirmAndPayment");
        }
        [Authorize]
        [Route("Shopping/SuccessfulPayment")]
        public async Task<IActionResult> SuccessfulPayment(string authority = null,
            string status = null, int shoppingId = -1)
        {
            if (shoppingId == -1)
            {
                return RedirectToAction(actionName: "ShoppingBasket");
            }

            var shopping = await _db.Shoppings.AsNoTracking()
                .Where(c => c.Id.Equals(shoppingId))
                .FirstOrDefaultAsync();
            if (shopping.IsLocalPayment &&
                shopping.Status.Equals(Status.WaitForRegister))
            {
                return RedirectToAction(actionName: "ShoppingBasket");
            }
            if (shopping.IsLocalPayment)
            {
                return View(model: shopping);
            }
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            int amount = (int)shopping.TotalPrice;
            if (status.Equals("OK"))
            {
                var zp = new ZarinPalGateWay.PaymentGatewayImplementationServicePortTypeClient();
                var result = await zp.PaymentVerificationAsync(MerchantID: "1e608052-d5c6-11e8-bfd3-005056a205be"
                    , Authority: authority, Amount: amount);
                if (result.Body.Status.Equals(100) || result.Body.Status.Equals(101))
                {
                    //update shopping
                    await UpdateShoppingAsync(shopping, result.Body.RefID.ToString(),Status.OnlinePaid);
                    //update user: total buy value and buy number of this user
                    user.BuyNumber++;
                    user.TotalBuyValue += shopping.TotalPrice;
                    await _userManager.UpdateAsync(user);
                    //decrease discount
                    await DecreaseDiscountNumberAsync(shopping);
                    //change status of orders and change product sell number and product stock
                    var orders = await _db.Orders
                         .Where(c => c.UserId.Equals(user.Id) &&
                         c.Status.Equals(Status.WaitForRegister))
                         .ToListAsync();
                    var products = new List<Product>();
                    var specialDiscounts = new List<SpecialDiscount>();
                    foreach (var item in orders)
                    {
                        item.Status = Status.Registered;
                        item.ShoppingId = shopping.Id;

                        var product = await _db.Products.Where(c => c.Id.Equals(item.ProductId))
                            .Include(c => c.SpecialDiscount).FirstOrDefaultAsync();
                        product.SellNumber++;
                        product.Stock--;
                        products.Add(product);
                        var specialDiscount = product.SpecialDiscount.Where(c => c.ProductId.Equals(product.Id)
                        && c.ExpirationDate > System.DateTime.Now &&
                                  c.ActivationDate <= System.DateTime.Now).FirstOrDefault();
                        if (specialDiscount != null)
                        {
                            specialDiscount.SellNumber++;
                            specialDiscounts.Add(specialDiscount);
                        }
                    }

                    await UpdateOrdersAsync(orders);
                    await UpdateProductsAsync(products);
                    await UpdateSpeicalDiscountsAsync(specialDiscounts);
                  
                    //Send SMS to customer 
                    try
                    {
                        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("677045782B79456B7242656C377A372B514A667157487031756D2B536E687954");
                        var verifysms = api.VerifyLookup(user.Mobile, user.FullName, shopping.TracingCode, shopping.TotalPrice.ToString(), "success");

                    }
                    catch (Kavenegar.Exceptions.ApiException ex)
                    {
                        // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.


                    }

                    //Send SMS to Admin and Manager
                    try
                    {
                        var webmasters = await _userManager.Users.Where(c => c.ClientRole.Equals(ClientRole.Admin) ||
                   c.ClientRole.Equals(ClientRole.Manager)).ToListAsync();
                        foreach (var item in webmasters)
                        {
                            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("677045782B79456B7242656C377A372B514A667157487031756D2B536E687954");
                            var verifysms = api.VerifyLookup(item.Mobile, shopping.TracingCode, shopping.TotalPrice.ToString(), shopping.ReceiverName, "order");
                        }
                    }
                    catch (Exception)
                    {


                    }
                    return View(model: shopping);
                }
            }

            return RedirectToAction(actionName: "ShoppingBasket");
        }

    }
}