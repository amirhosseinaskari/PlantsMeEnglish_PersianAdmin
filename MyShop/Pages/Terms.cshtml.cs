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
    public class TermsModel : PageModel
    {
        private readonly MyShop.Data.ApplicationDbContext _context;

        public TermsModel(MyShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Term Term { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Term = await _context.Terms.FirstOrDefaultAsync();

            if (Term == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
