using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;

namespace TilausDBApp.Controllers
{
    public class HenkilotController : Controller
    {
        // GET: Henkilot
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                Models.TilausDBEntities1 db = new TilausDBEntities1();    
                List<Henkilot> model = db.Henkilot.ToList(); 
                db.Dispose();  
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(model);   
            }
           
        }
    }
}