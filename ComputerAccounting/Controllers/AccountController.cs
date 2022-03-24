using ComputerAccounting.App_Data;
using ComputerAccounting.ListViewModel;
using ComputerAccounting.Models;
using System.Linq;
using System.Web.Mvc;

namespace ComputerAccounting.Controllers
{
    public class AccountController: Controller
    {
        CompContent db = new();

        public ActionResult Index() => View(db.Users.ToList());

        [HttpGet]
        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email.Contains("@gmail.ru") || model.Email.Contains("@mail.com"))
                {
                    ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                    return View(model);
                }
                User user = db.Users.Where(m => m.Email.Contains(model.Email)).FirstOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError("Email", "Данный email уже зарегестрирован в системе");
                    return View(model);
                }
                db.Users.Add(model);
                db.SaveChanges();
                return RedirectToAction("ListDevice", "Device");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Where(m => m.Email == login.Email && m.Password == login.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Неправильный логин");
                    ModelState.AddModelError("Password", "Неправильный пароль");
                    return View(login);
                }
                else
                {
                    if (user.Email.Contains("@gmail.ru") || login.Email.Contains("@mail.com"))
                    {
                        ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                        return View(login);
                    }
                    return RedirectToAction("ListDevice", "Device");
                }
            }
            return View(login);
        }

        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}
