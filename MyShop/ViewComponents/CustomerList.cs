using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
    public class CustomerList:ViewComponent
    {
      
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerList(UserManager<ApplicationUser> userManager)
        {
            
            _userManager = userManager;
        }

        //sort: 0- Latest  1-Oldest  2-Max Shopping number   3-Max Shopping Value
        //roleSort: 0-Public   1-Admin
        public async Task<IViewComponentResult> InvokeAsync(int sort = 0, 
            int roleSort = 0, 
            string customerSearch = null, int pageIndex = 1)
        {

            IQueryable<ApplicationUser> customers = null;
            if (!string.IsNullOrWhiteSpace(customerSearch))
            {
                customers = _userManager.Users.AsQueryable()
                    .AsNoTracking()
                    .Where(c => c.FullName.Trim().Contains(customerSearch.Trim()))
                    .OrderByDescending(c => c.RegisteredDate);
                var pageinatedList = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
               pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "CustomerList", model: pageinatedList);
            }else if(roleSort == 2)
            {
                customers = _userManager.Users.AsQueryable()
                   .AsNoTracking()
                   .Where(c => c.ClientRole.Equals(ClientRole.Admin) || c.ClientRole.Equals(ClientRole.Manager))
                   .OrderByDescending(c => c.RegisteredDate);
                var pageinatedList = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
               pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "CustomerList", model: pageinatedList);
            }else if(roleSort == 1)
            {
                customers = _userManager.Users.AsQueryable()
                 .AsNoTracking()
                 .Where(c => c.ClientRole.Equals(ClientRole.Public))
                 .OrderByDescending(c => c.RegisteredDate);
                var pageinatedList = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
               pageIndex: pageIndex, pageSize: 10);
                return View(viewName: "CustomerList", model: pageinatedList);
            }
            else
            {
                switch (sort)
                {
                    case 0:
                        customers = _userManager.Users.AsQueryable()
                       .AsNoTracking()
                       .OrderByDescending(c => c.RegisteredDate);
                        var pageinatedList01 = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
                       pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "CustomerList", model: pageinatedList01);
                    case 1:
                        customers = _userManager.Users.AsQueryable()
                       .AsNoTracking()
                       .OrderBy(c => c.RegisteredDate);
                        var pageinatedList02 = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
                       pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "CustomerList", model: pageinatedList02);
                    case 2:
                  
                        customers = _userManager.Users.AsQueryable()
                      .AsNoTracking()
                      .OrderByDescending(c => c.BuyNumber);
                        var pageinatedList03 = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
                       pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "CustomerList", model: pageinatedList03);
                    case 3:
                        customers = _userManager.Users.AsQueryable()
                      .AsNoTracking()
                      .OrderByDescending(c => c.TotalBuyValue);
                        var pageinatedList04 = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
                       pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "CustomerList", model: pageinatedList04);
                    default:
                        customers = _userManager.Users
                       .AsQueryable()
                       .AsNoTracking()
                       .OrderByDescending(c => c.RegisteredDate);


                        var pageinatedList = await PaginatedList<ApplicationUser>.CreateAsync(source: customers,
                            pageIndex: pageIndex, pageSize: 10);
                        return View(viewName: "CustomerList", model: pageinatedList);
                       
                }
               
            }
           
            
        }
    }

  
}
