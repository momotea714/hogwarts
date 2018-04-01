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
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD

        // GET: Group
        public ActionResult Index()
        {
            ViewBag.NowLecture = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowLecture);
            return View();
        }

        // GET: Group/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.IsEdit = false;
                return View(new Group());
            }
            else
            {
                ViewBag.IsEdit = true;
                var group = db.Groups.Find(id);
                if (group == null)
                {
                    return HttpNotFound();
                }
                ViewBag.GroupMember = db.GroupMembers.Where(x => x.GroupId == id);
                return View(group);
            }
        }

        // POST: Group/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LectureId,GroupName,Point")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        // POST: Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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

        #region PartialView

        public ActionResult GroupListView()
        {
            return PartialView("_GroupListView", db.Groups.ToList());
        }

        public ActionResult GroupMemberProgressView()
        {
            return PartialView("_GroupMemberProgressView");
        }

        public ActionResult GroupProgressView()
        {
            return PartialView("_GroupProgressView");
        }

        public ActionResult MemberView(string userId)
        {
            return PartialView("_MemberView",db.Users.Find(userId));
        }

        #endregion
    }
}
