using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebStore.Clients.Employees;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Services.Product;
using WebStore.Logging;
using Microsoft.Extensions.Logging;
using WebStore.Hubs;
using WebStore.Infrastructure.Middleware;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration Config) => Configuration = Config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            
            services.AddScoped<IValuesService, ValuesClient>();

            services.AddSingleton<IEmployeesData, EmployeesClient>(); 
            //services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddScoped<IProductData, ProductsClient>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartStore, CookiesCartStore>();
            services.AddScoped<IOrderService, OrdersClient>();

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders();

            #region Custom Identity implementation

            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserRoleStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserLockoutStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<Role>, RolesClient>();

            #endregion

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "Webshop-Identity";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Expiration = TimeSpan.FromDays(150);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenided";

                opt.SlidingExpiration = true;
            });



            services.AddSession();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();
            app.UseDefaultFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            //app.UseMiddleware(typeof(ErrorHandlingMiddleware)); alternative way to register middleware
            //app.UseMiddleware(new ErrorHandlingMiddleware(...)) alternative way to register middleware

            app.UseSession();

            app.UseSignalR(route => route.MapHub<InformationHub>("/Info"));

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
