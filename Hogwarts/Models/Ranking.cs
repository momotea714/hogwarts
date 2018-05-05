using Hogwarts.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Models
{
    public class UserRecord
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int LectureId { get; set; }

        public string UserId { get; set; }

        public int Point { get; set; }
    }

    public class GroupRecord
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int LectureId { get; set; }

        public int GroupId { get; set; }

        public int Point { get; set; }
    }

    public class UserPracticeRecordViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public double Progress { get; set; }

        public int Rank { get; set; }
    }

    public class UserPointRecordViewModel
    {
        public string UserId { get; set; }

        public string UserLoginId { get; set; }

        public string UserName { get; set; }

        public int Point { get; set; }

        public int Rank { get; set; }
    }

    public class GroupPracticeRecordViewModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public double Progress { get; set; }

        public int Rank { get; set; }
    }

    public class GroupPointRecordViewModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int Point { get; set; }

        public int Rank { get; set; }
    }

    public class RecordAjaxParam
    {
        public int UpdateCategory { get; set; }//1:UserRecord,2:GroupRecord
        public string UserId { get; set; }
        public int GroupId { get; set; }
        public int Point { get; set; }
    }

    public static class RecordManagement
    {
        public static JsonResult Update(RecordAjaxParam ajaxParam)
        {
            if (ajaxParam.UpdateCategory == 1)
            {
                return UpdateUserRecord(ajaxParam);
            }
            else
            {
                return UpdateGroupRecord(ajaxParam);
            }
        }

        private static JsonResult UpdateUserRecord(RecordAjaxParam ajaxParam)
        {
            var ajaxResult = new AjaxResult();
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            UserRecord userRecord;
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    userRecord = db.UserRecords.Where(x => x.UserId == ajaxParam.UserId
                                                            && x.LectureId == nowLecture.Id)
                                               .FirstOrDefault();
                    var isNew = false;
                    if (userRecord == null)
                    {
                        userRecord = new UserRecord
                        {
                            LectureId = nowLecture.Id,
                            Role = role,
                            UserId = ajaxParam.UserId,
                            Point = ajaxParam.Point,
                        };
                        isNew = true;
                    }

                    if (isNew)
                    {
                        db.UserRecords.Add(userRecord);
                    }
                    else
                    {
                        userRecord.Point = userRecord.Point + ajaxParam.Point;
                        db.Entry(userRecord).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    ajaxParam.Point = userRecord.Point;
                    ajaxResult.ResultData = ajaxParam;
                    ajaxResult.Result = true;
                }
                catch
                {
                    ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                    return null;
                }
            }

            return ajaxResult.GetJsonRsult();
        }

        private static JsonResult UpdateGroupRecord(RecordAjaxParam ajaxParam)
        {
            var ajaxResult = new AjaxResult();
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            var nowLecture = HogwartsSettingUtility.GetNowLecture();
            GroupRecord groupRecord;
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    groupRecord = db.GroupRecords.Where(x => x.GroupId == ajaxParam.GroupId
                                                            && x.LectureId == nowLecture.Id)
                                               .FirstOrDefault();
                    var isNew = false;
                    if (groupRecord == null)
                    {
                        groupRecord = new GroupRecord
                        {
                            LectureId = nowLecture.Id,
                            Role = role,
                            GroupId = ajaxParam.GroupId,
                            Point = ajaxParam.Point,
                        };
                        isNew = true;
                    }

                    if (isNew)
                    {
                        db.GroupRecords.Add(groupRecord);
                    }
                    else
                    {
                        groupRecord.Point = groupRecord.Point + ajaxParam.Point;
                        db.Entry(groupRecord).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    ajaxParam.Point = groupRecord.Point;
                    ajaxResult.ResultData = ajaxParam;
                    ajaxResult.Result = true;
                }
                catch
                {
                    ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                    return null;
                }
            }

            return ajaxResult.GetJsonRsult();
        }

    }
}