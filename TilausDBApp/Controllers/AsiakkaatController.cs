using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;

namespace TilausDBApp.Controllers
{
    public class AsiakkaatController : Controller
    {
        // GET: Asiakkaat
        public ActionResult Index()
        {
            
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                var asiakkaat = db.Asiakkaat.Include(a => a.Postitoimipaikat);
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(asiakkaat.ToList());
            }
        }

        public ActionResult Edit(int? id)       
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  
                TilausDBEntities1 db = new TilausDBEntities1();
                Asiakkaat asiakas = db.Asiakkaat.Find(id);
                if (asiakas == null) return HttpNotFound();
                
                var nro = db.Postitoimipaikat
                 .Select(s => new
                 {
                     Text = s.Postinumero + " " + s.Postitoimipaikka,
                     Value = s.Postinumero
                 })
                .ToList();
                ViewBag.Postinumero = new SelectList(nro, "Value", "Text", asiakas.Postinumero);
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View(asiakas);
            } 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AsiakasID,Nimi,Osoite,Postinumero,Postitoimipaikka")] Asiakkaat asiakas)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            if (ModelState.IsValid)
            {
                db.Entry(asiakas).State = EntityState.Modified;                                                           
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", asiakas.Postinumero);
            db.Dispose();
            return View(asiakas);
        }

        public ActionResult Create()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                var nro = db.Postitoimipaikat
           .Select(s => new
           {
               Text = s.Postinumero + " " + s.Postitoimipaikka,
               Value = s.Postinumero
           })
          .ToList();
                ViewBag.Postinumero = new SelectList(nro, "Value", "Text");
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AsiakasID,Nimi,Osoite,Postinumero,Postitoimipaikka")] Asiakkaat asiakas)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            if (ModelState.IsValid)
            {
                db.Asiakkaat.Add(asiakas);
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero");
            db.Dispose();
            return View(asiakas);
        }

        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Asiakkaat asiakas = db.Asiakkaat.Find(id);
                if (asiakas == null) return HttpNotFound();
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(asiakas);
            }                                             
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                Asiakkaat asiakas = db.Asiakkaat.Find(id);
                db.Asiakkaat.Remove(asiakas);
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["testmsg2"] = "<script>alert('Voit poistaa vain asiakkaan, jolla ei ole tilauksia! ');</script>";
                return RedirectToAction("Index");
            }
            
        }
    }
}