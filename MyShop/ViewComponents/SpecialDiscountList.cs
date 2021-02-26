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
    public class SpecialDiscountList:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public SpecialDiscountList(ApplicationDbContext db)
        {
            _db = db;
        }
        //parameters: = sortType: 0- Latest, 1- Oldest, 2- Active 3- Expired 4- Top Sell 5- Top Visited
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1, string text=null, int sortType = 0)
        {
            ViewData["dataSearchText"] = text;
            ViewData["dataSortType"] = sortType;

            if (!string.IsNullOrEmpty(text))
            {
                var discounts = _db.SpecialDiscount.AsQueryable()
                .AsNoTracking()
                .Where(c => c.ProductName.Contains(text.Trim()))
                .OrderByDescending(c => c.ActivationDate);
                var pageinatedList = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts, pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "SpecialDiscountList", model: pageinatedList);
            }
            else
            {
                switch (sortType)
                {
                    case 0:

                        var discounts01 = _db.SpecialDiscount
                            .AsQueryable()
                            .AsNoTracking()
                     .OrderByDescending(c => c.ActivationDate);
                        var pageinatedList01 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts01, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList01);


                    case 1:
                        var discounts02 = _db.SpecialDiscount
                               .AsQueryable()
                               .AsNoTracking()
                        .OrderBy(c => c.ActivationDate);
                        var pageinatedList02 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts02, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList02);

                    case 2:
                        var discounts03 = _db.SpecialDiscount
                             .AsQueryable()
                             .AsNoTracking()
                      .Where(c => c.ExpirationDate > System.DateTime.Now)
                      .OrderByDescending(c => c.ActivationDate);

                        var pageinatedList03 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts03, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList03);
                    case 3:
                        var discounts04 = _db.SpecialDiscount
                             .AsQueryable()
                             .AsNoTracking()
                        .Where(c => c.ExpirationDate <= System.DateTime.Now)
                      .OrderByDescending(c => c.ActivationDate);

                        var pageinatedList04 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts04, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList04);
                    case 4:
                        var discounts05 = _db.SpecialDiscount
                             .AsQueryable()
                             .AsNoTracking()
                      .OrderByDescending(c => c.ViewNumber);

                        var pageinatedList05 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts05, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList05);
                    case 5:
                        var discounts06 = _db.SpecialDiscount
                             .AsQueryable()
                             .AsNoTracking()
                      .OrderByDescending(c => c.ViewNumber);

                        var pageinatedList06 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts06, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList06);
                    default:
                        var discounts07 = _db.SpecialDiscount
                               .AsQueryable()
                               .AsNoTracking()
                        .OrderByDescending(c => c.ActivationDate);
                        var pageinatedList07 = await PaginatedList<SpecialDiscount>.CreateAsync(source: discounts07, pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "SpecialDiscountList", model: pageinatedList07);
                }
            }

        }
    }

  
}
