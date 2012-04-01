using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Raven.Client;
using SWTOR.Parser.Domain;

namespace SWTOR.Web.Controllers
{
    public class CombatController : Controller
    {
        private IDocumentSession session;

        public CombatController(IDocumentSession session)
        {
            this.session = session;
        }

        [HttpPost]
        public ActionResult Parse(string combatLog)
        {
            var logParser = new SWTOR.Parser.Parser();
            var combatParser = new SWTOR.Parser.CombatParser();

            var log = logParser.Parse(new StringReader(combatLog));
            var model = combatParser.Parse(log);

            combatParser.Clean(model);

            session.Store(model);
            session.SaveChanges();

            return RedirectToAction("Log", new { id = model.Id.ToString() });
        }

        [HttpGet]
        public ActionResult Log(string id)
        {
            Guid logId;

            if (Guid.TryParse(id,out logId))
            {
                var model = session.Query<CombatLog>().FirstOrDefault(m => m.Id == logId);
                if (model != null)
                    return View(model);
            }

            return RedirectToAction("Index","Home");
        }
    }
}
