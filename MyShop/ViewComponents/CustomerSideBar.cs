using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class CustomerSideBar : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerSideBar(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [ResponseCache(Duration = 200)]
        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
           
            return View("_SideBar", user);

        }
    }
}
