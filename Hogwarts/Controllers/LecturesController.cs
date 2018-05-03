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

            var userInRole = UserUtility.GetUserListInNowDisplayGroup();
            var lectures = db.Lectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            var subLectures = db.SubLectures.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();
            var questions = db.Questions.Where(x => x.Role == role).OrderBy(x => x.ShowOrder).ToList();

            var targetTraineeOfLectures = db.TargetTraineeOfLectures
                                            .Join(db.Lectures,
                                            t => t.LectureId,
                                            l => l.Id,
                                            (t, l) => new { t, l })
                                            .Where(x => x.l.Role == role)
                                            .ToList();
            var progressManageViewModels = db.UserAnswerStates
                                     .Where(x => x.Role == role)
                                     .GroupBy(x => x.QuestionId)
                                     .ToList();

            var dicUserCount = new Dictionary<int, int>();
            foreach (var question in questions)
            {
                var sublecture = subLectures.Where(x => x.Id == question.SubLectureId).FirstOrDefault();
                var lecture = lectures.Where(x => x.Id == sublecture.LectureId).FirstOrDefault();
                var userCount = targetTraineeOfLectures.Where(x => x.t.LectureId == lecture.Id).Count();
                dicUserCount.Add(question.Id, userCount != 0 ? userCount : userInRole.Count());
            }

            ViewBag.ProgressManageViewModels = progressManageViewModels
                                     .Where(x => dicUserCount.ContainsKey(x.Key))
                                     .Select(x =>
                                     new ProgressManageViewModel
                                     {
                                         QuestionId = x.Key,
                                         Progress = x.Sum(y => y.Progress) / dicUserCount[x.Key],
                                         UnderStandingLevel = x.Sum(y => y.UnderStandingLevel) / dicUserCount[x.Key],
                                         NotStartYet = dicUserCount[x.Key] - x.Count(y => y.Progress != 0),
                                         InProcess = x.Count(y => y.Progress != 0 && y.Progress != 100),
                                         Completion = x.Count(y => y.Progress == 100)
                                     }).ToList();
            ViewBag.Lectures = lectures;
            ViewBag.SubLectures = subLectures;
            ViewBag.Questions = questions;
            ViewBag.DicUserCount = dicUserCount;
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
