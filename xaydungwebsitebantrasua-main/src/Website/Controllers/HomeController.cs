using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProTechTiveGear.Models;

namespace ProTechTiveGear.Controllers
{
	public class HomeController : Controller
	{
		private ProTechTiveGearEntities db = new ProTechTiveGearEntities();

		public ActionResult Index()
		{
			// Lấy 6 tin tức mới nhất
			var latestNews = db.Blogs
				.OrderByDescending(b => b.DateImport)
				.Take(6)
				.ToList();

			return View(latestNews);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}