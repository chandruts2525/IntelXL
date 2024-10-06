using IntelXL.HttpHandler;

using IntelXLAdmin.Web.Models;

using Microsoft.AspNetCore.Authentication.Cookies;

using NLog;
using NLog.Web;



var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

//var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<IHttpHandler, HttpHandler>();
builder.Logging.ClearProviders();
//builder.Services.AddSession(options => {
//    options.IdleTimeout = TimeSpan.FromMinutes(60);
//});
builder.Services.AddAuthentication(
          CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(option =>
          {
              //option.Cookie.Name = "_user";
              option.LoginPath = "/Users/Login";
              option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
              //option.ExpireTimeSpan = TimeSpan.Zero;
              option.SlidingExpiration = true;
          }); 
builder.Host.UseNLog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}/{id?}");

app.Run();
LogManager.Shutdown();

