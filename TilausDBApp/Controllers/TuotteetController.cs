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
    public class TuotteetController : Controller
    {    
        // GET: Tuotteet
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
                List<Tuotteet> model = db.Tuotteet.ToList();                                                     
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
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
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);  

                TilausDBEntities1 db = new TilausDBEntities1();
                Tuotteet tuote = db.Tuotteet.Find(id);   

                if (tuote == null) return HttpNotFound();  
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View(tuote);                       
            }

        }


        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Edit([Bind(Include = "TuoteID,Nimi,Ahinta,KuvaLinkki")] Tuotteet tuote)

        {

            if (ModelState.IsValid)   
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                db.Entry(tuote).State = EntityState.Modified;                                                                  
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            return View(tuote);
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
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View();
            }
            
        }


        [HttpPost]  
        [ValidateAntiForgeryToken]  

        public ActionResult Create([Bind(Include = "Nimi,Ahinta,KuvaLinkki")] Tuotteet tuote)
        {
            if (ModelState.IsValid)     
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                db.Tuotteet.Add(tuote);   
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            ViewBag.LoggedStatus = "Kirjaudu ulos";
            return View(tuote);
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
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);   
                TilausDBEntities1 db = new TilausDBEntities1();
                Tuotteet tuote = db.Tuotteet.Find(id);          
                if (tuote == null) return HttpNotFound();      
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View(tuote);                            
            }
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                Tuotteet tuote = db.Tuotteet.Find(id);
                db.Tuotteet.Remove(tuote);               
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["testmsg"] = "<script>alert('Voit poistaa vain tuotteen, jota ei ole tilausriveissä! ');</script>";
                return RedirectToAction("Index");
            }
            
        }

        


    }



}