using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;

namespace TilausDBApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
            }
            else ViewBag.LoggedStatus = "Kirjaudu ulos";
            return View();
        }

        public ActionResult About()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
            }
            else ViewBag.LoggedStatus = "Kirjaudu ulos";
            ViewBag.Message = "Tätä sivua päivitetään.";
            return View();
        }

        public ActionResult Contact()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
            }
            else ViewBag.LoggedStatus = "Kirjaudu ulos";
            ViewBag.Message = "Tätä sivua päivitetään.";
            return View();
        }

        public ActionResult Login()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return View();
            }
            else
            {
                Session.Abandon();
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("Index", "Home"); 
            }
        }


        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Kirjautuminen onnistui";
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                Session["UserName"] = LoggedUser.UserName;
                db.Dispose();
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                db.Dispose();
                return View("Login", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Kirjaudu sisään";
            return RedirectToAction("Index", "Home"); 
        }

    }
}