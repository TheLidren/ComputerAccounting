using ComputerAccounting.App_Data;
using ComputerAccounting.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ComputerAccounting.Controllers
{
    public class TypeDeviceController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        public ActionResult ListTypeDevice() => View(db.TypeDevices.Where(m => m.Status).ToList());

        [HttpGet]
        public ActionResult CreateTypeDevice() => View();

        [HttpPost]
        public ActionResult CreateTypeDevice(TypeDevice typeDevice)
        {
            if (ModelState.IsValid)
            {
                TypeDevice type = db.TypeDevices.Where(m => m.Tittle.Contains(typeDevice.Tittle) && m.Status).FirstOrDefault();
                if (type != null)
                {
                    ModelState.AddModelError("Tittle", "Данный тип устройства существует с таким названием");
                    return View(typeDevice);
                }
                typeDevice.Tittle = trimmer.Replace(typeDevice.Tittle, " ").Trim();
                typeDevice.Status = true;
                db.TypeDevices.Add(typeDevice);
                db.SaveChanges();
                return RedirectToAction("ListTypeDevice");
            }
            return View(typeDevice);
        }

        [HttpGet]
        public ActionResult EditTypeDevice(int? typedeviceid)
        {
            TypeDevice typeDevice = db.TypeDevices.Find(typedeviceid);
            if (typedeviceid == null)
                return HttpNotFound();
            return View(typeDevice);
        }

        [HttpPost]
        public ActionResult EditTypeDevice(TypeDevice type)
        {
            if (ModelState.IsValid)
            {
                TypeDevice typeDevice = db.TypeDevices.Find(type.Id);
                type.Tittle = trimmer.Replace(type.Tittle, " ").Trim();
                typeDevice.Tittle = type.Tittle;
                db.SaveChanges();
                return RedirectToAction("ListTypeDevice");
            }
            return View(type);
        }

        [HttpGet]
        public ActionResult DeleteTypeDevice(int? typedeviceid)
        {
            TypeDevice typedevice = db.TypeDevices.Find(typedeviceid);
            if (typedeviceid == null)
                return HttpNotFound();
            typedevice.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListTypeDevice");
        }
    }
}
