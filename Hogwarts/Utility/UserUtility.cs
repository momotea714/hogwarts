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
    public static class UserUtility
    {
        public static ApplicationUser CreateUser(string userID, string userName, string email, string userPWD, string role)
        {
            using (var db = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                if (roleManager.RoleExists(role))
                {
                    var user = new ApplicationUser();
                    user.UserID = userID;
                    user.UserName = userID;
                    user.DisplayUserName = userName;
                    user.Email = email;

                    var chkUser = UserManager.Create(user, userPWD);

                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, role);
                    }
                    return user;
                }
            }
            return null;
        }

        public static bool CreateRole(string roleName)
        {
            using (var db = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                if (!roleManager.RoleExists(roleName))
                {
                    var role = new IdentityRole();
                    role.Name = roleName;
                    roleManager.Create(role);
                    return true;
                }
            }
            return false;
        }

        public static List<ApplicationUser> GetUserListInNowDisplayGroup()
        {
            using (var db = new ApplicationDbContext())
            {
                var nowDisplayRole = HogwartsSettingUtility.GetSetting(HogwartsSettingUtility.NowDisplayRole);
                var roleId = db.Roles.Where(x => x.Name == nowDisplayRole).First().Id;
                return db.Users.Where(x => x.Roles.Any(y => y.RoleId == roleId)).ToList();
            }
        }

        public static IEnumerable<IdentityRole> GetAllRoleExceptAdmin()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Roles.Where(x => x.Name != "Admin").ToList();
            }
        }
    }
}