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
    public class FAQModel : PageModel
    {
        private readonly MyShop.Data.ApplicationDbContext _context;

        public FAQModel(MyShop.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<FAQ> FAQ { get;set; }

        public async Task OnGetAsync()
        {
            FAQ = await _context.FAQs.AsNoTracking()
                .OrderBy(c=> c.Order).ToListAsync();
        }
    }
}
