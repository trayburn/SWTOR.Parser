using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SWTOR.Web.Controllers
{
    public class CombatController : Controller
    {
        [HttpPost]
        public ActionResult Parse(string combatLog)
        {
            var logParser = new SWTOR.Parser.Parser();
            var combatParser = new SWTOR.Parser.CombatParser();

            var log = logParser.Parse(new StringReader(combatLog));
            var model = combatParser.Parse(log);

            return View(model);
        }
    }
}
