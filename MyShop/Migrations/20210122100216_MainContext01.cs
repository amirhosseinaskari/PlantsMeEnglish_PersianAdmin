using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyShop.Migrations
{
    public partial class MainContext01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ClientRole = table.Column<int>(nullable: false),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(nullable: false),
                    IsMobileConfirmed = table.Column<bool>(nullable: false),
                    MobileVerificationCode = table.Column<string>(nullable: true),
                    ResetPasswordToken = table.Column<string>(nullable: true),
                    NumberOfResendVerificationCode = table.Column<byte>(nullable: false),
                    LastResendVerificationCode = table.Column<DateTime>(nullable: false),
                    CartBankNumber = table.Column<string>(maxLength: 100, nullable: true),
                    DiscountCode = table.Column<string>(nullable: true),
                    BuyNumber = table.Column<int>(nullable: false),
                    TotalBuyValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogCategories",
                columns: table => new
                {
                    BlogCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    BlogOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCategories", x => x.BlogCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "BlogPageBanners",
                columns: table => new
                {
                    BlogPageBannerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Alt = table.Column<string>(nullable: true),
                    TargetLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPageBanners", x => x.BlogPageBannerId);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PutOnHomePage = table.Column<bool>(nullable: false),
                    BrandOrder = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    DiscountCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    CategoryImage = table.Column<string>(nullable: true),
                    HasParent = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    CanAddProduct = table.Column<bool>(nullable: false),
                    PutOnHomePage = table.Column<bool>(nullable: false),
                    CatOrder = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DiscountCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    WhatsAppNumber = table.Column<string>(nullable: true),
                    Instagram = table.Column<string>(nullable: true),
                    Twitter = table.Column<string>(nullable: true),
                    Telegram = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Facebook = table.Column<string>(nullable: true),
                    YouTube = table.Column<string>(nullable: true),
                    Aparat = table.Column<string>(nullable: true),
                    GoogleMap = table.Column<string>(nullable: true),
                    GoogleMapLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanSendingToAllCity = table.Column<bool>(nullable: false),
                    CityPriceStatus = table.Column<int>(nullable: false),
                    HasMinAmountForFreeDelivery = table.Column<bool>(nullable: false),
                    MinAmountForFreeDelivery = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    Percent = table.Column<byte>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    ActivationDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    IsForAllCustomers = table.Column<bool>(nullable: false),
                    DiscountTarget = table.Column<int>(nullable: false),
                    IsForMinBuyValue = table.Column<bool>(nullable: false),
                    IsForMinBuyNumber = table.Column<bool>(nullable: false),
                    MinBuyValue = table.Column<double>(nullable: false),
                    MinBuyNumber = table.Column<int>(nullable: false),
                    IsExpired = table.Column<bool>(nullable: false),
                    UsedNumber = table.Column<int>(nullable: false),
                    RemainingNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeBanners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(nullable: true),
                    AltText = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TargetLink = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeBanners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomePage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    HasFastDeliveryOption = table.Column<bool>(nullable: false),
                    HasOriginalWarranty = table.Column<bool>(nullable: false),
                    HasLocalPaymentOption = table.Column<bool>(nullable: false),
                    Has24Support = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FooterDescription = table.Column<string>(nullable: true),
                    ShowHomeBanners = table.Column<bool>(nullable: false),
                    InstagramAPI = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    MessageDate = table.Column<DateTime>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkText = table.Column<string>(nullable: true),
                    MainText = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ProductTitle = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SubProductVariationIds = table.Column<string>(nullable: true),
                    ShoppingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SEOSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleAnalytics = table.Column<string>(nullable: true),
                    GoogleSearchConsole = table.Column<string>(nullable: true),
                    SiteMap = table.Column<string>(nullable: true),
                    FavIcon = table.Column<string>(nullable: true),
                    IsBlock = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEOSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shoppings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: true),
                    DeliveryPrice = table.Column<double>(nullable: false),
                    OrderIds = table.Column<string>(nullable: true),
                    OrdersCount = table.Column<int>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    AddressInformation = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    ReceiverMobileNumber = table.Column<string>(nullable: true),
                    PaymentDateTime = table.Column<DateTime>(nullable: false),
                    TracingCode = table.Column<string>(nullable: true),
                    BasePrice = table.Column<double>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    DiscountId = table.Column<int>(nullable: false),
                    DiscountCode = table.Column<string>(nullable: true),
                    DiscountPrice = table.Column<double>(nullable: false),
                    HasLocalPaymentOption = table.Column<bool>(nullable: false),
                    IsLocalPayment = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoppings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateName = table.Column<string>(nullable: true),
                    DeliveryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutUsContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutId = table.Column<int>(nullable: false),
                    ContentType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    AltTextForImage = table.Column<string>(nullable: true),
                    ContentOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutUsContents_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalTable: "Abouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    MyUserId = table.Column<string>(nullable: true),
                    UserFullName = table.Column<string>(nullable: true),
                    ReceiverName = table.Column<string>(nullable: true),
                    ReceiverMobileNumber = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    IsSelected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    SubmitDate = table.Column<DateTime>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    TicketStatus = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    BlogCategoryId = table.Column<int>(nullable: false),
                    CoverImage = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    SummaryDescription = table.Column<string>(nullable: true),
                    BlogOrder = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    ViewNumber = table.Column<int>(nullable: false),
                    RateNumber = table.Column<double>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_Blogs_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "BlogCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitlePage = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    RedirectURL = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    BrandId = table.Column<int>(nullable: false),
                    RateNumber = table.Column<double>(nullable: false),
                    NumberOfUserRater = table.Column<int>(nullable: false),
                    BasePrice = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Stock = table.Column<int>(nullable: false),
                    Unit = table.Column<string>(nullable: true),
                    HasFreeDelivery = table.Column<bool>(nullable: false),
                    AuthotityGuarantee = table.Column<bool>(nullable: false),
                    ReturnMonyGuarantee = table.Column<bool>(nullable: false),
                    LocalPayment = table.Column<bool>(nullable: false),
                    VarNumber = table.Column<int>(nullable: false),
                    Summary = table.Column<string>(nullable: true),
                    SellNumber = table.Column<int>(nullable: false),
                    ViewNumber = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsProductSaved = table.Column<bool>(nullable: false),
                    DiscountCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    MyIdInCatTable = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeSliderImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomePageId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    AlternativeText = table.Column<string>(nullable: true),
                    TargetLink = table.Column<string>(nullable: true),
                    ImageOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSliderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeSliderImages_HomePage_HomePageId",
                        column: x => x.HomePageId,
                        principalTable: "HomePage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobileHomeSliderImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomePageId = table.Column<int>(nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    AlternativeText = table.Column<string>(nullable: true),
                    TargetLink = table.Column<string>(nullable: true),
                    ImageOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileHomeSliderImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileHomeSliderImages_HomePage_HomePageId",
                        column: x => x.HomePageId,
                        principalTable: "HomePage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    DeliveryId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(nullable: true),
                    DeliveryPrice = table.Column<double>(nullable: false),
                    IsSetDeliveryPrice = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplyTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    SubmitDate = table.Column<DateTime>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplyTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    SubmitedDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogComments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(nullable: false),
                    ContentType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    AltTextForImage = table.Column<string>(nullable: true),
                    ContentOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogContents_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogTags_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ProductTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    SubmitedDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritProduct_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ProductImageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ImageOrder = table.Column<int>(nullable: false),
                    AltText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ProductImageId);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    ProductOptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    OptionTitle = table.Column<string>(nullable: true),
                    OptionValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.ProductOptionId);
                    table.ForeignKey(
                        name: "FK_ProductOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTechnicalContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ContentType = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    AltTextForImage = table.Column<string>(nullable: true),
                    ContentOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTechnicalContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTechnicalContents_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariations",
                columns: table => new
                {
                    ProductVariationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    HasDifferentPrice = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariations", x => x.ProductVariationId);
                    table.ForeignKey(
                        name: "FK_ProductVariations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    BasePrice = table.Column<double>(nullable: false),
                    DiscountPrice = table.Column<double>(nullable: false),
                    SellNumber = table.Column<int>(nullable: false),
                    ViewNumber = table.Column<int>(nullable: false),
                    ActivationDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialDiscount_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogReplyComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SubmitedDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    BlogCommentId = table.Column<int>(nullable: false),
                    IsFromAdmin = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogReplyComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogReplyComments_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReplyComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SubmitedDate = table.Column<DateTime>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    CommentId = table.Column<int>(nullable: false),
                    IsFromAdmin = table.Column<bool>(nullable: false),
                    IsChecked = table.Column<bool>(nullable: false),
                    BlogCommentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplyComments_BlogComments_BlogCommentId",
                        column: x => x.BlogCommentId,
                        principalTable: "BlogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReplyComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubProductVariations",
                columns: table => new
                {
                    SubProductVariationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    productId = table.Column<int>(nullable: false),
                    HasDefinedDifferentPrice = table.Column<bool>(nullable: false),
                    ProductVariationId = table.Column<int>(nullable: false),
                    ProductVariationTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubProductVariations", x => x.SubProductVariationId);
                    table.ForeignKey(
                        name: "FK_SubProductVariations_ProductVariations_ProductVariationId",
                        column: x => x.ProductVariationId,
                        principalTable: "ProductVariations",
                        principalColumn: "ProductVariationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Abouts",
                columns: new[] { "Id", "MetaDescription", "RedirectURL", "TitlePage" },
                values: new object[] { 1, null, null, null });

            migrationBuilder.InsertData(
                table: "BlogCategories",
                columns: new[] { "BlogCategoryId", "BlogOrder", "Title" },
                values: new object[] { 1, 0, "Others" });

            migrationBuilder.InsertData(
                table: "ContactUs",
                columns: new[] { "Id", "Address", "Aparat", "Email", "Facebook", "GoogleMap", "GoogleMapLink", "Instagram", "MetaDescription", "Mobile", "Phone", "RedirectURL", "Telegram", "TitlePage", "Twitter", "WhatsAppNumber", "YouTube" },
                values: new object[] { 1, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "Deliveries",
                columns: new[] { "Id", "CanSendingToAllCity", "CityPriceStatus", "HasMinAmountForFreeDelivery", "MinAmountForFreeDelivery" },
                values: new object[] { 1, true, 1, false, 0.0 });

            migrationBuilder.InsertData(
                table: "HomeBanners",
                columns: new[] { "Id", "AltText", "Image", "Order", "TargetLink", "Title" },
                values: new object[,]
                {
                    { 1, null, null, 0, null, null },
                    { 2, null, null, 1, null, null },
                    { 3, null, null, 2, null, null },
                    { 4, null, null, 3, null, null },
                    { 5, null, null, 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "HomePage",
                columns: new[] { "Id", "Description", "FooterDescription", "Has24Support", "HasFastDeliveryOption", "HasLocalPaymentOption", "HasOriginalWarranty", "InstagramAPI", "MetaDescription", "RedirectURL", "ShowHomeBanners", "SubTitle", "Title", "TitlePage" },
                values: new object[] { 1, null, null, true, true, true, true, null, null, null, false, null, null, null });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "Id", "IsPublished", "LinkText", "MainText" },
                values: new object[] { 1, false, null, null });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "DeliveryId", "StateName" },
                values: new object[,]
                {
                    { 26, 1, "MERU" },
                    { 27, 1, "MIGORI" },
                    { 28, 1, "MOMBASA" },
                    { 29, 1, "MURANG'A" },
                    { 30, 1, "NAIROBI" },
                    { 31, 1, "NAKURU" },
                    { 32, 1, "NANDI" },
                    { 33, 1, "NAROK" },
                    { 34, 1, "NYAMIRA" },
                    { 35, 1, "NYANDARUA" },
                    { 41, 1, "THARAKA-NTHI" },
                    { 37, 1, "SAMBURU" },
                    { 38, 1, "SIAYA" },
                    { 39, 1, "TITA TAVETA" },
                    { 40, 1, "TANA RIVER" },
                    { 25, 1, "MARSABIT" },
                    { 42, 1, "TRANS ANZOIA" },
                    { 43, 1, "TURKANA" },
                    { 44, 1, "UASIN GISHU" },
                    { 45, 1, "VIHIGA" },
                    { 46, 1, "WAJIR" },
                    { 36, 1, "NYERI" },
                    { 24, 1, "MANDERA" },
                    { 19, 1, "KWALE" },
                    { 22, 1, "MACHAKOS" },
                    { 1, 1, "BARINGO" },
                    { 2, 1, "BOMET" },
                    { 3, 1, "BUNGOMA" },
                    { 4, 1, "BUSIA" },
                    { 5, 1, "ELEGEYO-MARAKWET" },
                    { 6, 1, "EMBU" },
                    { 7, 1, "GARISSA" },
                    { 8, 1, "HOMA BAY" },
                    { 9, 1, "ISIOLO" },
                    { 10, 1, "KAJIADO" },
                    { 11, 1, "KAKAMEGA" },
                    { 12, 1, "KERICHO" },
                    { 13, 1, "KIAMBU" },
                    { 14, 1, "KILIFI" },
                    { 15, 1, "KIRINYAGA" },
                    { 16, 1, "KISII" },
                    { 17, 1, "KISUMU" },
                    { 18, 1, "KITUI" },
                    { 47, 1, "WEST POKOT" },
                    { 20, 1, "LAIKIPIA" },
                    { 21, 1, "LAMU" },
                    { 23, 1, "MAKUENI" }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "Content", "MetaDescription", "RedirectURL", "TitlePage" },
                values: new object[] { 1, "Terms of world wide e-commerce", null, null, null });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "DeliveryId", "DeliveryPrice", "IsSetDeliveryPrice", "StateId", "StateName", "Title" },
                values: new object[,]
                {
                    { 1, 1, -1.0, false, 1, "BARINGO", "TIATY" },
                    { 197, 1, -1.0, false, 30, "NAIROBI", "MAKADARA" },
                    { 196, 1, -1.0, false, 30, "NAIROBI", "EMBAKASI WEST" },
                    { 195, 1, -1.0, false, 30, "NAIROBI", "EMBAKASI EAST" },
                    { 194, 1, -1.0, false, 30, "NAIROBI", "EMBAKASI CENTRAL" },
                    { 193, 1, -1.0, false, 30, "NAIROBI", "EMBAKASI NORTH" },
                    { 192, 1, -1.0, false, 30, "NAIROBI", "EMBAKASI SOUTH" },
                    { 198, 1, -1.0, false, 30, "NAIROBI", "KAMUKUNJI" },
                    { 191, 1, -1.0, false, 30, "NAIROBI", "RUARAKA" },
                    { 189, 1, -1.0, false, 30, "NAIROBI", "ROYSAMBU" },
                    { 188, 1, -1.0, false, 30, "NAIROBI", "KIBRA" },
                    { 187, 1, -1.0, false, 30, "NAIROBI", "LANGATA" },
                    { 186, 1, -1.0, false, 30, "NAIROBI", "DAGORETTI SOUTH" },
                    { 185, 1, -1.0, false, 30, "NAIROBI", "DAGORETTI NORTH" },
                    { 184, 1, -1.0, false, 30, "NAIROBI", "WESTLANDS" },
                    { 190, 1, -1.0, false, 30, "NAIROBI", "KASARANI" },
                    { 183, 1, -1.0, false, 29, "MURANG'A", "GATANGA" },
                    { 199, 1, -1.0, false, 30, "NAIROBI", "STAREHE" },
                    { 201, 1, -1.0, false, 31, "NAKURU", "MOLO" },
                    { 215, 1, -1.0, false, 32, "NANDI", "CHESUMEI" },
                    { 214, 1, -1.0, false, 32, "NANDI", "NANDI HILLS" },
                    { 213, 1, -1.0, false, 32, "NANDI", "ALDAI" },
                    { 212, 1, -1.0, false, 32, "NANDI", "TINDERET" },
                    { 211, 1, -1.0, false, 31, "NAKURU", "NAKURU TOWN EAST" },
                    { 210, 1, -1.0, false, 31, "NAKURU", "NAKURU TOWN WEST" },
                    { 200, 1, -1.0, false, 30, "NAIROBI", "MATHARE" },
                    { 209, 1, -1.0, false, 31, "NAKURU", "BAHATI" },
                    { 207, 1, -1.0, false, 31, "NAKURU", "SUBUKIA" },
                    { 206, 1, -1.0, false, 31, "NAKURU", "KURESOI NORTH" },
                    { 205, 1, -1.0, false, 31, "NAKURU", "KURESOI SOUTH" },
                    { 204, 1, -1.0, false, 31, "NAKURU", "GILGIL" },
                    { 203, 1, -1.0, false, 31, "NAKURU", "NAIVASHA" },
                    { 202, 1, -1.0, false, 31, "NAKURU", "NJORO" },
                    { 208, 1, -1.0, false, 31, "NAKURU", "RONGAI" },
                    { 216, 1, -1.0, false, 32, "NANDI", "EMGWEN" },
                    { 182, 1, -1.0, false, 29, "MURANG'A", "KANDARA" },
                    { 180, 1, -1.0, false, 29, "MURANG'A", "KIGUMO" },
                    { 161, 1, -1.0, false, 26, "MERU", "CENTRAL IMENTI" },
                    { 160, 1, -1.0, false, 26, "MERU", "BUURI" },
                    { 159, 1, -1.0, false, 26, "MERU", "NORTH IMENTI" },
                    { 158, 1, -1.0, false, 26, "MERU", "TIGANIA EAST" },
                    { 157, 1, -1.0, false, 26, "MERU", "TIGANIA WEST" },
                    { 156, 1, -1.0, false, 26, "MERU", "IGEMBE NORTH" },
                    { 162, 1, -1.0, false, 26, "MERU", "SOUTH IMENTI" },
                    { 155, 1, -1.0, false, 26, "MERU", "IGEMBE CENTRAL" },
                    { 153, 1, -1.0, false, 25, "MARSABIT", "LAISAMIS" },
                    { 152, 1, -1.0, false, 25, "MARSABIT", "SAKU" },
                    { 151, 1, -1.0, false, 25, "MARSABIT", "NORTH HORR" },
                    { 150, 1, -1.0, false, 25, "MARSABIT", "MOYALE" },
                    { 149, 1, -1.0, false, 24, "MANDERA", "LAFEY" },
                    { 148, 1, -1.0, false, 24, "MANDERA", "MANDERA EAST" },
                    { 154, 1, -1.0, false, 26, "MERU", "IGEMBE SOUTH" },
                    { 181, 1, -1.0, false, 29, "MURANG'A", "MARAGWA" },
                    { 163, 1, -1.0, false, 27, "MIGORI", "RONGO" },
                    { 165, 1, -1.0, false, 27, "MIGORI", "SUNA EAST" },
                    { 179, 1, -1.0, false, 29, "MURANG'A", "KIHARU" },
                    { 178, 1, -1.0, false, 29, "MURANG'A", "MATHIOYA" },
                    { 177, 1, -1.0, false, 29, "MURANG'A", "KANGEMA" },
                    { 176, 1, -1.0, false, 28, "MOMBASA", "MVITA" },
                    { 175, 1, -1.0, false, 28, "MOMBASA", "LIKONI" },
                    { 174, 1, -1.0, false, 28, "MOMBASA", "NYALI" },
                    { 164, 1, -1.0, false, 27, "MIGORI", "AWENDO" },
                    { 173, 1, -1.0, false, 28, "MOMBASA", "KISAUNI" },
                    { 171, 1, -1.0, false, 28, "MOMBASA", "CHANGAMWE" },
                    { 170, 1, -1.0, false, 27, "MIGORI", "KURIA EAST" },
                    { 169, 1, -1.0, false, 27, "MIGORI", "KURIA WEST" },
                    { 168, 1, -1.0, false, 27, "MIGORI", "NYATIKE" },
                    { 167, 1, -1.0, false, 27, "MIGORI", "URIRI" },
                    { 166, 1, -1.0, false, 27, "MIGORI", "SUNA WEST" },
                    { 172, 1, -1.0, false, 28, "MOMBASA", "JOMVU" },
                    { 217, 1, -1.0, false, 32, "NANDI", "MOSOP" },
                    { 218, 1, -1.0, false, 33, "NAROK", "KILGORIS" },
                    { 219, 1, -1.0, false, 33, "NAROK", "EMURUA DIKIRR" },
                    { 269, 1, -1.0, false, 44, "UASIN GISHU", "TURBO" },
                    { 268, 1, -1.0, false, 44, "UASIN GISHU", "SOY" },
                    { 267, 1, -1.0, false, 43, "TURKANA", "TURKANA EAST" },
                    { 266, 1, -1.0, false, 43, "TURKANA", "TURKANA SOUTH" },
                    { 265, 1, -1.0, false, 43, "TURKANA", "LOIMA" },
                    { 264, 1, -1.0, false, 43, "TURKANA", "TURKANA CENTRAL" },
                    { 270, 1, -1.0, false, 44, "UASIN GISHU", "MOIBEN" },
                    { 263, 1, -1.0, false, 43, "TURKANA", "TURKANA WEST" },
                    { 261, 1, -1.0, false, 42, "TRANS NZOIA", "CHERANGANY" },
                    { 260, 1, -1.0, false, 42, "TRANS NZOIA", "KIMININI" },
                    { 259, 1, -1.0, false, 42, "TRANS NZOIA", "SABOTI" },
                    { 258, 1, -1.0, false, 42, "TRANS NZOIA", "ENDEBESS" },
                    { 257, 1, -1.0, false, 42, "TRANS NZOIA", "KWANZA" },
                    { 256, 1, -1.0, false, 41, "THARAKA-NITHI", "THARAKA" },
                    { 262, 1, -1.0, false, 43, "TURKANA", "TURKANA NORTH" },
                    { 255, 1, -1.0, false, 41, "THARAKA-NITHI", "CHUKA/IGAMBANG'OMBE" },
                    { 271, 1, -1.0, false, 44, "UASIN GISHU", "AINABKOI" },
                    { 273, 1, -1.0, false, 44, "UASIN GISHU", "KESSES" },
                    { 287, 1, -1.0, false, 46, "WEST POKOT", "KACHELIBA" },
                    { 286, 1, -1.0, false, 46, "WEST POKOT", "SIGOR" },
                    { 285, 1, -1.0, false, 46, "WEST POKOT", "KAPENGURIA" },
                    { 284, 1, -1.0, false, 46, "WAJIR", "WAJIR SOUTH" },
                    { 283, 1, -1.0, false, 46, "WAJIR", "ELDAS" },
                    { 282, 1, -1.0, false, 46, "WAJIR", "WAJIR WEST" },
                    { 272, 1, -1.0, false, 44, "UASIN GISHU", "KAPSERET" },
                    { 281, 1, -1.0, false, 46, "WAJIR", "TARBAJ" },
                    { 279, 1, -1.0, false, 46, "WAJIR", "WAJIR NORTH" },
                    { 278, 1, -1.0, false, 45, "VIHIGA", "EMUHAYA" },
                    { 277, 1, -1.0, false, 45, "VIHIGA", "LUANDA" },
                    { 276, 1, -1.0, false, 45, "VIHIGA", "HAMISI" },
                    { 275, 1, -1.0, false, 45, "VIHIGA", "SABATIA" },
                    { 274, 1, -1.0, false, 45, "VIHIGA", "VIHIGA" },
                    { 280, 1, -1.0, false, 46, "WAJIR", "WAJIR EAST" },
                    { 254, 1, -1.0, false, 41, "THARAKA-NITHI", "MAARA" },
                    { 253, 1, -1.0, false, 40, "TANA RIVER", "BURA" },
                    { 252, 1, -1.0, false, 40, "TANA RIVER", "GALOLE" },
                    { 233, 1, -1.0, false, 36, "NYERI", "TETU" },
                    { 232, 1, -1.0, false, 35, "NYANDARUA", "NDARAGWA" },
                    { 231, 1, -1.0, false, 35, "NYANDARUA", "OL JOROK" },
                    { 230, 1, -1.0, false, 35, "NYANDARUA", "OL KALOU" },
                    { 229, 1, -1.0, false, 35, "NYANDARUA", "KIPIPIRI" },
                    { 228, 1, -1.0, false, 35, "NYANDARUA", "KINANGOP" },
                    { 234, 1, -1.0, false, 36, "NYERI", "KIENI" },
                    { 227, 1, -1.0, false, 34, "NYAMIRA", "BORABU" },
                    { 225, 1, -1.0, false, 34, "NYAMIRA", "WEST MUGIRANGO" },
                    { 224, 1, -1.0, false, 34, "NYAMIRA", "KITUTU MASABA" },
                    { 223, 1, -1.0, false, 33, "NAROK", "NAROK WEST" },
                    { 222, 1, -1.0, false, 33, "NAROK", "NAROK SOUTH" },
                    { 221, 1, -1.0, false, 33, "NAROK", "NAROK EAST" },
                    { 220, 1, -1.0, false, 33, "NAROK", "NAROK NORTH" },
                    { 226, 1, -1.0, false, 34, "NYAMIRA", "NORTH MUGIRANGO" },
                    { 235, 1, -1.0, false, 36, "NYERI", "MATHIRA" },
                    { 236, 1, -1.0, false, 36, "NYERI", "OTHAYA" },
                    { 237, 1, -1.0, false, 36, "NYERI", "MUKURWEINI" },
                    { 290, 1, -1.0, false, 40, "TANA RIVER", "GARSEN" },
                    { 251, 1, -1.0, false, 39, "TAITA TAVETA", "VOI" },
                    { 250, 1, -1.0, false, 39, "TAITA TAVETA", "MWATATE" },
                    { 249, 1, -1.0, false, 39, "TAITA TAVETA", "WUNDANYI" },
                    { 248, 1, -1.0, false, 39, "TAITA TAVETA", "TAVETA" },
                    { 247, 1, -1.0, false, 38, "SIAYA", "RARIEDA" },
                    { 246, 1, -1.0, false, 38, "SIAYA", "BONDO" },
                    { 245, 1, -1.0, false, 38, "SIAYA", "GEM" },
                    { 244, 1, -1.0, false, 38, "SIAYA", "ALEGO USONGA" },
                    { 243, 1, -1.0, false, 38, "SIAYA", "UGUNJA" },
                    { 242, 1, -1.0, false, 38, "SIAYA", "UGENYA" },
                    { 241, 1, -1.0, false, 37, "SAMBURU", "SAMBURU EAST" },
                    { 240, 1, -1.0, false, 37, "SAMBURU", "SAMBURU NORTH" },
                    { 239, 1, -1.0, false, 37, "SAMBURU", "SAMBURU WEST" },
                    { 238, 1, -1.0, false, 36, "NYERI", "NYERI TOWN" },
                    { 147, 1, -1.0, false, 24, "MANDERA", "MANDERA SOUTH" },
                    { 146, 1, -1.0, false, 24, "MANDERA", "MANDERA NORTH" },
                    { 145, 1, -1.0, false, 24, "MANDERA", "BANISSA" },
                    { 144, 1, -1.0, false, 24, "MANDERA", "MANDERA WEST" },
                    { 52, 1, -1.0, false, 10, "KAJIADO", "KAJIADO CENTRAL" },
                    { 51, 1, -1.0, false, 10, "KAJIADO", "KAJIADO NORTH" },
                    { 50, 1, -1.0, false, 9, "ISIOLO", "ISIOLO SOUTH" },
                    { 49, 1, -1.0, false, 9, "ISIOLO", "ISIOLO NORHT" },
                    { 48, 1, -1.0, false, 8, "HOME BAY", "SUBA" },
                    { 47, 1, -1.0, false, 8, "HOME BAY", "MBITA" },
                    { 53, 1, -1.0, false, 10, "KAJIADO", "KAJIADO EAST" },
                    { 46, 1, -1.0, false, 8, "HOME BAY", "NDHIWA" },
                    { 44, 1, -1.0, false, 8, "HOME BAY", "RANGWE" },
                    { 43, 1, -1.0, false, 8, "HOME BAY", "KARACHUNOYO" },
                    { 42, 1, -1.0, false, 8, "HOME BAY", "KABONDO KASIPUL" },
                    { 41, 1, -1.0, false, 8, "HOME BAY", "KASIPUL" },
                    { 40, 1, -1.0, false, 7, "GARISSA", "IJARA" },
                    { 39, 1, -1.0, false, 7, "GARISSA", "FAFI" },
                    { 45, 1, -1.0, false, 8, "HOME BAY", "HOMA BAY TOWN" },
                    { 38, 1, -1.0, false, 7, "GARISSA", "DADAAB" },
                    { 54, 1, -1.0, false, 10, "KAJIADO", "KAJIADO WEST" },
                    { 56, 1, -1.0, false, 11, "KAKAMEGA", "LUGARI" },
                    { 70, 1, -1.0, false, 12, "KERICHO", "AINAMOI" },
                    { 69, 1, -1.0, false, 12, "KERICHO", "KIPKELION WEST" },
                    { 68, 1, -1.0, false, 12, "KERICHO", "KIPKELION EAST" },
                    { 67, 1, -1.0, false, 11, "KAKAMEGA", "IKOLOMANI" },
                    { 66, 1, -1.0, false, 11, "KAKAMEGA", "SHINYALU" },
                    { 65, 1, -1.0, false, 11, "KAKAMEGA", "KHWISERO" },
                    { 55, 1, -1.0, false, 10, "KAJIADO", "KAJIADO SOUTH" },
                    { 64, 1, -1.0, false, 11, "KAKAMEGA", "BUTERE" },
                    { 62, 1, -1.0, false, 11, "KAKAMEGA", "MUMIAS EAST" },
                    { 61, 1, -1.0, false, 11, "KAKAMEGA", "MUMIAS WEST" },
                    { 60, 1, -1.0, false, 11, "KAKAMEGA", "NAVAKHOLO" },
                    { 59, 1, -1.0, false, 11, "KAKAMEGA", "LURAMBI" },
                    { 58, 1, -1.0, false, 11, "KAKAMEGA", "MALAVA" },
                    { 57, 1, -1.0, false, 11, "KAKAMEGA", "LIKUYANI" },
                    { 63, 1, -1.0, false, 11, "KAKAMEGA", "MATUNGU" },
                    { 37, 1, -1.0, false, 7, "GARISSA", "LAGDERA" },
                    { 36, 1, -1.0, false, 7, "GARISSA", "BALAMBALA" },
                    { 35, 1, -1.0, false, 7, "GARISSA", "GARISSA TOWNSHIP" },
                    { 15, 1, -1.0, false, 3, "BUNGOMA", "BUMULA" },
                    { 14, 1, -1.0, false, 3, "BUNGOMA", "KABUCHAI" },
                    { 13, 1, -1.0, false, 3, "BUNGOMA", "SIRISIA" },
                    { 12, 1, -1.0, false, 3, "BUNGOMA", "MT. ELGON" },
                    { 11, 1, -1.0, false, 2, "BOMET", "KONOIN" },
                    { 10, 1, -1.0, false, 2, "BOMET", "BOMET CENTRAL" },
                    { 16, 1, -1.0, false, 3, "BUNGOMA", "KANDUYI" },
                    { 9, 1, -1.0, false, 2, "BOMET", "BOMET EAST" },
                    { 7, 1, -1.0, false, 2, "BOMET", "SOTIK" },
                    { 6, 1, -1.0, false, 1, "BARINGO", "ELDAMA RAVINE" },
                    { 5, 1, -1.0, false, 1, "BARINGO", "MOGOTIO" },
                    { 4, 1, -1.0, false, 1, "BARINGO", "BARINGO SOUTH" },
                    { 3, 1, -1.0, false, 1, "BARINGO", "BARINGO CENTRAL" },
                    { 2, 1, -1.0, false, 1, "BARINGO", "BARINGO NORTH" },
                    { 8, 1, -1.0, false, 2, "BOMET", "CHEPALUNGU" },
                    { 17, 1, -1.0, false, 3, "BUNGOMA", "WEBUYE EAST" },
                    { 18, 1, -1.0, false, 3, "BUNGOMA", "WEBUYE WEST" },
                    { 19, 1, -1.0, false, 3, "BUNGOMA", "KIMILILI" },
                    { 34, 1, -1.0, false, 6, "EMBU", "MBEERE NORTH" },
                    { 33, 1, -1.0, false, 6, "EMBU", "MBEERE SOUTH" },
                    { 32, 1, -1.0, false, 6, "EMBU", "RUNYENJES" },
                    { 31, 1, -1.0, false, 6, "EMBU", "MANYATTA" },
                    { 30, 1, -1.0, false, 5, "ELEGEYO-MARKAWET", "KEIYO NORTH" },
                    { 29, 1, -1.0, false, 5, "ELEGEYO-MARKAWET", "KEIYO SOUTH" },
                    { 28, 1, -1.0, false, 5, "ELEGEYO-MARKAWET", "MARKAWET EAST" },
                    { 27, 1, -1.0, false, 4, "BUSIA", "BUDALANGI" },
                    { 26, 1, -1.0, false, 4, "BUSIA", "FUNYULA" },
                    { 25, 1, -1.0, false, 4, "BUSIA", "BUTULA" },
                    { 24, 1, -1.0, false, 4, "BUSIA", "MATAYOS" },
                    { 23, 1, -1.0, false, 4, "BUSIA", "NAMBALE" },
                    { 22, 1, -1.0, false, 4, "BUSIA", "TESO SOUTH" },
                    { 21, 1, -1.0, false, 4, "BUSIA", "TESO NORTH" },
                    { 20, 1, -1.0, false, 3, "BUNGOMA", "TONGAREN" },
                    { 71, 1, -1.0, false, 12, "KERICHO", "BURETI" },
                    { 288, 1, -1.0, false, 46, "WEST POKOT", "POKOT SOUTH" },
                    { 72, 1, -1.0, false, 12, "KERICHO", "BELGUT" },
                    { 74, 1, -1.0, false, 13, "KIAMBU", "GATUNDU SOUTH" },
                    { 125, 1, -1.0, false, 20, "LAIKIPIA", "LAIKIPIA WEST" },
                    { 124, 1, -1.0, false, 19, "KWALE", "KINANGO" },
                    { 123, 1, -1.0, false, 19, "KWALE", "MATUGA" },
                    { 122, 1, -1.0, false, 19, "KWALE", "LUNGA LUNGA" },
                    { 121, 1, -1.0, false, 19, "KWALE", "MSAMBWENI" },
                    { 120, 1, -1.0, false, 18, "KITUI", "KITUI SOUTH" },
                    { 126, 1, -1.0, false, 20, "LAIKIPIA", "LAIKIPIA EAST" },
                    { 119, 1, -1.0, false, 18, "KITUI", "KITUI EAST" },
                    { 117, 1, -1.0, false, 18, "KITUI", "KITUI RURAL" },
                    { 116, 1, -1.0, false, 18, "KITUI", "KITUI WEST" },
                    { 115, 1, -1.0, false, 18, "KITUI", "MWINGI CENTRAL" },
                    { 114, 1, -1.0, false, 18, "KITUI", "MWINGI WEST" },
                    { 113, 1, -1.0, false, 18, "KITUI", "MWINGI NORTH" },
                    { 112, 1, -1.0, false, 17, "KISUMU", "NYAKACH" },
                    { 118, 1, -1.0, false, 18, "KITUI", "KITUI CENTRAL" },
                    { 111, 1, -1.0, false, 17, "KISUMU", "MUHORONI" },
                    { 127, 1, -1.0, false, 20, "LAIKIPIA", "LAIKIPIA NORTH" },
                    { 129, 1, -1.0, false, 21, "LAMU", "LAMU WEST" },
                    { 143, 1, -1.0, false, 23, "MAKUENI", "KIBWEZI EAST" },
                    { 142, 1, -1.0, false, 23, "MAKUENI", "KIBWEZI WEST" },
                    { 141, 1, -1.0, false, 23, "MAKUENI", "MAKUENI" },
                    { 140, 1, -1.0, false, 23, "MAKUENI", "KAITI" },
                    { 139, 1, -1.0, false, 23, "MAKUENI", "KILOME" },
                    { 138, 1, -1.0, false, 23, "MAKUENI", "MBOONI" },
                    { 128, 1, -1.0, false, 21, "LAMU", "LAMU EAST" },
                    { 137, 1, -1.0, false, 22, "MACHAKOS", "MWALA" },
                    { 135, 1, -1.0, false, 22, "MACHAKOS", "MAVOKO" },
                    { 134, 1, -1.0, false, 22, "MACHAKOS", "KATHIANI" },
                    { 133, 1, -1.0, false, 22, "MACHAKOS", "MATUNGULU" },
                    { 132, 1, -1.0, false, 22, "MACHAKOS", "KANGUNDO" },
                    { 131, 1, -1.0, false, 22, "MACHAKOS", "YATTA" },
                    { 130, 1, -1.0, false, 22, "MACHAKOS", "MASINGA" },
                    { 136, 1, -1.0, false, 22, "MACHAKOS", "MACHAKOS TOWN" },
                    { 110, 1, -1.0, false, 17, "KISUMU", "NYANDO" },
                    { 109, 1, -1.0, false, 17, "KISUMU", "SEME" },
                    { 108, 1, -1.0, false, 17, "KISUMU", "KISUMU CENTRAL" },
                    { 88, 1, -1.0, false, 14, "KILIFI", "KALOLENI" },
                    { 87, 1, -1.0, false, 14, "KILIFI", "KILIFI SOUTH" },
                    { 86, 1, -1.0, false, 14, "KILIFI", "KILIFI NORTH" },
                    { 85, 1, -1.0, false, 13, "KIAMBU", "LARI" },
                    { 84, 1, -1.0, false, 13, "KIAMBU", "LIMURU" },
                    { 83, 1, -1.0, false, 13, "KIAMBU", "KIKUYU" },
                    { 89, 1, -1.0, false, 14, "KILIFI", "RABAI" },
                    { 82, 1, -1.0, false, 13, "KIAMBU", "KABETE" },
                    { 80, 1, -1.0, false, 13, "KIAMBU", "KIAMBU" },
                    { 79, 1, -1.0, false, 13, "KIAMBU", "GITHUNGURI" },
                    { 78, 1, -1.0, false, 13, "KIAMBU", "RUIRU" },
                    { 77, 1, -1.0, false, 13, "KIAMBU", "THIKA TOWN" },
                    { 76, 1, -1.0, false, 13, "KIAMBU", "JUJA" },
                    { 75, 1, -1.0, false, 13, "KIAMBU", "GATUNDU NORTH" },
                    { 81, 1, -1.0, false, 13, "KIAMBU", "KIAMBAA" },
                    { 90, 1, -1.0, false, 14, "KILIFI", "GANZE" },
                    { 91, 1, -1.0, false, 14, "KILIFI", "MALINDI" },
                    { 92, 1, -1.0, false, 14, "KILIFI", "MAGARINI" },
                    { 107, 1, -1.0, false, 17, "KISUMU", "KISUMU WEST" },
                    { 106, 1, -1.0, false, 17, "KISUMU", "KISUMU EAST" },
                    { 105, 1, -1.0, false, 16, "KISII", "KITUTU CHACHE SOUTH" },
                    { 104, 1, -1.0, false, 16, "KISII", "KITUTU CHACHE NORTH" },
                    { 103, 1, -1.0, false, 16, "KISII", "NYARIBARI CHACHE" },
                    { 102, 1, -1.0, false, 16, "KISII", "NYARIBARI MASABA" },
                    { 101, 1, -1.0, false, 16, "KISII", "BOMACHOGE CHACHE" },
                    { 100, 1, -1.0, false, 16, "KISII", "BOBASI" },
                    { 99, 1, -1.0, false, 16, "KISII", "BOMACHOGE BORABU" },
                    { 98, 1, -1.0, false, 16, "KISII", "SOUTH MUGIRANGO" },
                    { 97, 1, -1.0, false, 16, "KISII", "BONCHARI" },
                    { 96, 1, -1.0, false, 15, "KIRINYAGA", "KIRINYAGA CENTRAL" },
                    { 95, 1, -1.0, false, 15, "KIRINYAGA", "NDIA" },
                    { 94, 1, -1.0, false, 15, "KIRINYAGA", "GICHUGU" },
                    { 93, 1, -1.0, false, 15, "KIRINYAGA", "MWEA" },
                    { 73, 1, -1.0, false, 12, "KERICHO", "SIGOWET/SOIN" },
                    { 289, 1, -1.0, false, 46, "WEST POKOT", "MARAKWET WEST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsContents_AboutId",
                table: "AboutUsContents",
                column: "AboutId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_MyUserId",
                table: "Addresses",
                column: "MyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BlogComments_BlogId",
                table: "BlogComments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogContents_BlogId",
                table: "BlogContents",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogReplyComments_BlogCommentId",
                table: "BlogReplyComments",
                column: "BlogCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCategoryId",
                table: "Blogs",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductId",
                table: "Comments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritProduct_ProductId",
                table: "FavoritProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSliderImages_HomePageId",
                table: "HomeSliderImages",
                column: "HomePageId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileHomeSliderImages_HomePageId",
                table: "MobileHomeSliderImages",
                column: "HomePageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_ProductId",
                table: "ProductTags",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTechnicalContents_ProductId",
                table: "ProductTechnicalContents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_ProductId",
                table: "ProductVariations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedCategories_ProductId",
                table: "RelatedCategories",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyComments_BlogCommentId",
                table: "ReplyComments",
                column: "BlogCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyComments_CommentId",
                table: "ReplyComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyTickets_TicketId",
                table: "ReplyTickets",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDiscount_ProductId",
                table: "SpecialDiscount",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubProductVariations_ProductVariationId",
                table: "SubProductVariations",
                column: "ProductVariationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutUsContents");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlogContents");

            migrationBuilder.DropTable(
                name: "BlogPageBanners");

            migrationBuilder.DropTable(
                name: "BlogReplyComments");

            migrationBuilder.DropTable(
                name: "BlogTags");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "FavoritProduct");

            migrationBuilder.DropTable(
                name: "HomeBanners");

            migrationBuilder.DropTable(
                name: "HomeSliderImages");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "MobileHomeSliderImages");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductOptions");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "ProductTechnicalContents");

            migrationBuilder.DropTable(
                name: "RelatedCategories");

            migrationBuilder.DropTable(
                name: "ReplyComments");

            migrationBuilder.DropTable(
                name: "ReplyTickets");

            migrationBuilder.DropTable(
                name: "SEOSettings");

            migrationBuilder.DropTable(
                name: "Shoppings");

            migrationBuilder.DropTable(
                name: "SpecialDiscount");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "SubProductVariations");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "HomePage");

            migrationBuilder.DropTable(
                name: "BlogComments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "ProductVariations");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "BlogCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
