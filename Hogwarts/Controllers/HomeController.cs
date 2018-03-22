using Hogwarts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Progress()
        {
            ViewBag.Lectures = db.Lectures.OrderBy(x => x.ShowOrder).ToList();
            ViewBag.SubLectures = db.SubLectures.OrderBy(x => x.ShowOrder).ToList();
            ViewBag.Questions = db.Questions.OrderBy(x => x.ShowOrder).ToList();
            return View();
        }
    }
}