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
        private readonly IStringParser parser;

        public APIController(IStringParser parser)
        {
            this.parser = parser;
        }

        [HttpPost]
        public ActionResult ParseText(string combatLog)
        {
            object returnData = parser.ParseString(combatLog);

            var ser = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult { Content = ser.Serialize(returnData), ContentType = "application/json" };
        }

        // This action is a remnant, an attempt to parse incoming file streams that has never 
        // been successful.  Still waiting to see an example which would work.
        [HttpPost]
        public ActionResult Parse()
        {
            object returnData = null;

            var parser = new SWTOR.Parser.Parser();
            if (Request.ContentLength > 0)
            {
                Stream stream = Request.InputStream;
                returnData = parser.Parse(new StreamReader(stream));
            }
            var ser = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult { Content = ser.Serialize(returnData), ContentType = "application/json" };
        }
    }
}
