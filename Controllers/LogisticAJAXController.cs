using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class LogisticAJAXController : Controller
    {
        public MyShopDB DB { get; set; }

        public LogisticAJAXController(MyShopDB DB)
        {
            this.DB = DB;
        }


        [HttpPost]
        public IActionResult GetCities()
        {
            var Cities = DB.City.Select(s => s.CityName).ToList();
            HttpContext.Request.Headers.Add("Content-Type", "text/html; charset=cp1251");
            return PartialView(Cities);
        }
    }
}