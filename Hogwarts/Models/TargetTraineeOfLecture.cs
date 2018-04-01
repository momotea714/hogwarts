using Hogwarts.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Models
{
    public class TargetTraineeOfLecture
    {
        public int Id { get; set; }

        public int LectureId { get; set; }
        //AspNetUsers.Id
        public string UserId { get; set; }
    }

    public static class TargetTraineeOfLectureUtility
    {
        public static JsonResult TargetTraineeOfLectureSave(int lectureId, List<string> userIds)
        {
            var ajaxResult = new AjaxResult();

            using (var db = new ApplicationDbContext())
            {
                try
                {
                    //既存データの削除
                    var targetTraineeOfLectures = db.TargetTraineeOfLectures.Where(x => x.LectureId == lectureId);

                    if (targetTraineeOfLectures.Any())
                    {
                        db.TargetTraineeOfLectures.RemoveRange(targetTraineeOfLectures);
                        db.SaveChanges();
                    }
                    //新規登録
                    var newTargetTraineeOfLectures = new List<TargetTraineeOfLecture>();
                    foreach (var userId in userIds)
                    {
                        newTargetTraineeOfLectures.Add(new TargetTraineeOfLecture
                        {
                            LectureId = lectureId,
                            UserId = userId
                        });
                    }
                    if (newTargetTraineeOfLectures.Any())
                    {
                        db.TargetTraineeOfLectures.AddRange(newTargetTraineeOfLectures);
                        db.SaveChanges();
                    }
                    ajaxResult.Result = true;
                }
                catch
                {
                    ajaxResult.Result = false;
                }
            }

            return ajaxResult.GetJsonRsult();
        }
    }
}