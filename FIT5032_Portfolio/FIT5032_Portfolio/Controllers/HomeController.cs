using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_Portfolio.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        // Reference: https://blog.elmah.io/the-ultimate-guide-to-secure-cookies-with-web-config-in-net/ 

        public ActionResult SetSecureCookie()
        {
            var cookie = new HttpCookie("Cookie");
            cookie.Value = "value";

            cookie.Secure = true;
            cookie.HttpOnly = true;

            Response.Cookies.Add(cookie);

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Communication()
        {
            return View();
        }
    }
}