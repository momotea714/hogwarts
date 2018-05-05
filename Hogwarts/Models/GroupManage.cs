using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int LectureId { get; set; }

        public string GroupName { get; set; }

        public int Point { get; set; }
    }

    public class GroupMember
    {
        public int Id { get; set; }
        //AspNetRoles.Name
        public string Role { get; set; }

        public int GroupId { get; set; }
        //AspNetUser.Id
        public string UserId { get; set; }
    }

    public class GroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int GroupMemberCount { get; set; }
    }
}