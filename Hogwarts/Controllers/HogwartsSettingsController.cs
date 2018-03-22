using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hogwarts.Models;
using Hogwarts.Utility;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Hogwarts.Controllers
{
    public class HogwartsSettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: HogwartsSettings
        public ActionResult Index()
        {
            ViewBag.Roles = UserUtility.GetAllRoleExceptAdmin();
            ViewBag.NowDisplayRole = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole);

            return View(db.HogwartsSettings.ToList());
        }

        // GET: HogwartsSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HogwartsSetting hogwartsSetting = db.HogwartsSettings.Find(id);
            if (hogwartsSetting == null)
            {
                return HttpNotFound();
            }
            return View(hogwartsSetting);
        }

        // GET: HogwartsSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: HogwartsSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HogwartsSetting hogwartsSetting = db.HogwartsSettings.Find(id);
            if (hogwartsSetting == null)
            {
                return HttpNotFound();
            }
            return View(hogwartsSetting);
        }

        // POST: HogwartsSettings/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SettingId,Setting")] HogwartsSetting hogwartsSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hogwartsSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hogwartsSetting);
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

        public ActionResult LectureSettingView()
        {
            var nowDisplayRole = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole);

            ViewBag.Lectures = db.Lectures.Where(x => x.Role == nowDisplayRole)
                                          .OrderBy(x => x.ShowOrder).ToList();
            return PartialView("_LectureSettingView");
        }

        public ActionResult LectureView(int lectureId)
        {
            ViewBag.Lecture = db.Lectures.Where(x => x.Id == lectureId).FirstOrDefault();
            ViewBag.SubLectures = db.SubLectures.Where(x => x.LectureId == lectureId)
                                                .OrderBy(x => x.ShowOrder).ToList();
            return PartialView("_LectureView");
        }

        public ActionResult SubLectureView(int subLectureId)
        {
            ViewBag.SubLecture = db.SubLectures.Where(x => x.Id == subLectureId).FirstOrDefault();
            ViewBag.Questions = db.Questions.Where(x => x.SubLectureId == subLectureId)
                                                .OrderBy(x => x.ShowOrder).ToList();
            return PartialView("_SubLectureView");
        }

        public ActionResult QuestionView(int questionId)
        {
            ViewBag.Question = db.Questions.Where(x => x.Id == questionId).FirstOrDefault();
            return PartialView("_QuestionView");
        }

        public ActionResult GetPartialView(AjaxParam ajaxParam)
        {
            if (ajaxParam.NodeCategoryCD == 1)
            {
                return LectureView(ajaxParam.Id);
            }
            else if (ajaxParam.NodeCategoryCD == 2)
            {
                return SubLectureView(ajaxParam.Id);
            }
            else if (ajaxParam.NodeCategoryCD == 3)
            {
                return QuestionView(ajaxParam.Id);
            }
            else
            {
                return null;
            }
        }

        public ActionResult RegistUserView()
        {
            ViewBag.Users = UserUtility.GetUserListInNowDisplayGroup();
            return PartialView("_RegistUserView");
        }
        #endregion

        #region API
        public JsonResult AddTreeObject(AjaxParam ajaxParam)
        {
            return ProgressManagement.AddTreeObject(ajaxParam);
        }
        public JsonResult EditTreeObject(AjaxParam ajaxParam)
        {
            return ProgressManagement.EditTreeObject(ajaxParam);
        }

        public JsonResult DeleteTreeObject(AjaxParam ajaxParam)
        {
            return ProgressManagement.DeleteTreeObject(ajaxParam);
        }

        public JsonResult ChangeNowDisplayRole(IdentityRole identityRole)
        {
            using (var db = new ApplicationDbContext())
            {
                var role = db.Roles.Find(identityRole.Id);
                HogwartsSettingUtility.UpdateHogwartsSetting(HogwartsSettingUtility.NowDisplayRole,role.Name);
            }
            return Json(new { result = "Redirect", url = Url.Action("Index", "HogwartsSettings") });
        }

        public JsonResult RegistUser(string text)
        {
            var ajaxResult = new AjaxResult();

            var rows = text.Replace("\r\n", "\n").Split('\n');
            var nowDisplayRole = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole);
            var email = nowDisplayRole + @"@obic.co.jp";
            var registCounter = 0;
            foreach (var row in rows)
            {
                if (row == string.Empty) continue;

                try
                {
                    var columns = row.Split('\t');
                    UserUtility.CreateUser(columns[0], columns[1], email, "P@ssw0rd", nowDisplayRole);
                    registCounter++;
                }
                catch (Exception e)
                {
                    ajaxResult.Message = e.Message;
                }
            }
            ajaxResult.ResultData = registCounter.ToString() + "人登録しました";
            ajaxResult.Result = true;
            return ajaxResult.GetJsonRsult();
        }
        #endregion

    }
}
