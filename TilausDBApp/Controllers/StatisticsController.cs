using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;
using TilausDBApp.ViewModels;

namespace TilausDBApp.Controllers
{
	public class StatisticsController : Controller
	{
		// GET: Statistics
		public ActionResult Index()
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

		public ActionResult TopMyynnit()
		{
			if (Session["UserName"] == null)
			{
				ViewBag.LoggedStatus = "Kirjaudu sisään";
				return RedirectToAction("login", "home");
			}
			else
			{
				ViewBag.LoggedStatus = "Kirjaudu ulos";
				TilausDBEntities1 db = new TilausDBEntities1();
				string tuotteenNimiList;
				string tuotteenMyyntiList;
				List<TopMyyntiClass> Myynnit = new List<TopMyyntiClass>();

				var myyntiData = from cs in db.TopMyynti
								 select cs;

				foreach (TopMyynti sales in myyntiData)
				{
					TopMyyntiClass OneSalesRow = new TopMyyntiClass();   
					OneSalesRow.Nimi = sales.Nimi;        
					OneSalesRow.Summa = (int)sales.Summa;  
					Myynnit.Add(OneSalesRow);   
				}

				tuotteenNimiList = "'" + string.Join("','", Myynnit.Select(n => n.Nimi).ToList()) + "'";
				tuotteenMyyntiList = string.Join(",", Myynnit.Select(n => n.Summa).ToList());

				ViewBag.tuotteenNimi = tuotteenNimiList;
				ViewBag.tuotteenMyynti = tuotteenMyyntiList;

				return View();
			}

		}

		public ActionResult MyynnitPaiva()
		{
			if (Session["UserName"] == null)
			{
				ViewBag.LoggedStatus = "Kirjaudu sisään";
				return RedirectToAction("login", "home");
			}
			else
			{
				ViewBag.LoggedStatus = "Kirjaudu ulos";
				TilausDBEntities1 db = new TilausDBEntities1();
				string weekDayList;
				string orderTimesList;
				List<TilauksetViikonpaivaClass> Myynnit = new List<TilauksetViikonpaivaClass>();

				var myyntiData = from cs in db.TilauksetViikonPaiva
								 select cs;

				foreach (TilauksetViikonPaiva sales in myyntiData)
				{
					TilauksetViikonpaivaClass OneSalesRow = new TilauksetViikonpaivaClass();   
					OneSalesRow.weekday = sales.weekday;        
					OneSalesRow.order_times = (int)sales.order_times;  
					Myynnit.Add(OneSalesRow);   
				}

				weekDayList = "'" + string.Join("','", Myynnit.Select(n => n.weekday).ToList()) + "'";
				orderTimesList = string.Join(",", Myynnit.Select(n => n.order_times).ToList());

				ViewBag.weekdays = weekDayList;
				ViewBag.ordertimes = orderTimesList;

				return View();
			}
		}
	}
}