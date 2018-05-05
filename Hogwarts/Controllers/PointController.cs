using Hogwarts.Models;
using Hogwarts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Controllers
{
    [Authorize]
    public class PointController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            var userInRole = UserUtility.GetUserListInNowDisplayGroup().ToDictionary(x => x.Id);

            ViewBag.UserPointRecords = RankingUtility.GetUserPointRecordViewModels(nowLecture.Id, userInRole).OrderBy(x => x.UserLoginId);

            ViewBag.GroupPointRecords = RankingUtility.GetGroupPointRecordViewModels(nowLecture.Id, userInRole);
            return View();
        }

        #region API
        public JsonResult Update(RecordAjaxParam ajaxParam)
        {
            return RecordManagement.Update(ajaxParam);
        }
        #endregion
    }
}