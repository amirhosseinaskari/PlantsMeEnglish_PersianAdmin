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
    public class CommentDetailViewComponent : ViewComponent
    {
        private readonly IWebHostEnvironment host;
        public CommentDetailViewComponent(IWebHostEnvironment _host)
        {
            host = _host;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            return View("CommentDetail");
        }
    }

}
