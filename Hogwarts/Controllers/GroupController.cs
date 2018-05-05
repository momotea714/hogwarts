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
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
            var belongGroup = db.GroupMembers.Where(x => x.Role == role
                                                 && x.UserId == user.Id)
                                             .FirstOrDefault();
            Group group;
            if (belongGroup == null)
            {
                group = new Group
                {
                    GroupName = "グループに所属していません。グループに参加してください。"
                };
            }
            else
            {
                group = db.Groups.Where(x => x.Id == belongGroup.Id).FirstOrDefault();
            }
            ViewBag.MyGroup = group;
            ViewBag.NowLecture = HogwartsSettingUtility.GetNowLecture();

            return View();
        }

        // GET: Group/Edit/5
        [Authorize]
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
                ViewBag.GroupMember = db.GroupMembers.Where(x => x.GroupId == id).ToList();
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

        [Authorize]
        public ActionResult GroupListView()
        {
            return PartialView("_GroupListView", db.Groups.ToList());
        }

        [Authorize]
        public ActionResult GroupMemberProgressView(int? groupId)
        {
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            if (groupId == null)
            {
                var user = db.Users.Where(x => x.UserName == User.Identity.Name).First();
                var belongGroup = db.GroupMembers.Where(x => x.Role == role
                                                     && x.UserId == user.Id)
                                                 .FirstOrDefault();
                if (belongGroup == null)
                {
                    ViewBag.HasError = true;
                    ViewBag.ErrorMessage = "所属がグループないか、グループを選択していません";
                    return PartialView("_GroupMemberProgressView");
                }

                groupId = belongGroup.GroupId;
            }
            var subLectures = db.SubLectures.Where(x => x.LectureId == nowLecture.Id).ToList();
            var subLectureIds = subLectures.Select(y => y.Id).ToList();
            var members = db.GroupMembers.Where(x => x.GroupId == groupId).ToList();
            var membersIds = members.Select(y => y.UserId).ToList();
            var questions = db.Questions.Where(x => subLectureIds.Contains(x.SubLectureId)).ToList();
            var questionIds = questions.Select(y => y.Id).ToList();

            ViewBag.UserAnserStates = db.UserAnswerStates.Where(x =>
                                        membersIds.Contains(x.UserId)
                                        && questionIds.Contains(x.QuestionId)).ToList();
            ViewBag.Questions = questions;
            ViewBag.SubLectures = subLectures;
            ViewBag.Members = db.Users.Where(x => membersIds.Contains(x.Id)).ToList();
            ViewBag.HasError = false;
            return PartialView("_GroupMemberProgressView");
        }

        public ActionResult MemberView(string userId)
        {
            return PartialView("_MemberView", db.Users.Find(userId));
        }

        #endregion

        #region API

        public ActionResult AddMember(string userId)
        {
            var member = db.Users.Where(x => x.UserID == userId).Single();
            return MemberView(member.Id);
        }

        public JsonResult RegistGroup(Group group, IEnumerable<string> UserIds, bool IsEdit)
        {
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            if (IsEdit)
            {
                var g = db.Groups.Find(group.Id);
                g.GroupName = group.GroupName;
                var groupMember = db.GroupMembers.Where(x => x.GroupId == group.Id);
                db.GroupMembers.RemoveRange(groupMember);
            }
            else
            {
                group.Point = 0;
                group.Role = role;
                group.LectureId = HogwartsSettingUtility.GetNowLecture().Id;
                db.Groups.Add(group);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            foreach (var userId in UserIds)
            {
                var member = new GroupMember
                {
                    GroupId = group.Id,
                    Role = role,
                    UserId = userId
                };
                db.GroupMembers.Add(member);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return Json(new { result = "Redirect", url = Url.Action("Index", "Group") });
        }

        #endregion
    }
}
