using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Areas.Admin.En.Controllers
{
    [Area("Admin_EN")]
    [Authorize(Roles = "Admin,Manager")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DeliveryController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Route("En/Admin/Delivery")]
        public async Task<IActionResult> Index()
        {
            var delivery =await  _db.Deliveries.FirstOrDefaultAsync();
            return View(model: delivery);
        }

       
        //Return cities of selected state
        //Handled with ajax
        [Route("En/Admin/Delivery/CityList")]
        [HttpPost]
        public IActionResult CityList(int stateId)
        {
            return ViewComponent(componentName: "CityList", arguments: new { id = stateId });
        }


        //Add city to delivery zone
        //Handled with ajax
        [Route("En/Admin/Deliver/AddCityToDeliverZone")]
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
        [Route("En/Admin/Delivery/CityTableOfDeliveryZone")]
        [HttpPost]
        public IActionResult CityTableOfDeliveryZone()
        {
            
                return PartialView(viewName: "_DeliveryZoneCities");
           
        }

        //Delete city from delivery zone
        //Handled by ajax
        [Route("En/Admin/Delivery/DeleteCityFromDeliveryZone")]
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

        
        //On different price for each city
        //Handled by ajax
        [Route("En/Admin/Delivery/OnDifferentPriceForEachCity")]
        [HttpPost]
        public IActionResult OnDifferentPriceForEachCity()
        {
            return PartialView("_SetPriceForEachCity");
        }

        //Add new delivery price for a city
        //Handled by ajax
        //parameters: => cityId , price
        [Route("En/Admin/Delivery/AddPriceForACity")]
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
        [Route("En/Admin/Delivery/DeletePriceCityDelivery")]
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
        [Route("En/Admin/Delivery/EditHasMinAmountForFreeDelivery")]
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

       
       

     
    }
}