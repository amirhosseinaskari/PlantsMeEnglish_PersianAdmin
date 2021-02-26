using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;

namespace MyShop.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _db;
        public CustomerController(UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده صحیح نمی باشد")]
            [Display(Name = "ایمیل")]
            public string Email { get; set; }

            [Required(ErrorMessage = "وارد کردن نام الزامی است")]
            [StringLength(100, ErrorMessage = "فرمت نام وارد شده صحیح نمی باشد", MinimumLength = 3)]
            [Display(Name = "نام و نام خانوادگی")]
            public string FullName { get; set; }
            [Required(ErrorMessage = "وارد کردن شماره همراه الزامی است")]
            [RegularExpression(@"^[0][9][0-9]{9}$",
                  ErrorMessage = "فرمت شماره موبایل صحیح نمی باشد")]
            public string Mobile { get; set; }

            [StringLength(100, ErrorMessage = "فرمت شماره کارت وارد شده صحیح نمی باشد", MinimumLength = 16)]
            [Display(Name = "شماره کارت بانکی")]
            public string CartBankNumber { get; set; }
        }


      

        //Main page of customer profile
        [Authorize]
        [Route("Customer")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(model: user);
        }
        //Show main customer information (email, mobile, name, ...)
        [Authorize]
        [Route("Customer/CustomerInformation")]
        [HttpGet]
        public async Task<IActionResult> CustomerInformation()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(model: user);
        }

        //Edit customer information
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CustomerInformation(string m = null)
        {
            Input.Mobile = ConvertClass.PersianToEnglish(Input.Mobile);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(Input.Mobile);
            }
            if (ModelState.IsValid)
            {
                user.Mobile = Input.Mobile;
                user.FullName = Input.FullName;
                user.Email = Input.Email;
                user.CartBankNumber = Input.CartBankNumber;
                await _userManager.SetUserNameAsync(user, Input.Mobile);
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, false);


                    HttpContext.Session.SetInt32("Message", (int)Messages.InformationChangedSuccessfully);
                    return View(viewName: "CustomerInformation", model: user);
                }

                foreach (var error in result.Errors)
                {
                    if (error.Code.Equals("DuplicateUserName"))
                    {

                        error.Description = "این شماره موبایل قبلا ثبت شده است";
                        ModelState.AddModelError("Mobile", error.Description);
                    }
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewName: "CustomerInformation", model: user);
        }

        //Show orders of this customer
        [Authorize]
        [Route("Customer/AllOrders")]
        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var shopping = await _db.Shoppings.AsNoTracking()
                .Where(c => c.UserId.Equals(user.Id)).ToListAsync();
            return View(model: shopping);
        }


        [Authorize]
        [Route("Customer/Messages")]
        [HttpGet]
        public async Task<IActionResult> CustomerMessages()
        {
            return View();
        }

        [Authorize]
        [Route("Customer/PartialViewMessages")]
        [HttpGet]
        public async Task<IActionResult> PartialViewMessages()
        {
            return PartialView(viewName: "_CustomerMessages");
        }
        [Authorize]
        [Route("Customer/SendNewMessage")]
        [HttpGet]
        public IActionResult SendNewMessage()
        {
            return PartialView(viewName: "_SendNewMessage");
        }
        [Authorize]
        [Route("Customer/SendNewMessage")]
        [HttpPost]
        public async Task<IActionResult> SendNewMessage(string ticketSubject, string ticketDescription)
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var ticket = new Ticket();
            ticket.Subject = ticketSubject;
            ticket.Description = ticketDescription;
            ticket.UserId = user.Id;
            ticket.FullName = user.FullName;
            ticket.IsChecked = false;
            await _db.Tickets.AddAsync(ticket);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.MessageSentSuccessfully);
            return RedirectToAction("CustomerMessages");

        }
        [Authorize]
        [Route("Customer/SendNewReplyMessage")]
        [HttpGet]
        public async Task<IActionResult> SendNewReplyMessage(int ticketId)
        {
            var ticket = await _db.Tickets.AsNoTracking()
                .Where(c => c.Id.Equals(ticketId)).FirstOrDefaultAsync();
            return PartialView(viewName: "_SendNewReplyMessage", model: ticket);
        }

        [Authorize]
        [Route("Customer/SendNewReplyMessage")]
        [HttpPost]
        public async Task<IActionResult> SendNewReplyMessage(int ticketId, string ticketDescription)
        {
            var ticket = await _db.Tickets.AsNoTracking()
                .Where(c => c.Id.Equals(ticketId)).FirstOrDefaultAsync();
            var replyTicket = new ReplyTicket();
            replyTicket.FullName = ticket.FullName;
            replyTicket.TicketId = ticket.Id;
            replyTicket.UserId = ticket.UserId;
            replyTicket.Description = ticketDescription;
            await _db.ReplyTickets.AddAsync(replyTicket);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.MessageSentSuccessfully);
            return RedirectToAction(actionName: "MessageDetail", routeValues: new { id = ticketId });
        }
        [Authorize]
        [Route("Customer/SideBar")]
        [HttpGet]
        public IActionResult SideBar()
        {
            var username = HttpContext.User.Identity.Name;
            return ViewComponent(componentName: "CustomerSideBar", arguments: new { userName = username });
        }

        [Authorize]
        [Route("Customer/MessageDetail")]
        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var ticket = await _db.Tickets.AsNoTracking()
                .Where(c => c.Id.Equals(id))
                .Include(c => c.ReplyTickets)
                .FirstOrDefaultAsync();
            return PartialView(viewName: "_MessageDetail", model: ticket);
        }

        //Delete a ticket
        [Authorize]
        [Route("Customer/DeleteTicket")]
        [HttpPost]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _db.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _db.Tickets.Remove(ticket);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
                return RedirectToAction(actionName: "Messages");
            }
            return RedirectToAction(actionName: "Messages");
        }
        //Ticket list (used in ticket pagination button)
        //handled by ajax
        [Route("Customer/TicketList")]
        [HttpPost]
        public IActionResult TicketList(int pageIndex = 1)
        {
            return ViewComponent(componentName: "TicketList", arguments: new { pageIndex = pageIndex });
        }
        //Change password
        [Authorize]
        [Route("Customer/ChangePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [Route("Customer/PartialViewChangePassword")]
        [HttpGet]
        public IActionResult PartialViewChangePassword()
        {
            return PartialView("_ChangePassword");
        }
       

    }
}