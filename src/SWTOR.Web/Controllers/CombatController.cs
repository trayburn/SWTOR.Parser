using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Raven.Client;
using SWTOR.Parser.Domain;
using System.Security.Cryptography;
using System.Text;
using SWTOR.Parser;
using SWTOR.Web.Data;

namespace SWTOR.Web.Controllers
{
    public class CombatController : Controller
    {
        private IRepository<CombatLog> repo;
        private IHashCreator hasher;
        private IStringParser parser;
        private ICombatParser combatParser;

        public CombatController(IRepository<CombatLog> repo, IHashCreator hasher, IStringParser parser, ICombatParser combatParser)
        {
            this.repo = repo;
            this.hasher = hasher;
            this.parser = parser;
            this.combatParser = combatParser;
        }

        [HttpPost]
        public ActionResult Parse(string combatLog)
        {
            var hash = hasher.CreateHash(combatLog);

            var model = repo.Query().FirstOrDefault(m => m.Id == hash);
            if (model != null)
                return RedirectToAction("Log", new { id = model.Id });

            var log = parser.ParseString(combatLog);
            model = combatParser.Parse(log);

            combatParser.Clean(model);
            model.Id = hash;
            repo.Store(model);
            repo.SaveChanges();

            return RedirectToAction("Log", new { id = model.Id });
        }

        [HttpGet]
        public ActionResult Log(string id)
        {
            var model = repo.Query().FirstOrDefault(m => m.Id == id);
            if (model != null)
                return View(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
