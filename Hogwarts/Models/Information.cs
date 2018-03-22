using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class Information
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string Content { get; set; }

        public string CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public string Title { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}