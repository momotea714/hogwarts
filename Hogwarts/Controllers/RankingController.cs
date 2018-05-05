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
    public class RankingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD

        // GET: Group
        public ActionResult Index()
        {
            return View();
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

        #region PartialView

        public ActionResult UserRanking()
        {
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
            var userInRole = UserUtility.GetUserListInNowDisplayGroup().ToDictionary(x => x.Id);

            ViewBag.UserPracticeRecords = RankingUtility.GetUserPracticeRecordViewModels(nowLecture.Id,userInRole);
            ViewBag.UserPointRecords = RankingUtility.GetUserPointRecordViewModels(nowLecture.Id, userInRole);

            return PartialView("_UserRankingView");
        }

        public ActionResult GroupRanking()
        {
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            var userInRole = UserUtility.GetUserListInNowDisplayGroup().ToDictionary(x => x.Id);

            ViewBag.GroupPracticeRecords = RankingUtility.GetGroupPracticeRecordViewModels(nowLecture.Id, userInRole);
            ViewBag.GroupPointRecords = RankingUtility.GetGroupPointRecordViewModels(nowLecture.Id, userInRole);
            return PartialView("_GroupRankingView");
        }

        #endregion
    }
}
