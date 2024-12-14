using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WebShop.Services;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 40))));

//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddDefaultIdentity<IdentityUser>( options =>
{
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppIdentityDbContext>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
//builder.Services.AddIdentityCore<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<AppIdentityDbContext>()
//    .AddDefaultUI();

//builder.Services.AddSignInManager<SignInManager<IdentityUser>>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppIdentityDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 40))));
//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddTransient<IUserService, IdentityUserService>();

builder.Services.AddMvc(mvcOtions => mvcOtions.EnableEndpointRouting = false);
//builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Cookies")
.AddCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
    options.ExpireTimeSpan = new TimeSpan(1, 0, 0);
});
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings 
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
//    options.LoginPath = "/Home/Login"; // If the LoginPath is not set here, 
//                                          // ASP.NET Core will default to /Account/Login 
//    options.LogoutPath = "/Home/Logout"; // If the LogoutPath is not set here, 
//                                            // ASP.NET Core will default to /Account/Logout 
//    options.AccessDeniedPath = "/Home/AccessDenied"; // If the AccessDeniedPath is 
//                                                        // not set here, ASP.NET Core  
//                                                        // will default to  
//                                                        // /Account/AccessDenied 
//    options.SlidingExpiration = true;
//});

builder.Services.Configure<RazorViewEngineOptions>(options => {
    options.ViewLocationFormats.Add("/Views/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Master/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Views/Common/{0}" + RazorViewEngine.ViewExtension);
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

//app.UseHttpsRedirection();
//app.UseRouting();


//app.MapIdentityApi<IdentityUser>();

app.UseAuthorization();
app.UseAuthentication();
app.UseStaticFiles();

// добавление компонентов mvc и определение маршрута
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

//app.UseMvc();
//app.UseRouting();
//app.MapRazorPages();

app.Run();
