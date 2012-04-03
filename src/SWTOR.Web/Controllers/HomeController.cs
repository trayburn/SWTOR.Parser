using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWTOR.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestIndex()
        {
            return View();
        }

        public ActionResult DPS()
        {
            return View();
        }

    }
}
