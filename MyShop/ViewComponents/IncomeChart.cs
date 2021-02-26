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
    public class IncomeChart : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public IncomeChart(ApplicationDbContext db)
        {
            _db = db;

        }
        //date range: 0- week 1- month 2- year
        public async Task<IViewComponentResult> InvokeAsync(int dateRange)
        {
            var shoppings = await _db.Shoppings.AsNoTracking()
                .Where(c => !c.Status.Equals(Status.Cancelled) && !c.Status.Equals(Status.WaitForRegister))
                .ToListAsync();
            var today = System.DateTime.Now.DayOfWeek;
            var persianDate = new PersianCalendar();

            switch (dateRange)
            {
                case 0:

                    string weeklySellNumbers = null;
                    string days = null;
                    for (int i = 0; i < 7; i++)
                    {
                        var lastWeek = System.DateTime.Now.Subtract(new TimeSpan(6 - i, 0, 0, 0));
                        var lastWeekShoppings = shoppings.Where(c => c.PaymentDateTime > lastWeek).ToList();
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
                                int sellNumberSunday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Sunday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberSunday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberSunday);
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
                                int sellNumberMonday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Monday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberMonday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberMonday);
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
                                int sellNumberTuesday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Tuesday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberTuesday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberTuesday);
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
                                int sellNumberWednesday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Wednesday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberWednesday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberWednesday);
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
                                int sellNumberThursday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Thursday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberThursday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberThursday);
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
                                int sellNumberFriday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Friday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberFriday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberFriday);
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
                                int sellNumberSaturday = lastWeekShoppings
                                    .Where(c => c.PaymentDateTime.DayOfWeek.Equals(DayOfWeek.Saturday)).Count();
                                if (weeklySellNumbers == null)
                                {
                                    weeklySellNumbers = sellNumberSaturday.ToString();
                                }
                                else
                                {
                                    weeklySellNumbers = string.Join(",", weeklySellNumbers, sellNumberSaturday);
                                }
                                continue;
                            default:
                                weeklySellNumbers = "0";
                                days = "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday";
                                continue;
                        }
                    }
                    var incomeChart01 = new IncomeChartClass();
                    incomeChart01.SellNumbers = weeklySellNumbers;
                    incomeChart01.TimeColumns = days;
                    return View(viewName: "IncomeChart", model: incomeChart01);
                case 1:

                    string monthlySellNumbers = null;
                    string months = null;

                    for (int i = 0; i < 12; i++)
                    {
                        var lastYear = System.DateTime.Now.Subtract(new TimeSpan(365 - (31 * i), 0, 0, 0));
                        var lastYearShoppings = shoppings.Where(c => c.PaymentDateTime > lastYear).ToList();
                        var lastMonth = System.DateTime.Now.Subtract(new TimeSpan(30 * i, 0, 0, 0));
                        if (System.DateTime.Now.Month < 7)
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
                                int sellNumberFarvardin = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberFarvardin.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberFarvardin);
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
                                int sellNumberOrdibehesht = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberOrdibehesht.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberOrdibehesht);
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
                                int sellNumberKhordad = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberKhordad.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberKhordad);
                                }
                                break;
                            case 4:
                                if (months == null)
                                {
                                    months = "April";
                                }
                                else
                                {
                                    months = string.Join(",", months, "April");
                                }
                                int sellNumberTir = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberTir.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberTir);
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
                                int sellNumberMordad = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberMordad.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberMordad);
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
                                int sellNumberShahrivar = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberShahrivar.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberShahrivar);
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
                                int sellNumberMehr = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberMehr.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberMehr);
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
                                int sellNumberAban = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberAban.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberAban);
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
                                int sellNumberAzar = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberAzar.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberAzar);
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
                                int sellNumberDey = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberDey.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberDey);
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
                                int sellNumberBahman = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberBahman.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberBahman);
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
                                int sellNumberEsfand = lastYearShoppings
                                    .Where(c => c.PaymentDateTime.Month.Equals(lastMonth.Month)).Count();
                                if (monthlySellNumbers == null)
                                {
                                    monthlySellNumbers = sellNumberEsfand.ToString();
                                }
                                else
                                {
                                    monthlySellNumbers = string.Join(",", monthlySellNumbers, sellNumberEsfand);
                                }
                                break;
                            default:
                                monthlySellNumbers = "0";
                                months = "January,February,March,April,May,June,July,August,September,October,November,December";
                                break;
                        }
                    }
                    var incomeChart02 = new IncomeChartClass();
                    incomeChart02.SellNumbers = monthlySellNumbers;
                    incomeChart02.TimeColumns = months;
                    return View(viewName: "IncomeChart", model: incomeChart02);
                case 2:
                    
                    string yearlySellNumbers = null;
                    string years = null;
                    for (int i = 1; i < 7; i++)
                    {
                        var last6Years = System.DateTime.Now.Subtract(new TimeSpan((i * 365), 0, 0, 0));
                        var last6YearsShoppings = shoppings.Where(c => c.PaymentDateTime > last6Years).ToList();
                        var lastYear01 = System.DateTime.Now.Subtract(new TimeSpan((365 * i - 365), 0, 0, 0));
                        var persianLastYear = persianDate.GetYear(lastYear01);
                        int lastYearShoppingsSellNumber = shoppings.Where(c => c.PaymentDateTime.Year.Equals(lastYear01.Year)).Count();
                        if (yearlySellNumbers == null)
                        {
                            yearlySellNumbers = lastYearShoppingsSellNumber.ToString();
                        }
                        else
                        {
                            yearlySellNumbers = string.Join(",", yearlySellNumbers, lastYearShoppingsSellNumber);
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
                    var incomeChart03 = new IncomeChartClass();
                    incomeChart03.SellNumbers = yearlySellNumbers;
                    incomeChart03.TimeColumns = years;
                    return View(viewName: "IncomeChart", model: incomeChart03);
                default:
                    var incomeChart04 = new IncomeChartClass();
                    incomeChart04.SellNumbers = "0";
                    incomeChart04.TimeColumns = "0,0,0,0,0";
                    return View(viewName: "IncomeChart", model: incomeChart04);

            }

        }
    }


}
