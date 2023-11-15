using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;
using TStore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSettings);
builder.Services.AddSingleton<IEmailSender, MailService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

// Config Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/dang-nhap";
    options.AccessDeniedPath = "/khong-co-quyen-truy-cap";
});


builder.Services.Configure<IdentityOptions>(options =>
{
    // Configs Password
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    // Configs Lockout
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Config User.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    // Config Login.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

});


// Add DI
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISlideService, SlideService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();


// Connect Db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDatabase") != null ? builder.Configuration.GetConnectionString("ConnectDatabase") : "Server=.;Database=CellPhonesDb;Integrated Security=true;TrustServerCertificate=True");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
      name: "areasRoute",
      pattern: "{area:exists}/{controller=Admin}/{action=Index}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{slug?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "gioi-thieu/{slug?}",
    defaults: new { controller = "About", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "san-pham",
    defaults: new { controller = "Home", action = "Product" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "san-pham/detail/{id?}",
    defaults: new { controller = "Home", action = "Detail" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "danh-muc-san-pham",
    defaults: new { controller = "Home", action = "Category" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "danh-muc-san-pham/detail/{id?}",
    defaults: new { controller = "Home", action = "DetailCategory" }
);


app.MapControllerRoute(
    name: "default",
    pattern: "feedback",
    defaults: new { controller = "Home", action = "Feedback" }
);

using (var scope = app.Services.CreateScope())
{
    await DataSeeders.SeedRolesAndAdminAsync(scope.ServiceProvider);
}

app.Run();
