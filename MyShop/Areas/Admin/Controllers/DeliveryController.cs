using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DeliveryController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("Admin/Delivery")]
        public async Task<IActionResult> Index()
        {
            var delivery =await  _db.Deliveries.FirstOrDefaultAsync();
            return View(model: delivery);
        }

        //Edit Can Send To All City 
        //Handled with ajax
        //parameters: => canSendToAllCity (true | false)
        [Route("Admin/Delivery/EditCanSendToAllCity")]
        [HttpPost]
        public async Task<IActionResult> EditCanSendToAllCity(bool canSendToAllCity)
        {
            try
            {
                var delivery =await _db.Deliveries.FirstOrDefaultAsync();
                delivery.CanSendingToAllCity = canSendToAllCity;
                if (!canSendToAllCity)
                {
                    var cities = await _db.Cities
                        .ToListAsync();
                    foreach (var item in cities)
                    {
                        item.DeliveryId = -1;
                    }
                    _db.Cities.UpdateRange(cities);
                    await _db.SaveChangesAsync();

                    var states = await _db.States
                        .ToListAsync();
                    foreach (var item in states)
                    {
                        item.DeliveryId = -1;
                    }
                    _db.States.UpdateRange(states);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var cities = await _db.Cities
                        .ToListAsync();
                    foreach (var item in cities)
                    {
                        item.DeliveryId = delivery.Id;
                    }
                    _db.Cities.UpdateRange(cities);
                    await _db.SaveChangesAsync();

                    var states = await _db.States
                       .ToListAsync();
                    foreach (var item in states)
                    {
                        item.DeliveryId = delivery.Id;
                    }
                    _db.States.UpdateRange(states);
                    await _db.SaveChangesAsync();
                }
                _db.Deliveries.Update(delivery);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception e)
            {

                return Json(false);
            }
            
        }

        //Return cities of selected state
        //Handled with ajax
        [Route("Admin/Delivery/CityList")]
        [HttpPost]
        public IActionResult CityList(int stateId)
        {
            return ViewComponent(componentName: "CityList", arguments: new { id = stateId });
        }


        //Add city to delivery zone
        //Handled with ajax
        [Route("Admin/Deliver/AddCityToDeliverZone")]
        [HttpPost]
        public async Task<IActionResult> AddCityToDeliveryZone(int cityId, int stateId)
        {
            var deliveryId = await _db.Deliveries.AsNoTracking()
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
            if (cityId != 0)
            {
                var city = await _db.Cities.FindAsync(cityId);
                city.DeliveryId = deliveryId;
                _db.Cities.Update(city);
                await _db.SaveChangesAsync();
                var state = await _db.States.FindAsync(stateId);
                state.DeliveryId = deliveryId;
                _db.States.Update(state);
                await _db.SaveChangesAsync();
            }
            else
            {
                var cities = await _db.Cities.Where(c => c.StateId.Equals(stateId))
                    .ToListAsync();
                foreach (var item in cities)
                {
                    item.DeliveryId = deliveryId;
                    
                }
                _db.Cities.UpdateRange(cities);
                await _db.SaveChangesAsync();
                var state = await _db.States.FindAsync(stateId);
                state.DeliveryId = deliveryId;
                _db.States.Update(state);
                await _db.SaveChangesAsync();
            }
            return ViewComponent(componentName: "CityTable", arguments: new { deliveryId = deliveryId });
        }


        //Cities of delivery zone
        //Handled by ajax
        [Route("Admin/Delivery/CityTableOfDeliveryZone")]
        [HttpPost]
        public IActionResult CityTableOfDeliveryZone()
        {
            
                return PartialView(viewName: "_DeliveryZoneCities");
           
        }

        //Delete city from delivery zone
        //Handled by ajax
        [Route("Admin/Delivery/DeleteCityFromDeliveryZone")]
        [HttpPost]
        public async Task<IActionResult> DeleteCityFromDeliveryZone(int id)
        {
            var deliveryId = await _db.Deliveries.AsNoTracking()
               .Select(c => c.Id)
               .FirstOrDefaultAsync();
            try
            {
                var city = await _db.Cities.FindAsync(id);
                city.DeliveryId = -1;
                _db.Cities.Update(city);
                await _db.SaveChangesAsync();
                var cities = await _db.Cities
                    .AsNoTracking()
                    .Where(c => c.StateId.Equals(city.StateId) &&
                    c.DeliveryId.Equals(deliveryId))
                    .CountAsync();
                if(cities < 1)
                {
                    var state = await _db.States.FindAsync(city.StateId);
                    state.DeliveryId = -1;
                    _db.States.Update(state);
                    await _db.SaveChangesAsync();
                }

                return Json(true);
            }
            catch (Exception)
            {
                return Json(true);
            }
        }

        //Edit city price status
        //Handled by ajax
        [Route("Admin/Delivery/EditCityPriceStatus")]
        [HttpPost]
        public async Task<IActionResult> EditCityPriceStatus(CityPriceStatus cityPriceStatus)
        {
            var delivery = await _db.Deliveries.FirstOrDefaultAsync();
            if (delivery != null)
            {
                delivery.CityPriceStatus = cityPriceStatus;
                _db.Deliveries.Update(delivery);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //On different price for each city
        //Handled by ajax
        [Route("Admin/Delivery/OnDifferentPriceForEachCity")]
        [HttpPost]
        public IActionResult OnDifferentPriceForEachCity()
        {
            return PartialView("_SetPriceForEachCity");
        }

        //Add new delivery price for a city
        //Handled by ajax
        //parameters: => cityId , price
        [Route("Admin/Delivery/AddPriceForACity")]
        [HttpPost]
        public async Task<IActionResult> AddPriceForACity(int cityId,int stateId,double price)
        {
            
            if (cityId.Equals(0))
            {
                var cities = await _db.Cities.AsNoTracking()
                    .Where(c=> c.StateId.Equals(stateId))
                    .ToListAsync();
                foreach (var item in cities)
                {

                    item.DeliveryPrice = price;
                    item.IsSetDeliveryPrice = true;
                    
                }
                _db.Cities.UpdateRange(cities);
                await _db.SaveChangesAsync();
                return ViewComponent(componentName: "CityPriceDeliveryTable");
            }
            else
            {
                var city = await _db.Cities.FindAsync(cityId);
                city.DeliveryPrice = price;
                city.IsSetDeliveryPrice = true;
                _db.Cities.Update(city);
                await _db.SaveChangesAsync();
                return ViewComponent(componentName: "CityPriceDeliveryTable");
            }
            

        }

        //Delete price of a city delivery
        //Handled by ajax
        [Route("Admin/Delivery/DeletePriceCityDelivery")]
        [HttpPost]
        public async Task<IActionResult> DeletePriceCityDelivery(int id)
        {
            var city = await _db.Cities.FindAsync(id);
            city.IsSetDeliveryPrice = false;
            city.DeliveryPrice = -1;
            _db.Cities.Update(city);
            await _db.SaveChangesAsync();
            return ViewComponent(componentName: "CityPriceDeliveryTable");
        }

        //Edit HasMinAmountForFreeDelivery 
        //Handled by ajax
        [Route("Admin/Delivery/EditHasMinAmountForFreeDelivery")]
        [HttpPost]
        public async Task<IActionResult> EditHasMinAmountForFreeDelivery(bool hasMinAmountForFreeDelivery)
        {
            var delivery = await _db.Deliveries.FirstOrDefaultAsync();
            if (delivery != null)
            {
                delivery.HasMinAmountForFreeDelivery = hasMinAmountForFreeDelivery;
                if (!hasMinAmountForFreeDelivery)
                {
                    delivery.MinAmountForFreeDelivery = 0;
                }
                _db.Deliveries.Update(delivery);
                await _db.SaveChangesAsync();
                return PartialView("_SetMinAmountForDelivery",model:delivery);
            }
            return Json(false);
        }

        //Set min amount for free delivery
        [Route("Admin/Delivery/SetMinAmountForFreeDelivery")]
        [HttpPost]
        public async Task<IActionResult> SetMinAmountForFreeDelivery(double amount)
        {
            var delivery = await _db.Deliveries.FirstOrDefaultAsync();
            if (delivery != null)
            {
                delivery.MinAmountForFreeDelivery = amount;
                _db.Deliveries.Update(delivery);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);

        }

       

     
    }
}