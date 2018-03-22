using Hogwarts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Utility
{
    public static class HogwartsSettingUtility
    {
        //ここで持つプロパティはstartup.csで設定してあげて。
        public static string NowDisplayRole = "NowDisplayRole";

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