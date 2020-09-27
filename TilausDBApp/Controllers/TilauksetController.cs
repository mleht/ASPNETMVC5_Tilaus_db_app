using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;
using TilausDBApp.ViewModels;

namespace TilausDBApp.Controllers
{
    public class TilauksetController : Controller
    {
        

        // GET: Tilaukset
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
                var tilaukset = db.Tilaukset.Include("Postitoimipaikat");
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(tilaukset.ToList());
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
                Tilaukset tilaus = db.Tilaukset.Find(id);
                if (tilaus == null) return HttpNotFound();

                ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaus.AsiakasID);  
                ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaus.Postinumero);
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(tilaus);
            }                         
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaus)     
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            if (ModelState.IsValid)   
            {
                db.Entry(tilaus).State = EntityState.Modified;                                                           
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaus.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaus.Postinumero);
            ViewBag.LoggedStatus = "Kirjaudu ulos";
            db.Dispose();
            return View(tilaus);
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

                ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");

                var nro = db.Postitoimipaikat
                  .Select(s => new
                  {
                      Text = s.Postinumero + " " + s.Postitoimipaikka,
                      Value = s.Postinumero
                  })
                 .ToList();
                ViewBag.Postinumero = new SelectList(nro, "Value", "Text");
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaus)
        {
            TilausDBEntities1 db = new TilausDBEntities1();

            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaus);
                db.SaveChanges();
                db.Dispose(); 
                return RedirectToAction("Index");
            }
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postinumero", tilaus.Postinumero);
            db.Dispose();
            return View(tilaus);
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
                Tilaukset tilaus = db.Tilaukset.Find(id);
                if (tilaus == null) return HttpNotFound();
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(tilaus);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                Tilaukset tilaus = db.Tilaukset.Find(id);
                db.Tilaukset.Remove(tilaus);
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            catch (Exception) 
            {
                TempData["testmsg"] = "<script>alert('Voit poistaa vain tilauksen, jolla ei ole tilausrivejä! ');</script>";
                return RedirectToAction("Index");
            }
        }


        public ActionResult TilausOtsikot()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Kirjaudu sisään";
                return RedirectToAction("login", "home");
            }
            else
            {
                TilausDBEntities1 db = new TilausDBEntities1();    
                var tilaukset = db.Tilaukset.Include("Postitoimipaikat");
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(tilaukset.ToList());
            }
        }

        public ActionResult _TilausRivit(int? orderid)
        {
            TilausDBEntities1 db = new TilausDBEntities1();
            var orderRowsList = from tr in db.Tilausrivit
                                join t in db.Tuotteet on tr.TuoteID equals t.TuoteID
                                where tr.TilausID == orderid
                                select new OrderRows
                                {TuoteID = tr.TuoteID,
                                Maara = tr.Maara,
                                Ahinta = tr.Ahinta,
                                Nimi = t.Nimi
                                };
            return PartialView(orderRowsList);
        }
    }

}