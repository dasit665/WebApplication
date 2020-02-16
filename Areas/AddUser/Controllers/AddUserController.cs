using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Area(areaName: "AddUser")]
    public class AddUserController : Controller
    {
        public MyShopDB DB { get; set; }

        public AddUserController(MyShopDB DB)
        {
            this.DB = DB;
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View(new VMAddUser());
        }

        [HttpPost]
        public IActionResult AddUser(VMAddUser userToAdd)
        {

            if (ModelState.IsValid == false)
            {
                return View(userToAdd);
            }
            else
            {
                var hashPassword = Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(userToAdd.Password)));

                bool FirstUser = DB.User.FirstOrDefault() == null ? true : false;

                DB.User.Add(new WebApplication.User
                {
                    Email = userToAdd.EMail,
                    Fname = userToAdd.FName,
                    Lname = userToAdd.LName,
                    Password = hashPassword
                });                

                DB.SaveChanges();

                var UserID = DB.User.Where(w => w.Email == userToAdd.EMail).Select(s => s.Id).FirstOrDefault();
                var RoleID = DB.Role.Where(w => w.RoleName == "Default").Select(s => s.Id).FirstOrDefault();

                DB.UserRoles.Add(new UserRoles { UserId = UserID, RoleId = RoleID });

                if(FirstUser == true)
                {
                    RoleID = DB.Role.Where(w => w.RoleName == "Admin").Select(s => s.Id).FirstOrDefault();
                    DB.UserRoles.Add(new UserRoles { UserId = UserID, RoleId = RoleID });
                }

                DB.SaveChanges();

                HttpContext.Response.Headers.Add("refresh", $"5;{Url.Action("Index", "Home", null, "http")}");
                return Content("User add...");
            }
        }
    }
}