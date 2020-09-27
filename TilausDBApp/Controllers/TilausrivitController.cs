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
    public class TilausrivitController : Controller
    {
        // GET: Tilausrivit
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
                List<Tilausrivit> model = db.Tilausrivit.ToList(); 
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(model);   
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
                TilausDBEntities1 db = new TilausDBEntities1();
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Tilausrivit tilausrivi = db.Tilausrivit.Find(id);
                if (tilausrivi == null) return HttpNotFound();
                ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID", tilausrivi.TilausID);
                ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi", tilausrivi.TuoteID);
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                // db.Dispose(); kaatuu jos tässä
                return View(tilausrivi);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausriviID, TilausID, TuoteID, Maara, Ahinta")] Tilausrivit tilausrivi)
        {
            TilausDBEntities1 db = new TilausDBEntities1();

            if (ModelState.IsValid)
            {
                db.Entry(tilausrivi).State = EntityState.Modified;   // using system.data.entity käyttöön, jotta entitystate on tunnistettu                                                          
                db.SaveChanges();
                // db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID", tilausrivi.TilausID);
            ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi", tilausrivi.TuoteID);
            ViewBag.LoggedStatus = "Kirjaudu ulos";
            // db.Dispose();
            return View(tilausrivi);
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
                ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID");
                ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi");
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                // db.Dispose(); kaatuu jos tässä
                return View();
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausriviID, TilausID, TuoteID, Maara, Ahinta")] Tilausrivit tilausrivi)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
 
                if (ModelState.IsValid)
                {
                    db.Tilausrivit.Add(tilausrivi);
                    db.SaveChanges();
                    // db.Dispose();
                    return RedirectToAction("Index");
                    // return RedirectToAction("Tilausrivit", "Create");
                }
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                ViewBag.TilausID = new SelectList(db.Tilaukset, "TilausID", "TilausID");
                ViewBag.TuoteID = new SelectList(db.Tuotteet, "TuoteID", "Nimi");
                // db.Dispose();
                return View(tilausrivi);
            
        }


        public ActionResult Delete(int? id)  // ID:llä etsitään Find-metodilla löytyykö sellaista tiekannasta
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);   // jos ID:tä ei löydy
                TilausDBEntities1 db = new TilausDBEntities1();
                Tilausrivit tilausrivi = db.Tilausrivit.Find(id);          // Sijoitetaan tiedot Tuotteet luokan tuote olioom
                if (tilausrivi == null) return HttpNotFound();      // Jos on null niin error
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                // db.Dispose(); kaatuu jos tässä
                return View(tilausrivi);                            // palautetaan näkymä
            }
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            Tilausrivit tilausrivi = db.Tilausrivit.Find(id);
            db.Tilausrivit.Remove(tilausrivi);               // poisto remove metodilla
            db.SaveChanges();
            db.Dispose();
            return RedirectToAction("Index");
        }

    }
}