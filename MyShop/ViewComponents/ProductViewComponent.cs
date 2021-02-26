using Microsoft.AspNetCore.Hosting;
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
    public class ProductViewComponent : ViewComponent
    {
        private readonly IWebHostEnvironment host;
        public ProductViewComponent(IWebHostEnvironment _host)
        {
            host = _host;
        }
        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            string path = System.IO.Path.Combine(host.WebRootPath, "js/admin/test.json");
            string mydata = await System.IO.File.ReadAllTextAsync(path);
            var jsonresult = JsonConvert.DeserializeObject<List<Product>>(mydata);
            var myProduct = jsonresult.Where(c => c.Id == productId).FirstOrDefault();
            return View(myProduct);

        }
    }
}
