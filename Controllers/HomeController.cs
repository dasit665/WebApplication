using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Models;

using WebApplication.Filters;

namespace WebApplication.Controllers
{
    [RolesFilter]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [Route("/Privacy")]
        [Authorize]
        public IActionResult Privacy()
        {
            var res = new StringBuilder();

            foreach (var i in User.Claims)
            {

                res.Append($"{i.Type}: {i.Value}\n");
            }

            return Content(res.ToString());
        }


        [Route("/SU")]
        [Authorize(Roles ="Admin")]
        public IActionResult SU()
        {
            var res = new StringBuilder();

            res.Append($"{User.FindFirst(ClaimTypes.Name)}\n");
            res.Append($"{User.FindFirst(ClaimTypes.Email)}\n");

            foreach (var i in User.FindAll(ClaimTypes.Role))
            {
                res.Append($"{i}\n");
            }

            res.Append(User.Identity.IsAuthenticated);

            return Content(res.ToString());
        }
    }
}