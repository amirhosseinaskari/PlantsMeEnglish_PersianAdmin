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
    public class CategoryProductList : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryProductList(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        //sort: 0- latest product
        //      1- top sell product
        //      2- top viewed product
        //      3- top price product
        //      4- low price product
        public async Task<IViewComponentResult> InvokeAsync(int pageIndex = 1,
            int defaultCategoryId = -1,
            int defaultBrandId = -1,
            List<int?> catIds = null, 
            List<int?> brandIds = null,
            int sort = 0,
            string searchText = null,
            int minValuePrice = 0,
            int maxValuePrice = 0,
            bool hasFreeDelivery = false,
            bool hasSellingStock = false)
        {
            var catIdsCount = 0;
            var brandIdsCount = 0;
            if(catIds != null)
            {
                catIdsCount = catIds.Count();
            }
            if (brandIds != null)
            {
                brandIdsCount = brandIds.Count();
            }
            if(!string.IsNullOrWhiteSpace(searchText) &&
                catIdsCount == 0 &&
                brandIdsCount == 0)
            {
                searchText = searchText.Trim().ToLower();
              

                //find products
                IQueryable<Product> products = _db.Products
                 .AsQueryable()
                 .AsNoTracking()
                 .Where(c => c.IsPublished && (c.Title.ToLower().Contains(searchText)))
                 .Include(c => c.Images)
                .Include(c => c.SpecialDiscount);

                //find categories
                List<int> myCatIds =await _db.Categories.AsNoTracking()
                    .Where(c => c.Title.ToLower().Contains(searchText)).Select(c => c.CategoryId)
                 .ToListAsync();
                foreach (var item in myCatIds)
                {
                    var nested = new MyShop.InfraStructure.NestedProduct(_db);
                    var otherProducts = await nested.GetProductsAsync((int)item, minValuePrice, maxValuePrice);
                    if (products == null)
                    {
                        products = otherProducts;
                    }
                    else
                    {
                        products = products.Union(otherProducts);
                    }
                }
                //find products by brand
                List<int> myBrandIds = await _db.Brands.AsNoTracking()
                   .Where(c => c.Title.ToLower().Contains(searchText))
                    .Select(c => c.BrandId)
                    .ToListAsync();
                foreach (var item in myBrandIds)
                {

                    IQueryable<Product> otherProducts = _db.Products
                       .AsQueryable()
                       .AsNoTracking()
                       .Where(c => c.IsPublished && c.BrandId.Equals((int)item))
                       .Include(c => c.Images)
                       .Include(c => c.SpecialDiscount);
                    if (products == null)
                    {
                        products = otherProducts;
                    }
                    else
                    {
                        products = products.Union(otherProducts);
                    }
                }
               
                //pagination list numbers 
                var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                //detect saved products in favorits
                if (User.Identity.IsAuthenticated)
                {
                    var userId = await _userManager.Users
                    .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
                    var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                        .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                    pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                }
                return View(viewName: "CategoryProductList", model: pageinatedList);
            }
           else if (defaultCategoryId <= 0 && defaultBrandId <= 0)
            {
                if (catIdsCount < 1 && brandIdsCount < 1)
                {
                    IQueryable<Product> products = _db.Products
                  .AsQueryable()
                  .AsNoTracking()
                  .Where(c => c.IsPublished && c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                  .Include(c => c.Images)
                 .Include(c => c.SpecialDiscount);
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                  
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                     pageIndex: pageIndex,
                     pageSize: 12,sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if (catIdsCount > 0 && brandIdsCount < 1)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                   
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if(catIdsCount < 1 && brandIdsCount > 0)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in brandIds)
                    {
                       
                       IQueryable<Product> otherProducts = _db.Products
                          .AsQueryable()
                          .AsNoTracking()
                          .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                         c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                          .Include(c => c.Images)
                          .Include(c => c.SpecialDiscount);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                 
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else
                {
                    IQueryable<Product> products01 = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products01 == null)
                        {
                            products01 = otherProducts;
                        }
                        else
                        {
                            products01 = products01.Union(otherProducts);
                        }
                    }
                    IQueryable<Product> products02 = null;
                    foreach (var item in brandIds)
                    {

                        IQueryable<Product> otherProducts = _db.Products
                           .AsQueryable()
                           .AsNoTracking()
                           .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                            c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                           .Include(c => c.Images)
                           .Include(c => c.SpecialDiscount);
                        if (products02 == null)
                        {
                            products02 = otherProducts;
                        }
                        else
                        {
                            products02 = products02.Union(otherProducts);
                        }
                    }
                    IQueryable<Product> products = null;
                    if (products01 != null)
                    {
                        products = products01.Intersect(products02);
                    }
                    else
                    {
                        products = products02;
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                   
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
            }
            else if(defaultCategoryId > 0 && defaultBrandId <= 0)
            {
                if (catIdsCount < 1 && brandIdsCount < 1)
                {
                    var nested = new MyShop.InfraStructure.NestedProduct(_db);
                    var products = await nested.GetProductsAsync(defaultCategoryId,minValuePrice,maxValuePrice);
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                    
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                     pageIndex: pageIndex,
                     pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if (catIdsCount > 0 && brandIdsCount < 1)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                    
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if (catIdsCount < 1 && brandIdsCount > 0)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in brandIds)
                    {

                        IQueryable<Product> otherProducts = _db.Products
                           .AsQueryable()
                           .AsNoTracking()
                           .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                           .Include(c => c.Images)
                           .Include(c => c.SpecialDiscount);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                  
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else
                {
                    IQueryable<Product> products01 = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products01 == null)
                        {
                            products01 = otherProducts;
                        }
                        else
                        {
                            products01 = products01.Union(otherProducts);
                        }
                    }
                    IQueryable<Product> products02 = null;
                    foreach (var item in brandIds)
                    {

                        IQueryable<Product> otherProducts = _db.Products
                           .AsQueryable()
                           .AsNoTracking()
                           .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                           .Include(c => c.Images)
                           .Include(c => c.SpecialDiscount);
                        if (products02 == null)
                        {
                            products02 = otherProducts;
                        }
                        else
                        {
                            products02 = products02.Union(otherProducts);
                        }
                    }
                    IQueryable<Product> products = null;
                    if (products01 != null)
                    {
                        products = products01.Intersect(products02);
                    }
                    else
                    {
                        products = products02;
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                   
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }

            }
            else if(defaultBrandId > 0 && defaultCategoryId <= 0)
            {
                if (catIdsCount < 1 && brandIdsCount < 1)
                {
                    IQueryable<Product> products = _db.Products
                  .AsQueryable()
                  .AsNoTracking()
                  .Where(c => c.IsPublished && c.BrandId.Equals(defaultBrandId) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                  .Include(c => c.Images)
                 .Include(c => c.SpecialDiscount);
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                    
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                     pageIndex: pageIndex,
                     pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if (catIdsCount > 0 && brandIdsCount < 1)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                    
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else if (catIdsCount < 1 && brandIdsCount > 0)
                {
                    IQueryable<Product> products = null;
                    foreach (var item in brandIds)
                    {

                        IQueryable<Product> otherProducts = _db.Products
                           .AsQueryable()
                           .AsNoTracking()
                           .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                           .Include(c => c.Images)
                           .Include(c => c.SpecialDiscount);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                   
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
                else
                {
                    IQueryable<Product> products = null;
                    foreach (var item in catIds)
                    {
                        var nested = new MyShop.InfraStructure.NestedProduct(_db);
                        var otherProducts = await nested.GetProductsAsync((int)item,minValuePrice,maxValuePrice);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    foreach (var item in brandIds)
                    {

                        IQueryable<Product> otherProducts = _db.Products
                           .AsQueryable()
                           .AsNoTracking()
                           .Where(c => c.IsPublished && c.BrandId.Equals((int)item) &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                           .Include(c => c.Images)
                           .Include(c => c.SpecialDiscount);
                        if (products == null)
                        {
                            products = otherProducts;
                        }
                        else
                        {
                            products = products.Union(otherProducts);
                        }
                    }
                    if (hasFreeDelivery)
                    {
                        products = products.Where(c => c.HasFreeDelivery)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    if (hasSellingStock)
                    {
                        products = products.Where(c => c.Stock > 0)
                            .Include(c => c.Images)
                            .Include(c => c.SpecialDiscount);
                    }
                    //sort products
                    products.AsQueryable().OrderBy(c => c.ViewNumber);
                   
                    //pagination list numbers
                    var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                    pageIndex: pageIndex,
                    pageSize: 12, sort: sort);
                    //detect saved products in favorits
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = await _userManager.Users
                        .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();
                        var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                            .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                        pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                    }
                    return View(viewName: "CategoryProductList", model: pageinatedList);
                }
            }

            else
            {
                IQueryable<Product> products = _db.Products
                  .AsQueryable()
                  .AsNoTracking()
                  .Where(c => c.IsPublished &&
                 c.BasePrice >= minValuePrice && c.BasePrice <= maxValuePrice)
                  .Include(c => c.Images)
                 .Include(c => c.SpecialDiscount);
                if (hasFreeDelivery)
                {
                    products = products.Where(c => c.HasFreeDelivery)
                        .Include(c => c.Images)
                        .Include(c => c.SpecialDiscount);
                }
                if (hasSellingStock)
                {
                    products = products.Where(c => c.Stock > 0)
                        .Include(c => c.Images)
                        .Include(c => c.SpecialDiscount);
                }
                //sort products
                products.AsQueryable().OrderBy(c => c.ViewNumber);
              
                //pagination list numbers
                var pageinatedList = await ProductPaginatedList.CreateAsync(source: products,
                 pageIndex: pageIndex,
                 pageSize: 12, sort: sort);
                //detect saved products in favorits
                if (User.Identity.IsAuthenticated)
                {
                    var userId = await _userManager.Users
                    .Where(c => c.UserName.Equals(HttpContext.User.Identity.Name))
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();
                    var favoritProductIds = await _db.FavoritProduct.AsNoTracking()
                        .Where(c => c.UserId.Equals(userId)).Select(c => c.ProductId).ToListAsync();
                    pageinatedList.Where(c => favoritProductIds.Contains(c.Id)).ToList().ForEach(c => c.IsProductSaved = true);

                }
                return View(viewName: "CategoryProductList", model: pageinatedList);
            }

        }


    }


}
