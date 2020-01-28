using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Filters
{
    public class RolesFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == true)
            {
                var DB = context.HttpContext.RequestServices.GetService(typeof(TestDB)) as TestDB;

                var UserEmail = context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
                var UserID = DB.Users.FirstOrDefault(f => f.Email == UserEmail).Id;


                var RolesInDB = (from userRoles in DB.UserRoles
                                 join roles in DB.Roles
                                 on userRoles.RoleId equals roles.Id
                                 where userRoles.UserId == UserID
                                 select roles.Role).ToList();

                List<string> AuthenticatedUserRoles = new List<string>();

                foreach (var i in context.HttpContext.User.FindAll(ClaimTypes.Role))
                {
                    AuthenticatedUserRoles.Add(i.Value);
                }

                RolesInDB = RolesInDB.OrderBy(o => o).ToList();
                AuthenticatedUserRoles = AuthenticatedUserRoles.OrderBy(o => o).ToList();

                if (AuthenticatedUserRoles.SequenceEqual(RolesInDB) == false)
                {
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.HttpContext.User = null;
                }

            }

            await next();
        }
    }
}
