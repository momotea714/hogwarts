using Hogwarts.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hogwarts.Models
{
    public class Lecture
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int ShowOrder { get; set; }

        public string LectureName { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }

    public class SubLecture
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int LectureId { get; set; }

        public int ShowOrder { get; set; }

        public string SubLectureName { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }

    public class Question
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int SubLectureId { get; set; }

        public int QuestionNO { get; set; }

        public int ShowOrder { get; set; }

        public int Difficulty { get; set; }

        public string QuestionName { get; set; }

        public int EstimateTime { get; set; }
    }

    public class UserAnswerState
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int QuestionId { get; set; }

        public string UserId { get; set; }

        public byte State { get; set; }

        public int Progress { get; set; }

        public int UnderStandingLevel { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }

    public class ProgressManageViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public int UnderStandingLevel { get; set; }
        public double NotStartYet { get; set; }
        public double InProcess { get; set; }
        public double Completion { get; set; }
        public int Progress { get; set; }
        public string Remark { get; set; }

    }

    public class AnswerStateAjaxParam
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Remark { get; set; }
        //0:progress,1:update
        public int UpdateKBN { get; set; }
    }

    public class SettingAjaxParam
    {
        public int NodeCategoryCD { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public int Difficulty { get; set; }
        public int EstimateTime { get; set; }
    }

    public static class AnswerStateManagement
    {
        public static JsonResult UpdateAnswerState(AnswerStateAjaxParam ajaxParam, ApplicationUser user)
        {
            var ajaxResult = new AjaxResult();
            var role = HogwartsSettingUtility.GetNowDisplayRole();
            UserAnswerState answerState;
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    answerState = db.UserAnswerStates.Where(x => x.QuestionId == ajaxParam.Id
                                                            && x.UserId == user.Id).FirstOrDefault();
                    var isNew = false;
                    if (answerState == null)
                    {
                        answerState = new UserAnswerState
                        {
                            QuestionId = ajaxParam.Id,
                            Role = role,
                            UserId = user.Id,
                            UpdateDateTime = DateTime.Now,
                        };
                        isNew = true;
                    }

                    if (ajaxParam.UpdateKBN == 0)
                    {
                        answerState.Progress = ajaxParam.Value;
                    }
                    else if (ajaxParam.UpdateKBN == 1)
                    {
                        answerState.UnderStandingLevel = ajaxParam.Value;
                    }
                    else
                    {
                        ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                        ajaxResult.ResultData = null;
                    }

                    if (isNew)
                    {
                        db.UserAnswerStates.Add(answerState);
                    }
                    else
                    {
                        db.Entry(answerState).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    ajaxResult.ResultData = answerState;
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

    public static class ProgressManagement
    {
        public static JsonResult AddTreeObject(SettingAjaxParam ajaxParam)
        {
            var ajaxResult = new AjaxResult();
            if (ajaxParam.NodeCategoryCD == 1)
            {
                ajaxResult.ResultData = new
                {
                    Data = AddLecture(ajaxParam),
                    NodeCategoryCD = 1
                };
            }
            else if (ajaxParam.NodeCategoryCD == 2)
            {
                ajaxResult.ResultData = new
                {
                    Data = AddSubLecture(ajaxParam),
                    NodeCategoryCD = 2
                };
            }
            else if (ajaxParam.NodeCategoryCD == 3)
            {
                ajaxResult.ResultData = new
                {
                    Data = AddQuestion(ajaxParam),
                    NodeCategoryCD = 3
                };
            }
            else
            {
                ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                ajaxResult.ResultData = null;
            }

            if (ajaxResult.ResultData != null)
            {
                ajaxResult.Result = true;
            }
            return ajaxResult.GetJsonRsult();
        }

        public static JsonResult EditTreeObject(SettingAjaxParam ajaxParam)
        {
            var ajaxResult = new AjaxResult();
            if (ajaxParam.NodeCategoryCD == 1)
            {
                ajaxResult.ResultData = new
                {
                    Data = EditLecture(ajaxParam),
                    NodeCategoryCD = 1
                };
            }
            else if (ajaxParam.NodeCategoryCD == 2)
            {
                ajaxResult.ResultData = new
                {
                    Data = EditSubLecture(ajaxParam),
                    NodeCategoryCD = 2
                };
            }
            else if (ajaxParam.NodeCategoryCD == 3)
            {
                ajaxResult.ResultData = new
                {
                    Data = EditQuestion(ajaxParam),
                    NodeCategoryCD = 3
                };
            }
            else
            {
                ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                ajaxResult.ResultData = null;
            }

            if (ajaxResult.ResultData != null)
            {
                ajaxResult.Result = true;
            }
            return ajaxResult.GetJsonRsult();
        }

        public static JsonResult DeleteTreeObject(SettingAjaxParam ajaxParam)
        {
            var ajaxResult = new AjaxResult();
            if (ajaxParam.NodeCategoryCD == 1)
            {
                ajaxResult.ResultData = new
                {
                    Data = DeleteLecture(ajaxParam),
                    NodeCategoryCD = 1,
                    ajaxParam.Id
                };
            }
            else if (ajaxParam.NodeCategoryCD == 2)
            {
                ajaxResult.ResultData = new
                {
                    Data = DeleteSubLecture(ajaxParam),
                    NodeCategoryCD = 2,
                    ajaxParam.Id
                };
            }
            else if (ajaxParam.NodeCategoryCD == 3)
            {
                ajaxResult.ResultData = new
                {
                    Data = DeleteQuestion(ajaxParam),
                    NodeCategoryCD = 3,
                    ajaxParam.Id
                };
            }
            else
            {
                ajaxResult.Message = "追加失敗。とりあえずF5更新やな";
                ajaxResult.ResultData = null;
            }

            if (ajaxResult.ResultData != null)
            {
                ajaxResult.Result = true;
            }
            return ajaxResult.GetJsonRsult();
        }

        private static Lecture AddLecture(SettingAjaxParam ajaxParam)
        {
            var showOrder = 1;
            if (new ApplicationDbContext().Lectures.Any())
            {
                showOrder = new ApplicationDbContext().Lectures.Max(x => x.ShowOrder) + 1;
            }

            var lecture = new Lecture
            {
                LectureName = ajaxParam.Text,
                ShowOrder = showOrder,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now,
                Role = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole)
            };
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    db.Lectures.Add(lecture);
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
            }
            return lecture;
        }
        private static SubLecture AddSubLecture(SettingAjaxParam ajaxParam)
        {
            var showOrder = 1;
            if (new ApplicationDbContext().SubLectures.Any())
            {
                showOrder = new ApplicationDbContext().SubLectures.Max(x => x.ShowOrder) + 1;
            }

            var subLecture = new SubLecture
            {
                LectureId = ajaxParam.ParentId,
                SubLectureName = ajaxParam.Text,
                ShowOrder = showOrder,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now,
                Role = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole)
            };
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    db.SubLectures.Add(subLecture);
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
            }
            return subLecture;
        }

        private static Question AddQuestion(SettingAjaxParam ajaxParam)
        {
            var questionNO = 1;
            if (new ApplicationDbContext().Questions.Any())
            {
                questionNO = new ApplicationDbContext().Questions.Max(x => x.QuestionNO) + 1;
            }

            var showOrder = 0;
            if (new ApplicationDbContext().Questions.Any())
            {
                showOrder = new ApplicationDbContext().Questions.Max(x => x.ShowOrder) + 1;
            }

            var question = new Question
            {
                SubLectureId = ajaxParam.ParentId,
                QuestionNO = questionNO,
                QuestionName = ajaxParam.Text,
                ShowOrder = showOrder,
                Difficulty = 1,
                EstimateTime = 5,
                Role = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole)
            };
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    db.Questions.Add(question);
                    db.SaveChanges();
                }
                catch
                {
                    return null;
                }
            }
            return question;
        }

        private static bool EditLecture(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var lecture = db.Lectures.Find(ajaxParam.Id);
                    lecture.LectureName = ajaxParam.Text;
                    db.Entry(lecture).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        private static bool EditSubLecture(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var subLecture = db.SubLectures.Find(ajaxParam.Id);
                    subLecture.SubLectureName = ajaxParam.Text;
                    db.Entry(subLecture).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static bool EditQuestion(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var question = db.Questions.Find(ajaxParam.Id);
                    question.QuestionName = ajaxParam.Text;
                    question.Difficulty = ajaxParam.Difficulty;
                    question.EstimateTime = ajaxParam.EstimateTime;
                    db.Entry(question).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static bool DeleteLecture(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var lecture = db.Lectures.Find(ajaxParam.Id);
                    var sublectures = db.SubLectures.Where(x => x.LectureId == lecture.Id);
                    if (sublectures.Any())
                    {
                        var questions = db.Questions.Where(x => sublectures.Any(y => y.Id == x.SubLectureId));
                        if (questions.Any())
                        {
                            db.Questions.RemoveRange(questions);
                        }
                        db.SubLectures.RemoveRange(db.SubLectures.Where(x => x.LectureId == lecture.Id));
                    }
                    db.Lectures.Remove(lecture);
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        private static bool DeleteSubLecture(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var sublecture = db.SubLectures.Find(ajaxParam.Id);
                    var questions = db.Questions.Where(x => x.SubLectureId == sublecture.Id);
                    if (questions.Any())
                    {
                        db.Questions.RemoveRange(questions);
                    }
                    db.SubLectures.Remove(sublecture);
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static bool DeleteQuestion(SettingAjaxParam ajaxParam)
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var question = db.Questions.Find(ajaxParam.Id);
                    db.Questions.Remove(question);
                    db.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}


