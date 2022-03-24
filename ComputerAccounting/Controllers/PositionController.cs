using ComputerAccounting.App_Data;
using ComputerAccounting.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ComputerAccounting.Controllers
{
    public class PositionController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        public ActionResult ListPosition() => View(db.Positions.Where(m => m.Status).ToList());

        [HttpGet]
        public ActionResult CreatePosition() => View();

        [HttpPost]
        public ActionResult CreatePosition(Position position)
        {
            if (ModelState.IsValid)
            {
                Position pos = db.Positions.Where(m => m.Tittle.Contains(position.Tittle) && m.Status).FirstOrDefault();
                if (pos != null)
                {
                    ModelState.AddModelError("Tittle", "Данная должность существует с таким названием");
                    return View(position);
                }
                position.Tittle = trimmer.Replace(position.Tittle, " ").Trim();
                position.Status = true;
                db.Positions.Add(position);
                db.SaveChanges();
                return RedirectToAction("ListPosition");
            }
            return View(position);
        }

        [HttpGet]
        public ActionResult EditPosition(int? positionid)
        {
            Position Position = db.Positions.Find(positionid);
            if (positionid == null)
                return HttpNotFound();
            return View(Position);
        }

        [HttpPost]
        public ActionResult EditPosition(Position pos)
        {
            if (ModelState.IsValid)
            {
                Position position = db.Positions.Find(pos.Id);
                pos.Tittle = trimmer.Replace(pos.Tittle, " ").Trim();
                position.Tittle = pos.Tittle;
                db.SaveChanges();
                return RedirectToAction("ListPosition");
            }
            return View(pos);
        }

        [HttpGet]
        public ActionResult DeletePosition(int? positionid)
        {
            Position position = db.Positions.Find(positionid);
            if (positionid == null)
                return HttpNotFound();
            position.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListPosition");
        }
    }
}
