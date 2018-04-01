using Hogwarts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Hogwarts.Utility
{
    public static class LectureUtility
    {
        public static IEnumerable<Lecture> GetLecturesInNowRole(string nowDisplayRole)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Lectures.Where(x => x.Role == nowDisplayRole).ToList();
            }
        }
    }
}