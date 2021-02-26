using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    public class SpecialDiscountController : Controller
    {
        [Route("Special-Offers")]
        public IActionResult AllSpecialDiscount()
        {
            return View();
        }

        [Route("SpecialDiscount/AllSpecialDiscountList")]
        [HttpPost]
        public async Task<IActionResult> AllSpecialDiscountList(int pageIndex = 1)
        {
            return ViewComponent(componentName: "AllSpecialDiscountList", arguments: new { pageIndex = pageIndex });
        }
    }
}