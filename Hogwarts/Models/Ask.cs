using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class Ask
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public int ParentAskId { get; set; }

        public string Content { get; set; }

        public string CreateUserId { get; set; }

        public string IsClosed { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }
    }
}