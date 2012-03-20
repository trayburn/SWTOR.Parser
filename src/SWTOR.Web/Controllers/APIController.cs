using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWTOR.Parser;
using System.IO;
using System.Web.Script.Serialization;

namespace SWTOR.Web.Controllers
{
    public class APIController : Controller
    {
        [HttpPost]
        public ActionResult Parse()
        {
            object returnData = null;

            var parser = new SWTOR.Parser.Parser();
            if (Request.Files.Count > 0)
            {
                Stream stream = Request.Files["combatLog"].InputStream;
                returnData = parser.Parse(new StreamReader(stream));
            }

            return Json(returnData);
        }

        [HttpPost]
        public ActionResult ParseText(string combatLog)
        {
            object returnData = null;

            var parser = new SWTOR.Parser.Parser();
            returnData = parser.Parse(new StringReader(combatLog));

            var ser = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult { Content = ser.Serialize(returnData), ContentType = "application/json" };
        }


    }
}
