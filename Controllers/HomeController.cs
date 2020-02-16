using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

using WebApplication.Infrastructure.Filters;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        [CookieActualFilter]
        public IActionResult Index()
        {
            var DB = HttpContext.RequestServices.GetService(typeof(MyShopDB)) as MyShopDB;
            if (DB.User.FirstOrDefault() == null)
            {
                var Route = Url.RouteUrl("adduser", null, "http");
                HttpContext.Response.Headers.Add("REFRESH", $"5;{Route}");
                return Content("Please create Administarator");
            }

            return View();
        }


        [Authorize(Policy = "OnlyForLogistic")]
        [Authorize(Roles = "Salller")]
        public IActionResult Logistic()
        {
            return View();
        }
    }
}