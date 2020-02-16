using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Areas.UsersAdmining.VMModels;

namespace WebApplication.Areas.UsersAdmining.Controllers
{
    [Area("UsersAdmining")]
    [Authorize(Roles ="Admin")]
    public class UserInfoController : Controller
    {
        public MyShopDB DB { get; set; }

        public UserInfoController(MyShopDB DB)
        {
            this.DB = DB;
        }


        public IActionResult AllUsers()
        {
            List<VMUsersAdmining> AllUsers = new List<VMUsersAdmining>();
            var UsersInDB = DB.User.ToList();

            foreach (var i in UsersInDB)
            {
                AllUsers.Add(new VMUsersAdmining() { ID = i.Id, EMail = i.Email, FName = i.Fname, LName = i.Lname });
            }

            return View(AllUsers);
        }

        public IActionResult ShowUserInfo(int? ID)
        {
            if(ID is null)
            {
                throw new Exception("ID is Null");
            }            

            return View(GetUser(ID.Value));
        }

        [HttpGet]
        public IActionResult EditUser(int? ID)
        {
            if (ID is null)
            {
                throw new Exception("myException ID is Null");
            }

            return View(GetUser(ID.Value));
        }

        [HttpGet]
        public IActionResult DeleteUser(int? ID)
        {
            if (ID is null)
            {
                throw new Exception("myException ID is Null");
            }

            var UserToDelete = DB.User.Where(w => w.Id == ID).FirstOrDefault();
            DB.User.Remove(UserToDelete);
            DB.SaveChanges();

            var route = Url.RouteUrl("allusers", null, "http");
            HttpContext.Response.Headers.Add("REFRESH", $"3;{route}");
            return Content("User is deleted...");

        }

        [HttpPost]
        public IActionResult EditUser(VMUsersAdmining ModifyUser)
        {
            if (ModifyUser is null)
            {
                throw new Exception("ModufyUser is Null");
            }

            var UserToModify = DB.User.Where(w => w.Id == ModifyUser.ID).FirstOrDefault();

            UserToModify.Email = ModifyUser.EMail;
            UserToModify.Fname = ModifyUser.FName;
            UserToModify.Lname = ModifyUser.LName;
            UserToModify.HasChanged = true;

            DB.UserRoles.RemoveRange(DB.UserRoles.Where(w => w.UserId == ModifyUser.ID));

            var RolesNamesModifyUser = ModifyUser.Roles.Select(s => s).Where(w => w.Value == true).Select(s => s.Key).ToList();
            int RoleID = default;

            foreach (var i in RolesNamesModifyUser)
            {
                RoleID = DB.Role.Where(w => w.RoleName == i).Select(s => s.Id).FirstOrDefault();
                DB.UserRoles.Add(new UserRoles { UserId = ModifyUser.ID, RoleId = RoleID });
            }

            UserToModify.HasChanged = true;

            DB.SaveChanges();

            var route = Url.RouteUrl("allusers", null, "http");

            HttpContext.Response.Headers.Add("REFRESH", $"3;{route}");

            return Content("User modified...");
        }

        [NonAction]
        private VMUsersAdmining GetUser(int ID)
        {
            var UserInDB = DB.User.Where(w => w.Id == ID).FirstOrDefault(); ;

            var UserRolesInDB = (from roles in DB.UserRoles
                                 join names in DB.Role
                                 on roles.RoleId equals names.Id
                                 where roles.UserId == ID
                                 select names.RoleName).ToList();

            var RoleNames = DB.Role.Select(s => s.RoleName).ToList();



            VMUsersAdmining UserInfo = new VMUsersAdmining
            {
                ID = UserInDB.Id,
                EMail = UserInDB.Email,
                FName = UserInDB.Fname,
                LName = UserInDB.Lname,
            };

            foreach (var i in RoleNames)
            {
                UserInfo.Roles.Add(i, UserRolesInDB.Contains(i) ? true : false);
            }

            return UserInfo;
        }
    }
}