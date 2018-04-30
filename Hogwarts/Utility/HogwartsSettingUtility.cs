using Hogwarts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Hogwarts.Utility
{
    public static class HogwartsSettingUtility
    {
        //ここで持つプロパティはstartup.csで設定してあげて。
        public static string NowDisplayRole = "NowDisplayRole";

        public static string NowLecture = "NowLecture";


        public static string GetNowDisplayRole()
        {
            return HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole);
        }

        public static Lecture GetNowLecture()
        {
            var lectureId = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowLecture);
            int id;
            if (!int.TryParse(lectureId, out id))
            {
                return new Lecture();
            }
            var lecture = new Lecture();
            using (var db = new ApplicationDbContext())
            {
                lecture = db.Lectures.Where(x => x.Id == id).FirstOrDefault();
            }

            return lecture;
        }

        public static string GetSetting(string settingId)
        {
            var setting = string.Empty;

            using (var db = new ApplicationDbContext())
            {
                setting = db.HogwartsSettings.Where(x => x.SettingId == settingId)
                   .First().Setting;
            }

            return setting;
        }

        public static bool UpdateHogwartsSetting(string settingId, string setting)
        {
            using (var db = new ApplicationDbContext())
            {
                var hogwartsSetting = db.HogwartsSettings.Where(x => x.SettingId == settingId).First();

                if (hogwartsSetting != null)
                {
                    hogwartsSetting.Setting = setting;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public static bool CreateHogwartsSettingInitialize(string settingId, string setting)
        {
            using (var db = new ApplicationDbContext())
            {
                var hogwartsSettings = db.HogwartsSettings.Where(x => x.SettingId == settingId);

                if (!hogwartsSettings.Any())
                {
                    var hogwartsSetting = new HogwartsSetting();
                    hogwartsSetting.SettingId = settingId;
                    hogwartsSetting.Setting = setting;
                    db.HogwartsSettings.Add(hogwartsSetting);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}