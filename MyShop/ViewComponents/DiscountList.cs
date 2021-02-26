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
    public class DiscountList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public DiscountList(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1, string text = null, int sortType = 0)
        {
            ViewData["dataSortType"] = sortType;
            ViewData["dataSearchText"] = text;
            if (!string.IsNullOrEmpty(text))
            {
                var discounts = _db.Discounts.AsQueryable()
                .AsNoTracking()
                .Where(c => c.Code.Contains(text.Trim()))
                .OrderByDescending(c => c.ActivationDate);
                var pageinatedList = await PaginatedList<Discount>.CreateAsync(source: discounts, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "DiscountList", model: pageinatedList);
            }
            else
            {
                switch (sortType)
                {
                    case 0:

                        var discounts01 = _db.Discounts
                            .AsQueryable()
                            .AsNoTracking()
                     .OrderByDescending(c => c.ActivationDate);
                        var pageinatedList01 = await PaginatedList<Discount>.CreateAsync(source: discounts01, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "DiscountList", model: pageinatedList01);


                    case 1:
                        var discounts02 = _db.Discounts
                               .AsQueryable()
                               .AsNoTracking()
                        .OrderBy(c => c.ActivationDate);
                        var pageinatedList02 = await PaginatedList<Discount>.CreateAsync(source: discounts02, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "DiscountList", model: pageinatedList02);

                    case 2:
                        var discounts03 = _db.Discounts
                             .AsQueryable()
                             .AsNoTracking()
                      .Where(c => c.ExpirationDate > System.DateTime.Now)
                      .OrderByDescending(c => c.ActivationDate);

                        var pageinatedList03 = await PaginatedList<Discount>.CreateAsync(source: discounts03, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "DiscountList", model: pageinatedList03);
                    case 3:
                        var discounts04 = _db.Discounts
                             .AsQueryable()
                             .AsNoTracking()
                        .Where(c => c.ExpirationDate < System.DateTime.Now)
                      .OrderByDescending(c => c.ActivationDate);

                        var pageinatedList04 = await PaginatedList<Discount>.CreateAsync(source: discounts04, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "DiscountList", model: pageinatedList04);
                    default:
                        var discounts05 = _db.Discounts
                               .AsQueryable()
                               .AsNoTracking()
                        .OrderByDescending(c => c.ActivationDate);
                        var pageinatedList05 = await PaginatedList<Discount>.CreateAsync(source: discounts05, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "DiscountList", model: pageinatedList05);
                }
            }
            
        }
    }

  
}
