using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.ViewModels;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        public TestDB DB { get; set; }
        public LoginController(TestDB DB)
        {
            this.DB = DB;
        }

        #region Login
        [HttpGet]
        [Route("/Login")]
        public IActionResult Login()
        {
            return View(new VMLogin());
        }

        [HttpPost]
        [Route("/Login/Login")]
        public async Task<IActionResult> Login(VMLogin login)
        {
            if(ModelState.IsValid == false)
            {
                return View(login);
            }
            else
            {

                var HashPassword = "";
                using (var pwdHash = SHA256.Create())
                {
                    HashPassword = Encoding.ASCII.GetString(pwdHash.ComputeHash(Encoding.ASCII.GetBytes(login.Password)));
                }

                var Login = DB.Users.FirstOrDefault(f => f.Email == login.EMail&&f.Password==HashPassword);
                
                if(Login is null)
                {
                    ModelState.AddModelError("LogginError", "e-mail или пароль не коректны");
                    return View(login);
                }


                var Identy = new Dictionary<string, string>();
                Identy.Add(nameof(Login.Email), Login.Email);
                Identy.Add(nameof(Login.Fname), Login.Fname);
                Identy.Add(nameof(Login.Lname), Login.Lname);
                Identy.Add(nameof(Login.Password), Login.Password);


                var Roles = (from userRoles in DB.UserRoles
                             join roles in DB.Roles
                             on userRoles.RoleId equals roles.Id
                             where userRoles.UserId == Login.Id
                             select roles.Role).ToArray();

                foreach (var i in Roles)
                {
                    Identy.Add(i, "Role");
                }

                await Identification(Identy);

                string refreshUrl = Url.Action("Index", "Home", null, "http");

                Response.Headers.Add("REFRESH", $"2;{refreshUrl}");
                return Content("Logining...");
            }
        }
        #endregion Login

        #region Registation
        [HttpGet]
        [Route("/Registation")]
        public IActionResult Gegistration()
        {
            return View(new VMAddUser());
        }


        [HttpPost]
        [Route("/Login/Registation")]
        public IActionResult Gegistration(VMAddUser user)
        {
            string refreshUrl = "";
            if (user == null)
            {
                refreshUrl = Url.RouteUrl("registaration", null, "http");
                Response.Headers.Add("REFRESH", $"4;{refreshUrl}");
                return Content("User is null");
            }

            if(ModelState.IsValid==false)
            {
                return View(user);
            }

            if(DB.Users.FirstOrDefault(f=>f.Email==user.EMail)!=null)
            {
                ModelState.AddModelError("EMail", "e-mail уже используеться");
                return View(user);
            }

            var HashPassword = "";
            using(var pwdHash = SHA256.Create())
            {
                HashPassword =  Encoding.ASCII.GetString(pwdHash.ComputeHash(Encoding.ASCII.GetBytes(user.Password)));
            }

            DB.Users.Add(new Users { Email = user.EMail, Fname = user.FName, Lname = user.LName, Password = HashPassword });
            DB.SaveChanges();

            var RoleID = DB.Roles.FirstOrDefault(f => f.Role == "Default_User").Id;
            var UserID = DB.Users.FirstOrDefault(f => f.Email == user.EMail).Id;

            DB.UserRoles.Add(new UserRoles { UserId = UserID, RoleId = RoleID });
            DB.SaveChanges();

            refreshUrl = Url.Action("ShowUsers", "Account", null, "http");
            Response.Headers.Add("REFRESH", $"4;{refreshUrl}");
            return Ok("User is add");
        }
        #endregion Registation

        [NonAction]
        private async Task Identification(Dictionary<string, string> Identy)
        {
            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, $"{Identy["Fname"]} {Identy["Lname"]}"),
                new Claim(ClaimTypes.Email, Identy["Email"]),
                new Claim("Password", Identy["Password"]),
            };


            foreach (var i in Identy)
            {
                if(i.Value=="Role")
                {
                    Claims.Add(new Claim(ClaimTypes.Role, i.Key));
                }
            }

            ClaimsIdentity ID = new ClaimsIdentity(Claims, "ApplicationCookie", ClaimTypes.Name, ClaimTypes.Role);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ID));
        }


        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");

        }
    }
}