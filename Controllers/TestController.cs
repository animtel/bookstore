using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Controllers
{
    public class TestController : IController
    {
        public void Execute(RequestContext RequestContext)
        {
            string ip = RequestContext.HttpContext.Request.UserHostName;
            var response = RequestContext.HttpContext.Response;
            response.Write("<h2>Ваш IP-адрес: " + ip + "</h2>");

        }
    }
}