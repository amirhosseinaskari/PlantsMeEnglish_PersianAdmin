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
    public class RegisteredUsersCount:ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisteredUsersCount(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usersCount = await _userManager.Users.AsNoTracking().CountAsync();
            var dateCondition = System.DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0));
            var newUsersCount = await _userManager.Users.AsNoTracking()
                .Where(c => c.RegisteredDate > dateCondition)
                .CountAsync();
            var registeredUsers = new RegisteredUsersClass();
            registeredUsers.AllUsers = usersCount;
            registeredUsers.NewUsersFromLastMonth = newUsersCount;
                
                
            return View(viewName: "RegisteredUsersCount", model: registeredUsers);
            
        }
    }

  
}
