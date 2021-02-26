using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class OrderDataTable : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public OrderDataTable(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int sort = 0, int pageIndex = 1, string searchText = null)
        {
            IQueryable<Shopping> orders = _db.Shoppings
                .AsQueryable()
                .AsNoTracking();
            if (!string.IsNullOrEmpty(searchText))
            {
                var myOrders =await _db.Orders
                    .AsNoTracking()
                    .Where(c => c.ProductTitle.Contains(searchText))
                    .Select(c=> c.Id.ToString())
                    .ToListAsync();
               

                orders = _db.Shoppings
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(c => c.FullName.Contains(searchText) ||
                    c.ReceiverName.Contains(searchText) || c.TracingCode.Contains(searchText));

                foreach (var item in myOrders)
                {
                    var shop = _db.Shoppings
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(c => c.OrderIds.Contains(item));
                    if (orders != null)
                    {
                       orders = orders.Union(shop);
                    }
                    else
                    {
                        orders = shop;
                    }
                }
                var pageinatedList = await PaginatedList<Shopping>
               .CreateAsync(source: orders, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "OrderDataTable", model: pageinatedList);
            }
            else
            {
                switch (sort)
                {
                    //Latest
                    case 0:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .OrderByDescending(c => c.PaymentDateTime);
                        break;
                    //Oldest
                    case 1:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .OrderBy(c => c.PaymentDateTime);
                        break;
                    //Wait for register
                    case 2:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .Where(c => c.Status.Equals(Status.WaitForRegister));
                        break;
                    //Registered
                    case 3:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .Where(c => c.Status.Equals(Status.Registered) || c.Status.Equals(Status.OnlinePaid));
                        break;
                    //Online Paid
                    case 4:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .Where(c => c.Status.Equals(Status.OnlinePaid));
                        break;
                    //Delivered
                    case 5:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .Where(c => c.Status.Equals(Status.Delivered));
                        break;
                    //Cancelled
                    case 6:
                        orders = _db.Shoppings
                                .AsQueryable()
                                .AsNoTracking()
                                .Where(c => c.Status.Equals(Status.Cancelled));
                        break;
                    default:
                        break;
                }
                var pageinatedList = await PaginatedList<Shopping>
                    .CreateAsync(source: orders, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "OrderDataTable", model: pageinatedList);
            }
            
        }
    }

}
