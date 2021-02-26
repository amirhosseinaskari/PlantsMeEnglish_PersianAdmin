using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using MyShop.InfraStructure;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class SearchController : Controller
    {
        //Blog Sort
        //parameters: = sortType: 0- Latest, 1- Oldest, 2- Top Views 3- Top Rate 4- Published blogs 5- Unpublished blogs 6- by order

        [Route("Admin/Search/BlogSortAndFilter")]
        [HttpPost]
        public IActionResult BlogSortAndFilter(int sortType)
        {
            return ViewComponent(componentName: "BlogSortAndFilter",
                arguments: new { sortType = sortType });
        }

        //Blog Search Text
        //parameters: => text: text searched
        //TODO: find blogs that contains the text in their title
        [Route("Admin/Search/BlogSearchText")]
        [HttpPost]
        public IActionResult BlogSearchText(string text)
        {

            return ViewComponent(componentName: "BlogSearch", arguments: new { text = text});
        }
    }
}