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
    public class KnowledgeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: Knowledge
        public ActionResult Index(int? knowledgeCategoryId)
        {
            //return View(db.Knowledges.ToList());
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var category = db.KnowledgeCategories.Where(x => x.Id == knowledgeCategoryId).FirstOrDefault();
            ViewBag.KnowledgeCategories = db.KnowledgeCategories.Where(x => x.Role == role)
                                                                .OrderByDescending(x => x.CategoryName)
                                                                .ToList();
            if (category == null)
            {
                category = new KnowledgeCategory
                {
                    Id = 0,
                    CategoryName = "全件"
                };
            }
            ViewBag.DisplayCategory = category;
            var knowledges = db.Knowledges.Where(x =>
                                    knowledgeCategoryId != null ? x.CategoryId == knowledgeCategoryId : true
                                    && x.Role == role)
                                   .OrderByDescending(x => x.UpdateDateTime)
                                   .ToList();
            var users = UserUtility.GetUserListInNowDisplayGroup().ToDictionary(u => u.Id);
            var knowledgeViewModels = knowledges.Select(x =>
            {
                ApplicationUser user;
                var existUser = false;
                if (users.TryGetValue(x.CreateUserId, out user))
                {
                    existUser = true;
                }
                return new KnowledgeViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CategoryId = x.CategoryId,
                    Content = x.Content.HtmlToPlainText(50),
                    CreateDateTime = x.CreateDateTime,
                    UpdateDateTime = x.UpdateDateTime,
                    Role = x.Role,
                    CreateUserId = existUser ? user.UserID : "manager",
                    UserName = existUser ? user.UserName : "講師陣",
                };
            });
            ViewBag.KnowledgeViewModels = knowledgeViewModels;
            return View();
        }

        // GET: Knowledge/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // GET: Knowledge/Create
        public ActionResult Create()
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            ViewBag.KnowledgeCategories = db.KnowledgeCategories.Where(x => x.Role == role)
                                                                .OrderByDescending(x => x.CategoryName)
                                                                .ToList();
            return View();
        }

        // POST: Knowledge/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,CategoryId,Content")] Knowledge knowledge)
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            if (ModelState.IsValid && knowledge.CategoryId > 0)
            {
                var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
                knowledge.IsPublic = true;
                knowledge.Role = role;
                knowledge.CreateDateTime = DateTime.Now;
                knowledge.UpdateDateTime = DateTime.Now;
                knowledge.CreateUserId = user.Id;
                db.Knowledges.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KnowledgeCategories = db.KnowledgeCategories.Where(x => x.Role == role)
                                                                .OrderByDescending(x => x.CategoryName)
                                                                .ToList();
            return View(knowledge);
        }

        public JsonResult CreateKnowledgeCategory(string knowledgeCategoryName)
        {
            if (db.KnowledgeCategories.Where(x => x.CategoryName == knowledgeCategoryName).Any())
            {
                return Json(new { result = "False", Message = "すでに存在しています。" });
            }
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var knowledgeCategory = new KnowledgeCategory();
            knowledgeCategory.CategoryName = knowledgeCategoryName;
            knowledgeCategory.Role = role;
            db.KnowledgeCategories.Add(knowledgeCategory);
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index", "Knowledge") });
        }

        // GET: Knowledge/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledges.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            ViewBag.KnowledgeCategories = db.KnowledgeCategories.Where(x => x.Role == role)
                                                                .OrderByDescending(x => x.CategoryName)
                                                                .ToList();
            return View(knowledge);
        }

        // POST: Knowledge/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,CategoryId,Content,IsPublic,Role,UpdateDateTime,CreateDateTime")] Knowledge knowledge)
        {
            if (ModelState.IsValid && knowledge.CategoryId > 0)
            {
                var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
                knowledge.CreateUserId = user.Id;
                knowledge.UpdateDateTime = DateTime.Now;
                db.Entry(knowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(knowledge);
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

        #region API
        public JsonResult ShowKnowledgeDetail(int knowledgeId)
        {
            var ajaxResult = new AjaxResult();
            try
            {
                ajaxResult.ResultData = db.Knowledges.Where(x => x.Id == knowledgeId).First();
                ajaxResult.Result = true;
            }
            catch
            {
                ajaxResult.Result = false;
                ajaxResult.Message = "存在しません。削除されています。";

            }

            return ajaxResult.GetJsonRsult();
        }

        public JsonResult DeleteKnowledge(int id)
        {
            Knowledge knowledge = db.Knowledges.Find(id);
            db.Knowledges.Remove(knowledge);
            db.SaveChanges();
            return Json(new { result = "Redirect", url = Url.Action("Index", "Knowledge") });
        }
        #endregion
    }
}
