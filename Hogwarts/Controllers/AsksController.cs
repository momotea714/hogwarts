using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Hogwarts.Models;
using Hogwarts.Utility;
using HtmlAgilityPack;

namespace Hogwarts.Controllers
{
    [Authorize]
    public class AsksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: Asks
        public ActionResult Index()
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var asks = db.Asks.Where(x => x.Role == role
                                       && x.ParentAskId == -1)
                              .OrderByDescending(x => x.UpdateDateTime)
                              .ToList();
            var answer = db.Asks.Where(x => x.Role == role
                                       && x.ParentAskId != -1)
                              .OrderBy(x => x.UpdateDateTime)
                              .ToList();
            var users = UserUtility.GetUserListInNowDisplayGroup().ToDictionary(u => u.Id);
            var askViewModels = asks.Select(x =>
            {
                ApplicationUser user;
                var existUser = false;
                var isMyAsk = false;
                if (users.TryGetValue(x.CreateUserId, out user))
                {
                    existUser = true;
                    if (user.UserID == User.Identity.Name)
                    {
                        isMyAsk = true;
                    }
                }
                return new AskViewModel
                {
                    Id = x.Id,
                    ParentAskId = x.ParentAskId,
                    Content = x.Content.HtmlToPlainText(50),
                    CreateDateTime = x.CreateDateTime,
                    UpdateDateTime = x.UpdateDateTime,
                    IsClosed = x.IsClosed,
                    Role = x.Role,
                    AnswerCount = answer.Count(y => y.ParentAskId == x.Id),
                    CreateUserId = existUser ? user.UserID : "manager",
                    CreateUserName = existUser ? user.UserName : "講師陣",
                    IsMyAsk = isMyAsk
                };
            });
            return View(askViewModels);
        }

        // GET: Asks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ask = db.Asks.Find(id);
            if (ask == null)
            {
                return HttpNotFound();
            }
            ViewBag.Answers = db.Asks.Where(x => x.ParentAskId == ask.Id).ToList();
            return View(ask);
        }

        // GET: Asks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asks/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content")] Ask ask)
        {
            if (ModelState.IsValid)
            {
                var role = HogwartsSettingUtility.GetNowDisplayRole();
                var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
                ask.CreateDateTime = DateTime.Now;
                ask.UpdateDateTime = DateTime.Now;
                ask.ParentAskId = -1;
                ask.Role = role;
                ask.IsClosed = false;
                ask.CreateUserId = user.Id;
                db.Asks.Add(ask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ask);
        }

        // GET: Asks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ask ask = db.Asks.Find(id);
            if (ask == null)
            {
                return HttpNotFound();
            }
            return View(ask);
        }

        // POST: Asks/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentAskId,Content,CreateUserId,IsClosed,CreateDateTime,UpdateDateTime")] Ask ask)
        {
            if (ModelState.IsValid)
            {
                ask.UpdateDateTime = DateTime.Now;
                db.Entry(ask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ask);
        }

        // GET: Asks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ask ask = db.Asks.Find(id);
            if (ask == null)
            {
                return HttpNotFound();
            }
            return View(ask);
        }

        // POST: Asks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ask ask = db.Asks.Find(id);
            db.Asks.Remove(ask);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        public JsonResult RegistAnswer(Ask answer)
        {
            try
            {
                var ask = db.Asks.Where(x => x.Id == answer.ParentAskId).FirstOrDefault();
                if (ask != null)
                {
                    var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
                    var role = HogwartsSettingUtility.GetNowDisplayRole();
                    answer.Role = role;
                    answer.IsClosed = false;
                    answer.CreateDateTime = DateTime.Now;
                    answer.UpdateDateTime = DateTime.Now;
                    answer.CreateUserId = user.Id;
                    db.Asks.Add(answer);
                    db.SaveChanges();
                }
                else
                {
                    return Json(new { result = "Error", message = "該当する質問は存在しません" });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { result = "Redirect", url = Url.Action("Index", "Asks") });
        }

        #endregion

    }
}
