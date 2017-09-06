using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Controllers
{
    public class RealController : Controller
    {
        // GET: Real
        public ActionResult Index()
        {
            string ip = Request.UserHostAddress;
            ViewBag.ip = ip;
            return View();
        }
    }
}