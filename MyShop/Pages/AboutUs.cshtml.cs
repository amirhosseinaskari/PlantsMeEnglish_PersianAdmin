using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;

namespace MyShop.Pages
{
    public class AboutUsModel : PageModel
    {

        private readonly ApplicationDbContext _db;
        public AboutUsModel(ApplicationDbContext db)
        {
            _db = db;

        }

        public About About { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
  
            About = await _db.Abouts.AsNoTracking().FirstOrDefaultAsync();

            if (About == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}