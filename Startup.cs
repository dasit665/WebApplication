using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<MyShopDB>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyShopDB"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login/Login/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Login/Login/AccessDenied");
                    options.Cookie.Name = "AuthenticationCookie";
                    options.Cookie.MaxAge = TimeSpan.FromDays(3.5);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForMailLogistic", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Email, "logistic.one@some.mail, logistic.two@some.mail");
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(def =>
            {
                def.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                def.MapControllerRoute(
                    name: "home",
                    pattern: "Home/Index");

                def.MapControllerRoute(
                    name: "login",
                    pattern: "/Login");

                def.MapControllerRoute(
                    name: "adduser",
                    pattern: "/AddUser");

                def.MapControllerRoute(
                    name: "useradmining",
                    pattern: "/UsersAdmining");

            });

            app.UseEndpoints(useradmining =>
            {
                useradmining.MapAreaControllerRoute(
                    name: "usersadminingDef",
                    areaName: "UsersAdmining",
                    pattern: "UsersAdmining/{controller=UserInfo}/{action=AllUsers}");

                useradmining.MapAreaControllerRoute(
                    name: "allusers",
                    areaName: "UsersAdmining",
                    pattern: "UsersAdmining/UserInfo/AllUsers");

                useradmining.MapAreaControllerRoute(
                    name: "showuserinfo",
                    areaName: "UsersAdmining",
                    pattern: "UsersAdmining/UserInfo/ShowUserInfo");

                useradmining.MapAreaControllerRoute(
                    name: "edituser",
                    areaName: "UsersAdmining",
                    pattern: "UsersAdmining/UserInfo/EditUser");

                useradmining.MapAreaControllerRoute(
                    name: "deleteuser",
                    areaName: "UsersAdmining",
                    pattern: "UsersAdmining/UserInfo/DeleteUser");

            });

            app.UseEndpoints(adduser =>
            {
                adduser.MapAreaControllerRoute(
                    name: "adduserDef",
                    areaName: "AddUser",
                    pattern: "AddUser/{controller=AddUser}/{action=AddUser}");

                adduser.MapAreaControllerRoute(
                    name: "adduser",
                    areaName: "AddUser",
                    pattern: "AddUser/AddUser/AddUser");
            });

            app.UseEndpoints(login =>
            {
                login.MapAreaControllerRoute(
                    name: "loginDef",
                    areaName: "Login",
                    pattern: "Login/{controller=Login}/{action=Login}");

                login.MapAreaControllerRoute(
                    name: "login",
                    areaName: "Login",
                    pattern: "Login/Login/Login");

                login.MapAreaControllerRoute(
                    name: "loginincorect",
                    areaName: "Login",
                    pattern: "Login/Login/Login");
            });
        }
    }
}