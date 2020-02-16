using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApplication.Areas.Login.ViewModels;
using WebApplication.Areas.UsersAdmining.VMModels;

namespace WebApplication.Areas.Login.Controllers
{
    [Area("Login")]
    public class LoginController : Controller
    {
        public MyShopDB DB { get; set; }

        public LoginController(MyShopDB DB)
        {
            this.DB = DB;
        }


        [HttpGet]
        public IActionResult Login()
        {
            if(DB.User.ToArray().Length==0)
            {
                var route = Url.RouteUrl("home", null, "http");
                return Redirect(route);
            }
            return View(new VMLogin());
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(VMLogin login)
        {
            if (ModelState.IsValid == false)
            {
                return View(login);
            }
            else
            {
                var hashPassword =
                    Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(login.Password)));

                var UserInDB = DB.User.Where(w => w.Email == login.EMail && w.Password == hashPassword).FirstOrDefault();

                if(UserInDB is null)
                {
                    var route = Url.RouteUrl("loginincorect", null, "http");
                    HttpContext.Response.Headers.Add("REFRESH", $"5;{route}");

                    return Content("User not found");
                }

                UserInDB.HasChanged = false;
                DB.SaveChanges();

                var UserToAuthentication = new VMUserToAuthentication
                {
                    ID = UserInDB.Id,
                    EMail = UserInDB.Email,
                    FName = UserInDB.Fname,
                    LName = UserInDB.Lname
                };

                UserToAuthentication.Roles = (from roles in DB.UserRoles
                                     join roleNames in DB.Role
                                     on roles.RoleId equals roleNames.Id
                                     where roles.UserId == UserInDB.Id
                                     select roleNames.RoleName).ToList();

                await Authenticate(UserToAuthentication);

                var Route = Url.RouteUrl("home", null, "http");
                return Redirect(Route);
            }
        }

        [NonAction]
        private async Task Authenticate(VMUserToAuthentication userToAuthentication)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userToAuthentication.FName),
                new Claim(ClaimTypes.Name, userToAuthentication.FName),
                new Claim(ClaimTypes.Surname, userToAuthentication.LName),
                new Claim(ClaimTypes.Email, userToAuthentication.EMail),
            };

            foreach (var i in userToAuthentication.Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, i));
            }

            ClaimsIdentity ID = new ClaimsIdentity(
                Claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ID));
        }

        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var Route = Url.RouteUrl("home", null, "http");
            return Redirect(Route);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {

            HttpContext.Response.Headers.Add("REFRESH", "5;/logout");

            return Content($"AccessDenied: {(ReturnUrl is null ? "" : ReturnUrl)}");
        }
    }
}