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
            string hash = ComputeHash(combatLog);
            var logParser = new SWTOR.Parser.Parser();
            var combatParser = new SWTOR.Parser.CombatParser();

            var log = logParser.Parse(new StringReader(combatLog));
            var model = combatParser.Parse(log);

            combatParser.Clean(model);
            model.Id = hash;

            session.Store(model);
            session.SaveChanges();

            return RedirectToAction("Log", new { id = model.Id });
        }

        private string ComputeHash(string data)
        {
            var MD5 = new MD5CryptoServiceProvider();
            char[] bArr = data.ToCharArray();
            int size = bArr.GetUpperBound(0);
            byte[] arr = new byte[size];
            System.Text.Encoding.Default.GetEncoder().GetBytes(bArr, 0, size, arr, 0, true);
            var retVal = MD5.ComputeHash(arr);

            var sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        [HttpGet]
        public ActionResult Log(string id)
        {
            var model = session.Query<CombatLog>().FirstOrDefault(m => m.Id == id);
            if (model != null)
                return View(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
