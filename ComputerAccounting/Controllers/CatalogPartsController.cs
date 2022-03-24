using ComputerAccounting.App_Data;
using ComputerAccounting.Models;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ComputerAccounting.Controllers
{
    public class CatalogPartsController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        public ActionResult ListCatalogParts() => View(db.CatalogParts.Where(m => m.Status).ToList());

        [HttpGet]
        public ActionResult CreateCatalogParts() => View();

        [HttpPost]
        public ActionResult CreateCatalogParts(CatalogParts catalogParts)
        {
            if (ModelState.IsValid)
            {
                CatalogParts catalog = db.CatalogParts.Where(m => m.Tittle.Contains(catalogParts.Tittle) && m.Status).FirstOrDefault();
                if (catalog != null)
                {
                    ModelState.AddModelError("Tittle", "Данная запчасть уже имеется в базе. Вы можете изменить количество.");
                    return View(catalogParts);
                }
                catalogParts.Tittle = trimmer.Replace(catalogParts.Tittle, " ").Trim();
                catalogParts.Status = true;
                db.CatalogParts.Add(catalogParts);
                db.SaveChanges();
                return RedirectToAction("ListCatalogParts");
            }
            return View(catalogParts);
        }

        [HttpGet]
        public ActionResult EditCatalogParts(int? catalogPartsid)
        {
            CatalogParts catalogParts = db.CatalogParts.Find(catalogPartsid);
            if (catalogPartsid == null)
                return HttpNotFound();
            return View(catalogParts);
        }

        [HttpPost]
        public ActionResult EditCatalogParts(CatalogParts parts)
        {
            if (ModelState.IsValid)
            {
                parts.Tittle = trimmer.Replace(parts.Tittle, " ").Trim();
                db.Entry(parts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListCatalogParts");
            }
            return View(parts);
        }

        [HttpGet]
        public ActionResult DeleteCatalogParts(int? catalogPartsid)
        {
            CatalogParts typedevice = db.CatalogParts.Find(catalogPartsid);
            if (catalogPartsid == null)
                return HttpNotFound();
            typedevice.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListCatalogParts");
        }
    }
}
