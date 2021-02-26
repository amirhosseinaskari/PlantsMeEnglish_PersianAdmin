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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.ViewComponents
{
    [ViewComponent]
    public class RegisterChart : ViewComponent
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterChart(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        //date range: 0- week 1- month 2- year
        public async Task<IViewComponentResult> InvokeAsync(int dateRange)
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            var today = System.DateTime.Now.DayOfWeek;
            var persianDate = new PersianCalendar();

            switch (dateRange)
            {
                case 0:

                    string weeklyRegisterNumbers = null;
                    string days = null;
                    for (int i = 0; i < 7; i++)
                    {
                        var lastWeek = System.DateTime.Now.Subtract(new TimeSpan(6 - i, 0, 0, 0));
                        var lastWeekRegisters = users.Where(c => c.RegisteredDate > lastWeek).ToList();
                        var lastday = System.DateTime.Now.Subtract(new TimeSpan(24 * i, 0, 0)).DayOfWeek;
                        switch (lastday)
                        {
                            case DayOfWeek.Sunday:
                                if (days == null)
                                {
                                    days = "Sunday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Sunday");
                                }
                                int registerNumberSunday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Sunday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberSunday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberSunday);
                                }
                                continue;
                            case DayOfWeek.Monday:
                                if (days == null)
                                {
                                    days = "Monday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Monday");
                                }
                                int registerNumberMonday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Monday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberMonday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberMonday);
                                }
                                continue;
                            case DayOfWeek.Tuesday:
                                if (days == null)
                                {
                                    days = "Tuesday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Tuesday");
                                }
                                int registerNumberTuesday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Tuesday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberTuesday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberTuesday);
                                }
                                continue;
                            case DayOfWeek.Wednesday:
                                if (days == null)
                                {
                                    days = "Wednesday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Wednesday");
                                }
                                int registerNumberWednesday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Wednesday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberWednesday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberWednesday);
                                }
                                continue;
                            case DayOfWeek.Thursday:
                                if (days == null)
                                {
                                    days = "Thursday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Thursday");
                                }
                                int registerNumberThursday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Thursday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberThursday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberThursday);
                                }
                                continue;
                            case DayOfWeek.Friday:
                                if (days == null)
                                {
                                    days = "Friday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Friday");
                                }
                                int registerNumberFriday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Friday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberFriday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberFriday);
                                }
                                continue;
                            case DayOfWeek.Saturday:
                                if (days == null)
                                {
                                    days = "Saturday";
                                }
                                else
                                {
                                    days = string.Join(",", days, "Saturday");
                                }
                                int registerNumberSaturday = lastWeekRegisters
                                    .Where(c => c.RegisteredDate.DayOfWeek.Equals(DayOfWeek.Saturday)).Count();
                                if (weeklyRegisterNumbers == null)
                                {
                                    weeklyRegisterNumbers = registerNumberSaturday.ToString();
                                }
                                else
                                {
                                    weeklyRegisterNumbers = string.Join(",", weeklyRegisterNumbers, registerNumberSaturday);
                                }
                                continue;
                            default:
                                weeklyRegisterNumbers = "0";
                                days = "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday";
                                continue;
                        }
                    }
                    var registerChart01 = new RegisterChartClass();
                    registerChart01.RegisterNumbers = weeklyRegisterNumbers;
                    registerChart01.TimeColumns = days;
                    return View(viewName: "RegisterChart", model: registerChart01);
                case 1:

                    string monthlyRegisterNumbers = null;
                    string months = null;

                    for (int i = 0; i < 12; i++)
                    {

                        var lastYear = System.DateTime.Now.Subtract(new TimeSpan(365 - (31 * i), 0, 0, 0));
                        
                        var lastYearUsers = users.Where(c => c.RegisteredDate > lastYear).ToList();
                        var lastMonth = System.DateTime.Now.Subtract(new TimeSpan(30 * i, 0, 0, 0));
                        if(System.DateTime.Now.Month < 7)
                        {
                            lastMonth = System.DateTime.Now.Subtract(new TimeSpan(31 * i, 0, 0, 0));
                        }
                        var persianLastMonth = persianDate.GetMonth(lastMonth);
                        switch (persianLastMonth)
                        {
                            case 1:
                                if (months == null)
                                {
                                    months = "January";
                                }
                                else
                                {
                                    months = string.Join(",", months, "January");
                                }
                                int registerNumberFarvardin = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberFarvardin.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberFarvardin);
                                }
                                break;
                            case 2:
                                if (months == null)
                                {
                                    months = "February";
                                }
                                else
                                {
                                    months = string.Join(",", months, "February");
                                }
                                int registerNumberOrdibehesht = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberOrdibehesht.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberOrdibehesht);
                                }
                                break;
                            case 3:
                                if (months == null)
                                {
                                    months = "March";
                                }
                                else
                                {
                                    months = string.Join(",", months, "March");
                                }
                                int registerNumberKhordad = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberKhordad.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberKhordad);
                                }
                                break;
                            case 4:
                                if (months == null)
                                {
                                    months = "April";
                                }
                                else
                                {
                                    months = string.Join(",", months,"April");
                                }
                                int registerNumberTir = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberTir.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberTir);
                                }
                                break;
                            case 5:
                                if (months == null)
                                {
                                    months = "May";
                                }
                                else
                                {
                                    months = string.Join(",", months,"May");
                                }
                                int registerNumberMordad = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberMordad.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberMordad);
                                }
                                break;
                            case 6:
                                if (months == null)
                                {
                                    months = "June";
                                }
                                else
                                {
                                    months = string.Join(",", months, "June");
                                }
                                int registerNumberShahrivar = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberShahrivar.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberShahrivar);
                                }
                                break;
                            case 7:
                                if (months == null)
                                {
                                    months = "July";
                                }
                                else
                                {
                                    months = string.Join(",",months, "July");
                                }
                                int registerNumberMehr = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberMehr.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberMehr);
                                }
                                break;
                            case 8:
                                if (months == null)
                                {
                                    months = "August";
                                }
                                else
                                {
                                    months = string.Join(",", months, "August");
                                }
                                int registerNumberAban = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberAban.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberAban);
                                }
                                break;
                            case 9:
                                if (months == null)
                                {
                                    months = "September";
                                }
                                else
                                {
                                    months = string.Join(",", months, "September");
                                }
                                int registerNumberAzar = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberAzar.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberAzar);
                                }
                                break;
                            case 10:
                                if (months == null)
                                {
                                    months = "October";
                                }
                                else
                                {
                                    months = string.Join(",", months, "October");
                                }
                                int registerNumberDey = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberDey.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberDey);
                                }
                                break;
                            case 11:
                                if (months == null)
                                {
                                    months = "November";
                                }
                                else
                                {
                                    months = string.Join(",", months, "November");
                                }
                                int registerNumberBahman = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberBahman.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberBahman);
                                }
                                break;
                            case 12:
                                if (months == null)
                                {
                                    months = "December";
                                }
                                else
                                {
                                    months = string.Join(",", months, "December");
                                }
                                int registerNumberEsfand = lastYearUsers
                                    .Where(c => c.RegisteredDate.Month.Equals(lastMonth.Month)).Count();
                                if (monthlyRegisterNumbers == null)
                                {
                                    monthlyRegisterNumbers = registerNumberEsfand.ToString();
                                }
                                else
                                {
                                    monthlyRegisterNumbers = string.Join(",", monthlyRegisterNumbers, registerNumberEsfand);
                                }
                                break;
                            default:
                                monthlyRegisterNumbers = "0";
                                months = "January,February,March,April,May,June,July,August,September,October,November,December";
                                break;
                        }
                    }
                    var registerChart02 = new RegisterChartClass();
                    registerChart02.RegisterNumbers = monthlyRegisterNumbers;
                    registerChart02.TimeColumns = months;
                    return View(viewName: "RegisterChart", model: registerChart02);
                case 2:
                    
                    string yearlyRegisterNumbers = null;
                    string years = null;
                    for (int i = 1; i < 7; i++)
                    {
                        var last6Years = System.DateTime.Now.Subtract(new TimeSpan((i * 365), 0, 0, 0));
                        var last6YearsUsers = users.Where(c => c.RegisteredDate > last6Years).ToList();
                        var lastYear01 = System.DateTime.Now.Subtract(new TimeSpan((365 * i - 365), 0, 0, 0));
                        var persianLastYear = persianDate.GetYear(lastYear01);
                        int lastYearRegisterNumber = users.Where(c => c.RegisteredDate.Year.Equals(lastYear01.Year)).Count();
                        if (yearlyRegisterNumbers == null)
                        {
                            yearlyRegisterNumbers = lastYearRegisterNumber.ToString();
                        }
                        else
                        {
                            yearlyRegisterNumbers = string.Join(",", yearlyRegisterNumbers, lastYearRegisterNumber);
                        }
                        if (years == null)
                        {
                            years = persianLastYear.ToString();
                        }
                        else
                        {
                            years = string.Join(",", years, persianLastYear);
                        }
                    }
                    var registerChart03 = new RegisterChartClass();
                    registerChart03.RegisterNumbers = yearlyRegisterNumbers;
                    registerChart03.TimeColumns = years;
                    return View(viewName: "RegisterChart", model: registerChart03);
                default:
                    var registerChart04 = new RegisterChartClass();
                    registerChart04.RegisterNumbers = "0,0,0,0,0,0";
                    registerChart04.TimeColumns = "0,0,0,0,0,0";
                    return View(viewName: "RegisterChart", model: registerChart04);

            }

        }
    }


}
