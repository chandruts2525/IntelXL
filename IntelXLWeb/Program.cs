using IntelXL.HttpHandler;
using IntelXLWeb.Models;
using NLog.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using NLog;
using ChatCore.Hubs;
using Firebase.Storage;

namespace IntelXLWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddTransient<IHttpHandler, HttpHandler>();
            builder.Services.Configure<FireBaseStorageConfig>(builder.Configuration.GetSection("FireBaseStorageConfig"));
            builder.Services.AddAuthentication(
           CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(option =>
           {
               option.LoginPath = "/User/Login";
               option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
               option.SlidingExpiration = true;
           }).AddGoogle(options =>
           {
               options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
               options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
           });
            builder.Services.AddSignalR();
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });           
            builder.Logging.ClearProviders();
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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<ChatHub>("/chatHub");
            app.Run();
            LogManager.Shutdown();
        }
    }
}