using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hogwarts.Models;
using Hogwarts.Utility;

namespace Hogwarts.Controllers
{
    [Authorize]
    public class LecturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: Lectures
        public ActionResult Index()
        {
            return View(db.Lectures.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region partialView

        public ActionResult UserProgress()
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
            ViewBag.Lectures = db.Lectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            ViewBag.SubLectures = db.SubLectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            ViewBag.Questions = db.Questions.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            ViewBag.UserAnswerStates = db.UserAnswerStates.Where(x => x.UserId == user.Id).ToList();
            ViewBag.NowLecture = HogwartsSettingUtility.GetNowLecture();
            return PartialView("_UserProgress");
        }

        public ActionResult ProgressManage()
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            ViewBag.Lectures = db.Lectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            ViewBag.SubLectures = db.SubLectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            ViewBag.Questions = db.Questions.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();

            return PartialView("_ProgressManage");
        }

        #endregion

        #region API
        public JsonResult UpdateAnswerState(AnswerStateAjaxParam ajaxParam)
        {
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
            return AnswerStateManagement.UpdateAnswerState(ajaxParam, user);
        }
        #endregion
    }
}
