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
    public class PostitoimipaikatController : Controller
    {
        
        // GET: Postitoimipaikat
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
                List<Postitoimipaikat> model = db.Postitoimipaikat.ToList(); 
                db.Dispose();  
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                return View(model);       
            }
        }

        public ActionResult Edit(string id)       

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
                Postitoimipaikat toimipaikka = db.Postitoimipaikat.Find(id);
                if (toimipaikka == null) return HttpNotFound();
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View(toimipaikka);
            }                        
        }

        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult Edit([Bind(Include = "Postinumero, Postitoimipaikka")] Postitoimipaikat toimipaikka)
        {
            if (ModelState.IsValid)   
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                db.Entry(toimipaikka).State = EntityState.Modified;                                                             
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            return View(toimipaikka);
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
        public ActionResult Create([Bind(Include = "Postinumero, Postitoimipaikka")] Postitoimipaikat toimipaikka)
        {
            if (ModelState.IsValid)     
            {
                    TilausDBEntities1 db = new TilausDBEntities1();
                    db.Postitoimipaikat.Add(toimipaikka);
                    db.SaveChanges();
                    db.Dispose();
                    return RedirectToAction("Index");  
            }
            return View(toimipaikka);
        }


        public ActionResult Delete(string id)  
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
                Postitoimipaikat toimipaikka = db.Postitoimipaikat.Find(id);          
                if (toimipaikka == null) return HttpNotFound();                      
                ViewBag.LoggedStatus = "Kirjaudu ulos";
                db.Dispose();
                return View(toimipaikka);                                            
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                TilausDBEntities1 db = new TilausDBEntities1();
                Postitoimipaikat toimipaikka = db.Postitoimipaikat.Find(id);
                db.Postitoimipaikat.Remove(toimipaikka);               
                db.SaveChanges();
                db.Dispose();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["testmsg"] = "<script>alert('Voit poistaa vain postitoimipaikan, jolla ei ole asiakkaita! ');</script>";
                return RedirectToAction("Index");
            }
            
        }

    }
}