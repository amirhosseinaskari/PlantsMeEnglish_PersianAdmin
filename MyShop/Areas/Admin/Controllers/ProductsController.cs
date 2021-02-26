using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using MyShop.Data;
using MyShop.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        public ProductsController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel : SEO
        {
            public int ProductId { get; set; }
            //Total Information:
            [Required(ErrorMessage = "تعیین کردن نام الزامی است",
                AllowEmptyStrings = false)]
            public string Title { get; set; }
            public int CategoryId { get; set; }
            public Category Category { get; set; }
            public string CategoryName { get; set; }
            public int BrandId { get; set; }

            //Discount/Stock and Pricing:
            public double BasePrice { get; set; }
            public double Discount { get; set; }
            public int Stock { get; set; }
            public int MinOrderNumber { get; set; }
            public string Unit { get; set; }

            //Product Features
            public bool HasFreeDelivery { get; set; }
            public bool AuthotityGuarantee { get; set; }
            public bool ReturnMonyGuarantee { get; set; }
            public bool LocalPayment { get; set; }
            //Product Summary
            public string Summary { get; set; }
            public int VarNumber { get; set; }
            public int SellNumber { get; set; }
            public int ViewNumber { get; set; }
            public bool IsPublished { get; set; }
            public List<IFormFile> Images { get; set; }
        }
        [Route("Admin/Products")]
        public IActionResult Index()
        {
            return View();
        }

        //Product List when click on pagination button 
        //TODO: go to the target page of product list
        //Parameteres: pageIndex: index of page 
        [Route("Admin/Products/ProductList")]
        [HttpPost]
        public IActionResult ProductList(int pageIndex = 1, string text = null, int catId = 0, int sortType = 0)
        {
            return ViewComponent(componentName: "ProductList",
                arguments: new { pageIndex = pageIndex, text = text, catId = catId, sortType = sortType });
        }
        //Create Product With a Title and a CategoryId
        [Route("Admin/Products/CreateProduct")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct()
        {
            if (ModelState.IsValid)
            {
                var product = new Product();
                product.Title = Input.Title.Trim();
                product.CategoryId = Input.CategoryId;
                var myCategoryName = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId.Equals(Input.CategoryId))
                    .Select(c => c.Title).FirstOrDefaultAsync();
                product.BrandId = -1;
                product.CreatedDate = System.DateTime.Now;
                product.IsPublished = false;
                product.CategoryName = myCategoryName;
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: "EditProduct", routeValues: new { id = product.Id });
            }
            HttpContext.Session.SetInt32("Message", (int)Messages.ErrorCreateProduct);
            return RedirectToAction("Index");
        }

        //Edit Product Page
        [Route("Admin/Products/EditProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id, string jumpTarget)
        {
            var product = await _db.Products.FindAsync(id);
            InputModel input = new InputModel();
            input.Title = product.Title;
            input.BasePrice = product.BasePrice;
            input.Discount = product.Discount;
            input.Stock = product.Stock;
            input.SellNumber = product.SellNumber;
            input.VarNumber = await _db.ProductVariations.AsNoTracking()
                .Where(c => c.ProductId.Equals(product.Id))
                .CountAsync();
            input.CategoryId = product.CategoryId;
            input.BrandId = product.BrandId;
            input.IsPublished = product.IsPublished;
            input.Summary = product.Summary;
            input.CategoryName = product.CategoryName;
            input.MetaDescription = product.MetaDescription;
            input.TitlePage = product.TitlePage;
            input.HasFreeDelivery = product.HasFreeDelivery;
            input.LocalPayment = product.LocalPayment;
            input.ReturnMonyGuarantee = product.ReturnMonyGuarantee;
            input.AuthotityGuarantee = product.AuthotityGuarantee;
            input.ProductId = id;
           
            if (!string.IsNullOrEmpty(jumpTarget))
            {
                HttpContext.Session.SetString("JumpTarget", jumpTarget);
            }

            return View(model: input);
        }

        //Edit Totla Information 
        //Contains: Title, Category, Brand
        [Route("Admin/Products/EditTotalInformation/{productId}")]
        [HttpPost]
        public async Task<IActionResult> EditTotalInformation(int productId)
        {
            var product = await _db.Products.FindAsync(productId);
            if (ModelState.IsValid)
            {
                product.Title = Input.Title.Trim();
                product.CategoryId = Input.CategoryId;
                product.CategoryName = await _db.Categories.AsNoTracking()
                    .Where(c => c.CategoryId.Equals(Input.CategoryId))
                    .Select(c => c.Title).FirstOrDefaultAsync();
                product.BrandId = Input.BrandId;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction("EditProduct", new { id = productId });
            }
            InputModel input = new InputModel();
            input.Title = product.Title;
            input.BasePrice = product.BasePrice;
            input.Discount = product.Discount;
            input.Stock = product.Stock;
            input.SellNumber = product.SellNumber;
            input.VarNumber = product.VarNumber;
            input.CategoryId = product.CategoryId;
            input.BrandId = product.BrandId;
            input.IsPublished = product.IsPublished;
            input.ProductId = product.Id;
          
            return View("EditProduct", model: Input);
        }

        //Publish Product (Ajax)
        [HttpPost]
        [Route("Admin/Products/Publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var product = await _db.Products.FindAsync(id);
            product.IsPublished = true;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        //UnPublish Product (Ajax)
        [HttpPost]
        [Route("Admin/Products/UnPublish")]
        public async Task<IActionResult> UnPublish(int id)
        {
            var product = await _db.Products.FindAsync(id);
            product.IsPublished = false;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> Images, int productId)
        {
            //Create images/products/large/product_ProductId Folder
            string largePath = System.IO.Path.Combine(_host.WebRootPath, "images/products", "large", "product_" + productId);
            if (!Directory.Exists(largePath))
            {
                Directory.CreateDirectory(largePath);
            }


            //Create images/products/medium/product_ProductId Folder
            string mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/products", "medium", "product_" + productId);
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }


            //Create images/products/small/product_ProductId Folder
            string smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/products", "small", "product_" + productId);
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }


            try
            {
                List<ProductImage> productImages = new List<ProductImage>();
                foreach (var item in Images)
                {
                    var imageName = "_" + System.DateTime.Now.ToString("yyyyMMddhhmmssms") +
                        Path.GetFileName(item.FileName);
                    //path of large image
                    var newLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/products/large/product_" + productId);
                    var finalNewLargePath = Path.Combine(newLargePath,imageName);
                    using (var stream = System.IO.File.Create(finalNewLargePath))
                    {
                        await item.CopyToAsync(stream);
                        await stream.DisposeAsync();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                    }
                    //path of medium image:
                    var newMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/medium/product_" + productId);
                    var finalNewMediumPath = System.IO.Path.Combine(newMediumPath, imageName);
                    //path of small image:
                    var newSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/small/product_" + productId);
                    var finalNewSmallPath = System.IO.Path.Combine(newSmallPath, imageName);

                 
                        using (var webPFileStream = new FileStream(finalNewLargePath, FileMode.Create))
                        {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(item.OpenReadStream())
                                        .Resize(new Size(600, 0))
                                        .Save(webPFileStream);
                        }
                    }

                        using (var webPFileStream = new FileStream(finalNewMediumPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(400, 0))
                                            .Save(webPFileStream);
                            }
                        }

                        using (var webPFileStream = new FileStream(finalNewSmallPath, FileMode.Create))
                        {
                            using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                            {
                                imageFactory.Load(item.OpenReadStream())
                                            .Resize(new Size(300, 0))
                                            .Save(webPFileStream);
                            }
                        }


                    var myimage = new ProductImage();
                    myimage.ProductId = productId;
                    myimage.ImageName = imageName;
                    myimage.ImageOrder = await _db.ProductImages
                        .AsNoTracking()
                        .Where(c => c.ProductId == productId).CountAsync();
                    productImages.Add(myimage);


                }
                await _db.ProductImages.AddRangeAsync(productImages);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }



        }

        [HttpPost]
        public IActionResult ImageList(int proId)
        {
            return ViewComponent("ProductImageList", new { proId = proId });
        }

        [HttpPost]
        public async Task<IActionResult> ImageAltText(int imageId, string altText)
        {
            var image = await _db.ProductImages.FindAsync(imageId);
            if (image != null)
            {
                image.AltText = altText;
                _db.ProductImages.Update(image);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeImageOrder(IEnumerable<int?> myIds)
        {
            int order = 1;
            List<ProductImage> productImages = new List<ProductImage>();
            foreach (int? item in myIds)
            {
                if (item == null)
                {
                    return Json(data: false);
                }
                ProductImage objProductImage = await _db.ProductImages.FindAsync(item);
                objProductImage.ImageOrder = order;
                productImages.Add(objProductImage);

                order++;
            }
            _db.ProductImages.UpdateRange(productImages);
            await _db.SaveChangesAsync();
            return Json(data: true);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductImage(int productId, int id)
        {
            var productImage = await _db.ProductImages.FindAsync(id);
            var oldLargePath = System.IO.Path.Combine(_host.WebRootPath, "images/products/large/product_" + productId);
            var finalOldLargePath = Path.Combine(oldLargePath, Path.GetFileName(productImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldLargePath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldLargePath);
                }

            }
            catch (Exception e)
            {
            }

            //Medium Images
            var oldMediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/medium/product_" + productId);
            var finalOldMediumPath = Path.Combine(oldMediumPath, Path.GetFileName(productImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldMediumPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldMediumPath);
                }

            }
            catch (Exception e)
            {
            }
            //Small Images
            var oldSmallPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/small/product_" + productId);
            var finalOldSmallPath = Path.Combine(oldSmallPath, Path.GetFileName(productImage.ImageName));
            try
            {
                if (System.IO.File.Exists(finalOldSmallPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(finalOldSmallPath);
                }

            }
            catch (Exception e)
            {
            }

            _db.ProductImages.Remove(productImage);
            await _db.SaveChangesAsync();
            var productImages = await _db.ProductImages.Where(c => c.ProductId.Equals(productId))
                .OrderBy(c => c.ImageOrder).ToListAsync();
            int order = 1;

            foreach (var item in productImages)
            {
                item.ImageOrder = order;

                order++;
            }
            _db.ProductImages.UpdateRange(productImages);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction("EditProduct", new { id = productId });
        }

        //Edit Product Pricing
        [Route("Admin/Products/EditProductPricing/{id}")]
        [HttpPost]
        public async Task<IActionResult> EditProductPricing(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.BasePrice = Input.BasePrice;
                product.Discount = Input.Discount;
                product.Stock = Input.Stock;
                product.Unit = Input.Unit;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();

                var specialDiscount = await _db.SpecialDiscount.Where(c => c.ProductId.Equals(id))
                    .FirstOrDefaultAsync();
                if (specialDiscount != null)
                {
                    specialDiscount.BasePrice = Input.BasePrice;
                    _db.SpecialDiscount.Update(specialDiscount);
                    await _db.SaveChangesAsync();
                }

                HttpContext.Session.SetInt32("Message", (int)Messages.EditedSuccessfully);
                return RedirectToAction("EditProduct", new { id = id });
            }

            HttpContext.Session.SetInt32("Message", (int)Messages.EditedWithError);
            return RedirectToAction("EditProduct", new { id = id });
        }

        //Edit Product Features:
        //1- Free Delivery
        [HttpPost]
        public async Task<IActionResult> EditHasFreeDelivery(int id, bool result)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.HasFreeDelivery = result;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //2- Authority Guarantee
        [HttpPost]
        public async Task<IActionResult> EditAuthotityGuarantee(int id, bool result)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.AuthotityGuarantee = result;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //3- Local Payment
        [HttpPost]
        public async Task<IActionResult> EditLocalPayment(int id, bool result)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.LocalPayment = result;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //4- Return Money
        [HttpPost]
        public async Task<IActionResult> EditReturnMonyGuarantee(int id, bool result)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.ReturnMonyGuarantee = result;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        //Product Description
        [Route("Admin/Products/ProductDescription")]
        [HttpPost]
        public async Task<IActionResult> ProductDescription(int id, string text)
        {
            try
            {
                var product = await _db.Products.FindAsync(id);
                product.Summary = text;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }

        }

        //SEO Setting
        //parameters: => id: product id/ titlePage: TitlePage / metaDescription: MetaDescription
        [Route("Admin/Products/SEO")]
        [HttpPost]
        public async Task<IActionResult> SEO(int id, string titlePage, string metaDescription, string redirectURL)
        {
            var product = await _db.Products.FindAsync(id);
            try
            {
                product.TitlePage = titlePage;
                product.MetaDescription = metaDescription;
                product.RedirectURL = redirectURL;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Publish Product
        //parameters: => id: product id/ publish: product publish situation
        [Route("Admin/Products/Publishment")]
        [HttpPost]
        public async Task<IActionResult> Publishment(int id, bool publish)
        {
            var product = await _db.Products.FindAsync(id);
            try
            {
                product.IsPublished = publish;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }
        }

        //Delete a product
        //parameters: => id: product id
        [Route("Admin/Products/DeleteProduct")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            var largePath = System.IO.Path.Combine(_host.WebRootPath, "images/products/large/product_" + id);
            if (Directory.Exists(largePath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(largePath, true);
                
            }

            var mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/medium/product_" + id);
            if (Directory.Exists(mediumPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(mediumPath, true);
            }

            var smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/small/product_" + id);
            if (Directory.Exists(smallPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(smallPath, true);
            }

            //Remove images/product_content/large/product_id/ Folder
            string largeContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "large", "product_" + id);
            if (Directory.Exists(largeContentPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(largeContentPath,true);
            }


            //Remove images/product_content/medium/product_id/ Folder
            string mediumContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "medium", "product_" + id);
            if (Directory.Exists(mediumContentPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(mediumContentPath,true);
            }


            //Remove images/product_content/small/product_id/ Folder
            string smallContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "small", "product_" + id);
            if (Directory.Exists(smallContentPath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                Directory.Delete(smallContentPath,true);
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "Products");
        }

        //Delete a group of products
        //parameters: => ids: product ids
        [Route("Admin/Products/DeleteGroupProduct")]
        [HttpPost]
        public async Task<IActionResult> DeleteGroupProduct(List<int?> ids)
        {
            var products = await _db.Products.Where(c => ids.Contains(c.Id)).ToListAsync();
            foreach (var item in products)
            {
                var largePath = System.IO.Path.Combine(_host.WebRootPath, "images/products/large/product_" + item.Id);
                if (Directory.Exists(largePath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(largePath, true);
                }

                var mediumPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/medium/product_" + item.Id);
                if (Directory.Exists(mediumPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(mediumPath, true);
                }

                var smallPath = System.IO.Path.Combine(_host.WebRootPath, "images/products/small/product_" + item.Id);
                if (Directory.Exists(smallPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(smallPath, true);
                }

                //Remove images/product_content/large/product_id/ Folder
                string largeContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "large", "product_" + item.Id);
                if (Directory.Exists(largeContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(largeContentPath, true);
                }


                //Remove images/product_content/medium/product_id/ Folder
                string mediumContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "medium", "product_" + item.Id);
                if (Directory.Exists(mediumContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(mediumContentPath, true);
                }


                //Remove images/product_content/small/product_id/ Folder
                string smallContentPath = System.IO.Path.Combine(_host.WebRootPath, "images/product_content", "small", "product_" + item.Id);
                if (Directory.Exists(smallContentPath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    Directory.Delete(smallContentPath, true);
                }

            }
            _db.Products.RemoveRange(products);
            await _db.SaveChangesAsync();
            HttpContext.Session.SetInt32("Message", (int)Messages.DeletedSuccessfully);
            return RedirectToAction(actionName: "Index", controllerName: "Products");
        }

        //Publish a group of products
        //parameters: => ids: product ids
        [Route("Admin/Products/PublishGroupProduct")]
        [HttpPost]
        public async Task<IActionResult> PublishGroupProduct(List<int?> ids)
        {
            try
            {
                var products = await _db.Products.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in products)
                {
                    item.IsPublished = true;
                }
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }


        }

        //UnPublish a group of products
        //parameters: => ids: product ids
        [Route("Admin/Products/UnPublishGroupProduct")]
        [HttpPost]
        public async Task<IActionResult> UnPublishGroupProduct(List<int?> ids)
        {
            try
            {
                var products = await _db.Products.Where(c => ids.Contains(c.Id)).ToListAsync();
                foreach (var item in products)
                {
                    item.IsPublished = false;
                }
                _db.Products.UpdateRange(products);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {

                return Json(false);
            }

        }
    }
}