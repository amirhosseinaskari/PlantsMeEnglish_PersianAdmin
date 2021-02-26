using Microsoft.AspNetCore.Hosting;
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
    public class BlogList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public BlogList(ApplicationDbContext db)
        {
            _db = db;
        }
        //sort: 0- recommended blogs  1- latest blogs  2- top rated blogs  3- top viewd blogs

        public async Task<IViewComponentResult> InvokeAsync(int sort = 0, List<int> blogCatIds = null, string searchText = null, int pageIndex = 1)
        {

            //if search text is not null
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                
                searchText = searchText.Trim();
                string normalizedSearchText01 = searchText.Replace("آ", "ا");
                string normalizedSearchText02 = searchText[0].ToString().Replace("ا", "آ") +
                    searchText.Substring(1, searchText.Length - 1);

                //find blogs matched 
                IQueryable<Blog> blogs = _db.Blogs
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.IsPublished && (c.Title.Contains(normalizedSearchText01) ||
                    c.Title.Contains(normalizedSearchText02) ||
                    c.Title.Contains(searchText) ||
                    c.Title.Replace("آ", "ا").Contains(searchText) ||
                    c.Title.Replace("آ", "ا").Contains(normalizedSearchText01) ||
                    c.Title.Replace("آ", "ا").Contains(normalizedSearchText02)) ||
                    c.SummaryDescription.Contains(normalizedSearchText01) ||
                    c.SummaryDescription.Contains(normalizedSearchText02) ||
                    c.SummaryDescription.Replace("آ", "ا").Contains(searchText) ||
                    c.SummaryDescription.Replace("آ", "ا").Contains(normalizedSearchText01) ||
                    c.SummaryDescription.Replace("آ", "ا").Contains(normalizedSearchText02))
                .OrderBy(c => c.BlogOrder);

                var blogCategoryIds = await _db.BlogCategories
                    .AsNoTracking()
                    .Where(c => c.Title.Contains(normalizedSearchText01) ||
                    c.Title.Contains(normalizedSearchText02) ||
                    c.Title.Contains(searchText) ||
                    c.Title.Replace("آ", "ا").Contains(searchText) ||
                    c.Title.Replace("آ", "ا").Contains(normalizedSearchText01) ||
                    c.Title.Replace("آ", "ا").Contains(normalizedSearchText02)).Select(c => c.BlogCategoryId).ToListAsync();
                foreach (var item in blogCategoryIds)
                {
                    IQueryable<Blog> myblogs = _db.Blogs
                        .AsQueryable()
                        .AsNoTracking()
                        .Where(c => c.IsPublished && c.BlogCategoryId.Equals(item));
                    if (blogs.Count() == 0)
                    {
                        blogs = myblogs;
                    }
                    else
                    {
                        blogs = blogs.Distinct().Union(myblogs);
                    }
                }
                var pageinatedList = await PaginatedList<Blog>.CreateAsync(source: blogs, pageIndex: pageIndex, pageSize: 12);
                return View(viewName: "BlogList", model: pageinatedList);
            }
            else if (blogCatIds != null)
            {
                //find blogs matched by blog category id and sort status
                IQueryable<Blog> blogs = null;
                foreach (var item in blogCatIds)
                {
                    var myBlogs = _db.Blogs.AsQueryable()
                                    .AsNoTracking()
                                    .Where(c => c.IsPublished && c.BlogCategoryId.Equals(item));
                    if (blogs != null)
                    {
                        blogs = blogs.Distinct().Union(myBlogs);
                    }
                    else
                    {
                        blogs = myBlogs;
                    }
                }
                if (blogs != null)
                {
                    switch (sort)
                    {
                        case 0:
                            blogs = blogs.OrderBy(c => c.BlogOrder);
                            break;
                        case 1:
                            blogs = blogs.OrderByDescending(c => c.CreatedDate);
                            break;
                        case 2:
                            blogs = blogs.OrderByDescending(c => c.RateNumber);
                            break;
                        case 3:
                            blogs = blogs.OrderByDescending(c => c.ViewNumber);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //find blogs matched by blog category id and sort status
                    blogs = _db.Blogs
                     .AsQueryable()
                     .AsNoTracking()
                     .Where(c => c.IsPublished);

                    switch (sort)
                    {
                        case 0:
                            blogs = blogs.OrderBy(c => c.BlogOrder);
                            break;
                        case 1:
                            blogs = blogs.OrderByDescending(c => c.CreatedDate);
                            break;
                        case 2:
                            blogs = blogs.OrderByDescending(c => c.RateNumber);
                            break;
                        case 3:
                            blogs = blogs.OrderByDescending(c => c.ViewNumber);
                            break;
                        default:
                            break;
                    }
                }

                var pageinatedList = await PaginatedList<Blog>.CreateAsync(source: blogs, pageIndex: pageIndex, pageSize: 12);
                return View(viewName: "BlogList", model: pageinatedList);
            }
            else
            {

                //find blogs matched by blog category id and sort status
                IQueryable<Blog> blogs = _db.Blogs
                .AsQueryable()
                .AsNoTracking()
                .Where(c => c.IsPublished);

                switch (sort)
                {
                    case 0:
                        blogs = blogs.OrderBy(c => c.BlogOrder);
                        break;
                    case 1:
                        blogs = blogs.OrderByDescending(c => c.CreatedDate);
                        break;
                    case 2:
                        blogs = blogs.OrderByDescending(c => c.RateNumber);
                        break;
                    case 3:
                        blogs = blogs.OrderByDescending(c => c.ViewNumber);
                        break;
                    default:
                        break;
                }
                var pageinatedList = await PaginatedList<Blog>.CreateAsync(source: blogs, pageIndex: pageIndex, pageSize: 12);
                return View(viewName: "BlogList", model: pageinatedList);
            }

        }
    }


}
