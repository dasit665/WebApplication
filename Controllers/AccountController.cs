using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.ViewModels;

namespace WebApplication.Controllers
{
    [Authorize(Roles="Admin")]
    public class AccountController : Controller
    {
        public TestDB DB { get; set; }
        public AccountController(TestDB DB)
        {
            this.DB = DB;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ShowUsers");
        }


        [HttpGet]
        public IActionResult ShowUsers()
        {
            var Users =
                DB.Users.Select(s => new VMAllUsers { ID = s.Id, EMail = s.Email, FName = s.Fname, LName = s.Lname })
                .ToList();

            return View(Users);
        }


        [HttpGet]
        public IActionResult UserInfo(int user = 0)
        {
            if (user == 0)
            {
                return Content("User is null");
            }
            return View(GetUser(user));
        }

        [Route("/Account/Change")]
        [HttpGet]
        public IActionResult Change(int ID = 0)
        {
            if (ID == 0)
            {
                return Content("ID is 0");
            }

            return View(GetUser(ID));
        }

        [Route("/Account/Change")]
        [HttpPost]
        public IActionResult Change(VMUserAndRoles user)
        {

            if (ModelState.IsValid == false)
            {
                return View(GetUser(user.ID));
            }
            else
            {
                var User = DB.Users.First(f => f.Id == Int32.Parse(Request.Form["ID"]));

                if (User != null)
                {
                    User.Fname = Request.Form["FName"];
                    User.Lname = Request.Form["LName"];
                    User.Email = Request.Form["EMail"];

                    var RolesToDelite = DB.UserRoles.Where(w => w.UserId == User.Id);
                    DB.UserRoles.RemoveRange(RolesToDelite);

                    var RolesInDB = DB.Roles.Select(s => s.Role).ToArray();

                    foreach (var i in Request.Form)
                    {
                        if (RolesInDB.Contains(i.Key) == true)
                        {
                            var RoleID = DB.Roles.First(f => f.Role == i.Key).Id;
                            DB.UserRoles.Add(new UserRoles { UserId = User.Id, RoleId = RoleID });
                        }
                    }
                    DB.SaveChanges();

                    Response.Headers.Add("REFRESH", "3;http://localhost:3189/account");
                    return Content("User is modified");
                }
                else
                {
                    return Content("User not found");
                }
            }
        }

        #region GetUser
        [NonAction]
        private VMUserAndRoles GetUser(int UserID)
        {
            var UserAndRoles = new VMUserAndRoles();
            UserAndRoles.ID = UserID;

            var User = this.DB.Users.FirstOrDefault(f => f.Id == UserID);

            UserAndRoles.FName = User.Fname;
            UserAndRoles.LName = User.Lname;
            UserAndRoles.EMail = User.Email;

            var AllRoles = DB.Roles.Select(s => s.Role).ToArray();

            var ActiveRoles = (from acRols in DB.UserRoles
                               join allRoles in DB.Roles
                               on acRols.RoleId equals allRoles.Id
                               where acRols.UserId == UserID
                               select allRoles.Role).ToList();

            var NotActiveRoles = AllRoles.Except(ActiveRoles);

            foreach (var i in ActiveRoles)
            {
                UserAndRoles.UserRoles.Add(i, true);
            }

            foreach (var i in NotActiveRoles)
            {
                UserAndRoles.UserRoles.Add(i, false);
            }

            UserAndRoles.UserRoles = UserAndRoles.UserRoles.OrderBy(o => o.Key).ToDictionary(s => s.Key, s => s.Value);

            return UserAndRoles;
        }
        #endregion GetUser
    }
}