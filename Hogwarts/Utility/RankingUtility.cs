using Hogwarts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Utility
{
    public static class RankingUtility
    {
        public static IEnumerable<UserPracticeRecordViewModel> GetUserPracticeRecordViewModels(int nowLectureId, Dictionary<string, ApplicationUser> userInRole)
        {
            using (var db = new ApplicationDbContext())
            {
                var subLectures = db.SubLectures.Where(x => x.LectureId == nowLectureId).ToList();
                var subLectureIds = subLectures.Select(y => y.Id).ToList();
                var questions = db.Questions.Where(x => subLectureIds.Contains(x.SubLectureId)).ToList();
                var questionIds = questions.Select(y => y.Id).ToList();
                var questionCount = questions.Count();

                var UserAnswerStateSummaries = db.UserAnswerStates.Where(x => questionIds.Contains(x.QuestionId))
                                                         .GroupBy(x => x.UserId)
                                                         .Select(x =>
                                                         new UserAnswerStateSummary
                                                         {
                                                             UserId = x.Key,
                                                             ProgressSum = x.Sum(y => y.Progress)
                                                         })
                                                         .ToDictionary(x => x.UserId);

                var targetTraineeOfLectures = db.TargetTraineeOfLectures
                                                .Join(db.Lectures,
                                                t => t.LectureId,
                                                l => l.Id,
                                                (t, l) => new { t, l })
                                                .Where(x => x.l.Id == nowLectureId)
                                                .ToList();

                var userPracticeRecordViewModels = new List<UserPracticeRecordViewModel>();
                var targetUserIds = new List<string>();
                if (targetTraineeOfLectures.Any())
                {
                    targetUserIds.AddRange(targetTraineeOfLectures.Select(x => x.t.UserId));
                }
                else
                {
                    targetUserIds.AddRange(userInRole.Select(x => x.Value.Id));
                }

                foreach (var targetUserId in targetUserIds)
                {
                    double progress;
                    UserAnswerStateSummary UserAnswerStateSummary;
                    if (UserAnswerStateSummaries.TryGetValue(targetUserId, out UserAnswerStateSummary) && questionCount > 0)
                    {
                        progress = Math.Round((double)(UserAnswerStateSummary.ProgressSum / questionCount), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        progress = 0;
                    }
                    var userPracticeRecordViewModel = new UserPracticeRecordViewModel
                    {
                        UserId = targetUserId,
                        UserName = userInRole[targetUserId].UserName,
                        Progress = progress,
                    };
                    userPracticeRecordViewModels.Add(userPracticeRecordViewModel);
                }
                userPracticeRecordViewModels = userPracticeRecordViewModels.OrderByDescending(x => x.Progress)
                                                                           .ToList();
                userPracticeRecordViewModels.ForEach(x =>
                        x.Rank = userPracticeRecordViewModels.Count(y => y.Progress > x.Progress) + 1);

                return userPracticeRecordViewModels;
            }
        }

        public static IEnumerable<UserPointRecordViewModel> GetUserPointRecordViewModels(int nowLectureId, Dictionary<string, ApplicationUser> userInRole)
        {
            using (var db = new ApplicationDbContext())
            {
                var userRecords = db.UserRecords.Where(x => x.LectureId == nowLectureId)
                                            .ToList();
                var targetTraineeOfLectures = db.TargetTraineeOfLectures
                                                .Join(db.Lectures,
                                                t => t.LectureId,
                                                l => l.Id,
                                                (t, l) => new { t, l })
                                                .Where(x => x.l.Id == nowLectureId)
                                                .ToList();
                if (targetTraineeOfLectures.Any())
                {
                    var userNotInUserRecords = targetTraineeOfLectures.Where(x => !userRecords.Any(y => y.UserId == x.t.UserId)).ToDictionary(x => x.t.UserId);
                    userRecords.AddRange(userNotInUserRecords.Select(x =>
                                                new UserRecord
                                                {
                                                    UserId = x.Key,
                                                    Point = 0,
                                                }));
                }
                else
                {
                    var userNotInUserRecords = userInRole.Where(x => !userRecords.Any(y => y.UserId == x.Key)).ToDictionary(x => x.Key);
                    userRecords.AddRange(userNotInUserRecords.Select(x =>
                                                new UserRecord
                                                {
                                                    UserId = x.Key,
                                                    Point = 0,
                                                }));
                }
                
                return userRecords.OrderByDescending(x => x.Point)
                                  .Select(x =>
                                         new UserPointRecordViewModel
                                         {
                                             UserId = x.UserId,
                                             UserLoginId = userInRole[x.UserId].UserID,
                                             UserName = userInRole[x.UserId].UserName,
                                             Point = x.Point,
                                             Rank = userRecords.Count(y => y.Point > x.Point) + 1
                                         })
                                    .ToList();
            }
        }

        public static IEnumerable<GroupPracticeRecordViewModel> GetGroupPracticeRecordViewModels(int nowLectureId, Dictionary<string, ApplicationUser> userInRole)
        {
            using (var db = new ApplicationDbContext())
            {
                var subLectures = db.SubLectures.Where(x => x.LectureId == nowLectureId).ToList();
                var subLectureIds = subLectures.Select(y => y.Id).ToList();
                var questions = db.Questions.Where(x => subLectureIds.Contains(x.SubLectureId)).ToList();
                var questionIds = questions.Select(y => y.Id).ToList();
                var questionCount = questions.Count();

                var groups = db.Groups.Join(
                                            db.GroupMembers,
                                            g => g.Id,
                                            gm => gm.GroupId,
                                            (g, gm) => new { g, gm }
                                            )
                                      .Where(x => x.g.LectureId == nowLectureId)
                                      .GroupBy(x => x.gm.GroupId)
                                      .Select(x => new GroupViewModel
                                      {
                                          GroupId = x.Key,
                                          GroupName = x.Max(y => y.g.GroupName),
                                          GroupMemberCount = x.Count()
                                      })
                                      .ToDictionary(x => x.GroupId);
                var groupRecords = db.Groups.Join(db.GroupMembers,
                                          g => g.Id,
                                          gm => gm.GroupId,
                                          (Group, GroupMember) => new { Group, GroupMember })
                                      .Join(
                                          db.UserAnswerStates,
                                          x => x.GroupMember.UserId,
                                          u => u.UserId,
                                          (Group, UserAnswerState) => new { Group, UserAnswerState })
                                      .Join(db.Questions,
                                          x => x.UserAnswerState.QuestionId,
                                          q => q.Id,
                                      (Group, Questions) => new { Group, Questions })
                                      .Join(db.SubLectures,
                                          x => x.Questions.SubLectureId,
                                          s => s.Id,
                                          (Group, SubLecture) => new { Group, SubLecture })
                                      .Where(x => x.SubLecture.LectureId == nowLectureId)
                                      .GroupBy(x => x.Group.Group.Group.Group.Id)
                                      .Select(x => new GroupPracticeRecordViewModel
                                      {
                                          GroupId = x.Key,
                                          GroupName = x.Max(y => y.Group.Group.Group.Group.GroupName),
                                          Progress = x.Sum(y => y.Group.Group.UserAnswerState.Progress)
                                      })
                                      .ToList();
                var groupsNotInGroupRecords = groups.Where(x => !groupRecords.Any(y => y.GroupId == x.Key))
                                                    .ToDictionary(x => x.Key);
                groupRecords.AddRange(groupsNotInGroupRecords.Select(x =>
                            new GroupPracticeRecordViewModel
                            {
                                GroupId = x.Key,
                                GroupName = x.Value.Value.GroupName,
                                Progress = 0
                            }));

                groupRecords.ForEach(x =>
                {
                    if (questionCount > 0 && groups[x.GroupId].GroupMemberCount > 0)
                    {
                        x.Progress = x.Progress / questionCount / groups[x.GroupId].GroupMemberCount;
                    }
                    else
                    {
                        x.Progress = 0;
                    }
                });

                groupRecords.ForEach(x => x.Rank = groupRecords.Count(y => y.Progress > x.Progress) + 1);

                return groupRecords.OrderByDescending(x => x.Progress);
            }
        }

        public static IEnumerable<GroupPointRecordViewModel> GetGroupPointRecordViewModels(int nowLectureId, Dictionary<string, ApplicationUser> userInRole)
        {
            using (var db = new ApplicationDbContext())
            {
                var groupRecords = db.GroupRecords.Where(x => x.LectureId == nowLectureId)
                                            .ToList();
                var groups = db.Groups.Where(x => x.LectureId == nowLectureId).ToDictionary(x => x.Id);
                var groupsNotInGroupRecords = groups.Where(x => !groupRecords.Any(y => y.GroupId == x.Key))
                                                    .ToDictionary(x => x.Key);
                var groupPointRecordViewModels = groupRecords.OrderByDescending(x => x.Point)
                                  .Select(x =>
                                         new GroupPointRecordViewModel
                                         {
                                             GroupId = x.GroupId,
                                             GroupName = groups[x.GroupId].GroupName,
                                             Point = x.Point,
                                         })
                                    .ToList();

                groupPointRecordViewModels.AddRange(groupsNotInGroupRecords.Select(x =>
                            new GroupPointRecordViewModel
                            {
                                GroupId = x.Key,
                                GroupName = x.Value.Value.GroupName,
                                Point = 0
                            }));
                groupPointRecordViewModels.ForEach(x => 
                                x.Rank = groupPointRecordViewModels.Count(y => y.Point > x.Point) + 1);

                return groupPointRecordViewModels;
            }
        }
    }
}