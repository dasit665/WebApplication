using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Infrastructure.Filters
{
    public class CookieActualFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == true)
            {

                var DB = context.HttpContext.RequestServices.GetService(typeof(MyShopDB)) as MyShopDB;


                var UserMail = context.HttpContext.User.Claims.Where(w => w.Type == ClaimTypes.Email).Select(s => s.Value).FirstOrDefault();

                if (DB.User.Where(w => w.Email == UserMail).FirstOrDefault() == null
                    ||
                    DB.User.Where(w => w.Email == UserMail).FirstOrDefault().HasChanged == true) GetOut();


                void GetOut()
                {
                    context.HttpContext.Response.Redirect("/Login");
                    context.HttpContext.Response.Cookies.Delete("AuthenticationCookie");
                }
            }
        }
    }
}

