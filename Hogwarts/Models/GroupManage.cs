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

        public string Role { get; set; }

        public int GroupId { get; set; }

        public string UserId { get; set; }
    }
}